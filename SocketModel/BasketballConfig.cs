using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketModel
{
    public class BasketballConfig
    {
        public BasketballConfig(int maxShootingCount, ShootingPosition[] shootingPositions)
        {
            MaxShootingCount = maxShootingCount;
            ShootingPositions = shootingPositions;
        }

        public int MaxShootingCount { get; private set; }
        public ShootingPosition[] ShootingPositions { get; private set; }
    }
}
