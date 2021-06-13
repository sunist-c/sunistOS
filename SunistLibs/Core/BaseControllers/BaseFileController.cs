using System;
using System.Collections.Generic;
using SunistLibs.Core.Enums;
using SunistLibs.Core.EventSystem;
using SunistLibs.Core.Interface.BaseStructure;
using SunistLibs.Core.Interface.MessageQueue;
using SunistLibs.Core.Interface.SystemController;
using SunistLibs.Core.Message;

namespace SunistLibs.Core.BaseControllers
{
    public abstract class BaseFileController : IFileController
    {
        public bool Logable { get; set; } = true;
        public bool Contactable { get; set; }
        public void LogHandler(string[] logData)
        {
            if(Logable)
                foreach (string s in logData)
                    Console.WriteLine(s);
        }

        public void SendMessageHandler(IMessage message)
        {
            throw new System.NotImplementedException();
        }

        public void ReceiveMessageHandler(IMessage message)
        {
            throw new System.NotImplementedException();
        }

        public event ConsoleLogHandler Log;
        public event SendMessageHandler SendMessage;
        public event RecieveMessageHandler ReceiveMessage;

        public abstract bool MakeDirectory(string path);

        public abstract bool RemoveDirectory(string path);

        public abstract bool CreateFile(string path, byte[] data = null, TextEncoding encoding = TextEncoding.ASCII);

        public abstract bool RemoveFile(string path);

        public abstract bool WriteToFile(string path, byte[] data = null, TextEncoding encoding = TextEncoding.ASCII);

        public abstract byte[] ReadFromFile(string path, TextEncoding encoding);

        public abstract bool IsExist(string path);

        public abstract bool IsDirectory(string path);

        public abstract bool IsFile(string path);

        public abstract List<IFile> List(string path);

        public BaseFileController()
        {
            Log += LogHandler;
        }
    }
}