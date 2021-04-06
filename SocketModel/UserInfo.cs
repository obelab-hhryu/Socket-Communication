using System.Runtime.Serialization;

namespace SocketModel
{
    public class UserInfo
    {
        public UserInfo(string name, string school)
        {
            Name = name;
            School = school;
        }

        public string Name { get; private set; }
        public string School { get; private set; }
    }
}
