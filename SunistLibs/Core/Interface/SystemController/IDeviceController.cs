using System.Collections.Generic;
using SunistLibs.Core.Device;
using SunistLibs.Core.Enums;
using SunistLibs.Core.Interface.BaseStructure;

namespace SunistLibs.Core.Interface.SystemController
{
    public interface IDeviceController : ISystemController
    {
        List<IDevice> Devices { get; }
        
        IDevice GetDevice(int deviceId);
        IDevice GetDevice(string deviceName);
        List<IDevice> List();
        
        int LoadDevice(int deviceId, string name, string description);
        bool LogoutDevice(int deviceId);
        DeviceStatus ApplyDevice(IProcess process, int deviceId);
        DeviceStatus FreeDevice(IProcess process, int deviceId);

        event DeviceOnOccupyHandler OccupyDeviceHandler;
        event DeviceOnFreeHandler FreeDeviceHandler;
        event DeviceOnLoadHandler LoadDeviceHandler;
        event DeviceOnLogoutHandler LogoutDeviceHandler;
    }
}