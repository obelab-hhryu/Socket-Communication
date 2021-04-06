using System.Runtime.Serialization;

namespace SocketModel
{
    public class MeasurementStage
    {
        public MeasurementStage(MeasurementState stage)
        {
            Stage = stage;
        }

        public MeasurementState Stage { get; private set; }
    }
}
