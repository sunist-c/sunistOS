namespace SunistLibs.Core.Interface.MessageQueue
{
    public interface ICommandLineInterface : IMessage
    {
        void Serialize<T>(T data);
        string[] Deserialize();
    }
}