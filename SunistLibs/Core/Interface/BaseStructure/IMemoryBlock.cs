using System;
using System.Collections.Generic;

namespace SunistLibs.Core.Interface.BaseStructure
{
    public interface IMemoryBlock
    {
        int Size { get; }
        List<Byte[]> ContentList { get; }
        int LastCall { get; }
        int FatherProcessId { get; }
        
        Byte[] SerializeObject(Object obj);
        Object DeserializeObject(Byte[] memoryBytes);
        void Call();
        void UnCall();
        void SetFather(int processId);
        int AddContent<T>(T data);
        void SetContent<T>(int contentIndex, T data);
        T Get<T>(int contextIndex);
        void RemoveContent(int contentIndex);
        void Clear();
    }
}