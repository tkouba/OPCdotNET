using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPC.Data;

namespace ServerInformation
{
    class Program
    {
        static void Main(string[] args)
        {
            string progID = args.Length > 0 ? args[0] : "Kepware.KEPServerEX.V5";

            OpcServer opcServer = new OpcServer();
            opcServer.Connect(progID);
            System.Threading.Thread.Sleep(500); // we are faster than some servers!
            SERVERSTATUS serverStatus = opcServer.GetStatus();
            Console.WriteLine("'{0}' vendor '{1}' version {2}.{3}.{4}",
                progID,
                serverStatus.szVendorInfo,
                serverStatus.wMajorVersion, serverStatus.wMinorVersion, serverStatus.wBuildNumber);
            Console.WriteLine("State       : {0}", serverStatus.eServerState);
            Console.WriteLine("Start time  : {0}", DateTime.FromFileTime(serverStatus.ftStartTime));
            Console.WriteLine("Last update : {0}", DateTime.FromFileTime(serverStatus.ftLastUpdateTime));
            Console.WriteLine("Current time: {0}", DateTime.FromFileTime(serverStatus.ftCurrentTime));
            // Bandwith and group count valid only for this connection
            Console.WriteLine("Bandwidth   : {0}", serverStatus.dwBandWidth);
            Console.WriteLine("Group count : {0}", serverStatus.dwGroupCount);
            opcServer.GetLocaleID(out int lcid);
            Console.WriteLine("Locale ID: {0}", lcid);
            opcServer.Disconnect();
        }
    }
}
