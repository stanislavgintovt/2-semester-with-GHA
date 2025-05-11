using RoutersConfigure;
namespace RoutersTests
{
    public class Tests
    {
        List<string> testExamplesAreConnective;
        List<string> testExamplesAreNotConnective;
        List<string> testExamplesHaveCycle;
        List<string> testExamplesHaveNotCycle;
        [SetUp]
        public void Setup()
        {
            testExamplesAreConnective = [];
            testExamplesAreNotConnective = [];
            testExamplesHaveCycle = [];
            testExamplesHaveNotCycle = [];
            testExamplesAreNotConnective.Add(
                "1: 2 (1), 3 (1)\n" +
                "2: 3 (1)\n" +
                "4: 5 (1), 6 (1)\n" +
                "5: 6 (1)");
            testExamplesAreConnective.Add(
                "1: 2 (1), 3 (1)\n" +
                "2: 3 (1)\n");
            testExamplesHaveCycle.Add(
                "1: 2 (1), 3 (1)\n" +
                "2: 3 (1)");
            testExamplesHaveCycle.Add(
                "1: 2 (1)\n" +
                "3: 4 (1), 5 (1)\n" +
                "4: 5 (1)");
            testExamplesHaveNotCycle.Add(
                "1: 2 (1), 3 (1)");
        }

        [Test]
        public void TestsForConnectivityCheck()
        {
            foreach (string testString in testExamplesAreNotConnective)
            {
                Assert.That(!Program.ReadGraphFromString(testString).IsConnective());
            }
            foreach (string testString in testExamplesAreConnective)
            {
                Assert.That(Program.ReadGraphFromString(testString).IsConnective());
            }
        }
        [Test]
        public void TestsForCyclicCheck()
        {
            foreach (string testString in testExamplesHaveNotCycle)
            {
                Assert.That(!Program.ReadGraphFromString(testString).HasCycle());
            }
            Assert.That((!Program.ReadGraphFromString(testExamplesHaveCycle[1]).HasCycle()));
            Assert.That(Program.ReadGraphFromString(testExamplesHaveCycle[1]).HasCycle(4));
        }
    }
}
