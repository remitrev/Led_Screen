using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Enumeration;

namespace Led_Screen
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BluetoothLEAdvertisementWatcher watcher;

        public ObservableCollection<BluetoothLEDevice> BluetoothLEDevices { get; } = new ObservableCollection<BluetoothLEDevice>();

        public BluetoothLEDevice bluetoothDeviceSelected = null;

        public MainWindow()
        {
            InitializeComponent();
            InitWatcher();
            bluetoothDevicesListBox.ItemsSource = BluetoothLEDevices.Select(device => device.Name);            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        #region Search Bluetooth Device
        private void InitWatcher()
        {
            this.watcher = new BluetoothLEAdvertisementWatcher();
            this.watcher = new BluetoothLEAdvertisementWatcher()
            {
                ScanningMode = BluetoothLEScanningMode.Passive
            };

            this.watcher.Received += Watcher_Received;
        }

        private async Task Start_SearchAsync()
        {
            this.watcher.Start();
            await Task.Delay(3000);
            this.watcher.Stop();
            bluetoothDevicesListBox.ItemsSource = BluetoothLEDevices.Select(device => device.Name);
        }

        private async void Watcher_Received(
            BluetoothLEAdvertisementWatcher sender,
            BluetoothLEAdvertisementReceivedEventArgs args)
        {
            var device = await BluetoothLEDevice.FromBluetoothAddressAsync(args.BluetoothAddress);            
            if (device != null && !CheckBluetoothDevicesByName(device.Name))
            {
                Debug.Print("Name :" + device.Name);
                await Dispatcher.InvokeAsync(() => BluetoothLEDevices.Add(device));
            }
        }


        #endregion

        private bool CheckBluetoothDevicesByName(string name)
        {
            foreach (var device in BluetoothLEDevices)
            {
                if (device.Name.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }

        private byte[] GetEnteteToSendContents(/*List content*/)
        {
            var bArr = new byte[64];
            bArr[0] = 119;
            bArr[1] = 97;
            bArr[2] = 110;
            bArr[3] = 103;
            bArr[4] = 0;
            bArr[5] = 0;
            //TODO: getFlash bar[6]
            //TODO: getMarquee bar[7]
            //TODO: getModeAndSpeed [8 - 15]
            //TODO: getMsgLength [16-31]
            bArr[32] = 0;
            bArr[33] = 0;
            bArr[34] = 0;
            bArr[35] = 0;
            bArr[36] = 0;
            bArr[37] = 0;
            //TODO: getDate [38-43] peut etre mettre à 0, à tester
            for (int i2 = 0; i2 < 19; i2++)
            {
                bArr[i2 + 44] = 0;
            }
            bArr[63] = 0;
            return bArr;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await Start_SearchAsync();
        }

        private void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            // Recupere le nom du Device
            string selectedDeviceName = bluetoothDevicesListBox.SelectedItem as string;

            // Recherche l'appareil BluetoothLEDevice correspondant dans la collection BluetoothLEDevices
            BluetoothLEDevice selectedDevice = BluetoothLEDevices.FirstOrDefault(device => device.Name == selectedDeviceName);

            // Affecte l'appareil sélectionné à la variable bluetoothDeviceSelected
            bluetoothDeviceSelected = selectedDevice;
        }
    }
}
