using Led_Screen.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Led_Screen
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        private MainWindowViewModel viewModel;
        public MainWindowView()
        {
            InitializeComponent();
            viewModel = new MainWindowViewModel();
            DataContext = viewModel;
            historique.ItemsSource = viewModel.allContents;
            bluetoothDevicesListBox.ItemsSource = viewModel.BluetoothLEDevices.Select(device => device.Name + " :" + device.BluetoothAddress);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            noDevices.Visibility = Visibility.Hidden;
            errorMessage.Visibility = Visibility.Hidden;
            viewModel.BluetoothLEDevices.Clear();
            await viewModel.Start_SearchAsync();
            bluetoothDevicesListBox.ItemsSource = viewModel.BluetoothLEDevices.Select(device => device.Name + " :" + device.BluetoothAddress);
            if(bluetoothDevicesListBox.Items.Count == 0)
            {
                noDevices.Visibility = Visibility.Visible;
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            errorMessage.Visibility = Visibility.Hidden;
            // Recupere le nom et l'adresse du Device
            string selectedDeviceString = bluetoothDevicesListBox.SelectedItem as string;

            var tabDevice = selectedDeviceString.Split(':');

            // Affecte l'appareil sélectionné à la variable bluetoothDeviceSelected
            viewModel.SelectBluetoothDevice(tabDevice[1]);
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Reset label error
            errorMessage.Visibility = Visibility.Hidden;

            if (allLEDScreen.IsChecked == true)
            {
                if (viewModel.BluetoothLEDevices.Count() == 0)
                {
                    errorMessage.Visibility = Visibility.Visible;
                    return;
                }

                try
                {
                    await viewModel.SendToAllDevices(TransformMessagesTextBox(), tag.Text);
                }
                catch (Exception exc)
                {
                    Debug.Print(exc.Message);
                }
                
            }
            else //Send to selected device
            {
                if(viewModel.bluetoothDeviceSelected != null)
                {
                    try
                    {
                        await viewModel.SendToSelectedDevice(TransformMessagesTextBox(), tag.Text);
                    }
                    catch (Exception exc)
                    {
                        Debug.Print(exc.Message);
                    }
                } else
                {
                    errorMessage.Visibility = Visibility.Visible;
                    Debug.Print("pas de devices");
                }
            }
            viewModel.FilterAndOrderHistorique(tagFilter.Text, "lastUse");
            historique.ItemsSource = viewModel.allContents;
        }   

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            errorMessage.Visibility = Visibility.Hidden;
        }


        private void TextBox1_TextChanged(object sender, TextChangedEventArgs e)
        { 
            if (firstMessage.Text == "")
            {
                secondMessage.IsReadOnly = true;
                secondMessage.Text = "";
            }
            else
            {
                secondMessage.IsReadOnly = false;
            }
            CheckButtonSearchDiplay();
        }

        private void TextBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (secondMessage.Text == "")
            {
                thirdMessage.IsReadOnly = true;
                thirdMessage.Text = "";
            }
            else
            {
                thirdMessage.IsReadOnly = false;
            }
        }

        private void TextBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (thirdMessage.Text == "")
            {
                forthMessage.IsReadOnly = true;
                forthMessage.Text = "";
            }
            else
            {
                forthMessage.IsReadOnly = false;
            }
        }

        private void TextBox4_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (forthMessage.Text == "")
            {
                _5Message.IsReadOnly = true;
                _5Message.Text = "";
            }
            else
            {
                _5Message.IsReadOnly = false;
            }
        }

        private void TextBox5_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_5Message.Text == "")
            {
                _6Message.IsReadOnly = true;
                _6Message.Text = "";
            }
            else
            {
                _6Message.IsReadOnly = false;
            }
        }

        private void TextBox6_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_6Message.Text == "")
            {
                _7Message.IsReadOnly = true;
                _7Message.Text = "";
            }
            else
            {
                _7Message.IsReadOnly = false;
            }
        }

        private void TextBox7_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_7Message.Text == "")
            {
                _8Message.IsReadOnly = true;
                _8Message.Text = "";
            }
            else
            {
                _8Message.IsReadOnly = false;
            }
        }

        private void TextBox8_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBoxTag_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckButtonSearchDiplay();
        }


        private List<string> TransformMessagesTextBox()
        {
            List<string> messages = new List<string>();
            messages.Add(firstMessage.Text);
            messages.Add(secondMessage.Text);
            messages.Add(thirdMessage.Text);
            messages.Add(forthMessage.Text);
            messages.Add(_5Message.Text);
            messages.Add(_6Message.Text);
            messages.Add(_7Message.Text);
            messages.Add(_8Message.Text);
            return messages;
        }

        private void CheckButtonSearchDiplay()
        {
            if(firstMessage.Text != "" && tag.Text != "")
            {
                sendMessage.IsEnabled = true;
            } else
            {
                sendMessage.IsEnabled = false;
            }
        }

        private void Button_Click_LastUse(object sender, RoutedEventArgs e)
        {
            viewModel.FilterAndOrderHistorique(tagFilter.Text, "lastUse");
            historique.ItemsSource = viewModel.allContents;
        }

        private void Button_Click_CreateDate(object sender, RoutedEventArgs e)
        {
            viewModel.FilterAndOrderHistorique(tagFilter.Text, "create");
            historique.ItemsSource = viewModel.allContents;
        }

        private void TextBox_TextChanged_TagFilter(object sender, TextChangedEventArgs e)
        {
            viewModel.FilterAndOrderHistorique(tagFilter.Text, "lastUse");
            historique.ItemsSource = viewModel.allContents;
        }
    }
}
