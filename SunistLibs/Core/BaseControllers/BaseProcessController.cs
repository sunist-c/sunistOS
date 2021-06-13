using System;
using System.Collections.Generic;
using System.Timers;
using SunistLibs.Core.Enums;
using SunistLibs.Core.EventSystem;
using SunistLibs.Core.Interface.BaseStructure;
using SunistLibs.Core.Interface.MessageQueue;
using SunistLibs.Core.Interface.SystemController;
using SunistLibs.Core.Message;
using SunistLibs.Core.Process;

namespace SunistLibs.Core.BaseControllers
{
    public abstract class BaseProcessController : IProcessController
    {
        #region Fields

        private List<IProcess> _runningProcesses;
        private List<IProcess> _queuingProcesses;
        private List<IProcess> _processesDictionary;
        protected IMemoryController _memoryController;
        private int _topIndex;

        #endregion
        
        #region Attributes

        public bool Logable { get; set; }
        public bool Contactable { get; set; }
        public List<IProcess> RunningProcesses => _runningProcesses;
        public List<IProcess> QueuingProcesses => _queuingProcesses;
        public List<IProcess> ProcessesDictionary => _processesDictionary;
        public IMemoryController MemoryController => _memoryController;
        public int ProcessCount => ProcessesDictionary.Count;
        public int MaxThreading { get; set; }
        private int TopIndex => ++_topIndex;

        #endregion

        #region Constructor

        public BaseProcessController(int maxThreading)
        {
            Logable = true;
            Contactable = true;
            _runningProcesses = new List<IProcess>();
            _queuingProcesses = new List<IProcess>();
            _processesDictionary = new List<IProcess>();
            MaxThreading = maxThreading;
            _topIndex = -1;
            
            Timer t = new Timer(100);
            t.Elapsed += (sender, args) =>
            {
                UpdateProcessHandler(ref _runningProcesses, ref _queuingProcesses, 1000, MaxThreading);
                SyncProcessHandler(ref _runningProcesses, ref _queuingProcesses, ref _processesDictionary);
                MemoryController.UpdateContext();
            };
            t.Enabled = true;
            t.AutoReset = true; 
            t.Start();
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
            // todo: Complete SendMessageHandler
            throw new System.NotImplementedException();
        }

        public void ReceiveMessageHandler(IMessage message)
        {
            // todo: Complete SendMessageHandler
            throw new System.NotImplementedException();
        }
        
        public int Create(string processName, ProcessOnRun mainMethod)
        {
            IProcess process = new BasicProcess()
            {
                ID = TopIndex,
                MaxRunningTime = Int32.MaxValue,
                Name = processName,
                Status = ProcessStatus.Ready
            };
            process.OnRun += mainMethod;
            ProcessesDictionary.Add(process);
            return process.ID;
        }

        public int Create(string processName, BaseProcess process)
        {
            process.Name = processName;
            process.ID = TopIndex;
            process.Status = ProcessStatus.Ready;
            ProcessesDictionary.Add(process);
            return process.ID;
        }

        public ProcessStatus Queue(int processId)
        {
            if (ProcessesDictionary?[processId] != null)
            {
                IProcess p = ProcessesDictionary[processId];
                QueueProcessHandler(ref _runningProcesses, ref _queuingProcesses, ref p, MaxThreading);
                _processesDictionary[processId] = p;
                return p.Status;
            }
            else
            {
                return ProcessStatus.Blocked;
            }
        }

        public ProcessStatus Queue(IProcess process)
        {
            if (ProcessesDictionary.Contains(process))
            {
                QueueProcessHandler(ref _runningProcesses, ref _queuingProcesses, ref process, MaxThreading);
                _processesDictionary[process.ID] = process;
                return process.Status;
            }
            else
            {
                return ProcessStatus.Blocked;
            }
        }

        public ProcessStatus Run(int processId)
        {
            if (ProcessesDictionary?[processId] != null)
            {
                IProcess p = ProcessesDictionary[processId];
                RunProcessHandler(ref _runningProcesses, ref _queuingProcesses, ref p, MaxThreading);
                _processesDictionary[processId] = p;
                return p.Status;
            }
            else
            {
                return ProcessStatus.Blocked;
            }
        }

