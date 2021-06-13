using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Timers;
using SunistLibs.Core.BaseControllers;
using SunistLibs.Core.Enums;
using SunistLibs.Core.Interface.BaseStructure;
using SunistLibs.Core.Interface.SystemController;
using SunistLibs.Core.Memory;

namespace SunistLibs.Core.Process
{
    public class ProcessController : BaseProcessController
    {
        public ProcessController(int maxThreading, ProcessManageAlgorithm algorithm) : base(maxThreading)
        {
            _memoryController = new MemoryController();
            switch (algorithm)
            {
                case ProcessManageAlgorithm.Fifo:
                    RunProcessHandler += ProcessManagementAlgorithm.FifoOnRunning;
                    QueueProcessHandler += ProcessManagementAlgorithm.FifoOnQueuing;
                    UpdateProcessHandler += ProcessManagementAlgorithm.FifoOnUpdate;
                    KillProcessHandler += ProcessManagementAlgorithm.FifoOnKilling;
                    break;
                case ProcessManageAlgorithm.RoundRobin:
                    RunProcessHandler += ProcessManagementAlgorithm.RoundRobinOnRunning;
                    QueueProcessHandler += ProcessManagementAlgorithm.RoundRobinOnQueuing;
                    UpdateProcessHandler += ProcessManagementAlgorithm.RoundRobinOnUpdate;
                    KillProcessHandler += ProcessManagementAlgorithm.RoundRobinOnKilling;
                    break;
                case ProcessManageAlgorithm.PriorityScheduling:
                    RunProcessHandler += ProcessManagementAlgorithm.PrioritySchedulingOnRunning;
                    QueueProcessHandler += ProcessManagementAlgorithm.PrioritySchedulingOnQueuing;
                    UpdateProcessHandler += ProcessManagementAlgorithm.PrioritySchedulingOnUpdate;
                    KillProcessHandler += ProcessManagementAlgorithm.PrioritySchedulingOnKilling;
                    break;
                default:
                    RunProcessHandler += ProcessManagementAlgorithm.FifoOnRunning;
                    QueueProcessHandler += ProcessManagementAlgorithm.FifoOnQueuing;
                    UpdateProcessHandler += ProcessManagementAlgorithm.FifoOnUpdate;
                    KillProcessHandler += ProcessManagementAlgorithm.FifoOnKilling;
                    break;
            }

            HangingProcessHandler += ProcessManagementAlgorithm.OnHanging;
            SyncProcessHandler += ProcessManagementAlgorithm.SyncProcessList;
        }

        // For algorithm switch recent, this class is not depend on override method to manage processes
        #region OverrideMethod

        public override ProcessStatus RunProcess(ref List<IProcess> runningProcess, ref List<IProcess> queuingProcess, ref IProcess process, int maxRunningCount)
        {
            throw new NotImplementedException();
        }

        public override ProcessStatus QueueProcess(ref List<IProcess> runningProcess, ref List<IProcess> queuingProcess, ref IProcess process, int maxRunningCount)
        {
            throw new NotImplementedException();
        }

        public override ProcessStatus KillProcess(ref List<IProcess> runningProcess, ref List<IProcess> queuingProcess, ref IProcess process, int maxRunningCount)
        {
            throw new NotImplementedException();
        }

        public override ProcessStatus HangingProcess(ref List<IProcess> runningProcess, ref List<IProcess> queuingProcess, ref IProcess process,
            int maxRunningCount)
        {
            throw new NotImplementedException();
        }

        public override ProcessStatus UpdateProcess(ref List<IProcess> runningProcess, ref List<IProcess> queuingProcess, int updateArgument, int maxRunningCount)
        {
            throw new NotImplementedException();
        }

        public override ProcessStatus SyncProcess(ref List<IProcess> runningProcess, ref List<IProcess> queuingProcess, ref List<IProcess> processesDictionary)
        {
            throw new NotImplementedException();
        }

        #endregion
        
    }
}