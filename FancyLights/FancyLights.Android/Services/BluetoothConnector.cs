using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FancyLights.Common;
using FancyLights.Droid.Services;
using FancyLights.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(BluetoothConnector))]
namespace FancyLights.Droid.Services
{
    public class BluetoothConnector : IBluetoothConnector
    {
        private BluetoothAdapter _bluetoothAdapter;
        private BluetoothDevice _bluetoothDevice;
        private BluetoothSocket _bluetoothSocket;

        public BluetoothConnector()
        {
            _bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
        }
        public bool IsConnected { get; set; }

        public void Connect(string deviceName)
        {
            try
            {
                if(_bluetoothAdapter == null)
                    _bluetoothAdapter = BluetoothAdapter.DefaultAdapter;

                try
                {
                    if (_bluetoothDevice == null)
                        _bluetoothDevice = _bluetoothAdapter.BondedDevices.Where(bd => bd.Name == deviceName).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Could not connect to {deviceName}. Could not pair with the device.");
                }

                try
                {
                    _bluetoothSocket = _bluetoothDevice.CreateRfcommSocketToServiceRecord(Java.Util.UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));
                    _bluetoothSocket.Connect();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Could not connect to {deviceName}. Could not create RF Comm socket.");
                }

                IsConnected = true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not connect to {deviceName}. Unknown error: {ex.Message}");
            }
        }

        public void Disconnect()
        {
            if (IsConnected)
            {
                IsConnected = false;
                _bluetoothDevice.Dispose();
                _bluetoothSocket.OutputStream.Close();
                _bluetoothSocket.Close();
                _bluetoothSocket = null;
                _bluetoothDevice = null;
                _bluetoothAdapter = null;
            }
        }

        public List<string> GetAvailableDevices()
        {
            if (!IsConnected)
                throw new Exception("Bluetooth is not connected to a device.");

            return _bluetoothAdapter.BondedDevices.Select(r=>r.Name).ToList();
        }

        public void SetLightOnTrigger(LightTrigger trigger)
        {
            try
            {
                if (IsConnected)
                {
                    byte[] rgb = new byte[] { (byte)CommandType.LightOnTrigger, (byte)trigger};
                    _bluetoothSocket.OutputStream.Write(rgb, 0, 2);
                    _bluetoothSocket.OutputStream.Flush();
                }
                else
                {
                    throw new Exception("Could not send. Bluetooth device is not connected");
                }
            }
            catch (Exception ex)
            {
                Disconnect();
                throw new Exception($"Could set light on trigger. Please connect again.\n {ex.Message}. ");
            }
        }

        public void SendRGB(byte red, byte blue, byte green)
        {
            try
            {
                if (IsConnected)
                {
                    byte[] rgb = new byte[] {(byte)CommandType.SetRGB, red, blue, green };
                    _bluetoothSocket.OutputStream.Write(rgb,0,4);
                    _bluetoothSocket.OutputStream.Flush();
                }
                else
                {
                    throw new Exception("Could not send. Bluetooth device is not connected");
                }
            }
            catch (Exception ex)
            {
                Disconnect();
                throw new Exception($"Could not send rgb data. {ex.Message}. Please connect again.");
            }
        }
    }
}