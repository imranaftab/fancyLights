using FancyLights.Common;
using FancyLights.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FancyLights.ViewModels
{
    public  class MainPageViewModel:BaseViewModel
    {
        private int _redColor;
        private int _blueColor;
        private int _greenColor;
        private Color _stairColor;
        private IBluetoothConnector _bluetoothService;
        private bool _isConnected;
        private ICommand _connectCommand;
        private ICommand _disconnectCommand;
        private LightTrigger _lightOnTrigger;
        
        public MainPageViewModel(IBluetoothConnector bluetoothService)
        {
            _bluetoothService = bluetoothService ?? throw new Exception("Bluetooth service is null.");
        }

        public ICommand ConnectCommand
        {
            get
            {
                if (_connectCommand == null)
                    _connectCommand = new Command(OnConnect);

                return _connectCommand;
            }
        }
        public ICommand DisconnectCommand
        {
            get
            {
                if (_disconnectCommand == null)
                    _disconnectCommand = new Command(OnDisconnect);

                return _disconnectCommand;
            }
        }
        

        private void SetLightOnTrigger()
        {
            try
            {
                if (_bluetoothService.IsConnected)
                {
                    _bluetoothService.SetLightOnTrigger(_lightOnTrigger);
                }
            }
            catch (Exception)
            {
            }
            
        }

        private void OnDisconnect(object obj)
        {
            try
            {
                if (_bluetoothService.IsConnected)
                {
                    _bluetoothService.Disconnect();
                    IsConnected = _bluetoothService.IsConnected;
                }
            }
            catch (Exception)
            {
            }
        }

        private void OnConnect(object obj)
        {
            try
            {
                if(_bluetoothService.IsConnected == false)
                {
                    _bluetoothService.Connect(Constants.BluetoothDeviceName);
                    IsConnected = _bluetoothService.IsConnected;
                }
            }
            catch (Exception)
            {
            }
        }
        public LightTrigger LightOnTrigger
        {
            get { return _lightOnTrigger; }
            set { SetProperty(ref _lightOnTrigger, value,onChanged:SetLightOnTrigger); }
        }
        public bool IsConnected
        {
            get { return _isConnected; }
            set { SetProperty(ref _isConnected, value); }
        }
        public int RedColor
        {
            get { return _redColor; }
            set { SetProperty(ref _redColor, value, onChanged: UpdateRGBLight); }
        }
        public int BlueColor
        {
            get { return _blueColor; }
            set { SetProperty(ref _blueColor, value, onChanged: UpdateRGBLight); }
        }

        public int GreenColor
        {
            get { return _greenColor; }
            set { SetProperty(ref _greenColor, value, onChanged: UpdateRGBLight); }
        }

        public Color StairColor
        {
            get { return _stairColor; }
            set { SetProperty(ref _stairColor, value); }
        }
        private void UpdateRGBLight()
        {
            try
            {
                StairColor = Color.FromRgb(_redColor, _greenColor, _blueColor);

                if (_bluetoothService.IsConnected)
                    _bluetoothService.SendRGB((byte)_redColor, (byte)_greenColor, (byte)_blueColor);
            }
            catch (Exception)
            {
            }
        }
    }
}
