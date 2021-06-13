using System.IO;
using SunistLibs.Core.Interface.BaseStructure;

namespace SunistLibs.Core.File
{
    public class BaseFile : IFile
    {
        private FileInfo _information;

        public FileInfo Information
        {
            get => _information;
            internal set => _information = value;
        }
    }
}