using System;
using System.Collections;
using System.Collections.Generic;
using SunistLibs.Core;
using SunistLibs.Core.Delegate;
using SunistLibs.Core.Enums;
using SunistLibs.DataStructure.Interfaces;
using SunistLibs.DataStructure.Output;

namespace SunistLibs
{
    public class MemoryController : IDisplayable
    {
        private Hashtable _memoryList;

        public MemoryController()
        {
            _memoryList = new Hashtable();
        }

        public MemoryStatus Cache(ulong processId, MemoryBlock memoryBlock)
        {
            return MemoryStatus.Caching;
        }

        public MemoryBlock Replace(ulong processId)
        {
            if (!_memoryList.Contains(processId))
            {
                // todo: 完成异常状态：找不到进程
                throw new Exception();
            }
            else
            {
                return _memoryList[processId] as MemoryBlock;
            }
        }

        public void List(params ulong[] processId)
        {
            
        }

        public void List()
        {
            List<string[]> memoryData = new List<string[]>();
            foreach (DictionaryEntry xEntry in _memoryList)
            {
                string[] record = new string[3];
                record[0] = (xEntry.Key is ulong ? (ulong) xEntry.Key : 0).ToString();
                record[1] = ((MemoryBlock) xEntry.Value).Status.ToString();
                record[2] = ((MemoryBlock) xEntry.Value).Size().ToString();
                
                memoryData.Add(record);
            }
            
            Display(new DisplaySource(
                "MemoryList",
                new string[] {"ProcessId", "MemoryStatus", "MemorySize"},
                memoryData
            ), DisplayMode.All);
        }
        
        public DisplaySource DisplayInfo(Display displayMethod, params object[] args)
        {
            return displayMethod(args);
        }

        public event DisplayEventHandler Display;
    }
}