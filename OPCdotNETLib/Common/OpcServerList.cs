using System;
using System.Runtime.InteropServices;
using OPC.Common.Interface;

namespace OPC.Common
{
    /// <summary>
    /// Opc server list
    /// </summary>
    public static class OpcServerList
    {
        private static readonly Guid OpcServerListGuid = new Guid("13486D51-4821-11D2-A494-3CB306C10000");
        /// <summary>
        /// The servers compliant with OPC Data Access 1.0 implement category with CatID
        /// </summary>
        public static readonly Guid OpcDataAccess10 = new Guid("63D5F430-CFE4-11d1-B2C8-0060083BA1FB");
        /// <summary>
        /// The servers compliant with OPC Data Access 2.0 implement category with CatID
        /// </summary>
        public static readonly Guid OpcDataAccess20 = new Guid("63D5F432-CFE4-11d1-B2C8-0060083BA1FB");
        /// <summary>
        /// The servers compliant with OPC Data Access 3.0 implement category with CatID
        /// </summary>
        public static readonly Guid OpcDataAccess30 = new Guid("CC603642-66D7-48f1-B69A-B625E73652D7");
        /// <summary>
        /// The servers compliant with OPC XML Data Access implement category with CatID
        /// </summary>
        public static readonly Guid OpcDataAccessXML = new Guid("3098EDA4-A006-48b2-A27F-247453959408");

        /// <summary>
        /// List all servers compliant with OPC Data Access 2.0
        /// </summary>        
        public static OpcServerDefinition[] ListAllData20()
        {					
            return ListAll(OpcDataAccess20);
        }

        /// <summary>
        /// List all
        /// </summary>
        /// <param name="catid">Catid</param>
        public static OpcServerDefinition[] ListAll(Guid catid)
        {
            OpcServerDefinition[] serverslist = new OpcServerDefinition[0];
            object enumObj = null;
            Type typeOfList = Type.GetTypeFromCLSID(OpcServerListGuid);
            object listObj = Activator.CreateInstance(typeOfList);
            try
            {
                IOPCServerList opcServerList = listObj as IOPCServerList;
                if (opcServerList == null)
                    Marshal.ThrowExceptionForHR(HRESULTS.E_ABORT);

                opcServerList.EnumClassesOfCategories(1, ref catid, 0, ref catid, out enumObj);
                if (enumObj == null)
                    Marshal.ThrowExceptionForHR(HRESULTS.E_ABORT);

                IEnumGUID ifEnum = enumObj as IEnumGUID;
                if (ifEnum == null)
                    Marshal.ThrowExceptionForHR(HRESULTS.E_ABORT);

                const int maxcount = 300;
                IntPtr ptrGuid = Marshal.AllocCoTaskMem(maxcount * 16);
                ifEnum.Next(maxcount, ptrGuid, out int count);
                if (count < 1)
                {
                    Marshal.FreeCoTaskMem(ptrGuid);
                    return serverslist;
                }

                serverslist = new OpcServerDefinition[count];

                byte[] guidbin = new byte[16];
                int runGuid = (int)ptrGuid;
                for (int i = 0; i < count; i++)
                {
                    serverslist[i] = new OpcServerDefinition();
                    Marshal.Copy((IntPtr)runGuid, guidbin, 0, 16);
                    serverslist[i].ClsID = new Guid(guidbin);
                    opcServerList.GetClassDetails(ref serverslist[i].ClsID,
                                            out serverslist[i].ProgID, out serverslist[i].Name);
                    runGuid += 16;
                }

                Marshal.FreeCoTaskMem(ptrGuid);
            }
            finally
            {
                if (!(enumObj == null))
                    Marshal.ReleaseComObject(enumObj);
                if (!(listObj == null))
                    Marshal.ReleaseComObject(listObj);
            }
            return serverslist;
        }
    }
}
