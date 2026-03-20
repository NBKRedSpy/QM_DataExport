using HarmonyLib;
using MGSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM_DataExport
{
    [HarmonyPatch(typeof(ConfigLoader), nameof(ConfigLoader.LoadSpecificFile))]
    internal class ConfigLoader_LoadSpecificFile_Patch
    {
        public void Prefix(string path)
        {

        }
    }
}
