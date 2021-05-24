using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunistLibs.Core.Delegate;
using SunistLibs.Core.Enums;
using SunistLibs.DataStructure.Interfaces;
using SunistLibs.DataStructure.Output;

namespace SunistLibs.Core
{
    public class Process
    {
        private string _name;
        private ulong _id;
        private ulong _cpuTime;
        private ulong _weight;
        private ProcessStatus _status;
        private MemoryBlock _memory;
        private ulong _exceptedRuntime;

        public Process()
        {
            _name = "Unknown";
            _id = 0;
            _cpuTime = 0;
            _weight = 0;
            _status = ProcessStatus.Ready;
            _memory = new MemoryBlock();
            _exceptedRuntime = UInt64.MaxValue;
        }

        public Process(string name, ulong id, ulong weight, MemoryBlock memory)
        {
            _name = name;
            _id = id;
            _cpuTime = 0;
            _weight = weight;
            _status = ProcessStatus.Ready;
            _memory = memory;
        }

        public ulong ExceptedRuntime
        {
            get => _exceptedRuntime;
            set => _exceptedRuntime = value;
        }

        public string Name
        {
            get => _name;
        }

        public ulong Id
        {
            get => _id;
        }

        public ulong CpuTime
        {
            get => _cpuTime;
            set => _cpuTime = value;
        }

        public ulong Weight
        {
            get => _weight;
            set => _weight = value;
        }

        public ProcessStatus Status
        {
            get => _status;
            set => _status = value;
        }

        public MemoryBlock Memory
        {
            get => _memory;
            set => _memory = value;
        }

        public T Run<T>(GeneralDelegate<T> method, params Object[] args)
        {
            if (method == null)
            {
                throw new ArgumentException();
            }
            else
            {
                return method(args);
            }
        }
    }
}
