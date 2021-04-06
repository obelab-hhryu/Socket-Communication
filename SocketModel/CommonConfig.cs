using System.Runtime.Serialization;

namespace SocketModel
{
    public class CommonConfig
    {
        public CommonConfig(UserInfo[] users, int playerCount, int sensitivity, int watingTime = 20, int playTime = 30, int resultTime = 15)
        {
            Users = users;
            PlayerCount = playerCount;
            Sensitivity = sensitivity;
            WatingTime = watingTime;
            PlayTime = playTime;
            ResultTime = resultTime;
        }

        public UserInfo[] Users { get; private set; }
        public int PlayerCount { get; private set; }
        public int Sensitivity { get; private set; }
        public int WatingTime { get; private set; }
        public int PlayTime { get; private set; }
        public int ResultTime { get; private set; }
    }
}
