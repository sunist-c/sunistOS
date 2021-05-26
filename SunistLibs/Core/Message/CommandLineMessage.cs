using Newtonsoft.Json;
using SunistLibs.Core.Interface.MessageQueue;
using SunistLibs.Core.Interface.SystemController;

namespace SunistLibs.Core.Message
{
    public class CommandLineMessage : ICommandLineInterface
    {
        public ISystemController Sender { get; set; }
        public ISystemController Reader { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SerializedData { get; set; }
        
        public void Serialize<T>(T data)
        {
            SerializedData = JsonConvert.SerializeObject(data);
        }

        public string[] Deserialize()
        {
            throw new System.NotImplementedException();
        }
    }
}