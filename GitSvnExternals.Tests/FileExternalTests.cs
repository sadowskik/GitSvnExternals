using System;
using GitSvnExternals.Core;
using NFluent;
using Xunit;

namespace GitSvnExternals.Tests
{
    public class FileExternalTests
    {
        private const string TestRepoPath = @"C:\Projects\testsvngit2";

        [Fact]
        public void clones_external_parent_as_a_separate_repository()
        {
            var runner = new RunnerMock();

            var external = new TestableFileExternal(new Uri(
                @"https://subversion.assembla.com/svn/svnandgittest/trunk/ExternalsWithFile1/y.txt"),
                "x.txt");

            external.Clone(runner, TestRepoPath);

            var cloneCommand = runner.ExecutedCommands[0];
            Check.That(cloneCommand.Command).IsEqualTo("git");
            Check.That(cloneCommand.Arguments).IsEqualTo(@"svn clone -r HEAD https://subversion.assembla.com/svn/svnandgittest/trunk/ExternalsWithFile1 git_externals\svn\svnandgittest\trunk\ExternalsWithFile1");
        }

        [Fact]
        public void links_external_to_file_in_cloned_directory()
        {
            var external = new TestableFileExternal(new Uri(
                @"https://subversion.assembla.com/svn/svnandgittest/trunk/ExternalsWithFile1/y.txt"),
                "x.txt");

            external.Link(TestRepoPath);

            Check.That(external.LinkPath).IsEqualTo(TestRepoPath + @"\x.txt");
            Check.That(external.TargetPath).IsEqualTo(TestRepoPath + @"\git_externals\svn\svnandgittest\trunk\ExternalsWithFile1\y.txt");           
        }
    }

    public class TestableFileExternal : FileExternal
    {
        public TestableFileExternal(Uri remotePath, string localPath) 
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