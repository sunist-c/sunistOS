using System;
using System.Collections.Generic;
using SunistLibs.Core.Enums;
using SunistLibs.Core.Interface.BaseStructure;
using SunistLibs.Core.Property;

namespace SunistLibs.Core.Process
{
    public static class ProcessManagementAlgorithm
    {
        #region EventMethods

        /// <summary>
        /// Priority Scheduling Process Manage Algorithm - OnUpdate
        /// </summary>
        /// <param name="runningProcess">running process list</param>
        /// <param name="queuingProcess">queuing process list</param>
        /// <param name="interval">the update interval(unit: 100ms)</param>
        /// <param name="maxRunningProcess">max running process count</param>
        public static void PrioritySchedulingOnUpdate(
            ref List<IProcess> runningProcess,
            ref List<IProcess> queuingProcess,
            int interval,
            int maxRunningProcess)
        {
            // Change all processes' weight if process is non-system process and remove non-running process
            for (int i = 0; i < runningProcess.Count; ++i)
            {
                IProcess p = runningProcess[i];
                if (p.Status != ProcessStatus.Running)
                {
                    if (p.Status == ProcessStatus.Hanging)
                    {
                        OnHanging(ref runningProcess, ref queuingProcess, ref p, maxRunningProcess);
                    }
                    else
                    {
                        PrioritySchedulingOnKilling(ref runningProcess, ref queuingProcess, ref p, maxRunningProcess);
                    }
                    
                    continue;
                }
                
                if (runningProcess[i].Weight < 900)
                {
                    runningProcess[i].SubWeight();
                }
            }

            for (int i = 0; i < queuingProcess.Count; ++i)
            {
                if (queuingProcess[i].Weight < 900)
                {
                    queuingProcess[i].AddWeight();
                }
            }
            
            // sort running/queuing list
            runningProcess.Sort((p1, p2) => -(p1.Weight - p2.Weight));
            queuingProcess.Sort((p1, p2) => -(p1.Weight - p2.Weight));
            
            // move processes from queuing list to running list and run it
            while (runningProcess.Count < maxRunningProcess && runningProcess.Count > 0)
            {
                IProcess p = queuingProcess[0];
                queuingProcess.Remove(p);
                PrioritySchedulingOnRunning(ref runningProcess, ref queuingProcess, ref p, maxRunningProcess);
            }

            for (int i = 0; i < queuingProcess.Count; ++i)
            {
                bool finished = true;
                for (int j = 0; j < runningProcess.Count; ++j)
                {
                    if (runningProcess[j].Weight > 900)
                    {
                        continue;
                    }
                    if (runningProcess[j].Weight < queuingProcess[i].Weight)
                    {
                        IProcess pr = runningProcess[j];
                        IProcess pq = queuingProcess[i];
                        finished = false;
                        PrioritySchedulingOnQueuing(ref runningProcess, ref queuingProcess, ref pr, maxRunningProcess);
                        PrioritySchedulingOnRunning(ref runningProcess, ref queuingProcess, ref pq, maxRunningProcess);
                    }
                }

                if (finished)
                {
                    break;
                }
            }
            
            // add running time for all running processes
            for (int i = 0; i < runningProcess.Count; ++i)
            {
                runningProcess[i].StillRunning();
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
        public static ProcessStatus PrioritySchedulingOnKilling(
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
        public static ProcessStatus PrioritySchedulingOnQueuing(
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
        public static ProcessStatus PrioritySchedulingOnRunning(
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
        public static void RoundRobinOnUpdate(
            ref List<IProcess> runningProcess,
            ref List<IProcess> queuingProcess,
            int interval,
            int maxRunningProcess)
        {
            // remove non-running progress
            for (int i = 0; i < runningProcess.Count; ++i)
            {
                IProcess p = runningProcess[i];
                if (p.Status != ProcessStatus.Running)
                {
                    // hanging progress
                    if (p.Status == ProcessStatus.Hanging)
                    {
                        OnHanging(ref runningProcess, ref queuingProcess, ref p, maxRunningProcess);
                    }
                    else
                    {
                        RoundRobinOnKilling(ref runningProcess, ref queuingProcess, ref p, maxRunningProcess);
                    }
                }
            }
            
            // if the running is up to limit, do nothing
            if (runningProcess.Count >= maxRunningProcess)
            {
                return;
            }
            
            // move processes from queuing list to running and run them base on their running time
            for (int i = 0; i < runningProcess.Count; ++i)
            {
                IProcess p = runningProcess[i];
                
                // if process is system process, do nothing
                if (p.Weight > 900)
                {
                    continue;
                }
                
                // if process's running time lager than interval, queue it to queuing list
                else if (p.RunningTime >= interval)
                {
                    RoundRobinOnQueuing(ref runningProcess, ref queuingProcess, ref p, interval);
                }
            }

            while (runningProcess.Count < maxRunningProcess && queuingProcess.Count > 0)
            {
                IProcess p = queuingProcess[0];
                queuingProcess.Remove(p);
                RoundRobinOnRunning(ref runningProcess, ref queuingProcess, ref p, maxRunningProcess);
            }
            
            // add running time for all running processes
            for (int i = 0; i < runningProcess.Count; ++i)
            {
                runningProcess[i].StillRunning();
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
        public static ProcessStatus RoundRobinOnKilling(
            ref List<IProcess> runningProcess,
            ref List<IProcess> queuingProcess,
            ref IProcess process, int maxRunningCount)
        {
            // RoundRobin Algorithm on killing is same to Fifo
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
        public static ProcessStatus RoundRobinOnQueuing(
            ref List<IProcess> runningProcess,
            ref List<IProcess> queuingProcess,
            ref IProcess process, int maxRunningCount)
        {
            // RoundRobin Algorithm on queuing is same to Fifo
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
        public static ProcessStatus RoundRobinOnRunning(
            ref List<IProcess> runningProcess,
            ref List<IProcess> queuingProcess,
            ref IProcess process, int maxRunningCount)
        {
            // RoundRobin Algorithm on running is same to Fifo
            return FifoOnRunning(ref runningProcess, ref queuingProcess, ref process, maxRunningCount);
        }

        /// <summary>
        /// First in first out Process Manage Algorithm - OnUpdate
        /// </summary>
        /// <param name="runningprocesses">running process list</param>
        /// <param name="queuingprocesses">queuing process list</param>
        /// <param name="maxrunningcount">max running process count</param>
        internal static void FifoOnUpdate(
            ref List<IProcess> runningprocesses,
            ref List<IProcess> queuingprocesses, 
            int updateargument, int maxrunningcount) 
        {
            // remove unrunning proesses
            for (int i = 0; i < runningprocesses.Count; ++i)
            {
                IProcess p = runningprocesses[i];
                if (p.Status != ProcessStatus.Running)
                {
                    // if process is hangout, call hanging event
                    if (p.Status == ProcessStatus.Hanging)
                    {
                        OnHanging(ref runningprocesses, ref queuingprocesses, ref p, maxrunningcount);
                    }
                    
                    // remove
                    FifoOnKilling(ref runningprocesses, ref queuingprocesses, ref p, maxrunningcount);
                }
            }
            
            // if running list is up to limit, do nothing
            if (runningprocesses.Count >= maxrunningcount)
            {
                return;
            }

            // move top processes in queuing list to running list and run them if available
            else
            {
                while (runningprocesses.Count < maxrunningcount && queuingprocesses.Count > 0)
                {
                    IProcess p = queuingprocesses[0];
                    queuingprocesses.Remove(p);
                    FifoOnRunning(ref runningprocesses, ref queuingprocesses, ref p, maxrunningcount);
                }
            }
            
            // add running time for all running processes
            for (int i = 0; i < runningprocesses.Count; ++i)
            {
                runningprocesses[i].StillRunning();
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
        internal static ProcessStatus FifoOnKilling(
            ref List<IProcess> runningprocesses, 
            ref List<IProcess> queuingprocesses,
            ref IProcess process, int maxrunningcount)
        {
            // just running process can be killed
            if (runningprocesses.Contains(process))
            {
                process.Abort();
                runningprocesses.Remove(process);
                FifoOnUpdate(ref runningprocesses, ref queuingprocesses, ProcessControllerProperty.ProcessUpdateInterval, maxrunningcount);
                return process.Status;
            }
            
            // if process is in queuing list, remove it
            else if (queuingprocesses.Contains(process))
            {
                process.Abort();
                queuingprocesses.Remove(process);
                return process.Status;
            }

            // otherwise, do nothing
            else
            {
                return process.Status;
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
        internal static ProcessStatus FifoOnQueuing(
            ref List<IProcess> runningprocesses,
            ref List<IProcess> queuingprocesses,
            ref IProcess process, int maxrunningcount)
        {
            // if process is in running list, stop it and queue
            if (runningprocesses.Contains(process))
            {
                FifoOnKilling(ref runningprocesses, ref queuingprocesses, ref process, maxrunningcount);
                FifoOnQueuing(ref runningprocesses, ref queuingprocesses, ref process, maxrunningcount);
                return process.Status;
            }
            
            // if process is in queuing list, do nothing
            else if (queuingprocesses.Contains(process))
            {
                return process.Status;
            }

            // queue process
            else
            {
                process.Status = ProcessStatus.Queuing;
                process.ReQueuing();
                queuingprocesses.Add(process);
                return process.Status;
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
        internal static ProcessStatus FifoOnRunning(
            ref List<IProcess> runningprocesses,
            ref List<IProcess> queuingprocesses,
            ref IProcess process, int maxrunningcount)
        {
            // if process is in running list, kill it and rerun
            if (runningprocesses.Contains(process))
            {
                FifoOnKilling(ref runningprocesses, ref queuingprocesses, ref process, maxrunningcount);
                FifoOnRunning(ref runningprocesses, ref queuingprocesses, ref process, maxrunningcount);
                return process.Status;
            }

            // if process is in queuing list, do nothing
            else if (queuingprocesses.Contains(process))
            {
                return process.Status;
            }
            
            // if running list is up to limit, queue it and do nothing
            else if (runningprocesses.Count >= maxrunningcount)
            {
                FifoOnQueuing(ref runningprocesses, ref queuingprocesses, ref process, maxrunningcount);
                return process.Status;
            }
            
            // run process
            else
            {
                process.Status = ProcessStatus.Running;
                runningprocesses.Add(process);
                process.Run();
                return process.Status;
            }
        }

        internal static void SyncProcessList(
            ref List<IProcess> runningProcesses,
            ref List<IProcess> queuingProcesses,
            ref List<IProcess> processesList)
        {
            // sync process between running/queuing list and process dictionary
            for (int i = 0; i < runningProcesses.Count; ++i)
            {
                if (processesList[runningProcesses[i].ID] != runningProcesses[i])
                {
                    processesList[runningProcesses[i].ID] = runningProcesses[i];
                }
            }

            for (int i = 0; i < queuingProcesses.Count; ++i)
            {
                if (processesList[queuingProcesses[i].ID] != queuingProcesses[i])
                {
                    processesList[queuingProcesses[i].ID] = queuingProcesses[i];
                }
            }
        }

        internal static void OnHanging(
            ref List<IProcess> runningProcess,
            ref List<IProcess> queuingProcess,
            ref IProcess process, int maxRunningCount)
        {
            // remove process in running/queuing list
            if (runningProcess.Contains(process))
            {
                runningProcess.Remove(process);
            }
            else if (queuingProcess.Contains(process))
            {
                queuingProcess.Remove(process);
            }
        }
        #endregion
    }
}