using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using SunistLibs.Core.Delegate;
using SunistLibs.Core.Enums;
using SunistLibs.DataStructure.Interfaces;
using SunistLibs.DataStructure.Output;
using SunistLibs.Core.Algorithm;

namespace SunistLibs.Core
{
    public class ProcessController : IDisplayable
    {
        private List<Process> _processesList;
        private MemoryController _memoryController;
        private ulong _maxRunningProcessCount;
        private ProcessManagerAlgorithm _managerAlgorithm;
        private ulong _usedProcesCount;
        private List<Process> _runningProcess;
        private List<Process> _queuingProcess;
        private Timer _timer;

        public ProcessController(ulong maxRunningProcessCount = 1, 
            ProcessManagerAlgorithm managerAlgorithm = ProcessManagerAlgorithm.Auto)
        {
            _runningProcess = new List<Process>();
            _memoryController = new MemoryController();
            _queuingProcess = new List<Process>();
            _processesList = new List<Process>();
            _maxRunningProcessCount = maxRunningProcessCount;
            _managerAlgorithm = managerAlgorithm;
            _usedProcesCount = 0;
            Create("ProcessController", new MemoryBlock(), WeightType.System, false);
            

            (_processesList[0] as Process).Status = ProcessStatus.Running;
            _runningProcess.Add((Process)_processesList[0]);
            
            _timer = new Timer(100);
            _timer.AutoReset = true;
            _timer.Enabled = true;
            _timer.Elapsed += (sender, args) =>
            {
                foreach (Process xProcess in _runningProcess)
                {
                    xProcess.CpuTime += 1;
                }
            };
            _timer.Start();
        }
        

        private ProcessStatus Start(Process process)
        {
            process.Status = ProcessStatus.Running;
            return process.Status;
        }

        private ProcessStatus Abort(Process process)
        {
            process.Status = ProcessStatus.Blocked;
            process.Memory = null;
            return process.Status;
        }

        private ProcessStatus Hangout(Process process)
        {
            
            return process.Status;
        }

        private ProcessStatus Queuing(Process process)
        {
            return process.Status;
        }

        private ulong GetNewProcessWeight(WeightType weight)
        {
            switch (weight)
            {
                // todo: 等待精细化权限
                case WeightType.System:
                    return 9;
                case WeightType.Service:
                    return 6;
                case WeightType.Users:
                    return 3;
                case WeightType.Guide:
                    return 0;
                case WeightType.Temporary:
                    return 0;
                default:
                    return 0;
            }
        }

        private ulong GetNewProcessId()
        {
            return _usedProcesCount++;
        }

        public DisplaySource DisplayInfo(Display displayMethod, params object[] args)
        {
            return displayMethod(args);
        }

        public event DisplayEventHandler Display;

        public ulong Create(string name, MemoryBlock context, WeightType weight, bool needMessage = false)
        {
            var x = new Process(name, GetNewProcessId(), GetNewProcessWeight(weight), context);
            _processesList.Add(x);
            Display?.Invoke(new DisplaySource(
                "New Process Create Succeed",
                new string[] {"ProcessId", "ProcessName", "ProcessMemorySize"},
                new string[]
                {
                    x.Id.ToString(),
                    x.Name,
                    x.Memory.Size().ToString()
                }
            ), needMessage ? DisplayMode.All : DisplayMode.System);
            return x.Id;
        }

        public ulong Run<T>(ulong processId, bool needMessage, GeneralDelegate<T> method, params Object[] args)
        {
            if (false)
            {
                // todo: 完成异常状态：找不到进程
                throw new Exception();
            }
            else
            {
                try
                {
                    Process p = _processesList[(int)processId] as Process;
                    
                    // todo: 将任务调度算法分配到具体实例
                    switch (_managerAlgorithm)
                    {
                        case ProcessManagerAlgorithm.Auto:
                            ProcessManageAlgorithm.FifoOnRunning(ref _runningProcess, ref _queuingProcess ,_maxRunningProcessCount, ref p);
                            break;
                        case ProcessManagerAlgorithm.SJF:
                            break;
                        case ProcessManagerAlgorithm.FIFO:
                            break;
                        default:
                            break;
                    }
                    
                    

                    Start(p);
                    p.Run<T>(method, args);
                    _processesList[(int)processId] = p;
                    
                    Display?.Invoke(new DisplaySource(

                    ), needMessage ? DisplayMode.All : DisplayMode.System);
                    return p.Id;
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        public ulong Kill(ulong processId, bool needMessage = true)
        {
            if (false)
            {
                // todo: 完成异常状态：找不到进程
                throw new Exception();
            }
            else
            {
                try
                {
                    Process p = _processesList[(int)processId] as Process;
                    if (p.Status == ProcessStatus.Running)
                    {
                        Abort(p);
                    }
                    else
                    {
                        // todo: 未完成状态未完成
                    }

                    _processesList[(int)processId] = p;
                    return p.Id;
                }
                catch
                {
                    throw;
                }
            }
        }

        public void List(params ProcessStatus[] statuses)
        {
            List<string[]> processData = new List<string[]>();
            foreach (Process tProcess in _processesList)
            {
                foreach (ProcessStatus xStatus in statuses)
                {
                    if (tProcess.Status == xStatus)
                    {
                        string[] record = new string[4];
                        record[0] = tProcess.Id.ToString();
                        record[1] = tProcess.Name;
                        record[2] = tProcess.Weight.ToString();
                        record[3] = tProcess.CpuTime.ToString();
                        processData.Add(record);
                    }
                }
            }
            
            Display(new DisplaySource(
                "MemoryList",
                new string[] {"ProcessId", "ProcessName", "ProcessWeight", "CPU Time"},
                processData
            ), DisplayMode.All);
        }
    }
}