using System;
using System.Collections.Generic;
using SunistLibs.Core.Device;
using SunistLibs.Core.Enums;
using SunistLibs.Core.EventSystem;
using SunistLibs.Core.Interface.BaseStructure;
using SunistLibs.Core.Interface.MessageQueue;
using SunistLibs.Core.Interface.SystemController;
using SunistLibs.Core.Message;

namespace SunistLibs.Core.BaseControllers
{
    public abstract class BaseDeviceController : IDeviceController
    {
        public bool Logable { get; set; } = true;
        public bool Contactable { get; set; } = true;
        
        public void LogHandler(string[] logData)
        {
            foreach (string s in logData)
            {
                Console.WriteLine(s);
            }
        }

        public void SendMessageHandler(IMessage message)
        {
            throw new System.NotImplementedException();
        }

        public void ReceiveMessageHandler(IMessage message)
        {
            throw new System.NotImplementedException();
        }

        public event ConsoleLogHandler Log;
        public event SendMessageHandler SendMessage;
        public event RecieveMessageHandler ReceiveMessage;
        
        public List<IDevice> Devices { get; } = new List<IDevice>();
        
        public IDevice GetDevice(int deviceId)
        {
            if (Devices?[deviceId] is null) return null;
            else return Devices[deviceId];
        }

        public IDevice GetDevice(string deviceName)
        {
            foreach (IDevice device in Devices)
                if (device.Name == deviceName)
                    return device;
            
            return null;
        }

        public List<IDevice> List()
        {
            return Devices;
        }
        

        public abstract int LoadDevice(int deviceId, string name, string description);

        public abstract bool LogoutDevice(int deviceId);

        public abstract DeviceStatus ApplyDevice(IProcess process, int deviceId);

        public abstract DeviceStatus FreeDevice(IProcess process, int deviceId);

        public event DeviceOnOccupyHandler OccupyDeviceHandler;
        public event DeviceOnFreeHandler FreeDeviceHandler;
        public event DeviceOnLoadHandler LoadDeviceHandler;
        public event DeviceOnLogoutHandler LogoutDeviceHandler;

        public BaseDeviceController()
        {
            Log += LogHandler;
        }
    }
}