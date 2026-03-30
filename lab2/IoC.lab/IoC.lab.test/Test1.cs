using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace IoC.lab.test
{
    [TestClass]
    public sealed class TestIoClab

    {
        private const string Expected = "Hello, World!";
        [TestMethod]
        public void TestMainMethodShouldReturnExpectedString()
        {
            var result = String.Empty;
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                HelloWorld hw = new();
                hw.WriteHelloWorldLine();
                result = sw.ToString().Trim();
                Assert.AreEqual(Expected, result);
            }
            Assert.AreEqual(Expected, result);
        }
    }
}
