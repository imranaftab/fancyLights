using FancyLights.Common;
using FancyLights.Interfaces;
using FancyLights.ViewModels;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FancyLights
{
	public partial class MainPage : ContentPage
	{
        private IBluetoothConnector instance;

        public MainPage()
		{
			InitializeComponent();
            BindingContext = new MainPageViewModel(DependencyService.Get<IBluetoothConnector>()); 
            
		}

        private void btn_GetAvailableDevices(object sender, EventArgs ev)
        {
            //if(instance == null)
            //    instance = DependencyService.Get<IBluetoothConnector>();
            //if(!instance.IsConnected)
            //    instance.Connect(Constants.BluetoothDeviceName);

            //instance.SendRGB(0, 1, 2);
        }
        private async void btn_RequestPermissions(object sender, EventArgs ex)
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted || true)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await DisplayAlert("Need location", "Gunna need that location", "OK");
                    }
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await DisplayAlert("Need location", "Gunna need that location", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(Permission.Location))
                        status = results[Permission.Location];
                }

                //if (status == PermissionStatus.Granted)
                //{
                //    var results = await CrossGeolocator.Current.GetPositionAsync(10000);
                //    LabelGeolocation.Text = "Lat: " + results.Latitude + " Long: " + results.Longitude;
                //}
                //else if (status != PermissionStatus.Unknown)
                //{
                //    await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                //}
            }
            catch (Exception x)
            {

                //LabelGeolocation.Text = "Error: " + ex;
            }
        }

    }
}
