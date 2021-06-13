using System.Collections.Generic;
using SunistLibs.Core.Interface.BaseStructure;

namespace SunistLibs.Core.Memory
{
    public delegate int OnAddHandler(
        IMemoryBlock staticData, int fatherProcessId,
        ref List<IMemoryBlock> cacheList, 
        ref List<IMemoryBlock> swapList,
        int maxPaging);

    public delegate int OnReplaceHandler(
        IMemoryBlock newData, int fatherProcessId,
        ref List<IMemoryBlock> cacheList,
        ref List<IMemoryBlock> swapList,
        int maxPaging);

    public delegate IMemoryBlock OnCallHandler(
        int memoryBlockIndex, int fatherProcessId,
        ref List<IMemoryBlock> cacheList,
        ref List<IMemoryBlock> swapList);

    public delegate void OnUpdateHandler(ref List<IMemoryBlock> cacheList);

    public delegate bool OnChangeHandler(
        MemoryBlock newData, int fatherProcessId,
        ref List<IMemoryBlock> cacheList,
        ref List<IMemoryBlock> swapList);
}