using System.Collections.Generic;
using SunistLibs.Core.Enums;
using SunistLibs.Core.Interface.BaseStructure;
using SunistLibs.Core.Memory;

namespace SunistLibs.Core.Interface.SystemController
{
    public interface IMemoryController : ISystemController
    {
        List<IMemoryBlock> CacheList { get; }
        List<IMemoryBlock> SwapList { get; }
        int MaxPaging { get; set; }

        int AddContext(IProcess process, MemoryBlock staticData);
        IMemoryBlock CallContext(IProcess process);
        bool ChangeContext(IProcess process, MemoryBlock newContext);
        void UpdateContext();
        List<int> List();

        event OnAddHandler AddContextHandler;
        event OnCallHandler CallContextHandler;
        event OnReplaceHandler ReplaceContextHandler;
        event OnChangeHandler ChangeContextHandler;
        event OnUpdateHandler UpdateContextHandler;
    }
}