using System;
using SunistLibs.Core.Enums;
using SunistLibs.DataStructure.Output;

namespace SunistLibs.Core.Delegate
{
    public delegate T GeneralDelegate<T>(params Object[] args);

    public delegate DisplaySource Display(params Object[] args);

    public delegate bool DisplayEventHandler(DisplaySource displaySource, DisplayMode mode);
}