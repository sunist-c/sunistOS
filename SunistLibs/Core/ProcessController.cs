using System;
using System.Collections;
using SunistLibs.Core.Delegate;
using SunistLibs.Core.Enums;
using SunistLibs.DataStructure.Interfaces;
using SunistLibs.DataStructure.Output;

namespace SunistLibs.Core
{
    public class ProcessController : IDisplayable
    {
        private Hashtable _processesList;
        private MemoryController _memoryController;

        public ProcessController()
        {
            _memoryController = new MemoryController();
            _processesList = new Hashtable();
        }

        private ProcessStatus Start(Process process)
        {
            return process.Status;
        }

        private ProcessStatus Abort(Process process)
        {
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
            return 0;
        }

        public DisplaySource DisplayInfo(Display displayMethod, params object[] args)
        {
            return displayMethod(args);
        }

        public event DisplayEventHandler Display;

        public ulong Create(string name, MemoryBlock context, WeightType weight, bool needMessage = false)
        {
            var x = new Process(name, GetNewProcessId(), GetNewProcessWeight(weight), context);
            _processesList.Add(x.Id, x);
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
            if (!_processesList.Contains(processId))
            {
                // todo: 完成异常状态：找不到进程
                throw new Exception();
            }
            else
            {
                try
                {
                    Process p = _processesList[processId] as Process;
                    p.Run<T>(method, args);
                    _processesList[processId] = p;
                    
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
            if (!_processesList.Contains(processId))
            {
                // todo: 完成异常状态：找不到进程
                throw new Exception();
            }
            else
            {
                try
                {
                    Process p = _processesList[processId] as Process;
                    if (p.Status == ProcessStatus.Running)
                    {
                        Abort(p);
                    }

                    _processesList[processId] = p;
                    return p.Id;
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}