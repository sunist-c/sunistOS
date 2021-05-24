using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                Console.WriteLine(source.DataTable.Columns[0].ToString() + source.DataTable.Columns[1].ToString() + source.DataTable.Columns[2].ToString());
                Console.WriteLine(source.DataTable.Rows[0][0].ToString() + source.DataTable.Rows[0][1].ToString() + source.DataTable.Rows[0][2].ToString());
                return true;
            };
            controller.Create("test", new MemoryBlock(), WeightType.System, true);
            
        }
    }
}
