namespace SkyQuant.Services
{
    public interface ILogger
    {
        public static void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
