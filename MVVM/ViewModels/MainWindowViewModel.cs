﻿using Led_Screen.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;

namespace Led_Screen.MVVM.ViewModels
{
    class MainWindowViewModel
    {
        public ObservableCollection<BluetoothLEDevice> BluetoothLEDevices { get; set; }
        public BluetoothLEDevice bluetoothDeviceSelected { get; set; }
        public MainModel mainModel;

        #region private data
        private BluetoothLEAdvertisementWatcher watcher;
        private Dictionary<string, byte[]> characterMapper;
        private String UUID_CHARACTERISTICS_WRITE = "fee1";
        private String UUID_SERVICE = "fee0";
        private Guid serviceUUID = Guid.Parse("0000fee0-0000-1000-8000-00805f9b34fb");
        private Guid characteristicsUUID = Guid.Parse("0000fee1-0000-1000-8000-00805f9b34fb");        
        #endregion private

        public MainWindowViewModel()
        {
            mainModel = new MainModel();
            BluetoothLEDevices = new ObservableCollection<BluetoothLEDevice>();
            bluetoothDeviceSelected = null;
            InitWatcher();
            InitMappeur();
        }

        public void FilterAndOrderHistorique(string filter, string order)
        {
            mainModel.ApplyFilterOrOrderOnList(filter, order);
        }

        #region Search Bluetooth Device
        public async Task Start_SearchAsync()
        {
            this.watcher.Start();
            await Task.Delay(2000);
            this.watcher.Stop();
            Debug.Print("Fin de recherche");
        }

        public void SelectBluetoothDevice(string address)
        {
            bluetoothDeviceSelected = BluetoothLEDevices.FirstOrDefault(device => device.BluetoothAddress == ulong.Parse(address)); ;
        }

        #region private method
        private void InitWatcher()
        {
            this.watcher = new BluetoothLEAdvertisementWatcher();
            this.watcher = new BluetoothLEAdvertisementWatcher()
            {
                ScanningMode = BluetoothLEScanningMode.Passive
            };

            this.watcher.Received += Watcher_Received;
        }



        private async void Watcher_Received(
            BluetoothLEAdvertisementWatcher sender,
            BluetoothLEAdvertisementReceivedEventArgs args)
        {
            var device = await BluetoothLEDevice.FromBluetoothAddressAsync(args.BluetoothAddress);
            if (device != null && !CheckBluetoothDevicesByAddress(device.BluetoothAddress) && device.Name.Equals("LSLED"))
            {
                Debug.Print("Name :" + device.Name);
                await Application.Current.Dispatcher.InvokeAsync(() => BluetoothLEDevices.Add(device));
            }
        }