        public ProcessStatus Run(IProcess process)
        {
            if (ProcessesDictionary.Contains(process))
            {
                RunProcessHandler(ref _runningProcesses, ref _queuingProcesses, ref process, MaxThreading);
                _processesDictionary[process.ID] = process;
                return process.Status;
            }
            else
            {
                return ProcessStatus.Blocked;
            }
        }

        public ProcessStatus Kill(int processId)
        {
            if (ProcessesDictionary?[processId] != null)
            {
                IProcess p = ProcessesDictionary[processId];
                KillProcessHandler(ref _runningProcesses, ref _queuingProcesses, ref p, MaxThreading);
                _processesDictionary[processId] = p;
                return p.Status;
            }
            else
            {
                return ProcessStatus.Blocked;
            }
        }

        public ProcessStatus Kill(IProcess process)
        {
            if (ProcessesDictionary.Contains(process))
            {
                KillProcessHandler(ref _runningProcesses, ref _queuingProcesses, ref process, MaxThreading);
                _processesDictionary[process.ID] = process;
                return process.Status;
            }
            else
            {
                return ProcessStatus.Blocked;
            }
        }

        public List<IProcess> List(params ProcessStatus[] listedStatuses)
        {
            if (listedStatuses.Length == 0)
            {
                return ProcessesDictionary;
            }
            else
            {
                List<IProcess> list = new List<IProcess>();
                foreach (IProcess process in ProcessesDictionary)
                {
                    foreach (ProcessStatus listedStatus in listedStatuses)
                    {
                        if (process.Status == listedStatus)
                        {
                            list.Add(process);
                        }
                    }
                }

                return list;
            }
        }

        public IProcess Find(int processId)
        {
            if (ProcessesDictionary?[processId] != null)
            {
                return ProcessesDictionary[processId];
            }
            else
            {
                return null;
            }
        }

        public List<IProcess> Find(string processName)
        {
            List<IProcess> list = new List<IProcess>();
            foreach (IProcess process in ProcessesDictionary)
            {
                if (process.Name == processName)
                {
                    list.Add(process);
                }
            }

            return list;
        }
        
        #endregion

        #region ControllerMessagableEvent
        
        public event ConsoleLogHandler Log;
        public event SendMessageHandler SendMessage;
        public event RecieveMessageHandler ReceiveMessage;

        #endregion
        
        #region ProcessManagementEvent

        public event ProcessManageOnRunHandler RunProcessHandler;
        public event ProcessManageOnQueueHandler QueueProcessHandler;
        public event ProcessManageOnKillHandler KillProcessHandler;
        public event ProcessManageOnHangingHandler HangingProcessHandler;
        public event ProcessManageOnUpdateHandler UpdateProcessHandler;
        public event ProcessManageOnSyncHandler SyncProcessHandler;

        #endregion

        #region OverrideMethods

        public abstract ProcessStatus RunProcess(
            ref List<IProcess> runningProcess,
            ref List<IProcess> queuingProcess,
            ref IProcess process, int maxRunningCount);
        
        public abstract ProcessStatus QueueProcess(
            ref List<IProcess> runningProcess,
            ref List<IProcess> queuingProcess,
            ref IProcess process, int maxRunningCount);
        
        public abstract ProcessStatus KillProcess(
            ref List<IProcess> runningProcess,
            ref List<IProcess> queuingProcess,
            ref IProcess process, int maxRunningCount);
        
        public abstract ProcessStatus HangingProcess(
            ref List<IProcess> runningProcess,
            ref List<IProcess> queuingProcess,
            ref IProcess process, int maxRunningCount);
        
        public abstract ProcessStatus UpdateProcess(
            ref List<IProcess> runningProcess,
            ref List<IProcess> queuingProcess,
            int updateArgument, int maxRunningCount);
        
        public abstract ProcessStatus SyncProcess(
            ref List<IProcess> runningProcess,
            ref List<IProcess> queuingProcess,
            ref List<IProcess> processesDictionary);

        #endregion
    }
}