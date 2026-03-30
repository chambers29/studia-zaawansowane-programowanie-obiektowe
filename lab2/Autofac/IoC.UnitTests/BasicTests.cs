using System;
using System.IO;
using Autofac;
using FluentAssertions;
using Xunit;

namespace IoC.UnitTests
{
    // Sample service to test lifetimes, added for the new tests
    public interface IUnitOfWork
    {
        Guid Id { get; }
    }

    public class UnitOfWork : IUnitOfWork
    {
        public Guid Id { get; } = Guid.NewGuid();
    }

    public class BasicTests : IDisposable
    {
        // Nowa metoda buduj¹ca kontener Autofac na podstawie konfiguracji z kodu C#
        //public static IContainer BuildContainerFromAutofacCode()
        //{
        //    var builder = new ContainerBuilder();

        //    // Odpowiednik Unity: cont.RegisterType<ICalculator, CatCalc>("catcalc");
        //    builder.RegisterType<CatCalc>().Named<ICalculator>("catcalc");

        //    // Odpowiednik Unity: cont.RegisterType<ICalculator, PlusCalc>("pluscalc");
        //    builder.RegisterType<PlusCalc>().Named<ICalculator>("pluscalc");

        //    // Odpowiednik Unity: cont.RegisterType<ICalculator, StateCalc>("statecalc", new ContainerControlledLifetimeManager(), new InjectionConstructor(1));
        //    builder.RegisterType<StateCalc>()
        //           .Named<ICalculator>("statecalc")
        //           .WithParameter("startingNumber", 1) // Wstrzykiwanie wartoœci prymitywnej do konstruktora
        //           .SingleInstance(); // Odpowiednik ContainerControlledLifetimeManager (singleton)

        //    // --- Rejestracje dla LabWorker (Constructor Injection) ---
        //    // Odpowiednik Unity: cont.RegisterType<LabWorker>(new InjectionConstructor(new ResolvedParameter("catcalc")));
        //    builder.Register(c => new LabWorker(c.ResolveNamed<ICalculator>("catcalc")))
        //           .As<LabWorker>();

        //    // Odpowiednik Unity: cont.RegisterType<LabWorker>("state", new InjectionConstructor(new ResolvedParameter("statecalc")));
        //    builder.Register(c => new LabWorker(c.ResolveNamed<ICalculator>("statecalc")))
        //           .Named<LabWorker>("state");

        //    // --- Rejestracje dla LabWorker2 (Method/Setter Injection) ---
        //    // Odpowiednik Unity: new InjectionMethod("SetCalculator", new ResolvedParameter("pluscalc"))
        //    // W Autofac realizuje siê to przez zdarzenie OnActivated
        //    builder.RegisterType<LabWorker2>()
        //           .OnActivated(e => e.Instance.SetCalculator(e.Context.ResolveNamed<ICalculator>("pluscalc")));

        //    builder.RegisterType<LabWorker2>()
        //           .Named<LabWorker2>("state")
        //           .OnActivated(e => e.Instance.SetCalculator(e.Context.ResolveNamed<ICalculator>("statecalc")));

        //    // --- Rejestracje dla LabWorker3 (Property Injection) ---
        //    // Odpowiednik Unity: new InjectionProperty("Calculator", new ResolvedParameter("catcalc"))
        //    // W Autofac mo¿na to zrealizowaæ przy tworzeniu obiektu lub przez OnActivated
        //    builder.Register(c => new LabWorker3 { Calculator = c.ResolveNamed<ICalculator>("catcalc") })
        //           .As<LabWorker3>();

        //    builder.Register(c => new LabWorker3 { Calculator = c.ResolveNamed<ICalculator>("statecalc") })
        //           .Named<LabWorker3>("state");

        //    // --- Rejestracje dla testów Lifetime Scopes ---
        //    builder.RegisterType<UnitOfWork>().As<IUnitOfWork>(); // Domyœlnie InstancePerDependency
        //    builder.RegisterType<UnitOfWork>().Named<IUnitOfWork>("single").SingleInstance();
        //    builder.RegisterType<UnitOfWork>().Named<IUnitOfWork>("scoped").InstancePerLifetimeScope();


        //    return builder.Build();
        //}



        //// Nowa metoda buduj¹ca kontener Autofac - odpowiednik konfiguracji z App.config
        //public static IContainer BuildContainerFromAutofacConfigEquivalent()
        //{
        //    var builder = new ContainerBuilder();

