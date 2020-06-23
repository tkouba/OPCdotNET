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
            ListAllServers(OpcServerList.OpcDataAccess10);

            Console.WriteLine("OPC Data Access 2.0");
            ListAllServers(OpcServerList.OpcDataAccess20);

            Console.WriteLine("OPC Data Access 3.0");
            ListAllServers(OpcServerList.OpcDataAccess30);

            Console.WriteLine("OPC XML Data Access");
            ListAllServers(OpcServerList.OpcDataAccessXML);
        }

        private static void ListAllServers(Guid catid)
        {
            try
            {
                foreach (var server in OpcServerList.ListAll(catid))
                {
                    Console.WriteLine("  '{0}' ID={1} [{2}]", server.Name, server.ProgID, server.ClsID);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  *** ERROR *** {0}", ex.Message);
            }
        }
    }
}
