using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SunistLibs.Core;
using SunistLibs.Core.Enums;

namespace TestCLI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            ProcessController controller = new ProcessController();
            controller.Display += (source, mode) =>
            {
                if (mode == DisplayMode.All)
                    Console.WriteLine($"Process Created: ID {source.DataTable.Rows[0][0]}, CPU Time: {source.DataTable.Rows[0][3]}");
                return true;
            };

            ulong i = 1;
            for (int ii = 0; ii < 10; ii++, i++)
            {
                controller.List(ProcessStatus.Running);
                Thread.Sleep(100);
            }
        }
    }
}
