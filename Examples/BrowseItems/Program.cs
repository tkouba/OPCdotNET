using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPC.Data;

namespace BrowseItems
{
    class Program
    {
        static void Main(string[] args)
        {
            string progID = args.Length > 0 ? args[0] : "Kepware.KEPServerEX.V5";

            OpcServer opcServer = new OpcServer();
            opcServer.Connect(progID);
            System.Threading.Thread.Sleep(500); // we are faster than some servers!

            foreach (string item in opcServer.BrowseItemIDs(OPCBROWSETYPE.OPC_FLAT))
            {
                Console.WriteLine(item);
            }
            opcServer.Disconnect();
        }
    }
}
