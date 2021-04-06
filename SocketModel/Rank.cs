namespace SocketModel
{
    public class Rank
    {
        public Rank(UserRank[] bestArray, UserRank[] worstArray, UserRank[] playerArray)
        {
            BestArray = bestArray;
            WorstArray = worstArray;
            PlayerArray = playerArray;
        }

        public UserRank[] BestArray { get; }
        public UserRank[] WorstArray { get; }
        public UserRank[] PlayerArray { get; }
    }
}
