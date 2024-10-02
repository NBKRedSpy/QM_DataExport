using MGSC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QM_DataExport
{
    internal class DataExportProcessor
    {
        public void Export(string exportDirectory)
        {
            Directory.CreateDirectory(exportDirectory);

            List<string> configFileNames = new List<string>()
            {
                "config_difficulty",
                "config_globals",
                "config_items",
                "config_monsters",
                "config_drops",
                "config_wounds",
                "config_mercenaries",
                "config_spacesandbox",
                "config_barter",
                "config_magnum"
            };

            string currentAssetName;
            foreach (string assetName in configFileNames)
            {
                try
                {
                    currentAssetName = assetName;

                    TextAsset obj = Resources.Load(assetName) as TextAsset;

                    if (obj == null)
                    {
                        throw new NotImplementedException("Failed open " + assetName + " in Resources folder.");
                    }


                    string exportFilePath = Path.Combine(exportDirectory, assetName + ".tsv");

                    string output = obj.text;

                    //Save some disk wear 
                    if(!File.Exists(exportFilePath) || File.ReadAllText(exportFilePath) != output)
                    {
                        File.WriteAllText(exportFilePath, output);
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"Error processing asset '{assetName}'. {ex.Message}", ex);
                }
            }
        }

    }
}
