using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SunistLibs.Core.Memory
{
    public class MemoryBlock
    {
        private static byte[] SerializeObject(object obj)
        {
            if (obj == null)
                return null;
            //内存实例
            MemoryStream ms = new MemoryStream();
            //创建序列化的实例
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);//序列化对象，写入ms流中  
            byte[] bytes = ms.GetBuffer();
            return bytes;
        }

        private static object DeserializeObject(byte[] bytes)
        {
            object obj = null;
            if (bytes == null)
                return obj;
            //利用传来的byte[]创建一个内存流
            MemoryStream ms = new MemoryStream(bytes);
            ms.Position = 0;
            BinaryFormatter formatter = new BinaryFormatter();
            obj = formatter.Deserialize(ms); //把内存流反序列成对象  
            ms.Close();
            return obj;
        }
        
        private List<KeyValuePair<int, byte[]>> _memoryData;
        private int _topIndex;
        private int _size;

        public int Size => _size;

        public MemoryBlock()
        {
            _memoryData = new List<KeyValuePair<int, byte[]>>();
            _size = 0;
            _topIndex = -1;
        }

        private int GetIndex()
        {
            return ++_topIndex;
        }

        public int Add<T>(T data)
        {
            byte[] buff = SerializeObject(data);
            _memoryData.Add(new KeyValuePair<int, byte[]>(GetIndex(), buff));
            _size += sizeof(byte) * buff.Length;
            return _topIndex;
        }

        public void Set<T>(int index, T newData)
        {
            byte[] buff = SerializeObject(newData);
            _size -= sizeof(byte) * _memoryData[index].Value.Length;
            _memoryData[index] = new KeyValuePair<int, byte[]>(index, buff);
            _size += sizeof(byte) * buff.Length;
        }

        public T Get<T>(int index)
        {
            return (T) DeserializeObject(_memoryData[index].Value);
        }

        public void Clear()
        {
            _memoryData.Clear();
            _size = 0;
        }

        public void Remove(int index)
        {
            _size -= sizeof(byte) * _memoryData[index].Value.Length;
            _memoryData[index] = new KeyValuePair<int, byte[]>(index, new byte[0]);
        }
    }
}