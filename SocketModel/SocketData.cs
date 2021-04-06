using System;

namespace SocketModel
{
    public class SocketData : EventArgs
    {
        public SocketData(DataType type, CommonConfig commonConfig = null,
            MeasurementStage measurementStage = null, Measurement measurement = null,
            Result result = null, Rank rank = null)
        {
            Type = type;
            CommonConfig = commonConfig;
            MeasurementStage = measurementStage;
            Measurement = measurement;
            Result = result;
            Rank = rank;
        }

        public DataType Type { get; }
        public CommonConfig CommonConfig { get; }
        public MeasurementStage MeasurementStage { get; }
        public Measurement Measurement { get; }
        public Result Result { get; }
        public Rank Rank { get; }
    }
}