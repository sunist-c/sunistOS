using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Timers;
using SunistLibs.Core.Enums;
using SunistLibs.Core.Interface.ProcessSystem;
using SunistLibs.Core.Interface.SystemController;
using SunistLibs.Core.Memory;

namespace SunistLibs.Core.Process
{
    public class ProcessController : BaseProcess, ISystemController
    {
        private Hashtable indexs;
        private List<IProcess> RunningProcesses;
        private List<IProcess> QueuingProcesses;
        private List<IProcess> ProcessesList;
        private ProcessManageAlgorithm _manageAlgorithm;
        private int MaxRunningCount;
        private Timer Timer;
        private int _processIndex;
        private int ProcessIndex
        {
            get => ++_processIndex;
        }
        public int Size
        {
            get => RunningProcesses.Count;
        }

        #region ProcessManageEvent

        private event ProcessManageOnRunHandler ProcessOnRun;
        private event ProcessManageOnQueueHandler ProcessOnQueue;
        private event ProcessManageOnKillHandler ProcessOnKill;
        private event ProcessManageOnUpdateHandler ProcessOnUpdate;

        #endregion

        #region InterfaceImplementations

        public int Create(string ProcessName)
        {
            var p = new BasicProcess()
            {
                Name = ProcessName,
                ID = ProcessIndex,
                MaxRunningTime = Int32.MaxValue,
                Status = ProcessStatus.Ready
            };
            ProcessesList.Add(p);
            return p.ID;
        }
        
        public int Create(string ProcessName, BaseProcess process)
        {
            process.Name = ProcessName;
            process.ID = ProcessIndex;
            process.MaxRunningTime = Int32.MaxValue;
            process.Status = ProcessStatus.Ready;
            ProcessesList.Add(process);
            return process.ID;
        }

        public bool Kill(int ProcessIndex)
        {
            if (ProcessesList?[ProcessIndex] != null)
            {
                var p = ProcessesList?[ProcessIndex];
                if (RunningProcesses.Contains(p) || QueuingProcesses.Contains(p))
                {
                    ProcessOnKill(ref RunningProcesses, ref QueuingProcesses, ref p, MaxRunningCount);
                    return p.Status == ProcessStatus.Blocked;
                }

                return true;
            }
            else
            {
                return true;
            }
        }

