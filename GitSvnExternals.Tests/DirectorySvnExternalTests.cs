using System;
using System.IO;
using GitSvnExternals.Core;
using NFluent;
using Xunit;

namespace GitSvnExternals.Tests
{
    public class DirectorySvnExternalTests
    {
        private const string TestRepoPath = @"C:\Projects\testsvngit2";

        [Fact]
        public void clones_external_as_a_separate_repository()
        {
            var runner = new RunnerMock();

            var external = new DirectoryExternal(
                remotePath: new Uri(@"https://subversion.assembla.com/svn/svnandgittest/trunk/Externals"),
                localPath: ".buildtools");

            external.Clone(runner, TestRepoPath);

            var cloneCommand = runner.ExecutedCommands[0];
            Check.That(cloneCommand.Command).IsEqualTo("git");
            Check.That(cloneCommand.Arguments).IsEqualTo(@"svn clone -r HEAD https://subversion.assembla.com/svn/svnandgittest/trunk/Externals .git_externals\.buildtools");
        }

        [Fact]
        public void links_external_to_cloned_dir()
        {
            var external = new DirectoryExternal(
                remotePath: new Uri(@"https://subversion.assembla.com/svn/svnandgittest/trunk/Externals"),
                localPath: ".buildtools");

            external.Link(TestRepoPath);

            Check.That(Directory.Exists(TestRepoPath + @"\.buildtools")).IsTrue();
        }
    }
}