using System.Collections.Generic;
using SunistLibs.Core.BaseControllers;
using SunistLibs.Core.Enums;
using SunistLibs.Core.Interface.BaseStructure;

namespace SunistLibs.Core.Memory
{
    public class MemoryController : BaseMemoryController
    {
        public MemoryController(int maxPaging = 10) : base(maxPaging)
        {
            AddContextHandler += MemoryReplacementAlgorithm.FifoOnAddContext;
            CallContextHandler += MemoryReplacementAlgorithm.FifoOnCallContext;
            ChangeContextHandler += MemoryReplacementAlgorithm.OnChangeContext;
            ReplaceContextHandler += MemoryReplacementAlgorithm.FifoOnReplaceContext;
            UpdateContextHandler += MemoryReplacementAlgorithm.OnUpdateContext;
        }

        // for designed reasons, the override methods dose not work
        public override int ContextOnAdd(IMemoryBlock staticData, int fatherProcessId, ref List<IMemoryBlock> cacheList, ref List<IMemoryBlock> swapList, int maxPaging)
        {
            throw new System.NotImplementedException();
        }

        public override int ContextOnReplaced(IMemoryBlock newData, int fatherProcessId, ref List<IMemoryBlock> cacheList, ref List<IMemoryBlock> swapList, int maxPaging)
        {
            throw new System.NotImplementedException();
        }

        public override IMemoryBlock ContextOnCall(int memoryBlockIndex, int fatherProcessId, ref List<IMemoryBlock> cacheList, ref List<IMemoryBlock> swapList)
        {
            throw new System.NotImplementedException();
        }

        public override void ContextOnUpdate(ref List<IMemoryBlock> cacheList)
        {
            throw new System.NotImplementedException();
        }

        public override bool ContextOnChange(MemoryBlock newData, int fatherProcessId, ref List<IMemoryBlock> cacheList, ref List<IMemoryBlock> swapList)
        {
            throw new System.NotImplementedException();
        }
    }
}