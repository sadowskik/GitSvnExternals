using GitSvnExternals.Core;
using NFluent;
using Xunit;

namespace GitSvnExternals.Tests
{
    [LongRunning]
    public class CommandRunnerTests
    {
        [Fact]
        public void retrives_externals()
        {
            var consoleRunner = new ConsoleRunner();
            var resultReader = consoleRunner.Run("git", "svn show-externals", @"C:\Projects\testsvngit2");

            Check.That(resultReader.ReadToEnd()).IsNotEmpty();
        }
    }
}