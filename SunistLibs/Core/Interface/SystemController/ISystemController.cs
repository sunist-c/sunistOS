using SunistLibs.Core.EventSystem;
using SunistLibs.Core.Interface.MessageQueue;
using SunistLibs.Core.Message;

namespace SunistLibs.Core.Interface.SystemController
{
    public interface ISystemController
    {
        bool Logable { get; set; }
        bool Contactable { get; set; }

        void LogHandler(string[] logData);
        void SendMessageHandler(IMessage message);
        void ReceiveMessageHandler(IMessage message);

        event ConsoleLogHandler Log;
        event SendMessageHandler SendMessage;
        event RecieveMessageHandler ReceiveMessage;
    }
}