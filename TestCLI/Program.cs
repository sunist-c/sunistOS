using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SunistLibs.Core;
using SunistLibs.Core.Enums;
using SunistLibs.Core.File;
using SunistLibs.Core.Interface.BaseStructure;
using SunistLibs.Core.Memory;
using SunistLibs.Core.Process;
using SunistLibs.Tools.IO;

namespace TestCLI
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // ProcessController x = new ProcessController(10, ProcessManageAlgorithm.Fifo);
            // for (int i = 0; i < 50; ++i)
            // {
            //     int p = x.Create($"test{i}", objects =>
            //     {
            //         Console.WriteLine($"Task {i} is finished");
            //         Thread.Sleep(1000);
            //     });
            //     IProcess pp = x.Find(p);
            //     Thread.Sleep(100);
            //     int m = x.MemoryController.AddContext(pp, new MemoryBlock());
            //     x.Run(p);
            // }
            //
            // Thread.Sleep(5000);
            // var list = x.List();
            // foreach (IProcess process in list)
            // {
            //     Console.WriteLine($"Process Name {process.Name}, Status {process.Status}, RunningTime {process.RunningTime}");
            // }

            FileController fileController = new FileController();
            var list = fileController.List("~");
            foreach (IFile file in list)
            {
                Console.WriteLine(file.Information.Name);
            }
        }
    }
}
