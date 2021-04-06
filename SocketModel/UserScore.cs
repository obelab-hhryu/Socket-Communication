using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketModel
{
    public class UserScore
    {
        public UserScore(UserInfo user, double score)
        {
            User = user;
            Score = score;
        }

        public UserInfo User { get; private set; }
        public double Score { get; private set; }
    }
}