        private bool CheckBluetoothDevicesByAddress(ulong address)
        {
            foreach (var device in BluetoothLEDevices)
            {
                if (device.BluetoothAddress.Equals(address))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
        #endregion

        #region Send Messages to devices

        public async Task SendToSelectedDevice(List<string> messages, string tag)
        {
            await SendToOneDeviceAsync(bluetoothDeviceSelected, messages, tag);
        }

        public async Task SendToAllDevices(List<string> messages, string tag)
        {
            foreach (var device in BluetoothLEDevices)
            {
                await SendToOneDeviceAsync(device, messages, tag);
            }
        }

        #region Get headers
        private byte[] GetFirstEnteteToSendContents()
        {
            var bArr = new byte[16];
            bArr[0] = 119;
            bArr[1] = 97;
            bArr[2] = 110;
            bArr[3] = 103;
            bArr[4] = 0;
            bArr[5] = 0;
            //TODO: getFlash bar[6]
            bArr[6] = 0;
            //TODO: getMarquee bar[7]
            bArr[7] = 0;
            //TODO: getModeAndSpeed [8 - 15]
            for (int i2 = 0; i2 < 8; i2++)
            {
                if (i2 % 2 == 0)
                {
                    bArr[i2 + 8] = 48;
                }
                else
                {
                    bArr[i2 + 8] = 0;
                }
            }
            return bArr;
        }
        private byte[] GetSecondEnteteToSendContents(List<String> messages)
        {
            var bArr = new byte[16];

            // Transform the message length into 2 bytes
            bArr[0] = (byte)(messages[0].Length >> 8);  // MSB
            bArr[1] = (byte)messages[0].Length;         // LSB
            if (messages[1] != "")
            {
                bArr[2] = (byte)(messages[1].Length >> 8);  // MSB
                bArr[3] = (byte)messages[1].Length;         // LSB
                if (messages[2] != "")
                {
                    bArr[4] = (byte)(messages[2].Length >> 8);  // MSB
                    bArr[5] = (byte)messages[2].Length;         // LSB
                    if (messages[3] != "")
                    {
                        bArr[6] = (byte)(messages[3].Length >> 8);  // MSB
                        bArr[7] = (byte)messages[3].Length;         // LSB
                        if (messages[4] != "")
                        {
                            bArr[8] = (byte)(messages[4].Length >> 8);  // MSB
                            bArr[9] = (byte)messages[4].Length;         // LSB
                            if (messages[5] != "")
                            {
                                bArr[10] = (byte)(messages[5].Length >> 8);  // MSB
                                bArr[11] = (byte)messages[5].Length;         // LSB
                                if (messages[6] != "")
                                {
                                    bArr[12] = (byte)(messages[6].Length >> 8);  // MSB
                                    bArr[13] = (byte)messages[6].Length;         // LSB
                                    if (messages[7] != "")
                                    {
                                        bArr[14] = (byte)(messages[7].Length >> 8);  // MSB
                                        bArr[15] = (byte)messages[7].Length;         // LSB

                                    }
                                    else
                                    {
                                        bArr[14] = 0;
                                        bArr[15] = 0;
                                    }
                                }
                                else
                                {
                                    for (int i2 = 12; i2 < 14; i2++)
                                    {
                                        bArr[i2 + 2] = 0;
                                    }
                                }
                            }
                            else
                            {
                                for (int i2 = 10; i2 < 14; i2++)
                                {
                                    bArr[i2 + 2] = 0;
                                }
                            }
                        }
                        else
                        {
                            for (int i2 = 8; i2 < 14; i2++)
                            {
                                bArr[i2 + 2] = 0;
                            }
                        }
                    }
                    else
                    {
                        for (int i2 = 6; i2 < 14; i2++)
                        {
                            bArr[i2 + 2] = 0;
                        }
                    }
                }
                else
                {
                    for (int i2 = 4; i2 < 14; i2++)
                    {
                        bArr[i2 + 2] = 0;
                    }
                }
            }
            else
            {
                for (int i2 = 2; i2 < 14; i2++)
                {
                    bArr[i2 + 2] = 0;
                }
            }

            return bArr;
        }

        private byte[] GetThirdEnteteToSendContents()
        {
            //Can put date here
            var bArr = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                bArr[i] = 0;
            }
            return bArr;
        }
        private byte[] GetForthEnteteToSendContents()
        {
            var bArr = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                bArr[i] = 0;
            }
            return bArr;
        }
        #endregion

        #region private methods
        private async Task SendToOneDeviceAsync(BluetoothLEDevice device, List<string> messages, string tag)
        {
            //Create in DB and in list
            mainModel.CreateOrUpdateMessages(messages, tag);

            if (messages[0] == "")
            {
                throw new Exception("No message to send");
            }
            Debug.Print("Debut de l'envoie");

            var test = await device.GetGattServicesAsync();

            GattDeviceService service = null;
            foreach (var unService in test.Services)
            {
                if (unService.Uuid.Equals(serviceUUID))
                {
                    service = unService;
                }
            }

            var characteristicResult = await service.GetCharacteristicsForUuidAsync(characteristicsUUID);
            GattCharacteristic characteristic = characteristicResult.Characteristics.First();
            List<byte[]> contents = new List<byte[]>();
            contents.Add(GetFirstEnteteToSendContents());
            //Concat messages
            contents.Add(GetSecondEnteteToSendContents(messages));
            contents.Add(GetThirdEnteteToSendContents());
            contents.Add(GetForthEnteteToSendContents());

            var messageInListByte = transformMessage(messages);
            foreach (var paquet in messageInListByte)
            {
                contents.Add(paquet);
            }

            foreach (var content in contents)
            {
                IBuffer buffer = Windows.Security.Cryptography.CryptographicBuffer.CreateFromByteArray(content);
                await characteristic.WriteValueAsync(buffer);
            }
            Debug.Print("Envoye");


        }

