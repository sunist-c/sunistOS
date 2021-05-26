using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SunistLibs.Core.Enums;
using SunistLibs.Core.Interface.ProcessSystem;

namespace SunistLibs.Core.Process
{
    #region ProcessActionDelegate

    public delegate void ProcessOnRun(params Object[] args);
    
    public delegate void ProcessOnAbort();

    #endregion

    #region ProcessManageDelegate
    
    public delegate ProcessStatus ProcessManageOnRunHandler(
        ref List<IProcess> runningProcesses,
        ref List<IProcess> queuingProcesses,
        ref IProcess process,
        int maxRunningCount
    );
    
    public delegate ProcessStatus ProcessManageOnQueueHandler(
        ref List<IProcess> runningProcesses,
        ref List<IProcess> queuingProcesses,
        ref IProcess process,
        int maxRunningCount
    );

    public delegate ProcessStatus ProcessManageOnKillHandler(
        ref List<IProcess> runningProcesses,
        ref List<IProcess> queuingProcesses,
        ref IProcess process,
        int maxRunningCount
    );

    public delegate void ProcessManageOnUpdateHandler(
        ref List<IProcess> runningProcesses,
        ref List<IProcess> queuingProcesses,
        int updateArgument,
        int maxRunningCount
    );
    
    #endregion
}