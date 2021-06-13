using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using SunistLibs.Core.Interface.BaseStructure;

namespace SunistLibs.Core.Memory
{
    public class BaseMemoryBlock : IMemoryBlock
    {
        private int _topIndex;
        private int _size;
        private List<byte[]> _contentList;
        private int _lastCall;
        private int _fatherProcessId;

        private int TopIndex => ++_topIndex;
        public int Size => _size;
        public List<byte[]> ContentList => _contentList;
        public int LastCall => _lastCall;
        public int FatherProcessId => _fatherProcessId;

        public byte[] SerializeObject(object obj)
        {
            if (obj is null) return null;
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);  
            byte[] bytes = ms.GetBuffer();
            return bytes;
        }

        public object DeserializeObject(byte[] memoryBytes)
        {
            object obj = null;
            if (memoryBytes is null) return obj;
            MemoryStream ms = new MemoryStream(memoryBytes);
            ms.Position = 0;
            BinaryFormatter formatter = new BinaryFormatter();
            obj = formatter.Deserialize(ms);   
            ms.Close();
            return obj;
        }

        public void Call()
        {
            _lastCall = 0;
        }

        public void UnCall()
        {
            _lastCall += 1;
        }

        public void SetFather(int processId)
        {
            _fatherProcessId = processId;
        }

        public int AddContent<T>(T data)
        {
            _contentList.Add(SerializeObject(data));
            _size += _contentList.Last().Length * sizeof(byte);
            return TopIndex;
        }

        public void SetContent<T>(int contentIndex, T data)
        {
            byte[] content = _contentList?[contentIndex];
            if (content is null) return;
            _size -= content.Length * sizeof(byte);
            _contentList[contentIndex] = SerializeObject(data);
            _size += _contentList[contentIndex].Length * sizeof(byte);
        }

        public T Get<T>(int contextIndex)
        {
            byte[] content = _contentList?[contextIndex];
            if (content is null) return default;
            return DeserializeObject(content) is T ? (T) DeserializeObject(content) : default;
        }

        public void RemoveContent(int contentIndex)
        {
            if (_contentList?[contentIndex] is null) return;
            _size -= _contentList[contentIndex].Length * sizeof(byte);
            _contentList[contentIndex] = null;
        }

        public void Clear()
        {
            for (int i = 0; i < _contentList.Count; ++i)
            {
                _contentList[i] = null;
            }

            _size = 0;
        }

        public BaseMemoryBlock()
        {
            _contentList = new List<byte[]>();
            _lastCall = 0;
            _topIndex = -1;
        }
    }
}