using System;
using System.Threading;
using System.Threading.Tasks;
using SunistLibs.Core.Enums;
using SunistLibs.Core.Interface.ProcessSystem;
using SunistLibs.Core.Memory;
using Timer = System.Timers.Timer;

namespace SunistLibs.Core.Process
{
    public abstract class BaseProcess : IProcess
    {
        private string _name;
        private int _id;
        private ProcessStatus _status;
        private MemoryBlock _context;
        private int _weight;
        private int _maxRunningTime;
        private int _runningTime;
        private CancellationTokenSource cts;
        private CancellationToken ct;
        private bool _inited;

        public string Name
        {
            get => _name;
            set
            {
                if (!_inited)
                {
                    _name = value;
                    _inited = true;
                }
            }
        }

        public int ID
        {
            get => _id;
            set
            {
                if (!_inited)
                {
                    _id = value;
                    _inited = true;
                }
            }
        }
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
        public int MemoryUsage
        {
            get => _context.Size;
        }

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

        public void ReQueuing()
        {
            _runningTime = 0;
        }
        
        /// <summary>
        /// Process's Entrance Method, yet not support arguments
        /// </summary>
        /// <param name="args">arguments</param>
        public abstract void Main(params Object[] args);
        
        public void Run()
        {
            OnRunMethod();
        }

        public void Abort()
        {
            cts = new CancellationTokenSource();
            ct = cts.Token;
            cts.Cancel();
            _context.Clear();
            Status = ProcessStatus.Blocked;
        }

        public event ProcessOnRun OnRun;
        public event ProcessOnAbort OnAbort;

        public BaseProcess()
        {
            _inited = false;
            _name = "";
            _id = 0;
            _status = ProcessStatus.Ready;
            _context = new MemoryBlock();
            _weight = 0;
            _runningTime = 0;
            _maxRunningTime = Int32.MaxValue;
            OnRun += Main;
            OnAbort += Abort;
        }

        private async Task OnRunMethod(params Object[] args)
        {
            Task task = Task.Run(() =>
            {
                OnRun(args);
            });
            Timer t = new Timer(100);
            t.Elapsed += (sender, eventArgs) =>
            {
                if (ct.IsCancellationRequested)
                {
                    task.Dispose();
                    t.Stop();
                    t.Enabled = false;
                }
            };
            t.Enabled = true;
            t.Start();
            t.AutoReset = true;
            await task;
            this.Status = ProcessStatus.Hanging;
        }
    }
}