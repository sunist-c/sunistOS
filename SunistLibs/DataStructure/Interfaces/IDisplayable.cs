using System;
using System.Data;
using SunistLibs.Core.Delegate;
using SunistLibs.DataStructure.Output;

namespace SunistLibs.DataStructure.Interfaces
{
    public interface IDisplayable
    {
        DisplaySource DisplayInfo(Display displayMethod, params Object[] args);

        event DisplayEventHandler Display;
    }
}