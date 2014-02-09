using System;
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

            var external = new TestablesDirectoryExternal(
                remotePath: new Uri(@"https://subversion.assembla.com/svn/svnandgittest/trunk/Externals"),
                localPath: ".buildtools");

            external.Clone(runner, TestRepoPath);

            var cloneCommand = runner.ExecutedCommands[0];
            Check.That(cloneCommand.Command).IsEqualTo("git");
            Check.That(cloneCommand.Arguments).IsEqualTo(@"svn clone -r HEAD https://subversion.assembla.com/svn/svnandgittest/trunk/Externals git_externals\svn\svnandgittest\trunk\Externals");
        }

        [Fact]
        public void links_external_to_cloned_dir()
        {
            var external = new TestablesDirectoryExternal(
                remotePath: new Uri(@"https://subversion.assembla.com/svn/svnandgittest/trunk/Externals"),
                localPath: ".buildtools");

            external.Link(TestRepoPath);

            Check.That(external.LinkPath).IsEqualTo(TestRepoPath + @"\.buildtools");
            Check.That(external.TargetPath).IsEqualTo(TestRepoPath + @"\git_externals\svn\svnandgittest\trunk\Externals");            
        }
    }

    public class TestablesDirectoryExternal : DirectoryExternal
    {
        public TestablesDirectoryExternal(Uri remotePath, string localPath)
            : base(remotePath, localPath)
        {
        }

        protected override bool CreateLink(string link, string target, LinkTypeFlag type)
        {
            LinkPath = link;
            TargetPath = target;
            return true;
        }

        public string LinkPath { get; private set; }

        public string TargetPath { get; private set; }        
    }
}