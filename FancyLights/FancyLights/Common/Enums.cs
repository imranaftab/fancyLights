using System;
using System.Collections.Generic;
using System.Text;

namespace FancyLights.Common
{
    public enum CommandType { LightOnTrigger, SetRGB };
    public enum LightTrigger { AlwaysOn, Auto};// here Auto means light on when  motion is detected
}
