namespace SocketModel
{
    public class ShootingPosition
    {
        public ShootingPosition(double x, double y, double z, int standardValue)
        {
            X = x;
            Y = y;
            Z = z;
            StandardValue = standardValue;
        }

        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }
        public int StandardValue { get; private set; }
    }
}
