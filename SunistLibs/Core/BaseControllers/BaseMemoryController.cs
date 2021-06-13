using System;
using System.Collections.Generic;
using SunistLibs.Core.Enums;
using SunistLibs.Core.EventSystem;
using SunistLibs.Core.Interface.BaseStructure;
using SunistLibs.Core.Interface.MessageQueue;
using SunistLibs.Core.Interface.SystemController;
using SunistLibs.Core.Memory;
using SunistLibs.Core.Message;

namespace SunistLibs.Core.BaseControllers
{
    public abstract class BaseMemoryController : IMemoryController
    {
        #region Fields

        protected List<IMemoryBlock> _cacheList;
        protected List<IMemoryBlock> _swapList;

        #endregion

        #region Attributes

        public bool Logable { get; set; }
        public bool Contactable { get; set; }
        public List<IMemoryBlock> CacheList => _cacheList;
        public List<IMemoryBlock> SwapList => _swapList;
        public int MaxPaging { get; set; }

        #endregion

        #region Constructor

        public BaseMemoryController(int maxPaging = 10)
        {
            Logable = true;
            Contactable = true;
            _cacheList = new List<IMemoryBlock>();
            _swapList = new List<IMemoryBlock>();
            MaxPaging = maxPaging;
            Log += LogHandler;
        }

        #endregion
        
        #region InterfaceImplement
        
        public void LogHandler(string[] logData)
        {
            if (Logable)
                foreach (string s in logData)
                    Console.WriteLine(s);
        }

        public void SendMessageHandler(IMessage message)
        {
            throw new System.NotImplementedException();
        }

        public void ReceiveMessageHandler(IMessage message)
        {
            throw new System.NotImplementedException();
        }

        public int AddContext(IProcess process, MemoryBlock staticData)
        {
            int p = AddContextHandler(staticData, process.ID, ref _cacheList, ref _swapList, MaxPaging);
            Log(new[] {$"Process {process.Name}, ID {process.ID} has added a context as index {p}"});
            return p;
        }

        public IMemoryBlock CallContext(IProcess process)
        {
            for (int i = 0; i < _cacheList.Count; ++i)
            {
                if (_cacheList[i].FatherProcessId == process.ID)
                {
                    Log(new[] {$"Process {process.Name}, ID {process.ID} is calling context with index {i}"});
                    return CallContextHandler(i, process.ID, ref _cacheList, ref _swapList);
                }
            }

            return null;
        }

        public bool ChangeContext(IProcess process, MemoryBlock newContext)
        {
            if (ChangeContextHandler(newContext, process.ID, ref _cacheList, ref _swapList))
            {
                Log(new[] {$"Process {process.Name}, ID {process.ID} has changed context"});
                return true;
            }

            return false;
        }

        public void UpdateContext()
        {
            UpdateContextHandler(ref _cacheList);
        }

        public List<int> List()
        {
            List<int> list = new List<int>();
            foreach (IMemoryBlock memoryBlock in _cacheList)
                list.Add(memoryBlock.FatherProcessId);
            return list;
        }

        #endregion

        #region ControllerMessagabeEvent

        public event ConsoleLogHandler Log;
        public event SendMessageHandler SendMessage;
        public event RecieveMessageHandler ReceiveMessage;

        #endregion

        #region MemoryReplacementEvent

        public event OnAddHandler AddContextHandler;
        public event OnCallHandler CallContextHandler;
        public event OnReplaceHandler ReplaceContextHandler;
        public event OnChangeHandler ChangeContextHandler;
        public event OnUpdateHandler UpdateContextHandler;

        #endregion

        #region OverrideMethods

        public abstract int ContextOnAdd(
            IMemoryBlock staticData, int fatherProcessId,
            ref List<IMemoryBlock> cacheList, 
            ref List<IMemoryBlock> swapList,
            int maxPaging);

        public abstract int ContextOnReplaced(
            IMemoryBlock newData, int fatherProcessId,
            ref List<IMemoryBlock> cacheList,
            ref List<IMemoryBlock> swapList,
            int maxPaging);

        public abstract IMemoryBlock ContextOnCall(
            int memoryBlockIndex, int fatherProcessId,
            ref List<IMemoryBlock> cacheList,
            ref List<IMemoryBlock> swapList);

        public abstract void ContextOnUpdate(ref List<IMemoryBlock> cacheList);

        public abstract bool ContextOnChange(
            MemoryBlock newData, int fatherProcessId,
            ref List<IMemoryBlock> cacheList,
            ref List<IMemoryBlock> swapList);

        #endregion

    }
}