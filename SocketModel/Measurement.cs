using System.Runtime.Serialization;

namespace SocketModel
{
    public class Measurement
    {
        public Measurement(UserData[] userDataArray)
        {
            UserDataArray = userDataArray;
        }

        public UserData[] UserDataArray { get; }
    }
}
