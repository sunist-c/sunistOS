using System;
using System.Collections.Generic;
using SunistLibs.Core.Enums;
using SunistLibs.Core.Interface.BaseStructure;

namespace SunistLibs.Core.Memory
{
    public static class MemoryReplacementAlgorithm
    {
        private static int FindLongestUnCallMemoryBlock(List<IMemoryBlock> memoryBlocks)
        {
            IMemoryBlock unCallingBlock = memoryBlocks[0];
            int index = 0, i = 0;
            foreach (IMemoryBlock m in memoryBlocks)
            {
                if (unCallingBlock.LastCall < m.LastCall)
                {
                    unCallingBlock = m;
                    index = i;
                }
                i++;
            }
            return index;
        }

        private static bool MemoryBlockUnique(
            List<IMemoryBlock> cacheList, 
            List<IMemoryBlock> swapList,
            int fatherProcessId)
        {
            foreach (IMemoryBlock m in cacheList)
                if (m.FatherProcessId == fatherProcessId)
                    return false;
            foreach (IMemoryBlock m in swapList)
                if (m.FatherProcessId == fatherProcessId)
                    return false;
            return true;
        }

        public static void OnUpdateContext(ref List<IMemoryBlock> cacheList)
        {
            for (int i = 0; i < cacheList.Count; ++i) 
                cacheList[i].UnCall();
        }

        public static bool OnChangeContext(
            MemoryBlock newData, int fatherProcessId,
            ref List<IMemoryBlock> cacheList,
            ref List<IMemoryBlock> swapList)
        {
            if (fatherProcessId == cacheList[fatherProcessId].FatherProcessId)
            {
                cacheList[fatherProcessId] = newData;
                cacheList[fatherProcessId].Call();
                return true;
            }
            else
            {
                for (int i = 0; i < swapList.Count; ++i)
                {
                    if (swapList[i].FatherProcessId == fatherProcessId)
                    {
                        swapList[i] = newData;
                        return true;
                    }
                }
            }

            return false;
        }
        
        public static IMemoryBlock FifoOnCallContext(
            int memoryBlockIndex, int fatherProcessId,
            ref List<IMemoryBlock> cacheList,
            ref List<IMemoryBlock> swapList)
        {
            if (fatherProcessId == cacheList[memoryBlockIndex].FatherProcessId)
            {
                cacheList[memoryBlockIndex].Call();
                return cacheList[memoryBlockIndex];
            }
            else return null;
        }

        public static int FifoOnReplaceContext(
            IMemoryBlock newData, int fatherProcessId,
            ref List<IMemoryBlock> cacheList,
            ref List<IMemoryBlock> swapList,
            int maxPaging)
        {
            // find longest uncalled memory block and add it to swap list 
            int m = FindLongestUnCallMemoryBlock(cacheList);

            // Console.WriteLine($"FIND {m} is longest, {cacheList[m].LastCall}");
            swapList.Add(cacheList[m]);
            
            // replace newData to swapped data
            cacheList[m] = newData;
            return m;
        }
        
        public static int FifoOnAddContext(
            IMemoryBlock staticData, int fatherProcessId,
            ref List<IMemoryBlock> cacheList, 
            ref List<IMemoryBlock> swapList,
            int maxPaging)
        {
            // if cacheList is addable and context is unique, add context
            if (cacheList.Count < maxPaging)
            {
                cacheList.Add(staticData);
                return cacheList.Count - 1;
            }
            else if (!MemoryBlockUnique(cacheList, swapList, fatherProcessId))
                return -1;

            // is cacheList is up to limit, start replacement
            else
                return FifoOnReplaceContext(staticData, fatherProcessId, ref cacheList, ref swapList, maxPaging);
        }
    }
}