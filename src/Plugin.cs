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


        [Hook(ModHookType.AfterConfigsLoaded)]
        public static void AfterConfig(IModContext context)
        {
            Directory.CreateDirectory(ConfigDirectories.ModPersistenceFolder);

            string dataDirectory = Path.Combine(ConfigDirectories.ModPersistenceFolder, "Data");
            DataExportProcessor processor = new DataExportProcessor();
            processor.Export(dataDirectory);
        }
    }
}
