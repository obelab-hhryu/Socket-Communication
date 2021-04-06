using Newtonsoft.Json;
using SocketModel;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace SocketServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Server _server;

        private string _gameConfigJson = string.Empty;
        private string _gameProcessPath = string.Empty;

        private UserInfo[] _users;
        private int _watingTime = 0;
        private int _playTime = 5;

        private object _lockObj = new object();
        private bool _isMeasuring = false;

        private const int DELAY_SEC = 124; // millisec

        private readonly string[] _jsonFilePathArray = new string[]
        {
            Path.Combine(Directory.GetCurrentDirectory(), "json_sample", "common_config.json"),
            Path.Combine(Directory.GetCurrentDirectory(), "json_sample", "mea_stage_started.json"),
            Path.Combine(Directory.GetCurrentDirectory(), "json_sample", "mea_stage_finished.json"),
            Path.Combine(Directory.GetCurrentDirectory(), "json_sample", "rank.json"),
        };

        //private readonly int[] _measurementValueArray = new int[]
        //{
        //    5, 10 ,15, 20
        //};

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnSelectGame(object sender, RoutedEventArgs e)
        {
            _server = new Server("127.0.0.1", 5555);
            _server.LogGenerated += Server_LogGenerated;
            _server.ClientConnected += Server_ClientConnected;
            _server.GameResultReceived += Server_GameResultReceived;

            GetCommonConfig();
        }

        private void GetCommonConfig()
        {
            string jsonStr = string.Empty;

            using (var sr = new StreamReader(_jsonFilePathArray[0]))
            {
                jsonStr = sr.ReadToEnd();
            }

            if (string.IsNullOrEmpty(jsonStr) == false)
            {
                JsonConvert.DefaultSettings = () => new JsonSerializerSettings { Formatting = Formatting.Indented };
                var jsonObj = JsonConvert.DeserializeObject<SocketData>(jsonStr.ToString());

                _users = jsonObj.CommonConfig.Users;
                _watingTime = jsonObj.CommonConfig.WatingTime;
                _playTime = jsonObj.CommonConfig.PlayTime;
            }
        }

        private void Server_GameResultReceived(object sender, System.EventArgs e)
        {
            if (e is SocketData args)
            {
                Dispatcher.Invoke(delegate
                {
                    switch (args.Type)
                    {
                        case DataType.Result:
                            lock (_lockObj)
                            {
                                _isMeasuring = false;
                            }

                            foreach (var item in args.Result.UserScores)
                            {
                                this.logManager.Items.Add($"User: {item.User.Name}/{item.User.School}, Score: {item.Score}");
                                this.logManager.Items.Add("--------------------------------------------------------");
                            }

                            var data = ConvertJsonToByteArray(_jsonFilePathArray[3]);
                            bool isSent = _server.Send(data);

                            if (isSent)
                            {
                                Dispatcher.Invoke(delegate
                                {
                                    this.logManager.Items.Add("Send rank.");
                                });
                            }
                            break;

                        case DataType.Rank:
                            break;
                    }
                });
            }
        }

        private void Server_ClientConnected(object sender, System.EventArgs e)
        {
            if (e is ClientConnectedEventArgs args)
            {
                Dispatcher.Invoke(delegate
                {
                    this.logManager.Items.Add("Client connected.");
                });

                #region Send Common config
                SendSocket(args.Socket, _jsonFilePathArray[0], "Send Common config.");
                #endregion

                #region Send Start measurement sign.
                SendSocket(args.Socket, _jsonFilePathArray[1], "Send MeasurementStage as start.");
                #endregion

                #region Start sending mesurement.
                //Thread.Sleep((_watingTime - 1) * 1000);
                StartSendingMeasurement(args.Socket);
                #endregion

                #region Send finished measurement sign.
                //SendSocket(args.Socket, _jsonFilePathArray[2], "Send MeasurementStage as finished.");
                #endregion
            }

        }
        private void StartSendingMeasurement(Socket socket)
        {
            Task.Run(() =>
            {
                _isMeasuring = true;

                while (true)
                {
                    var byteDataToSend = GenerateMeasurementPacket();
                    if (byteDataToSend != null)
                    {
                        try
                        {
                            socket.Send(byteDataToSend);
                        }
                        catch (Exception e)
                        {
                        }

                        Thread.Sleep(DELAY_SEC);

                        App.Current.Dispatcher.Invoke(delegate
                        {
                            this.logManager.Items.Add("Send Measurement");
                        });
                    }

                    lock (_lockObj)
                    {
                        if (_isMeasuring == false)
                        {
                            break;
                        }
                    }
                }
            });
        }

        private void Server_LogGenerated(object sender, System.EventArgs e)
        {
            if (e is LogGeneratedEventArgs args)
            {
                Dispatcher.Invoke(delegate
                {
                    this.logManager.Items.Add(args.Message);
                });
            }
        }

        private void SendSocket(Socket socket, string jsonFilePath, string logMessage)
        {
            var json = ConvertJsonToByteArray(jsonFilePath);
            socket.Send(json);

            Thread.Sleep(DELAY_SEC);

            Dispatcher.Invoke(delegate
            {
                this.logManager.Items.Add(logMessage);
            });
        }

        private byte[] GenerateMeasurementPacket()
        {
            if (_users == null) return null;

            var rnd = new Random();
            UserData[] userDataContainedMeasurementArray = new UserData[_users.Length];


            for (int i = 0; i < _users.Length; i++)
            {
                var user = _users[i];
                userDataContainedMeasurementArray[i] = new UserData(user.Name, user.School, rnd.Next(1, 20));
            }

            var meaData = new Measurement(userDataContainedMeasurementArray);
            var data = new SocketData(DataType.Measurement, null, null, meaData, null, null);

            var jsonStr = JsonConvert.SerializeObject(data);

            return Encoding.UTF8.GetBytes(jsonStr);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems[0] is ComboBoxItem ctrl)
            {
                string content = ctrl.Content.ToString();

                if (content == "레이싱")
                {
                    _gameConfigJson = _jsonFilePathArray[1];
                    _gameProcessPath = Path.Combine(Directory.GetCurrentDirectory(), "Games", "Racing");
                }
                else if (content == "농구")
                {
                    _gameConfigJson = _jsonFilePathArray[2];
                    _gameProcessPath = Path.Combine(Directory.GetCurrentDirectory(), "Games", "BasketBall");
                }
                else if (content == "알키우기")
                {
                    _gameConfigJson = _jsonFilePathArray[3];
                    _gameProcessPath = Path.Combine(Directory.GetCurrentDirectory(), "Games", "Egg");
                }
            }
        }

        private byte[] ConvertJsonToByteArray(string filePath)
        {
            using (var sr = new StreamReader(filePath))
            {
                return Encoding.UTF8.GetBytes(sr.ReadToEnd());
            }
        }
    }
}