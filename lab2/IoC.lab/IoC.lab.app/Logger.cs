namespace IoC.lab.app
{
    class Logger : ILogger
    {
        public void Log(string s) { Console.WriteLine("logger: " + s); }
    }
}
