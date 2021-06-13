using SunistLibs.Core.Interface.MessageQueue;

namespace SunistLibs.Core.Message
{
    public delegate void SendMessageHandler(IMessage message);

    public delegate void RecieveMessageHandler(IMessage message);
}