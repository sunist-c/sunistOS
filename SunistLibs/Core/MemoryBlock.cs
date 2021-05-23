using System;
using System.Collections;

namespace SunistLibs.Core
{
    public class MemoryBlock
    {
        private Hashtable _data;

        public MemoryBlock()
        {
            _data = new Hashtable();
            _data.Add("size", (ulong)0);
        }

        public void AddData(Object key, Object data)
        {
            _data.Add(key, data);
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