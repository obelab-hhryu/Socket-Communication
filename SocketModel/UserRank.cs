using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketModel
{
    public class UserRank
    {
        public UserRank(UserInfo user, int ranking)
        {
            User = user;
            Ranking = ranking;
        }

        public UserInfo User { get; }
        public int Ranking { get; }
    }
}