        //    // Rejestracje z <aliases> nie s¹ potrzebne, u¿ywamy bezpoœrednio typów

        //    // --- ICalculator registrations ---
        //    builder.RegisterType<CatCalc>().Named<ICalculator>("catcalc");
        //    builder.RegisterType<PlusCalc>().Named<ICalculator>("pluscalc");
        //    builder.RegisterType<StateCalc>().Named<ICalculator>("statecalc")
        //           .WithParameter("startingNumber", 1)
        //           .SingleInstance();

        //    // --- LabWorker registrations ---
        //    builder.Register(c => new LabWorker(c.ResolveNamed<ICalculator>("catcalc"))).As<LabWorker>();
        //    builder.Register(c => new LabWorker(c.ResolveNamed<ICalculator>("statecalc"))).Named<LabWorker>("state");

        //    // --- LabWorker2 registrations ---
        //    builder.RegisterType<LabWorker2>()
        //           .OnActivated(e => e.Instance.SetCalculator(e.Context.ResolveNamed<ICalculator>("pluscalc")));
        //    builder.RegisterType<LabWorker2>()
        //           .Named<LabWorker2>("state")
        //           .OnActivated(e => e.Instance.SetCalculator(e.Context.ResolveNamed<ICalculator>("pluscalc"))); // W configu by³o pluscalc

        //    // --- LabWorker3 registrations ---
        //    builder.Register(c => new LabWorker3 { Calculator = c.ResolveNamed<ICalculator>("catcalc") }).As<LabWorker3>();
        //    builder.Register(c => new LabWorker3 { Calculator = c.ResolveNamed<ICalculator>("catcalc") }).Named<LabWorker3>("state"); // W configu by³o catcalc

        //    // --- ILogger registrations ---
        //    builder.RegisterType<Logger>().As<ILogger>();
        //    builder.RegisterType<NumLogger>().Named<ILogger>("numlog")
        //           .WithParameter("i", 1)
        //           .SingleInstance();

        //    // --- Worker registrations ---
        //    builder.RegisterType<MetWorker>().Named<MetWorker>("numwork")
        //           .OnActivated(e => e.Instance.m_log = e.Context.ResolveNamed<ILogger>("numlog"));

        //    // Odpowiednik <instance name="inlog" type="ILogger" value="100" typeConverter="NumLogConv"/>
        //    // W Autofac rejestrujemy po prostu instancjê z wymagan¹ wartoœci¹
        //    builder.Register(c => new NumLogger(100)).Named<ILogger>("inlog");

        //    builder.RegisterType<SetWorker>().Named<SetWorker>("inwork")
        //           .OnActivated(e => e.Instance.SetLogger(e.Context.ResolveNamed<ILogger>("inlog")));

        //    builder.RegisterType<MetWorker>().Named<MetWorker>("num2work")
        //           .OnActivated(e => e.Instance.m_log = e.Context.ResolveNamed<ILogger>("inlog"));
            
        //    // --- Rejestracje dla testów Lifetime Scopes ---
        //    builder.RegisterType<UnitOfWork>().As<IUnitOfWork>(); // Domyœlnie InstancePerDependency
        //    builder.RegisterType<UnitOfWork>().Named<IUnitOfWork>("single").SingleInstance();
        //    builder.RegisterType<UnitOfWork>().Named<IUnitOfWork>("scoped").InstancePerLifetimeScope();


        //    return builder.Build();
        //}


        //private IContainer _container; // Zmiana z UnityContainer na IContainer
        //private readonly StringWriter _fileWriter;

        //public string Trimmed => _fileWriter.ToString().Trim();

        //public BasicTests()
        //{
        //    _fileWriter = new StringWriter();
        //    Console.SetOut(_fileWriter);
        //}

        //// Metoda pomocnicza zosta³a zaktualizowana, aby u¿ywaæ metod Autofac
        //public IContainer BuildContainerBasedOnFlag(bool useAppConfigEquivalent)
        //{
        //    _container = useAppConfigEquivalent ? BuildContainerFromAutofacConfigEquivalent() : BuildContainerFromAutofacCode();
        //    return _container;
        //}

