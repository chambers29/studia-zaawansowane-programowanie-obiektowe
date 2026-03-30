namespace IoC.lab.app
{
    class Worker // wstrzykiwanie przez konstruktor
    {
        public Worker(ILogger log)
        {
            m_log = log;
        }
        public void Work()
        {
            m_log.Log("begin");
            m_log.Log("end");
        }
        private ILogger m_log;
    }
}