        public bool Queue(int ProcessIndex)
        {
            if (ProcessesList?[ProcessIndex] != null)
            {
                var p = ProcessesList[ProcessIndex];
                if (!RunningProcesses.Contains(p) && !QueuingProcesses.Contains(p))
                {
                    ProcessOnQueue(ref RunningProcesses, ref QueuingProcesses, ref p, MaxRunningCount);
                    return p.Status == ProcessStatus.Queuing;
                }

                return false;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Overrides

        public ProcessController(int maxThreadCount,
            ProcessManageAlgorithm manageAlgorithm = ProcessManageAlgorithm.Fifo) 
            : base()
        {
            Name = "ProcessController";
            ID = -1;
            _processIndex = -1;
            Context.Add<Hashtable>(new Hashtable());
            RunningProcesses = new List<IProcess>();
            QueuingProcesses = new List<IProcess>();
            ProcessesList = new List<IProcess>();
            
            Status = ProcessStatus.Running;

            _manageAlgorithm = manageAlgorithm;
            switch (_manageAlgorithm)
            {
                case ProcessManageAlgorithm.Fifo:
                    ProcessOnRun += FifoOnRunning;
                    ProcessOnQueue += FifoOnQueuing;
                    ProcessOnKill += FifoOnKilling;
                    ProcessOnUpdate += FifoOnUpdate;
                    break;
                case ProcessManageAlgorithm.RoundRobin:
                    ProcessOnRun += RoundRobinOnRunning;
                    ProcessOnQueue += RoundRobinOnQueuing;
                    ProcessOnKill += RoundRobinOnKilling;
                    ProcessOnUpdate += RoundRobinOnUpdate;
                    break;
                case ProcessManageAlgorithm.PriorityScheduling:
                    ProcessOnRun = PrioritySchedulingOnRunning;
                    ProcessOnQueue = PrioritySchedulingOnQueuing;
                    ProcessOnKill = PrioritySchedulingOnKilling;
                    ProcessOnUpdate = PrioritySchedulingOnUpdate;
                    break;
            }
            
            Timer = new Timer(100);
            Timer.Elapsed += (sender, args) =>
            {
                for (int i= 0; i < RunningProcesses.Count; ++i)
                {
                    IProcess process = RunningProcesses[i];
                    if (process.Status != ProcessStatus.Running)
                    {
                        process.Abort();
                        if (QueuingProcesses.Count > 0)
                        {
                            IProcess t = QueuingProcesses[0];
                            QueuingProcesses.Remove(t);
                            RunningProcesses.Add(t);
                            t.Run();
                        }
                    }
                    else
                    {
                        process.StillRunning();
                    }
                }

                ProcessOnUpdate(ref RunningProcesses, ref QueuingProcesses, 100, MaxRunningCount);
            };

            Timer.Enabled = true;
            Timer.AutoReset = true;
            Timer.Start();
        }
        
        public override void Main(params object[] args)
        {
            return;
        }

        #endregion
        
        #region EventMethods

        /// <summary>
        /// Priority Scheduling Process Manage Algorithm - OnUpdate
        /// </summary>
        /// <param name="runningProcess">running process list</param>
        /// <param name="queuingProcess">queuing process list</param>
        /// <param name="interval">the update interval(unit: 100ms)</param>
        /// <param name="maxRunningProcess">max running process count</param>
        public void PrioritySchedulingOnUpdate(
            ref List<IProcess> runningProcess,
            ref List<IProcess> queuingProcess,
            int interval,
            int maxRunningProcess)
        {
            for (int i = 0; i < queuingProcess.Count; ++i)
            {
                IProcess xEntry = queuingProcess[i];
                xEntry.AddWeight();
            }
            queuingProcess.Sort(((processA, processB) => (processA.Weight - processB.Weight)));
            
            for (int i = 0; i < runningProcess.Count; ++i)
            {
                IProcess xEntry = runningProcess[i];
                if (xEntry.Weight < 900 && xEntry.Weight < (queuingProcess.Count > 0 ? queuingProcess[0].Weight : 0))
                {
                    PrioritySchedulingOnKilling(ref runningProcess, ref queuingProcess, ref xEntry, maxRunningProcess);
                    xEntry.ReQueuing();
                    PrioritySchedulingOnRunning(ref runningProcess, ref queuingProcess, ref xEntry, maxRunningProcess);
                }
            }
        }
        
        /// <summary>
        /// Priority Scheduling Process Manage Algorithm - OnKill
        /// </summary>
        /// <param name="runningProcess">running process list</param>
        /// <param name="queuingProcess">queuing process list</param>
        /// <param name="process">process which call run method</param>
        /// <param name="maxRunningCount">max running process count</param>
        /// <returns>process final status, blocked</returns>
        public ProcessStatus PrioritySchedulingOnKilling(
            ref List<IProcess> runningProcess,
            ref List<IProcess> queuingProcess, 
            ref IProcess process, int maxRunningCount)
        {
            return FifoOnKilling(ref runningProcess, ref queuingProcess, ref process, maxRunningCount);
        }
        
        /// <summary>
        /// Priority Scheduling Process Manage Algorithm - OnQueue
        /// </summary>
        /// <param name="runningProcess">running process list</param>
        /// <param name="queuingProcess">queuing process list</param>
        /// <param name="process">process which call run method</param>
        /// <param name="maxRunningCount">max running process count</param>
        /// <returns>process final status, running or queuing</returns>
        public ProcessStatus PrioritySchedulingOnQueuing(
            ref List<IProcess> runningProcess,
            ref List<IProcess> queuingProcess, 
            ref IProcess process, int maxRunningCount)
        {
            return FifoOnQueuing(ref runningProcess, ref queuingProcess, ref process, maxRunningCount);
        }
        
        /// <summary>
        /// Priority Scheduling Process Manage Algorithm - OnRun
        /// </summary>
        /// <param name="runningProcess">running process list</param>
        /// <param name="queuingProcess">queuing process list</param>
        /// <param name="process">process which call run method</param>
        /// <param name="maxRunningCount">max running process count</param>
        /// <returns>process final status, running or queuing</returns>
        public ProcessStatus PrioritySchedulingOnRunning(
            ref List<IProcess> runningProcess,
            ref List<IProcess> queuingProcess, 
            ref IProcess process, int maxRunningCount)
        {
            return FifoOnRunning(ref runningProcess, ref queuingProcess, ref process, maxRunningCount);
        }
        
        /// <summary>
        /// First in first out Process Manage Algorithm - OnUpdate
        /// </summary>
        /// <param name="runningProcess">running process list</param>
        /// <param name="queuingProcess">queuing process list</param>
        /// <param name="interval">the update interval(unit: 100ms)</param>
        /// <param name="maxRunningProcess">max running process count</param>
        public void RoundRobinOnUpdate(
            ref List<IProcess> runningProcess,
            ref List<IProcess> queuingProcess,
            int interval,
            int maxRunningProcess)
        {
            for (int i = 0; i < runningProcess.Count; ++i)
            {
                IProcess xProcess = runningProcess[i];
                if (xProcess.Weight < 900 && xProcess.RunningTime >= interval)
                {
                    RoundRobinOnKilling(ref runningProcess, ref queuingProcess, ref xProcess, maxRunningProcess);
                    xProcess.ReQueuing();
                    RoundRobinOnRunning(ref runningProcess, ref queuingProcess, ref xProcess, maxRunningProcess);
                }
            }
        }
        
        /// <summary>
        /// Round Robin Process Manage Algorithm - OnKill
        /// </summary>
        /// <param name="runningProcess">running process list</param>
        /// <param name="queuingProcess">queuing process list</param>
        /// <param name="process">process which call run method</param>
        /// <param name="maxRunningCount">max running process count</param>
        /// <returns>process final status, blocked</returns>
        public ProcessStatus RoundRobinOnKilling(
            ref List<IProcess> runningProcess,
            ref List<IProcess> queuingProcess,
            ref IProcess process, int maxRunningCount)
        {
            return FifoOnKilling(ref runningProcess, ref queuingProcess, ref process, maxRunningCount);
        }
        
        /// <summary>
        /// Round Robin Process Manage Algorithm - OnQueue
        /// </summary>
        /// <param name="runningProcess">running process list</param>
        /// <param name="queuingProcess">queuing process list</param>
        /// <param name="process">process which call run method</param>
        /// <param name="maxRunningCount">max running process count</param>
        /// <returns>process final status, running or queuing</returns>
        public ProcessStatus RoundRobinOnQueuing(
            ref List<IProcess> runningProcess,
            ref List<IProcess> queuingProcess,
            ref IProcess process, int maxRunningCount)
        {
            return FifoOnQueuing(ref runningProcess, ref queuingProcess, ref process, maxRunningCount);
        }
        
        /// <summary>
        /// Round Robin Process Manage Algorithm - OnRun
        /// </summary>
        /// <param name="runningProcess">running process list</param>
        /// <param name="queuingProcess">queuing process list</param>
        /// <param name="process">process which call run method</param>
        /// <param name="maxRunningCount">max running process count</param>
        /// <returns>process final status, running or queuing</returns>
        public ProcessStatus RoundRobinOnRunning(
            ref List<IProcess> runningProcess,
            ref List<IProcess> queuingProcess,
            ref IProcess process, int maxRunningCount)
        {
            return FifoOnRunning(ref runningProcess, ref queuingProcess, ref process, maxRunningCount);
        }

        /// <summary>
        /// First in first out Process Manage Algorithm - OnUpdate
        /// </summary>
        /// <param name="runningprocesses">running process list</param>
        /// <param name="queuingprocesses">queuing process list</param>
        /// <param name="maxrunningcount">max running process count</param>
        private void FifoOnUpdate(
            ref List<IProcess> runningprocesses,
            ref List<IProcess> queuingprocesses, 
            int updateargument, int maxrunningcount) 
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
        private ProcessStatus FifoOnKilling(
            ref List<IProcess> runningprocesses, 
            ref List<IProcess> queuingprocesses,
            ref IProcess process, int maxrunningcount)
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
        private ProcessStatus FifoOnQueuing(
            ref List<IProcess> runningprocesses,
            ref List<IProcess> queuingprocesses,
            ref IProcess process, int maxrunningcount)
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
        private ProcessStatus FifoOnRunning(
            ref List<IProcess> runningprocesses,
            ref List<IProcess> queuingprocesses,
            ref IProcess process, int maxrunningcount)
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
        #endregion

        
    }
}