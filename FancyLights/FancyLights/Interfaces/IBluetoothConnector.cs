using FancyLights.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FancyLights.Interfaces
{
    public interface IBluetoothConnector
    {
        bool IsConnected { get; set; }
        void Connect(string deviceName);
        void Disconnect();
        List<string> GetAvailableDevices();
        void SendRGB(byte red, byte green, byte blue);
        void SetLightOnTrigger(LightTrigger trigger);
    }
}
