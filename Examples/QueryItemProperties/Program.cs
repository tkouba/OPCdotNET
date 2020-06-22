using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPC.Common;
using OPC.Data;

namespace QueryItemProperties
{
    class Program
    {
        static void Main(string[] args)
        {
            string progID = args.Length > 0 ? args[0] : "Kepware.KEPServerEX.V5";
            string itemID = args.Length > 1 ? args[1] : "Simulation Examples.Functions.Ramp1";

            OpcServer opcServer = new OpcServer();
            opcServer.Connect(progID);
            System.Threading.Thread.Sleep(500); // we are faster than some servers!

            OpcProperty[] props = opcServer.QueryAvailableProperties(itemID);
            int[] propIDs = new int[props.Length];
            for (int i = 0; i < props.Length; i++)
            {
                propIDs[i] = props[i].PropertyID;
            }
            OpcPropertyData[] data = opcServer.GetItemProperties(itemID, propIDs);

            for (int i = 0; i < props.Length; i++)
            {
                Console.Write(" {0}: '{1}' ({2}) = ", props[i].PropertyID, props[i].Description, Extensions.VarEnumToString(props[i].DataType));
                if (data[i].Error == HRESULTS.S_OK)
                    Console.WriteLine(data[i].Data);
                else
                    Console.WriteLine("!ERROR:{0}", data[i].Error);
            }

            opcServer.Disconnect();
        }
    }
}
