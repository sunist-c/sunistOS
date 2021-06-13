using System.IO;
using SunistLibs.Core.Interface.BaseStructure;

namespace SunistLibs.Core.File
{
    public class BaseDirectory : IDirectory
    {
        private DirectoryInfo _information;
        public DirectoryInfo Information
        {
            get => _information;
            internal set => _information = value;
        }
    }
}