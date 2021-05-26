using SunistLibs.Core.Enums;
using SunistLibs.Core.Interface.ProcessSystem;

namespace SunistLibs.Core.Process
{
    public class BaseProcess : IProcess
    {
        private string _name;
        private int _id;
        private ProcessStatus _status;
        private MemoryBlock _context;
        private int _weight;
        private int _maxRunningTime;
        private int _runningTime;
        
        public string Name { get => _name; }
        public int ID { get => _id; }
        public ProcessStatus Status 
        { 
            get => _status;
            set { _status = value; }
        }
        public MemoryBlock Context { get => _context; }
        public int Weight { get => _weight; }
        public int MaxRunningTime
        {
            get => _maxRunningTime;
            set
            {
                _maxRunningTime = value;
            }
        }
        public int RunningTime { get => _runningTime; }

        public void AddWeight()
        {
            _weight = (_weight % 99 == 0) ? _weight : ++_weight;
        }

        public void SubWeight()
        {
            _weight = (_weight % 100 == 0) ? _weight : --_weight;
        }

        public void StillRunning()
        {
            _runningTime += 1;
        }

        public BaseProcess(int id, string name)
        {
            _name = name;
            _id = id;
            _status = ProcessStatus.Ready;
            _context = new MemoryBlock();
            _weight = 0;
            _runningTime = 0;
            _maxRunningTime = 0;
        }
    }
}