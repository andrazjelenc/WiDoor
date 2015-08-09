# WiDoor

Backdoor created by Windows Hosted network (netsh wlan set hostednetwork...)
It is legit service integrated into Windows7 and later. Only place where the detection is possible is under Adapter settings. There are no warning when somebody connects to the hosted wireless. Microsoft refused to fix - make hosted network easily to control.

Wireless card can host AP and be connected to the wireless hotspot at the same time, because it host AP on the same channel that is used by hotspot. No connecting error is present while starting or closing backdoor.

Orginal idea from: https://github.com/Vivek-Ramachandran/wi-door

Youtube talk: https://www.youtube.com/watch?v=T6yc0Toyt2A

Orginal app is writen in C++, I've made it in C#.


The application must be run with admin rights!