        private List<byte[]> transformMessage(List<string> messages)
        {
            var res = new List<byte[]>();
            var temp = new List<byte[]>();
            //Concatene tout les messages
            var mess = messages[0] + messages[1] + messages[2] + messages[3] + messages[4] + messages[5] + messages[6] + messages[7];

            //Pour chacun des caracteres du message
            foreach (var c in mess)
            {
                byte[] content = new byte[11];
                

                this.characterMapper.TryGetValue(c + "_byte", out content);
                //Si on ne connait pas le caractère on le remplace par un caractère vide
                if (content == null)
                {
                    for (int i = 0; i < 11; i++)
                    {
                        content[i] = 0;
                    }
                }
                temp.Add(content);
            }
            double division = mess.Length * 11.0 / 16.0;
            int nbPaquets = 0;
            if (division % 1 == 0)
            {
                nbPaquets = (int)division;
            }
            else
            {
                nbPaquets = (int)Math.Ceiling(division);
            }

            int compt = 0;
            int currentKey = 0;
            for (int i = 0; i < nbPaquets; i++)
            {
                var final = new byte[16];
                for (int i2 = 0; i2 < 16; i2++)
                {
                    if (compt < mess.Length)
                    {
                        final[i2] = temp[compt][currentKey];
                        currentKey++;
                        if (currentKey == 11)
                        {
                            currentKey = 0;
                            compt++;
                        }
                    }
                    else
                    {
                        final[i2] = 0;
                    }
                }
                res.Add(final);
            }
            return res;
        }
        #endregion

        #endregion

