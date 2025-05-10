using RoutersConfigure;
namespace RoutersTests
{
    public class Tests
    {
        List<string> testExamplesAreConnective = [];
        List<string> testExamplesAreNotConnective = [];
        List<string> testExamplesHaveCycle = [];
        List<string> testExamplesHaveNotCycle = [];
        [SetUp]
        public void Setup()
        {
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
                "1: 2 (1), 3 (1)\n");
        }

        [Test]
        public void TestsOfCorrectWorkOfConnectivityCheck()
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
        public void TestsOfCorrectWorkOfCyclicCheck()
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
    }
}
