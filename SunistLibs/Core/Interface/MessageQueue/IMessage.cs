using System;
using SunistLibs.Core.Interface.SystemController;
using SunistLibs.Core.Message;

namespace SunistLibs.Core.Interface.MessageQueue
{
    public interface IMessage
    {
        ISystemController Sender { get; set; }
        ISystemController Reader { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string SerializedData { get; set; }
    }
}