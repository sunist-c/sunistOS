using SunistLibs.Core.Enums;

namespace SunistLibs.Core.Interface.BaseStructure
{
    public interface IDevice
    {
        int ID { get; }
        string Name { get; }
        string Description { get; }
        int FatherProcessID { get; }
        DeviceStatus Status { get; set; }
        
    }
}