        //[Theory]
        //[InlineData(true)]
        //[InlineData(false)]
        //public void ResolvingCatCalc_ShouldReturnCatCalc(bool useAppConfig)
        //{
        //    BuildContainerBasedOnFlag(useAppConfig);
        //    // Zmiana: Resolve() w Autofac, a nie Resolve<T>()
        //    var catCalc = _container.ResolveNamed<ICalculator>("catcalc");
        //    catCalc.Should().NotBeNull();
        //}

        //[Theory]
        //[InlineData(true)]
        //[InlineData(false)]
        //public void EvalWithCatCalc_ShouldReturn_12(bool useAppConfig)
        //{
        //    BuildContainerBasedOnFlag(useAppConfig);
        //    var catCalc = _container.ResolveNamed<ICalculator>("catcalc");
        //    catCalc.Eval("1", "2").Should().Be("12");
        //}

        //[Theory]
        //[InlineData(true)]
        //[InlineData(false)]
        //public void ResolvingLabWorker_ShouldReturnLabWorker(bool useAppConfig)
        //{
        //    BuildContainerBasedOnFlag(useAppConfig);
        //    var labWorker = _container.Resolve<LabWorker>();
        //    labWorker.Should().NotBeNull();
        //}

        //[Theory]
        //[InlineData(true)]
        //[InlineData(false)]
        //public void WorkWithLabWorker_ShouldReturn_12(bool useAppConfig)
        //{
        //    BuildContainerBasedOnFlag(useAppConfig);
        //    var labWorker = _container.Resolve<LabWorker>();
        //    labWorker.Work("1", "2");
        //    Trimmed.Should().Be("12");
        //}

        //[Theory]
        //[InlineData(true)]
        //[InlineData(false)]
        //public void ResolvingLabWorker2_ShouldReturnLabWorker2(bool useAppConfig)
        //{
        //    BuildContainerBasedOnFlag(useAppConfig);
        //    var labWorker = _container.Resolve<LabWorker2>();
        //    labWorker.Should().NotBeNull();
        //}

        //[Theory]
        //[InlineData(true)]
        //[InlineData(false)]
        //public void WorkWithLabWorker2_ShouldReturn_3(bool useAppConfig)
        //{
        //    BuildContainerBasedOnFlag(useAppConfig);

        //    // W konfiguracji "code" LabWorker2 dostaje pluscalc, a w "config" te¿ pluscalc
        //    var labWorker = _container.Resolve<LabWorker2>();
        //    labWorker.Work("1", "2");
        //    Trimmed.Should().Be("3");
        //}

        //[Theory]
        //[InlineData(true)]
        //[InlineData(false)]
        //public void ResolvingLabWorker3_ShouldReturnLabWorker3(bool useAppConfig)
        //{
        //    BuildContainerBasedOnFlag(useAppConfig);
        //    var labWorker = _container.Resolve<LabWorker3>();
        //    labWorker.Should().NotBeNull();
        //}

        //[Theory]
        //[InlineData(true)]
        //[InlineData(false)]
        //public void WorkWithLabWorker3_ShouldReturn_12(bool useAppConfig)
        //{
        //    BuildContainerBasedOnFlag(useAppConfig);
        //    var labWorker = _container.Resolve<LabWorker3>();
        //    labWorker.Work("1", "2");
        //    Trimmed.Should().Be("12");
        //}

        //[Theory]
        //[InlineData(true)]
        //[InlineData(false)]
        //public void ResolvingPlusCalc_ShouldReturnPlusCalc(bool useAppConfig)
        //{
        //    BuildContainerBasedOnFlag(useAppConfig);
        //    var plusCalc = _container.ResolveNamed<ICalculator>("pluscalc");
        //    plusCalc.Should().NotBeNull();
        //}

        //[Theory]
        //[InlineData(true)]
        //[InlineData(false)]
        //public void EvalWithPlusCalc_ShouldReturn3(bool useAppConfig)
        //{
        //    BuildContainerBasedOnFlag(useAppConfig);
        //    var plusCalc = _container.ResolveNamed<ICalculator>("pluscalc");
        //    plusCalc.Eval("1", "2").Should().Be("3");
        //}

        //[Theory]
        //[InlineData(true)]
        //[InlineData(false)]
        //public void ResolvingStateCalc_ShouldReturnStateCalc(bool useAppConfig)
        //{
        //    BuildContainerBasedOnFlag(useAppConfig);
        //    var stateCalc = _container.ResolveNamed<ICalculator>("statecalc");
        //    stateCalc.Should().NotBeNull();
        //}

