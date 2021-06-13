using SunistLibs.Core.BaseControllers;
using SunistLibs.Core.Enums;
using SunistLibs.Core.Interface.BaseStructure;

namespace SunistLibs.Core.Device
{
    public class DeviceController : BaseDeviceController
    {
        private int _topIndex = -1;
        private int topIndex
        {
            get => ++_topIndex;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceId">It's not use</param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public override int LoadDevice(int deviceId, string name, string description)
        {
            int id = topIndex;
            Devices.Add(new BaseDevice(id, name, description));
            return id;
        }

        public override bool LogoutDevice(int deviceId)
        {
            if (Devices?[deviceId] is null) return false;
            else return Devices[deviceId].Logout() == DeviceStatus.Offline;
        }

        public override DeviceStatus ApplyDevice(IProcess process, int deviceId)
        {
            if (Devices?[deviceId] is null) return DeviceStatus.Offline;
            else if (Devices[deviceId].Status != DeviceStatus.Free) return DeviceStatus.Free;
            else return Devices[deviceId].Occupy(process.ID);
        }

        public override DeviceStatus FreeDevice(IProcess process, int deviceId)
        {
            if (Devices?[deviceId] is null) return DeviceStatus.Offline;
            else if (Devices[deviceId].FatherProcessID != process.ID) return DeviceStatus.Occupied;
            else return Devices[deviceId].Free();
        }

        public DeviceController() : base()
        {
            OccupyDeviceHandler += ApplyDevice;
            FreeDeviceHandler += FreeDevice;
            LogoutDeviceHandler += LogoutDevice;
            LoadDeviceHandler += LoadDevice;
        }
    }
}