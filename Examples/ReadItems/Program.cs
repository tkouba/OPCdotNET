using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using OPC.Common;
using OPC.Data;

namespace ReadItems
{
    class Program
    {

        static void Main(string[] args)
        {
            string progID = args.Length > 0 ? args[0] : "Kepware.KEPServerEX.V5";

            OpcServer opcServer = new OpcServer();
            opcServer.Connect(progID);
            System.Threading.Thread.Sleep(500); // we are faster than some servers!

            OpcGroup opcGroup = opcServer.AddGroup("SampleGroup", false, 900);

            List<string> itemNames = new List<string>();
            if (args.Length > 1)
            {
                for (int i = 1; i < args.Length; i++)
                {
                    itemNames.Add(args[i]);
                }
            }
            else
            {
                itemNames.Add("Simulation Examples.Functions.Ramp1");
                itemNames.Add("Simulation Examples.Functions.Random1");
            }

            OpcItemDefinition[] opcItemDefs = new OpcItemDefinition[itemNames.Count];
            for (int i = 0; i < opcItemDefs.Length; i++)
            {
                opcItemDefs[i] = new OpcItemDefinition(itemNames[i], true, i, VarEnum.VT_EMPTY);
            }

            opcGroup.AddItems(opcItemDefs, out OpcItemResult[] opcItemResult);
            if (opcItemResult == null)
            {
                Console.WriteLine("Error add items - null value returned");
                return;
            }

            int[] serverHandles = new int[opcItemResult.Length];
            for (int i = 0; i < opcItemResult.Length; i++)
            {
                if (HRESULTS.Failed(opcItemResult[i].Error))
                {
                    Console.WriteLine("AddItems - failed {0}", itemNames[i]);
                    opcGroup.Remove(true);
                    opcServer.Disconnect();
                    return;
                }
                else
                {
                    serverHandles[i] = opcItemResult[i].HandleServer;
                }
            }

            opcGroup.SetEnable(true);
            opcGroup.Active = true;

            bool result = opcGroup.Read(OPCDATASOURCE.OPC_DS_CACHE, serverHandles, out OpcItemState[] states);

            foreach (OpcItemState s in states)
            {
                if (HRESULTS.Succeeded(s.Error))
                    Console.WriteLine(" {0}: {1} (Q:{2} T:{3})", s.HandleClient, s.DataValue, s.Quality, DateTime.FromFileTime(s.TimeStamp));
                else
                    Console.WriteLine(" {0}: ERROR = 0x{1:x} !", s.HandleClient, s.Error);
            }

            opcGroup.Remove(true);
            opcServer.Disconnect();
        }

    }
}
