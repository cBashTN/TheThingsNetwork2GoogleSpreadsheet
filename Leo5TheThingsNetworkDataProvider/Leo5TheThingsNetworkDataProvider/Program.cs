namespace Leo5TheThingsNetworkDataProvider
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var run = new TheThingsNetworkConnector();
            while (true)
            {
                run.ConnectAndListen();
            }
        }
    }
}