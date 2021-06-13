using System;
using SunistLibs.Core.Enums;
using SunistLibs.Core.Memory;
using SunistLibs.Core.Process;

namespace SunistLibs.Core.Interface.BaseStructure
{
    public interface IProcess
    { 
        string Name { get; }
        int ID { get; }
        ProcessStatus Status { get; set; }
        MemoryBlock Context { get; }
        int Weight { get; }
        int MaxRunningTime { get; set; }
        int RunningTime { get; }
        void AddWeight();
        void SubWeight();
        void StillRunning();
        void ReQueuing();

        void Main(params Object[] args);
        void Run();
        void Abort();
        event ProcessOnRun OnRun;
        event ProcessOnAbort OnAbort;
    }
}