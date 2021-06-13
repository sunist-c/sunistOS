using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SunistLibs.Core.Enums;
using SunistLibs.Core.EventSystem;
using SunistLibs.Core.Interface.BaseStructure;
using SunistLibs.Core.Interface.MessageQueue;
using SunistLibs.Core.Interface.SystemController;
using SunistLibs.Core.Message;

namespace SunistLibs.Core.File
{
    public class FileController: IFileController
    {
        public bool Logable { get; set; }
        public bool Contactable { get; set; }
        public void LogHandler(string[] logData)
        {
            throw new System.NotImplementedException();
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
        
        public bool MakeDirectory(string path)
        {
            DirectoryInfo info = Directory.CreateDirectory(path);
            return info.Exists;
        }

        public bool RemoveDirectory(string path)
        {
            try
            {
                Directory.Delete(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CreateFile(string path, byte[] data = null, TextEncoding encoding = TextEncoding.ASCII)
        {
            using (Stream s = System.IO.File.Create(path))
            {
                if (s is null) return false;
                else
                {
                    Encoding e = Encoding.Default;
                    switch (encoding)
                    {
                        case TextEncoding.UTF8:
                            e = Encoding.UTF8;
                            break;
                        case TextEncoding.GBK:
                            e = Encoding.Unicode;
                            break;
                        case TextEncoding.ASCII:
                            e = Encoding.ASCII;
                            break;
                        case TextEncoding.UNICODE:
                            e = Encoding.Unicode;
                            break;
                        default:
                            e = Encoding.Default;
                            break;
                    }

                    using (StreamWriter sw = new StreamWriter(s, e))
                    {
                        try
                        {
                            sw.Write(data);
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
            }
        }

        public bool RemoveFile(string path)
        {
            try
            {
                System.IO.File.Delete(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool WriteToFile(string path, byte[] data = null, TextEncoding encoding = TextEncoding.ASCII)
        {
            if (System.IO.File.Exists(path))
            {
                using (Stream s = System.IO.File.Open(path, FileMode.Append))
                {
                    Encoding e = Encoding.Default;
                    switch (encoding)
                    {
                        case TextEncoding.UTF8:
                            e = Encoding.UTF8;
                            break;
                        case TextEncoding.GBK:
                            e = Encoding.Unicode;
                            break;
                        case TextEncoding.ASCII:
                            e = Encoding.ASCII;
                            break;
                        case TextEncoding.UNICODE:
                            e = Encoding.Unicode;
                            break;
                        default:
                            e = Encoding.Default;
                            break;
                    }

                    StreamWriter sw = new StreamWriter(s, e);

                    try
                    {
                        sw.Write(data);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            else return CreateFile(path, data, encoding);
        }

        public byte[] ReadFromFile(string path, TextEncoding encoding)
        {
            if (System.IO.File.Exists(path))
            {
                using (Stream s = System.IO.File.Open(path, FileMode.Open))
                {
                    Encoding e = Encoding.Default;
                    switch (encoding)
                    {
                        case TextEncoding.UTF8:
                            e = Encoding.UTF8;
                            break;
                        case TextEncoding.GBK:
                            e = Encoding.Unicode;
                            break;
                        case TextEncoding.ASCII:
                            e = Encoding.ASCII;
                            break;
                        case TextEncoding.UNICODE:
                            e = Encoding.Unicode;
                            break;
                        default:
                            e = Encoding.Default;
                            break;
                    }

                    using (StreamReader sr = new StreamReader(s))
                    {
                        byte[] b = new byte[sr.BaseStream.Length];
                        sr.BaseStream.Read(b, 0, b.Length);
                        sr.BaseStream.Seek(0, SeekOrigin.Begin);
                        return b;
                    }
                }
            }
            else return null;
        }

        public bool IsExist(string path)
        {
            return System.IO.File.Exists(path);
        }

        public bool IsDirectory(string path)
        {
            return Directory.Exists(path);
        }

        public bool IsFile(string path)
        {
            return System.IO.File.Exists(path);
        }

        public List<IFile> List(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            var files = info.GetFiles();
            List<IFile> list = new List<IFile>();
            foreach (FileInfo fileInfo in files)
                list.Add(new BaseFile(){Information = fileInfo});
            return list;
        }
    }
}