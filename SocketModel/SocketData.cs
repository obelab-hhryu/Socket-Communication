using System;

namespace SocketModel
{
    public class SocketData : EventArgs
    {
        public SocketData(DataType type, CommonConfig commonConfig = null,
            RacingConfig racingConfig = null, BasketballConfig basketballConfig = null, EggConfig eggConfig = null,
            MeasurementStage measurementStage = null, Measurement measurement = null,
            Result result = null, Rank rank = null)
        {
            Type = type;
            CommonConfig = commonConfig;
            RacingConfig = racingConfig;
            BasketballConfig = basketballConfig;
            EggConfig = eggConfig;
            MeasurementStage = measurementStage;
            Measurement = measurement;
            Result = result;
            Rank = rank;
        }

        public DataType Type { get; }
        public CommonConfig CommonConfig { get; }
        public RacingConfig RacingConfig { get; }
        public BasketballConfig BasketballConfig { get; }
        public EggConfig EggConfig { get; }
        public MeasurementStage MeasurementStage { get; }
        public Measurement Measurement { get; }
        public Result Result { get; }
        public Rank Rank { get; }
    }
}