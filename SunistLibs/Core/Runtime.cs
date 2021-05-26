using SunistLibs.Core.Enums;
using SunistLibs.Core.Process;

namespace SunistLibs.Core
{
    public static class Runtime
    {
        public static ProcessController Controller;

        static Runtime()
        {
            Controller = new ProcessController(4, ProcessManageAlgorithm.Fifo);
        }
    }
}