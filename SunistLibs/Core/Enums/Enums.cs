﻿namespace SunistLibs.Core.Enums
{
    public enum ProcessStatus
    {
        Ready,
        Running,
        Blocked,
        Queuing,
        Hanging
    }

    public enum AuthorityType
    {
        Root,
        User,
        Guest,
        Interface
    }

    public enum WeightType
    {
        System,
        Service,
        Users,
        Guide,
        Temporary
    }

    public enum DeviceType
    {
        Input,
        Output,
        Operator,
        Disk,
        Memory,
        Others
    }

    public enum DisplayMode
    {
        All,
        System,
        User,
        None
    }
}