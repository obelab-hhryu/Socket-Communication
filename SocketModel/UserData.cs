using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketModel
{
    public class UserData
    {
        public UserData(string name, string school, int value)
        {
            Name = name;
            School = school;
            Value = value;
        }

        public string Name { get; }
        public string School { get; }
        public int Value { get; }
    }
}
