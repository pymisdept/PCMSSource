using System;
using System.Collections.Generic;
using System.Text;
using SimpleControls;
using SimpleControls.SimpleSystemInfo;
using SimpleControls.SimpleCrypto;
using System.IO;

namespace PCCore
{
    [Serializable]
    public class ServerInformation
    {       
        private static string _hostName;
        public static string HostName
        {
            get { return _hostName; }
        }

        private static string _ipAddress;
        public static string IPAddress
        {
            get { return _ipAddress; }
        }

        private static string _userName;
        public static string UserName
        {
            get { return _userName; }
        }

        private static string _userDomainName;
        public static string UserDomainName
        {
            get { return _userDomainName; }
        }

        private static string _macAddress;
        public static string MACAddress
        {
            get { return _macAddress; }
        }

        private static string _mbSerialNumber;
        public static string MBSerialNumber
        {
            get { return _mbSerialNumber; }
        }

        private static string _processorId;
        public static string ProcessorId
        {
            get { return _processorId; }
        }


        public static void GetServerInformation(string serverIPAddress)
        {
            _hostName = SimpleNetworkInfo.GetHostName();
            _ipAddress = String.Empty;
            _macAddress = String.Empty;

            string[] ips = SimpleNetworkInfo.GetIPAddresses();
            if (ips != null && ips.Length > 0)
            {
                if (String.IsNullOrEmpty(serverIPAddress))
                {
                    _ipAddress = ips[0];
                    _macAddress = SimpleNetworkInfo.GetMacAddress(_ipAddress);
                }
                else
                {
                    for (int i = 0; i < ips.Length; i++)
                    {
                        if (ips[i].Equals(serverIPAddress))
                        {
                            _ipAddress = serverIPAddress;
                            _macAddress = SimpleNetworkInfo.GetMacAddress(_ipAddress);
                            break;
                        }
                    }
                }
                if (String.IsNullOrEmpty(_macAddress))
                {
                    throw new ApplicationException("Server Information Error: No Valid Server IP Address Found!");
                }
            }

            _userDomainName = SimpleNetworkInfo.GetUserDomainName();
            _userName = SimpleNetworkInfo.GetUserName();
            _processorId = SimpleProcessorInfo.GetProcessorId();
            _mbSerialNumber = SimpleMBInfo.SerialNumber;
        }

        public static string HashValue
        {
            get
            {
                try
                {
                    if (String.IsNullOrEmpty(_macAddress)) 
                        GetServerInformation(null);
                    StringBuilder sb = new StringBuilder();
                    sb.Append(_hostName);
                    sb.Append(_macAddress);
                    sb.Append(SimpleUtils.IfNull(_mbSerialNumber, "NoMBSerial"));
                    sb.Append(SimpleUtils.IfNull(_processorId, "NoCPUId"));
                    CryptoHash md5 = new CryptoHash(CryptoHash.Hashes.MD5);
                    string hash = md5.GetHashInString(sb.ToString());
                    return hash;
                }
                catch
                {
                    #if DEBUG
                        throw;
                    #else
                        return "--------------------------------";
                    #endif
                }
            }
        }

        public static string toString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Host Name: {0}\r\n", _hostName);
            sb.AppendFormat("User Name: {0}\r\n", _userName);
            sb.AppendFormat("IP Address: {0}\r\n", _ipAddress);
            sb.AppendFormat("MAC Address: {0}\r\n", _macAddress);
            sb.AppendFormat("Processor Id: {0}\r\n", _processorId);
            sb.AppendFormat("MB Serial: {0}\r\n", _mbSerialNumber);
            return sb.ToString();
        }
    } // end of class
}
