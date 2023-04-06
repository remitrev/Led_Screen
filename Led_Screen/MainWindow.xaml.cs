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

        public MainWindow()
        {
            InitializeComponent();
            InitWatcher();
            //listBox.ItemsSource = BluetoothLEDevices.Select(device => device.Name);
            bluetoothDevicesListBox.ItemsSource = BluetoothLEDevices.Select(device => device.Name);            
        }

        /*private async void Button_Click(object sender, RoutedEventArgs e)
        {
            BluetoothLEDevices.Clear();

            // Recherche les périphériques Bluetooth LE disponibles.
            var devices = await DeviceInformation.FindAllAsync(BluetoothLEDevice.GetDeviceSelector());

            // Ajoute les périphériques à la liste.
            foreach (var device in devices)
            {
                var bluetoothLEDevice = await BluetoothLEDevice.FromIdAsync(device.Id);
                BluetoothLEDevices.Add(bluetoothLEDevice);
            }

            // Met à jour les noms des périphériques dans la liste.
            listBox.ItemsSource = BluetoothLEDevices.Select(device => device.Name);
        }*/

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

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.watcher.Start();
            await Task.Delay(3000);
            this.watcher.Stop();
            bluetoothDevicesListBox.ItemsSource = BluetoothLEDevices.Select(device => device.Name);
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private async void Watcher_Received(
            BluetoothLEAdvertisementWatcher sender,
            BluetoothLEAdvertisementReceivedEventArgs args)
        {
            var device = await BluetoothLEDevice.FromBluetoothAddressAsync(args.BluetoothAddress);            
            if (device != null && !CheckBluetoothDevices(device.Name))
            {
                Debug.Print("Name :" + device.Name);
                await Dispatcher.InvokeAsync(() => BluetoothLEDevices.Add(device));
            }
        }

        private bool CheckBluetoothDevices(string name)
        {
            foreach(var device in BluetoothLEDevices)
            {
                if (device.Name.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }       
    }
}
