using SunistLibs.Core.Enums;
using SunistLibs.Core.Interface.BaseStructure;

namespace SunistLibs.Core.Device
{
    public delegate DeviceStatus DeviceOnOccupyHandler(IProcess process, int deviceId);

    public delegate DeviceStatus DeviceOnFreeHandler(IProcess process, int deviceId);

    public delegate bool DeviceOnLogoutHandler(int deviceId);

    public delegate int DeviceOnLoadHandler(int deviceId, string deviceName, string deviceDescription);
}