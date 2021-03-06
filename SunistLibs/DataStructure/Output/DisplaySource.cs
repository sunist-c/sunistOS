using System.Collections.Generic;
using System.Data;

namespace SunistLibs.DataStructure.Output
{
    public class DisplaySource
    {
        private string _sourceName;
        private string[] _columnsName;
        private List<string[]> _rowData;
        private DataTable _dataTable;
        private string _statusDescription;

        public string SourceName
        {
            get => _sourceName;
        }

        public DataTable DataTable
        {
            get => _dataTable;
        }

        public string StatusDescription
        {
            get => _statusDescription;
        }

        public DisplaySource()
        {
            _columnsName = new string[1];
            _rowData = new List<string[]>();
            _dataTable = new DataTable();

            _sourceName = "General Source";
            _columnsName = new string[] {"Title"};
            _rowData = new List<string[]>();
            _rowData.Add(new string[] {"This is a data that ctor generated"});
            
            _dataTable.Columns.Add(_columnsName[0]);
            _dataTable.Rows.Add(_rowData[0]);
            _statusDescription = "This is a data that ctor generated";
        }

        public DisplaySource(string sourceName, string[] columnsName, List<string[]> rowData, 
            string description = "Exec Finished")
        {
            _columnsName = new string[1];
            _rowData = new List<string[]>();
            _dataTable = new DataTable();

            _statusDescription = description;
            _sourceName = sourceName;
            _columnsName = columnsName;
            _rowData = rowData;
            foreach (var s in columnsName)
            {
                _dataTable.Columns.Add(s);
            }

            foreach (var data in rowData)
            {
                _dataTable.Rows.Add(data);
            }
        }
        
        public DisplaySource(string sourceName, string[] columnsName, string[] rowData)
        {
            _columnsName = new string[1];
            _rowData = new List<string[]>();
            _dataTable = new DataTable();
            
            _sourceName = sourceName;
            _columnsName = columnsName;
            _rowData = new List<string[]>();
            _rowData.Add(rowData);
            
            foreach (var s in columnsName)
            {
                _dataTable.Columns.Add(s);
            }
            _dataTable.Rows.Add(rowData);
        }
    }
}