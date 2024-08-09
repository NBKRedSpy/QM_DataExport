using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QM_DataExport
{

    public enum LineType
    {
        Invalid = 0,
        TableName,
        TableHeader, 
        TableEnd,
        Data,
        /// <summary>
        /// The empty lines between tables.
        /// </summary>
        Whitespace
    }
    /// <summary>
    /// Parses the game's config file format which contains multiple tables that are demarked by #[table name] and #end
    /// </summary>
    internal class GameTableParser : IEnumerator<string>
    {
        private List<string> Data { get; set; }

        public bool IsInTable { get; private set;  }
        
        /// <summary>
        /// The name of the current table.
        /// </summary>
        public string TableName { get; private set; }

        /// <summary>
        /// Text for the current line.
        /// </summary>
        private string CurrentLine { get; set; }

        /// <summary>
        /// The line type for the current line.
        /// </summary>
        public LineType LineType { get; set; }

        /// <summary>
        /// Current line index
        /// </summary>
        private int Index { get; set; }

        public GameTableParser(List<string> data)
        {
            Data = data;
            Index = -1;
        }

        public string Current => CurrentLine;

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            try
            {

                LineType previousLineType = LineType;
                LineType = LineType.Invalid;

                ++Index;
                if (Index >= Data.Count)
                {
                    CurrentLine = string.Empty;
                    return false;
                }

                CurrentLine = Data[Index];

                if (CurrentLine.StartsWith("#end"))
                {
                    if (!IsInTable)
                    {
                        throw new ApplicationException($"Found table end without being in a table");
                    }

                    IsInTable = false;
                    LineType = LineType.TableEnd;
                }
                else if (CurrentLine.StartsWith("#"))
                {
                    if (IsInTable)
                    {
                        throw new ApplicationException($"Found start of table while already in a table.  Table Name '{TableName}'");
                    }

                    LineType = LineType.TableName;
                    IsInTable = true;
                    TableName = CurrentLine;
                }

                if (LineType == LineType.Invalid)
                {
                    if (previousLineType == LineType.TableName)
                    {
                        //Assume this is the header line.
                        LineType = LineType.TableHeader;
                    }
                    else 
                    {
                        //Just assume whitespace if out of a table.
                        LineType = IsInTable ? LineType.Data : LineType.Whitespace;
                    }
                }

                return true;

            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error Parsing line number {Index + 1}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Returns the text of the first column.
        /// </summary>
        /// <returns></returns>
        public string FirstColumn()
        {
            return CurrentLine.Split('\t')[0];
        }

        public void Reset()
        {
            Index = -1;
        }

        public void Dispose()
        {
            return;
        }
    }
}
