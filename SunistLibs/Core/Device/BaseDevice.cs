using SunistLibs.Core.Enums;
using SunistLibs.Core.Interface.BaseStructure;

namespace SunistLibs.Core.Device
{
    public class BaseDevice : IDevice
    {
        private int _id = 0;
        private string _name = "";
        private string _description = "";
        private int _fatherProcessId = -1;
        
        public int ID 
        { 
            get => _id;
            private set => _id = value;
        }

        public string Name
        {
            get => _name;
            private set => _name = value;
        }

        public string Description
        {
            get => _description;
            private set => _description = value;
        }

        public int FatherProcessID
        {
            get => _fatherProcessId;
            private set => _fatherProcessId = value;
        }

        public DeviceStatus Status { get; set; } = DeviceStatus.Offline;

        public DeviceStatus Occupy(int processId)
        {
            if (Status != DeviceStatus.Free) return Status;
            else
            {
                FatherProcessID = processId;
                Status = DeviceStatus.Occupied;
                return Status;
            }
        }

        public DeviceStatus Logout()
        {
            if (Status == DeviceStatus.Offline) return Status;
            Free();
            Status = DeviceStatus.Offline;
            return Status;
        }

        public DeviceStatus Free()
        {
            if (Status == DeviceStatus.Occupied)
            {
                FatherProcessID = -1;
                Status = DeviceStatus.Free;
                return Status;
            }
            else
            {
                return Status;
            }
        }
        
        public BaseDevice(int id, string name, string description)
        {
            ID = id;
            Name = name;
            Description = description;
        }
    }
}