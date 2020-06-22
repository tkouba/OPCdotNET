using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPC.Common;

namespace ServerList
{
    class Program
    {
        static void Main()
        {

            Console.WriteLine("OPC Data Access 1.0");
            foreach (var server in OpcServerList.ListAll(OpcServerList.OpcDataAccess10))
            {
                Console.WriteLine("'{0}' ID={1} [{2}]", server.Name, server.ProgID, server.ClsID);
            }

            Console.WriteLine("OPC Data Access 2.0");
            foreach (var server in OpcServerList.ListAllData20())
            {
                Console.WriteLine("'{0}' ID={1} [{2}]", server.Name, server.ProgID, server.ClsID);
            }

            Console.WriteLine("OPC Data Access 3.0");
            foreach (var server in OpcServerList.ListAll(OpcServerList.OpcDataAccess30))
            {
                Console.WriteLine("'{0}' ID={1} [{2}]", server.Name, server.ProgID, server.ClsID);
            }

            Console.WriteLine("OPC XML Data Access");
            foreach (var server in OpcServerList.ListAll(OpcServerList.OpcDataAccessXML))
            {
                Console.WriteLine("'{0}' ID={1} [{2}]", server.Name, server.ProgID, server.ClsID);
            }
        }
    }
}