        #region Init Mapper
        private void InitMappeur()
        {
            this.characterMapper = new Dictionary<string, byte[]>();

            //Majuscules
            this.characterMapper.Add("A_byte", new byte[] { 0, 126, 129, 129, 129, 126, 129, 129, 129, 126, 0 });
            this.characterMapper.Add("B_byte", new byte[] { 0, 255, 137, 137, 137, 126, 137, 137, 137, 255, 0 });
            this.characterMapper.Add("C_byte", new byte[] { 0, 62, 65, 129, 129, 128, 129, 129, 65, 62, 0 });
            this.characterMapper.Add("D_byte", new byte[] { 0, 254, 145, 137, 137, 137, 137, 137, 145, 254, 0 });
            this.characterMapper.Add("E_byte", new byte[] { 0, 255, 129, 129, 129, 126, 129, 129, 129, 255, 0 });
            this.characterMapper.Add("F_byte", new byte[] { 0, 255, 129, 129, 129, 126, 129, 129, 129, 129, 0 });
            this.characterMapper.Add("G_byte", new byte[] { 0, 62, 65, 129, 129, 128, 133, 133, 65, 62, 0 });
            this.characterMapper.Add("H_byte", new byte[] { 0, 129, 129, 129, 129, 126, 129, 129, 129, 129, 0 });
            this.characterMapper.Add("I_byte", new byte[] { 0, 62, 8, 8, 8, 8, 8, 8, 8, 62, 0 });
            this.characterMapper.Add("J_byte", new byte[] { 0, 2, 1, 1, 1, 1, 129, 129, 65, 62, 0 });
            this.characterMapper.Add("K_byte", new byte[] { 0, 129, 133, 137, 145, 162, 145, 137, 133, 129, 0 });
            this.characterMapper.Add("L_byte", new byte[] { 0, 129, 129, 129, 129, 129, 129, 129, 129, 255, 0 });
            this.characterMapper.Add("M_byte", new byte[] { 0, 129, 195, 165, 165, 153, 129, 129, 129, 129, 0 });
            this.characterMapper.Add("N_byte", new byte[] { 0, 129, 193, 161, 145, 137, 133, 131, 129, 129, 0 });
            this.characterMapper.Add("O_byte", new byte[] { 0, 62, 65, 129, 129, 129, 129, 129, 65, 62, 0 });
            this.characterMapper.Add("P_byte", new byte[] { 0, 254, 145, 145, 145, 254, 128, 128, 128, 128, 0 });
            this.characterMapper.Add("Q_byte", new byte[] { 0, 62, 65, 129, 129, 129, 145, 145, 73, 190, 128 });
            this.characterMapper.Add("R_byte", new byte[] { 0, 254, 145, 145, 145, 254, 136, 137, 133, 129, 0 });
            this.characterMapper.Add("S_byte", new byte[] { 0, 62, 65, 128, 128, 62, 1, 1, 129, 62, 0 });
            this.characterMapper.Add("T_byte", new byte[] { 0, 255, 8, 8, 8, 8, 8, 8, 8, 8, 0 });
            this.characterMapper.Add("U_byte", new byte[] { 0, 129, 129, 129, 129, 129, 129, 129, 65, 62, 0 });
            this.characterMapper.Add("V_byte", new byte[] { 0, 129, 129, 129, 129, 66, 66, 36, 24, 24, 0 });
            this.characterMapper.Add("W_byte", new byte[] { 0, 129, 129, 129, 153, 165, 165, 165, 66, 66, 0 });
            this.characterMapper.Add("X_byte", new byte[] { 0, 129, 66, 36, 24, 24, 36, 66, 129, 129, 0 });
            this.characterMapper.Add("Y_byte", new byte[] { 0, 129, 66, 36, 24, 24, 24, 24, 24, 60, 0 });
            this.characterMapper.Add("Z_byte", new byte[] { 0, 255, 2, 4, 8, 16, 32, 64, 128, 255, 0 });

            //Minuscules
            this.characterMapper.Add("a_byte", new byte[] { 0, 0, 30, 33, 65, 65, 65, 65, 63, 0, 0 });
            this.characterMapper.Add("b_byte", new byte[] { 0, 192, 192, 224, 208, 200, 200, 200, 248, 0, 0 });
            this.characterMapper.Add("c_byte", new byte[] { 0, 0, 60, 66, 128, 128, 128, 66, 60, 0, 0 });
            this.characterMapper.Add("d_byte", new byte[] { 0, 28, 12, 12, 12, 12, 140, 204, 120, 0, 0 });
            this.characterMapper.Add("e_byte", new byte[] { 0, 0, 60, 66, 126, 128, 128, 66, 60, 0, 0 });
            this.characterMapper.Add("f_byte", new byte[] { 0, 14, 17, 16, 60, 16, 16, 16, 40, 0, 0 });
            this.characterMapper.Add("g_byte", new byte[] { 0, 0, 60, 66, 66, 66, 62, 2, 60, 66, 60 });
            this.characterMapper.Add("h_byte", new byte[] { 0, 192, 192, 224, 208, 200, 200, 200, 200, 0, 0 });
            this.characterMapper.Add("i_byte", new byte[] { 0, 24, 0, 56, 24, 24, 24, 24, 60, 0, 0 });
            this.characterMapper.Add("j_byte", new byte[] { 0, 6, 0, 14, 6, 6, 6, 70, 58, 0, 0 });
            this.characterMapper.Add("k_byte", new byte[] { 0, 192, 192, 200, 208, 240, 208, 200, 196, 0, 0 });
            this.characterMapper.Add("l_byte", new byte[] { 0, 56, 24, 24, 24, 24, 24, 24, 60, 0, 0 });
            this.characterMapper.Add("m_byte", new byte[] { 0, 0, 198, 255, 146, 146, 146, 146, 146, 0, 0 });
            this.characterMapper.Add("n_byte", new byte[] { 0, 0, 216, 236, 212, 212, 204, 204, 204, 0, 0 });
            this.characterMapper.Add("o_byte", new byte[] { 0, 0, 56, 68, 130, 130, 130, 68, 56, 0, 0 });
            this.characterMapper.Add("p_byte", new byte[] { 0, 240, 104, 100, 100, 120, 96, 96, 240, 0, 0 });
            this.characterMapper.Add("q_byte", new byte[] { 0, 0, 60, 66, 66, 66, 62, 2, 6, 0, 0 });
            this.characterMapper.Add("r_byte", new byte[] { 0, 0, 216, 236, 212, 212, 192, 192, 192, 0, 0 });
            this.characterMapper.Add("s_byte", new byte[] { 0, 0, 126, 128, 124, 2, 2, 130, 124, 0, 0 });
            this.characterMapper.Add("t_byte", new byte[] { 0, 16, 16, 60, 16, 16, 16, 17, 14, 0, 0 });
            this.characterMapper.Add("u_byte", new byte[] { 0, 0, 68, 68, 68, 68, 60, 0, 0, 0, 0 });
            this.characterMapper.Add("v_byte", new byte[] { 0, 0, 68, 68, 68, 40, 16, 0, 0, 0, 0 });
            this.characterMapper.Add("w_byte", new byte[] { 0, 0, 146, 146, 146, 146, 146, 84, 40, 0, 0 });
            this.characterMapper.Add("x_byte", new byte[] { 0, 0, 132, 72, 48, 48, 72, 132, 0, 0, 0 });
            this.characterMapper.Add("y_byte", new byte[] { 0, 0, 68, 68, 68, 60, 4, 4, 24, 0, 0 });
            this.characterMapper.Add("z_byte", new byte[] { 0, 0, 126, 64, 32, 16, 8, 4, 126, 0, 0 });

            //Symboles et ponctuations
            this.characterMapper.Add("!_byte", new byte[] { 0, 0, 0, 126, 0, 0, 0, 0, 0, 0, 0 });
            this.characterMapper.Add("\"_byte", new byte[] { 0, 6, 6, 0, 0, 0, 0, 0, 0, 0, 0 });
            this.characterMapper.Add("#_byte", new byte[] { 0, 20, 126, 20, 20, 126, 20, 0, 0, 0, 0 });
            this.characterMapper.Add("$_byte", new byte[] { 0, 36, 42, 122, 42, 18, 0, 0, 0, 0, 0 });
            this.characterMapper.Add("%_byte", new byte[] { 0, 98, 100, 8, 16, 38, 70, 0, 0, 0, 0 });
            this.characterMapper.Add("&_byte", new byte[] { 0, 60, 74, 86, 98, 70, 60, 0, 0, 0, 0 });
            this.characterMapper.Add("'_byte", new byte[] { 0, 0, 6, 6, 0, 0, 0, 0, 0, 0, 0 });
            this.characterMapper.Add("(_byte", new byte[] { 0, 0, 28, 34, 66, 0, 0, 0, 0, 0, 0 });
            this.characterMapper.Add(")_byte", new byte[] { 0, 0, 66, 34, 28, 0, 0, 0, 0, 0, 0 });
            this.characterMapper.Add("*_byte", new byte[] { 0, 8, 42, 28, 42, 8, 0, 0, 0, 0, 0 });
            this.characterMapper.Add("+_byte", new byte[] { 0, 8, 8, 62, 8, 8, 0, 0, 0, 0, 0 });
            this.characterMapper.Add(",_byte", new byte[] { 0, 0, 0, 0, 0, 24, 24, 8, 16, 0, 0 });
            this.characterMapper.Add("-_byte", new byte[] { 0, 0, 0, 0, 62, 0, 0, 0, 0, 0, 0 });
            this.characterMapper.Add("._byte", new byte[] { 0, 0, 0, 0, 0, 24, 24, 0, 0, 0, 0 });
            this.characterMapper.Add("/_byte", new byte[] { 0, 2, 4, 8, 16, 32, 64, 0, 0, 0, 0 });
            this.characterMapper.Add(":_byte", new byte[] { 0, 0, 24, 24, 0, 24, 24, 0, 0, 0, 0 });
            this.characterMapper.Add(";_byte", new byte[] { 0, 0, 24, 24, 0, 24, 24, 8, 16, 0, 0 });
            this.characterMapper.Add("<_byte", new byte[] { 0, 0, 8, 16, 32, 16, 8, 0, 0, 0, 0 });
            this.characterMapper.Add("=_byte", new byte[] { 0, 0, 0, 62, 0, 62, 0, 0, 0, 0, 0 });
            this.characterMapper.Add(">_byte", new byte[] { 0, 0, 32, 16, 8, 16, 32, 0, 0, 0, 0 });
            this.characterMapper.Add("?_byte", new byte[] { 0, 0, 36, 66, 8, 16, 0, 16, 0, 0, 0 });
            this.characterMapper.Add("@_byte", new byte[] { 0, 60, 66, 153, 161, 157, 64, 60, 0, 0, 0 });
            this.characterMapper.Add("[_byte", new byte[] { 0, 0, 126, 66, 66, 66, 66, 126, 0, 0, 0 });
            this.characterMapper.Add("]_byte", new byte[] { 0, 0, 126, 66, 66, 66, 66, 126, 0, 0, 0 });
            this.characterMapper.Add("^_byte", new byte[] { 8, 20, 34, 0, 0, 0, 0, 0, 0, 0, 0 });
            this.characterMapper.Add("__byte", new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 126 });
            this.characterMapper.Add("`_byte", new byte[] { 0, 0, 16, 8, 0, 0, 0, 0, 0, 0, 0 });
            this.characterMapper.Add("{_byte", new byte[] { 0, 0, 8, 16, 32, 16, 8, 0, 0, 0, 0 });
            this.characterMapper.Add("|_byte", new byte[] { 0, 0, 24, 24, 24, 24, 24, 0, 0, 0, 0 });
            this.characterMapper.Add("}_byte", new byte[] { 0, 0, 32, 16, 8, 16, 32, 0, 0, 0, 0 });
            this.characterMapper.Add("~_byte", new byte[] { 0, 0, 40, 68, 0, 0, 0, 0, 0, 0, 0 });
            this.characterMapper.Add(" _byte", new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

            //Chiffres
            this.characterMapper.Add("0_byte", new byte[] { 0, 60, 66, 66, 66, 66, 66, 66, 66, 60, 0 });
            this.characterMapper.Add("1_byte", new byte[] { 0, 24, 24, 24, 24, 24, 24, 24, 24, 24, 0 });
            this.characterMapper.Add("2_byte", new byte[] { 0, 60, 66, 2, 4, 8, 16, 32, 64, 126, 0 });
            this.characterMapper.Add("3_byte", new byte[] { 0, 60, 66, 2, 4, 28, 4, 2, 66, 60, 0 });
            this.characterMapper.Add("4_byte", new byte[] { 0, 4, 12, 20, 36, 68, 126, 4, 4, 4, 0 });
            this.characterMapper.Add("5_byte", new byte[] { 0, 126, 64, 64, 120, 2, 2, 2, 66, 60, 0 });
            this.characterMapper.Add("6_byte", new byte[] { 0, 28, 32, 64, 120, 66, 66, 66, 66, 60, 0 });
            this.characterMapper.Add("7_byte", new byte[] { 0, 126, 2, 4, 8, 16, 32, 32, 32, 32, 0 });
            this.characterMapper.Add("8_byte", new byte[] { 0, 60, 66, 66, 66, 60, 66, 66, 66, 60, 0 });
            this.characterMapper.Add("9_byte", new byte[] { 0, 60, 66, 66, 66, 62, 2, 2, 4, 56, 0 });
        }
        #endregion
    }


}
