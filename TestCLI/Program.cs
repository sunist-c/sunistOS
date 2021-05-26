using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SunistLibs.Core;
using SunistLibs.Core.Enums;
using SunistLibs.Core.Process;

namespace TestCLI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ProcessController x = new ProcessController(4, ProcessManageAlgorithm.Fifo);

            Thread.Sleep(10000);
            Console.WriteLine("Finished");
        }
    }
}
