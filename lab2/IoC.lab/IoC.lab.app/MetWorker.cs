
namespace IoC.lab.app
{
    class MetWorker
    {
        public void Work()
        {
            m_log.Log("begin");
            m_log.Log("end");
        }
        public ILogger m_log { get; set; }
    }

}
