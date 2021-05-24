using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SunistShell.ViewModel.Index
{
    public class IndexStatus : INotifyPropertyChanged
    {
        private string _pd;
        private string _sd;
        private uint _pv;
        private bool _rm;
        private string[] _his;
        private List<KeyValuePair<string, DataTable>> _hs;

        public List<KeyValuePair<string, DataTable>> HistorySource
        {
            get => _hs;
            set
            {
                _hs = value;
                OnPropertyChanged("HistorySource");
            }
        }
        
        public string[] History
        {
            get => _his;
            set
            {
                _his = value;
                OnPropertyChanged("History");
            }
        }
        
        public string ProgressDescription 
        {
            get
            {
                return _pd;
            }

            set
            {
                _pd = value;
                OnPropertyChanged("ProgressDescription");
            }
        }

        public uint ProgressValue
        {
            get
            {
                return _pv;
            }

            set
            {
                _pv = value;
                OnPropertyChanged("ProgressValue");
            }
        }

        public string StatusDescription
        {
            get
            {
                return _sd;
            }

            set
            {
                _sd = value;
                OnPropertyChanged("StatusDescription");
            }
        }

        public bool RootMode
        {
            get
            {
                return _rm;
            }

            set
            {
                _rm = value;
                OnPropertyChanged("RootMode");
            }
        }

        public IndexStatus()
        {
            ProgressDescription = "就绪";
            ProgressValue = 0;
            StatusDescription = "欢迎使用SunistOS";
            History = new string[5];
            HistorySource = new List<KeyValuePair<string, DataTable>>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
