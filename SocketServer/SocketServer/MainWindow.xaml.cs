using SocketModel;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

        private static readonly string[] JsonFilePaths = new string[]
        {
            Path.Combine(Directory.GetCurrentDirectory(), "json_sample", "common_config.json"),
            Path.Combine(Directory.GetCurrentDirectory(), "json_sample", "racing_config.json"),
            Path.Combine(Directory.GetCurrentDirectory(), "json_sample", "basketball_config.json"),
            Path.Combine(Directory.GetCurrentDirectory(), "json_sample", "egg_config.json"),
            Path.Combine(Directory.GetCurrentDirectory(), "json_sample", "mea_stage_started.json"),
            Path.Combine(Directory.GetCurrentDirectory(), "json_sample", "measurement.json"),
            Path.Combine(Directory.GetCurrentDirectory(), "json_sample", "mea_stage_finished.json"),
            Path.Combine(Directory.GetCurrentDirectory(), "json_sample", "rank.json"),
        };

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
                            foreach (var item in args.Result.UserScores)
                            {
                                this.logManager.Items.Add($"User: {item.User.Name}/{item.User.School}, Score: {item.Score}");
                                this.logManager.Items.Add("--------------------------------------------------------");
                            }

                            var data = ConvertJsonToByteArray(JsonFilePaths[7]);
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

                var json = ConvertJsonToByteArray(JsonFilePaths[0]);
                args.Socket.Send(json);

                Thread.Sleep(500);

                Dispatcher.Invoke(delegate
                {
                    this.logManager.Items.Add("Send Common config.");
                });

                json = ConvertJsonToByteArray(_gameConfigJson);
                args.Socket.Send(json);

                Dispatcher.Invoke(delegate
                {
                    this.logManager.Items.Add("Send Game config.");
                });

                Thread.Sleep(500);

                json = ConvertJsonToByteArray(JsonFilePaths[4]);
                args.Socket.Send(json);

                Dispatcher.Invoke(delegate
                {
                    this.logManager.Items.Add("Send MeasurementStage.");
                });

                Thread.Sleep(500);

                for (int i = 0; i < 10; i++)
                {
                    var measurement = ConvertJsonToByteArray(JsonFilePaths[5]);
                    args.Socket.Send(measurement);

                    Dispatcher.Invoke(delegate
                    {
                        this.logManager.Items.Add("Send measurement value.");
                    });

                    Thread.Sleep(500);
                }

                json = ConvertJsonToByteArray(JsonFilePaths[6]);
                args.Socket.Send(json);

                Dispatcher.Invoke(delegate
                {
                    this.logManager.Items.Add("Send MeasurementStage.");
                });
            }
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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems[0] is ComboBoxItem ctrl)
            {
                string content = ctrl.Content.ToString();

                if (content == "레이싱")
                {
                    _gameConfigJson = JsonFilePaths[1];
                    _gameProcessPath = Path.Combine(Directory.GetCurrentDirectory(), "Games", "Racing");
                }
                else if (content == "농구")
                {
                    _gameConfigJson = JsonFilePaths[2];
                    _gameProcessPath = Path.Combine(Directory.GetCurrentDirectory(), "Games", "BasketBall");
                }
                else if (content == "알키우기")
                {
                    _gameConfigJson = JsonFilePaths[3];
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

