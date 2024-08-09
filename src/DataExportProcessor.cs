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


                    string exportFilePath = Path.Combine(exportDirectory, assetName + ".txt");

                    string output;

                    if (Plugin.Config.IncludeLocalizationText)
                    {
                        output = TranslateTables(obj.text.Split('\r', '\n').ToList());
                    }
                    else
                    {
                        output = obj.text;
                    }

                    File.WriteAllText(exportFilePath, output);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"Error processing asset '{assetName}'. {ex.Message}", ex);
                }
            }
        }

        private string TranslateTables(List<string> source)
        {
            int lineNumber = 0;     //one based line number.

            try
            {
                StringBuilder sb = new StringBuilder();
                GameTableParser parser = new GameTableParser(source);

                bool hasIdColumn = false;

                while (parser.MoveNext())
                {
                    lineNumber++;
                    string line = parser.Current;

                    switch (parser.LineType)
                    {
                        case LineType.TableHeader:
                            hasIdColumn = parser.FirstColumn().StartsWith("Id");
                            sb.AppendLine("LocalText\t" + line);
                            break;
                        case LineType.Data:
                            if (hasIdColumn)
                            {
                                //Will return the original text or blank if a localization key is not found.
                                sb.Append(Localization.Get(parser.FirstColumn())); 
                                sb.Append('\t');
                                sb.AppendLine(line);
                            }
                            else
                            {
                                sb.AppendLine(line);
                            }
                            break;

                        case LineType.TableName:
                        case LineType.TableEnd:
                        case LineType.Whitespace:
                            sb.AppendLine(line);
                            break;
                        default:
                            throw new ApplicationException($"Unexpected line type '{parser.LineType}'");
                    }
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Processing Error on line {lineNumber}");
            }
        }
    }
}
