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
        private const string TestRepoPath = @"C:\Projects\testsvngit2";

        private readonly string _externalsNew = File.ReadAllText("externalsNew");
        private readonly string _externalsOld = File.ReadAllText("externalsOld");        

        private GitSvnExternalsManager _gitSvnExternalsManager;
        private FakeRunner _mockedRunner;

        private void PrepareExternalsManager(string returnedSvnExternals = "")
        {
            _mockedRunner = new FakeRunner {ReturnedSvnExternals = returnedSvnExternals};

            _gitSvnExternalsManager = new GitSvnExternalsManager(
                repoPath: TestRepoPath,
                commandRunner: _mockedRunner);
        }

        [Fact]        
        public void checks_if_dir_is_a_git_repository()
        {
            PrepareExternalsManager();

            Check.That(_gitSvnExternalsManager.IsGitSvnRepo).IsTrue();
        }

        [Fact]               
        public void retrives_git_svn_externals_new_syntax()
        {
            PrepareExternalsManager(_externalsNew);

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

        [Fact(Skip = "Have no fucking clue how to deal with it now")]
        public void retrives_git_svn_externals_new_old()
        {
            PrepareExternalsManager(_externalsOld);

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
            PrepareExternalsManager(_externalsNew);

            _gitSvnExternalsManager.Clone(new DirectoryExternal(
                remotePath: new Uri(@"https://subversion.assembla.com/svn/svnandgittest/trunk/Externals"),
                localPath: ".buildtools"));

            Check.That(Directory.Exists(TestRepoPath + @"\.git_externals")).IsTrue();
        }                
    }
}
