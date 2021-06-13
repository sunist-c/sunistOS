using SunistLibs.Core.Interface.BaseStructure;

namespace SunistLibs.Core.Device
{
    public delegate void DeviceOnOccupy(IProcess process, bool Blocked);

    public delegate void DeviceOnFree();
}