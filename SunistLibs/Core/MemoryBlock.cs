using System;
using System.Collections;
using SunistLibs.Core.Enums;

namespace SunistLibs.Core
{
    public class MemoryBlock
    {
        private Hashtable _data;
        private MemoryStatus _status;

        public MemoryStatus Status
        {
            get => _status;
            set => _status = value;
        }

        public MemoryBlock()
        {
            _data = new Hashtable();
            _data.Add("size", (ulong)0);
        }

        public bool Alloc(ulong size, Object key, Object value)
        {
            _data.Add(key, value);
            _data["size"] = Size() + size;
            return true;
        }

        public TValueType GetData<TKeyType, TValueType>(TKeyType key)
        {
            return (TValueType)_data[key];
        }

        public ulong Size()
        {
            return _data["size"] as ulong? ?? 0;
        }
    }
}