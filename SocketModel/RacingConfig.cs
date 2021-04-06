namespace SocketModel
{
    public class RacingConfig
    {
        public RacingConfig(int minSpeed, int maxSpeed)
        {
            MinSpeed = minSpeed;
            MaxSpeed = maxSpeed;
        }

        public int MinSpeed { get; private set; }
        public int MaxSpeed { get; private set; }
    }
}
