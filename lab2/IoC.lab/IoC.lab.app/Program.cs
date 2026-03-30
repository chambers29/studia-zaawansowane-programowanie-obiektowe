using Autofac;
using Autofac.Configuration;
using IoC.lab.app;
using Microsoft.Extensions.Configuration;
IContainer container = null;

container = BuildContainerImp();
//container = BuildContainerDec();

var w1 = container.Resolve<Worker>();
w1.Work();
var w2 = container.ResolveNamed<MetWorker>("numwork");
w2.Work();
var w3 = container.ResolveNamed<MetWorker>("numwork");
w3.Work();

static IContainer BuildContainerImp()
{
    var builder = new ContainerBuilder();
    builder.RegisterType<Logger>().As<ILogger>();
    builder.RegisterType<Worker>();
    // Rejestracja nazwanego, singletonowego loggera z parametrem w konstruktorze
    builder.RegisterType<NumLogger>()
            .Named<ILogger>("numlog")
            .WithParameter("i", 1)
            .SingleInstance()
            ;
    // Rejestracja MetWorker z wstrzyknięciem właściwości m_log
    builder.RegisterType<MetWorker>()
            .Named<MetWorker>("numwork")
            .OnActivated(e => e.Instance.m_log = e.Context.ResolveNamed<ILogger>("numlog"));

    var container = builder.Build();
    return container;
}

static IContainer BuildContainerDec()
{
    var builder = new ContainerBuilder();
    var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    builder.RegisterModule(new ConfigurationModule(config.GetSection("autofac")));
    var container = builder.Build();
    return container;
}



//HelloWorld hw = new();
//hw.WriteHelloWorldLine();
//public class HelloWorld
//{
//    public void WriteHelloWorldLine()
//    {
//        Console.WriteLine("Hello, World!");
//    }
//}
