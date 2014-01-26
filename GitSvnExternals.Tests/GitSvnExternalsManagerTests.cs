using System;
using System.IO;
using System.Linq;
using GitSvnExternals.Core;
using NFluent;
using Xunit;

namespace GitSvnExternals.Tests
{
    public class GitSvnExternalsManagerTests
    {
        private readonly GitSvnExternalsManager _gitSvnExternalsManager;
        private readonly FakeRunner _mockedRunner;

        public GitSvnExternalsManagerTests()
        {
            _mockedRunner = new FakeRunner {ReturnedSvnExternals = Externals};

            _gitSvnExternalsManager = new GitSvnExternalsManager(
                repoPath: TestRepoPath,
                commandRunner: _mockedRunner);
        }

        [Fact]        
        public void checks_if_dir_is_a_git_repository()
        {
            Check.That(_gitSvnExternalsManager.IsGitSvnRepo).IsTrue();
        }

        [Fact]               
        public void retrives_git_svn_externals()
        {
            var externals = _gitSvnExternalsManager.Externals.ToList();

            Check.That(externals).ContainsExactly(new SvnExternal[]
            {
                new DirectoryExternal(
                    new Uri(@"https://subversion.assembla.com/svn/svnandgittest/trunk/Externals"),
                    ".buildtools"),

                new FileExternal(new Uri(
                    @"https://subversion.assembla.com/svn/svnandgittest/trunk/ExternalsWithFile1/x.txt"),
                    "x.txt"),

                new FileExternal(
                    new Uri(@"https://subversion.assembla.com/svn/svnandgittest/trunk/ExternalsWithFile2/y.txt"),
                    "y.txt")
            });
        }

        [Fact]
        public void stores_externals_in_specific_directory()
        {
            _gitSvnExternalsManager.Clone(new DirectoryExternal(
                remotePath: new Uri(@"https://subversion.assembla.com/svn/svnandgittest/trunk/Externals"),
                localPath: ".buildtools"));

            Check.That(Directory.Exists(TestRepoPath + @"\.git_externals")).IsTrue();
        }

        [Fact]
        public void clones_external_as_a_separate_repository()
        {
            _gitSvnExternalsManager.Clone(new DirectoryExternal(
                remotePath: new Uri(@"https://subversion.assembla.com/svn/svnandgittest/trunk/Externals"),
                localPath: ".buildtools"));

            var cloneCommand = _mockedRunner.ExecutedCommands[0];

            Check.That(cloneCommand.Command).IsEqualTo("git");
            Check.That(cloneCommand.Arguments).IsEqualTo(@"svn clone -r HEAD https://subversion.assembla.com/svn/svnandgittest/trunk/Externals .git_externals\.buildtools");
        }

        private const string TestRepoPath = @"C:\Projects\testsvngit2";

        private const string Externals = @"
# /
/https://subversion.assembla.com/svn/svnandgittest/trunk/Externals .buildtools
/https://subversion.assembla.com/svn/svnandgittest/trunk/ExternalsWithFile1/x.txt x.txt
/https://subversion.assembla.com/svn/svnandgittest/trunk/ExternalsWithFile2/y.txt y.txt";
        
    }
}
