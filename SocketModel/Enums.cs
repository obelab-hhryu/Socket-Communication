namespace SocketModel
{
    public enum DataType
    {
        CommonConfig,
        RacingConfig,
        BasketballConfig,
        EggConfig,
        MeasurementStage,
        Measurement,
        Result,
        Rank,
    }

    public enum ProcessStateType
    {
        Loaded,
        Unloaded,
    }

    public enum GameType
    {
        Racing,
        Basketball,
        Egg,
    }

    public enum MeasurementState
    {
        Started,
        Finished,
    }
}
