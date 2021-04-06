namespace SocketModel
{
    public class Result
    {
        public Result(UserScore[] userScores)
        {
            UserScores = userScores;
        }

        public UserScore[] UserScores { get; private set; }
    }
}