        //[Theory]
        //[InlineData(true)]
        //[InlineData(false)]
        //public void ResolvingStateCalcMultipleTimes_ShouldReturnTheSameStateCalc(bool useAppConfig)
        //{
        //    BuildContainerBasedOnFlag(useAppConfig);
        //    var stateCalc = _container.ResolveNamed<ICalculator>("statecalc");
        //    var stateCalc2 = _container.ResolveNamed<ICalculator>("statecalc");
        //    stateCalc.Should().Be(stateCalc2);
        //}

        //[Theory]
        //[InlineData(true)]
        //[InlineData(false)]
        //public void MultipleEvalsWithStateCalc_ShouldReturnIncrementedValue(bool useAppConfig)
        //{
        //    BuildContainerBasedOnFlag(useAppConfig);
        //    var stateCalc = _container.ResolveNamed<ICalculator>("statecalc");
        //    var stateCalc2 = _container.ResolveNamed<ICalculator>("statecalc");
        //    stateCalc.Eval("1", "2").Should().Be("121");
        //    stateCalc2.Eval("1", "2").Should().Be("122");
        //}

        //// --- NOWE I ZAKTUALIZOWANE TESTY DLA LIFETIME SCOPES ---

        //[Theory]
        //[InlineData(true)]
        //[InlineData(false)]
        //public void InstancePerDependency_CreatesNewInstanceEveryTime(bool useAppConfig)
        //{
        //    // Arrange
        //    BuildContainerBasedOnFlag(useAppConfig);

        //    // Act
        //    var uow1 = _container.Resolve<IUnitOfWork>();
        //    var uow2 = _container.Resolve<IUnitOfWork>();

        //    // Assert
        //    uow1.Id.Should().NotBe(uow2.Id, "InstancePerDependency should create new instances for each resolution.");
        //}

        //[Theory]
        //[InlineData(true)]
        //[InlineData(false)]
        //public void SingleInstance_IsSameInRootAndScope(bool useAppConfig)
        //{
        //    // Arrange
        //    BuildContainerBasedOnFlag(useAppConfig);

        //    // Act
        //    var rootInstance = _container.ResolveNamed<IUnitOfWork>("single");
        //    using (var scope = _container.BeginLifetimeScope())
        //    {
        //        var scopeInstance = scope.ResolveNamed<IUnitOfWork>("single");
                
        //        // Assert
        //        rootInstance.Id.Should().Be(scopeInstance.Id, "SingleInstance should be the same across all scopes.");
        //    }
        //}

        //[Theory]
        //[InlineData(true)]
        //[InlineData(false)]
        //public void InstancePerLifetimeScope_IsSameWithinSameScope(bool useAppConfig)
        //{
        //    // Arrange
        //    BuildContainerBasedOnFlag(useAppConfig);

        //    // Act
        //    using (var scope = _container.BeginLifetimeScope())
        //    {
        //        var uow1 = scope.ResolveNamed<IUnitOfWork>("scoped");
        //        var uow2 = scope.ResolveNamed<IUnitOfWork>("scoped");

        //        // Assert
        //        uow1.Id.Should().Be(uow2.Id, "InstancePerLifetimeScope should return the same instance within the same scope.");
        //    }
        //}

        //[Theory]
        //[InlineData(true)]
        //[InlineData(false)]
        //public void InstancePerLifetimeScope_IsDifferentInDifferentScopes(bool useAppConfig)
        //{
        //    // Arrange
        //    BuildContainerBasedOnFlag(useAppConfig);

        //    // Act
        //    IUnitOfWork uow1;
        //    using (var scope1 = _container.BeginLifetimeScope())
        //    {
        //        uow1 = scope1.ResolveNamed<IUnitOfWork>("scoped");
        //    }

        //    IUnitOfWork uow2;
        //    using (var scope2 = _container.BeginLifetimeScope())
        //    {
        //        uow2 = scope2.ResolveNamed<IUnitOfWork>("scoped");
        //    }

        //    // Assert
        //    uow1.Id.Should().NotBe(uow2.Id, "InstancePerLifetimeScope should create different instances for different scopes.");
        //}

        //public void Dispose()
        //{
        //    _container?.Dispose();
        //    _fileWriter.Dispose();
        //}
    }
}

