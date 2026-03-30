namespace IoC.lab.app
{
    class NumLogger : ILogger
    {
        public NumLogger(int i = 0) { id = i; }
        public void Log(string s) { Console.WriteLine("logger({0}): " + s, id++); }
        private int id;
    }

}
