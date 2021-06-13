using System;
using System.Data;
using System.IO;
using System.Text;

namespace SunistLibs.Tools.IO
{
    public class CsvHelper
    {
        private DataTable _data;
        private bool _hasColumnsName;

        public DataTable Data => _data;
        public bool HasColumnsName => _hasColumnsName;
        

        public void Load(string path, bool hasColumnsName = false, Encoding encoding = null)
        {
            _hasColumnsName = hasColumnsName;
            _data = LoadCsv(path, hasColumnsName, encoding);
        }

        public void ConsoleDisplay()
        {
            string buffer = "";
            if (HasColumnsName)
            {
                Console.WriteLine("Head");
                for (int i = 0; i < _data.Columns.Count; ++i)
                    buffer += $"{_data.Columns[i]}\t";
                Console.WriteLine(buffer);
            }
            else
            {
                for (int i = 0; i < _data.Rows.Count; ++i)
                {
                    buffer = "";
                    for (int j = 0; j < _data.Columns.Count; ++j)
                    {
                        buffer += $"{_data.Rows[i][j]}\t";
                    }

                    Console.WriteLine(buffer);
                }
            }
        }

        public static DataTable LoadCsv(string path, bool hasColumnsName = false, Encoding encoding = null)
        {
            DataTable dt = new DataTable();

            if (encoding is null) encoding = Encoding.Default;
            FileStream fs = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(fs, encoding);

            string buffer = "";
            bool IsFirst = true;
            while (!((buffer = sr.ReadLine()) is null))
            {
                string[] bufferArr = buffer.Split(',');
                if (IsFirst)
                {
                    if (hasColumnsName)
                        foreach (string s in bufferArr)
                            dt.Columns.Add(s);
                    else
                    {
                        foreach (string s in bufferArr) dt.Columns.Add();
                        dt.Rows.Add(bufferArr);
                    }

                    IsFirst = false;
                }
                else dt.Rows.Add(bufferArr);
            }

            return dt;
        }
    }
}