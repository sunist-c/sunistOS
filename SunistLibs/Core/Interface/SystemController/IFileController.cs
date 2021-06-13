using System.Collections.Generic;
using SunistLibs.Core.Enums;
using SunistLibs.Core.Interface.BaseStructure;

namespace SunistLibs.Core.Interface.SystemController
{
    public interface IFileController : ISystemController
    {
        bool MakeDirectory(string path);
        bool RemoveDirectory(string path);
        bool CreateFile(string path, byte[] data = null, TextEncoding encoding = TextEncoding.ASCII);
        bool RemoveFile(string path);
        bool WriteToFile(string path, byte[] data = null, TextEncoding encoding = TextEncoding.ASCII);
        byte[] ReadFromFile(string path, TextEncoding encoding);
        bool IsExist(string path);
        bool IsDirectory(string path);
        bool IsFile(string path);
        List<IFile> List(string path);
    }
}