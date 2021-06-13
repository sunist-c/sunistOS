using System.Collections.Generic;
using SunistLibs.Core.Enums;
using SunistLibs.Core.Interface.BaseStructure;
using SunistLibs.Core.Process;

namespace SunistLibs.Core.Interface.SystemController
{
    public interface IProcessController : ISystemController
    {
        List<IProcess> RunningProcesses { get; }
        List<IProcess> QueuingProcesses { get; }
        List<IProcess> ProcessesDictionary { get; }
        IMemoryController MemoryController { get; }
        int ProcessCount { get; }
        int MaxThreading { get; set; }

        int Create(string processName, ProcessOnRun mainMethod);
        int Create(string processName, BaseProcess process);
        ProcessStatus Queue(int processId);
        ProcessStatus Queue(IProcess process);
        ProcessStatus Run(int processId);
        ProcessStatus Run(IProcess process);
        ProcessStatus Kill(int processId);
        ProcessStatus Kill(IProcess process);
        List<IProcess> List(params ProcessStatus[] listedStatuses);
        IProcess Find(int processId);
        List<IProcess> Find(string processName);

        event ProcessManageOnRunHandler RunProcessHandler;
        event ProcessManageOnQueueHandler QueueProcessHandler;
        event ProcessManageOnKillHandler KillProcessHandler;
        event ProcessManageOnHangingHandler HangingProcessHandler;
        event ProcessManageOnUpdateHandler UpdateProcessHandler;
        event ProcessManageOnSyncHandler SyncProcessHandler;
    }
}