using System;
using System.Collections.Generic;
using SunistLibs.Core.Enums;
using SunistLibs.Core.Interface.ProcessSystem;

namespace SunistLibs.Core.Process
{
    public class ProcessController : BaseProcess
    {
        private List<IProcess> _runningProcesses;
        private List<IProcess> _queuingProcesses;
        private List<IProcess> _processesList;
        private ProcessManageAlgorithm _manageAlgorithm;
        private int _maxRunningCount;
        private IProcess _operatingProcess;

        private event ProcessManageOnRunHandler ProcessOnRun;
        private event ProcessManageOnQueueHandler ProcessOnQueue;
        private event ProcessManageOnKillHandler ProcessOnKill;
        private event ProcessManageOnUpdateHandler ProcessOnUpdate;
        
        public ProcessController(int id, string name, ProcessManageAlgorithm manageAlgorithm = ProcessManageAlgorithm.Fifo) 
            : base(-1, "ProcessController")
        {
            _manageAlgorithm = manageAlgorithm;
            switch (_manageAlgorithm)
            {
                case ProcessManageAlgorithm.Fifo:
                    ProcessOnRun += FifoOnRunning;
                    ProcessOnQueue += FifoFonQueuing;
                    ProcessOnKill += FifoOnKilling;
                    ProcessOnUpdate += FifoOnUpdate;
                    break;
            }
            
            
        }

        private void FifoOnUpdate(ref List<IProcess> runningprocesses, ref List<IProcess> queuingprocesses, int updateargument, int maxrunningcount)
        {
            for (int i = 0; i < runningprocesses.Count; ++i)
            {
                IProcess xEntry = runningprocesses[i];
                if (xEntry.MaxRunningTime != Int32.MaxValue && xEntry.RunningTime >= xEntry.MaxRunningTime && xEntry.Weight < 9)
                {
                    FifoOnKilling(ref runningprocesses, ref queuingprocesses, ref xEntry, maxrunningcount);
                }
            }
        }

        /// <summary>
        /// First in first out Process Manage Algorithm - OnKill
        /// </summary>
        /// <param name="runningprocesses">running process list</param>
        /// <param name="queuingprocesses">queuing process list</param>
        /// <param name="process">process which call run method</param>
        /// <param name="maxrunningcount">max running process count</param>
        /// <returns>process final status, blocked</returns>
        private ProcessStatus FifoOnKilling(ref List<IProcess> runningprocesses, ref List<IProcess> queuingprocesses, ref IProcess process, int maxrunningcount)
        {
            if (runningprocesses.Contains(process))
            {
                runningprocesses.Remove(process);
                if (queuingprocesses.Count > 0)
                {
                    queuingprocesses[0].Status = ProcessStatus.Running;
                    runningprocesses.Add(queuingprocesses[0]);
                    queuingprocesses.Remove(queuingprocesses[0]);
                    return ProcessStatus.Blocked;
                }
                else
                {
                    return ProcessStatus.Blocked;
                }
            }
            else if (queuingprocesses.Contains(process))
            {
                queuingprocesses.Remove(process);
                return ProcessStatus.Blocked;
            }
            else
            {
                return ProcessStatus.Blocked;
            }
        }

        /// <summary>
        /// First in first out Process Manage Algorithm - OnQueue
        /// </summary>
        /// <param name="runningprocesses">running process list</param>
        /// <param name="queuingprocesses">queuing process list</param>
        /// <param name="process">process which call run method</param>
        /// <param name="maxrunningcount">max running process count</param>
        /// <returns>process final status, queuing or running</returns>
        private ProcessStatus FifoFonQueuing(ref List<IProcess> runningprocesses, ref List<IProcess> queuingprocesses, ref IProcess process, int maxrunningcount)
        {
            if (runningprocesses.Contains(process))
            {
                return ProcessStatus.Running;
            }
            else
            {
                if (queuingprocesses.Contains(process))
                {
                    queuingprocesses.Remove(process);
                    queuingprocesses.Add(process);
                    return ProcessStatus.Running;
                }
                else
                {
                    queuingprocesses.Add(process);
                    return ProcessStatus.Running;
                }
            }
        }

        /// <summary>
        /// First in first out Process Manage Algorithm - OnRun
        /// </summary>
        /// <param name="runningprocesses">running process list</param>
        /// <param name="queuingprocesses">queuing process list</param>
        /// <param name="process">process which call run method</param>
        /// <param name="maxrunningcount">max running process count</param>
        /// <returns>process final status, queuing or running</returns>
        private ProcessStatus FifoOnRunning(ref List<IProcess> runningprocesses, ref List<IProcess> queuingprocesses, ref IProcess process, int maxrunningcount)
        {
            if (runningprocesses.Contains(process))
            {
                return ProcessStatus.Running;
            }
            else if (runningprocesses.Count >= maxrunningcount)
            {
                if (queuingprocesses.Contains(process))
                {
                    return ProcessStatus.Queuing;
                }
                else
                {
                    queuingprocesses.Add(process);
                    return ProcessStatus.Queuing;
                }
            }
            else
            {
                runningprocesses.Add(process);
                process.Status = ProcessStatus.Running;
                return ProcessStatus.Running;
            }
        }
    }
}