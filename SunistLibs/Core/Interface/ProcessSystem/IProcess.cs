using SunistLibs.Core.Enums;
using SunistLibs.Core.Interface.SystemController;

namespace SunistLibs.Core.Interface.ProcessSystem
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
    }
}