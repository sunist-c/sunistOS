using System;
using System.Collections.Generic;
using System.Linq;
using SunistLibs.Core.Enums;

namespace SunistLibs.Core.Algorithm
{
    public static class ProcessManageAlgorithm
    {
        #region Fifo

        public static ProcessStatus FifoOnRunning(ref List<Process> runningProcess, ref List<Process> queuingProcess, 
            ulong maxRunnningProcess, ref Process process)
        {
            if (runningProcess.Contains(process))
            {
                return ProcessStatus.Running;
            }
            else if ((ulong) runningProcess.Count >= maxRunnningProcess)
            {
                if (queuingProcess.Contains(process))
                {
                    return ProcessStatus.Queuing;
                }
                else
                {
                    queuingProcess.Add(process);
                    return ProcessStatus.Queuing;
                }
            }
            else
            {
                runningProcess.Add(process);
                process.Status = ProcessStatus.Running;
                return ProcessStatus.Running;
            }
        }

        public static ProcessStatus FifoOnKilling(ref List<Process> runningProcess, ref List<Process> queuingProcess,
            ulong maxRunningProcess, ref Process process)
        {
            if (runningProcess.Contains(process))
            {
                runningProcess.Remove(process);
                if (queuingProcess.Count > 0)
                {
                    queuingProcess[0].Status = ProcessStatus.Running;
                    runningProcess.Add(queuingProcess[0]);
                    queuingProcess.Remove(queuingProcess[0]);
                    return ProcessStatus.Blocked;
                }
                else
                {
                    return ProcessStatus.Blocked;
                }
            }
            else if (queuingProcess.Contains(process))
            {
                queuingProcess.Remove(process);
                return ProcessStatus.Blocked;
            }
            else
            {
                return ProcessStatus.Blocked;
            }
        }

        public static void FifoOnUpdate(ref List<Process> runningProcess, ref List<Process> queuingProcess,
            ulong maxRunningProcess)
        {
            for (int i = 0; i < runningProcess.Count; ++i)
            {
                Process xEntry = runningProcess[i];
                if (xEntry.ExceptedRuntime != UInt64.MaxValue && xEntry.CpuTime >= xEntry.ExceptedRuntime && xEntry.Weight < 9)
                {
                    FifoOnKilling(ref runningProcess, ref queuingProcess, maxRunningProcess, ref xEntry);
                }
            }
        }

        #endregion

        #region RoundRobin

        public static ProcessStatus RoundRobinOnRunning(ref List<Process> runningProcess,
            ref List<Process> queuingProcess,
            ulong maxRunningProcess, ref Process process)
        {
            return FifoOnRunning(ref runningProcess, ref queuingProcess, maxRunningProcess, ref process);
        }

        public static ProcessStatus RoundRobinOnKilling(ref List<Process> runningProcess,
            ref List<Process> queuingProcess,
            ulong maxRunningProcess, ref Process process)
        {
            return FifoOnKilling(ref runningProcess, ref queuingProcess, maxRunningProcess, ref process);
        }

        public static void RoundRobinOnUpdate(ref List<Process> runningProcess,
            ref List<Process> queuingProcess,
            ulong maxRunningProcess, ulong interval)
        {
            for (int i = 0; i < runningProcess.Count; ++i)
            {
                Process xProcess = runningProcess[i];
                if (xProcess.Weight < 9 && xProcess.CpuTime >= interval)
                {
                    RoundRobinOnKilling(ref runningProcess, ref queuingProcess, maxRunningProcess, ref xProcess);
                    xProcess.CpuTime = 0;
                    RoundRobinOnRunning(ref runningProcess, ref queuingProcess, maxRunningProcess, ref xProcess);
                }
            }
        }


        #endregion

        #region PriorityScheduling

        public static ProcessStatus PrioritySchedulingOnRunning(ref List<Process> runningProcess,
            ref List<Process> queuingProcess,
            ulong maxRunningProcess, ref Process process)
        {
            return FifoOnRunning(ref runningProcess, ref queuingProcess, maxRunningProcess, ref process);
        }

        public static ProcessStatus PrioritySchedulingOnKilling(ref List<Process> runningProcess,
            ref List<Process> queuingProcess,
            ulong maxRunningProcess, ref Process process)
        {
            return FifoOnKilling(ref runningProcess, ref queuingProcess, maxRunningProcess, ref process);
        }

        public static void PrioritySchedulingOnUpdate(ref List<Process> runningProcess,
            ref List<Process> queuingProcess,
            ulong maxRunningProcess)
        {
            for (int i = 0; i < queuingProcess.Count; ++i)
            {
                Process xEntry = queuingProcess[i];
                if (xEntry.Weight <= 3)
                {
                    xEntry.Weight = ++xEntry.Weight > 3 ? --xEntry.Weight : xEntry.Weight;
                    queuingProcess[i] = xEntry;
                }
                else if (xEntry.Weight <= 6)
                {
                    xEntry.Weight = ++xEntry.Weight > 6 ? --xEntry.Weight : xEntry.Weight;
                    queuingProcess[i] = xEntry;
                }
                else
                {
                    xEntry.Weight = ++xEntry.Weight >= 9 ? --xEntry.Weight : xEntry.Weight;
                    queuingProcess[i] = xEntry;
                }
            }
            queuingProcess.Sort(((processA, processB) =>
            {
                return (int) (processA.Weight - processB.Weight);
            }));
            
            for (int i = 0; i < runningProcess.Count; ++i)
            {
                Process xEntry = runningProcess[i];
                if (xEntry.Weight < 9 && xEntry.Weight < (queuingProcess.Count > 0 ? queuingProcess[0].Weight : 0))
                {
                    PrioritySchedulingOnKilling(ref runningProcess, ref queuingProcess, maxRunningProcess, ref xEntry);
                    xEntry.Weight = 0;
                    PrioritySchedulingOnRunning(ref runningProcess, ref queuingProcess, maxRunningProcess, ref xEntry);
                }
            }
        }
        
        #endregion
    }
}