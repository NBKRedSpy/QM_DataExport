using HarmonyLib;
using MGSC;
using QM_MissionExpirationHighlight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QM_DataExport
{
    public static class Plugin
    {
        
        public static string ModAssemblyName => Assembly.GetExecutingAssembly().GetName().Name;

        public static ConfigDirectories ConfigDirectories = new ConfigDirectories();


        [Hook(ModHookType.BeforeBootstrap)]
        public static void BeforeBootStrap(IModContext context)
        {
            Directory.CreateDirectory(ConfigDirectories.DataDirectory);
            DataExportProcessor processor = new DataExportProcessor();
            processor.LocalizationExport(ConfigDirectories.DataDirectory);
            new Harmony("NBKRedSpy_" + ModAssemblyName).PatchAll();
        }
    }
}
