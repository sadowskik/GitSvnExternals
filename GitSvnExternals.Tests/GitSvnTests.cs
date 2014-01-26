using System;
using System.Linq;
using GitSvnExternals.Core;
using NFluent;
using Xunit;

namespace GitSvnExternals.Tests
{
    public class GitSvnTests
    {
        private readonly GitSvn _gitSvn;
        
        public GitSvnTests()
        {
            _gitSvn = new GitSvn(@"C:\Projects\testsvngit2", new FakeRunner {SvnExternals = Externals});
        }

        [Fact]        
        public void checks_if_dir_is_a_git_repository()
        {
            Check.That(_gitSvn.IsGitSvnRepo).IsTrue();
        }

        [Fact]               
        public void retrives_git_svn_externals()
        {
            var externals = _gitSvn.Externals.ToList();

            Check.That(externals).ContainsExactly(new[]
            {
                new SvnExternal(new Uri(@"https://subversion.assembla.com/svn/svnandgittest/trunk/Externals"), ".buildtools"),
                new SvnExternal(new Uri(@"https://subversion.assembla.com/svn/svnandgittest/trunk/ExternalsWithFile1/x.txt"), "x.txt"),
                new SvnExternal(new Uri(@"https://subversion.assembla.com/svn/svnandgittest/trunk/ExternalsWithFile2/y.txt"), "y.txt")
            });
        }

        private const string Externals = @"
# /
/https://subversion.assembla.com/svn/svnandgittest/trunk/Externals .buildtools
/https://subversion.assembla.com/svn/svnandgittest/trunk/ExternalsWithFile1/x.txt x.txt
/https://subversion.assembla.com/svn/svnandgittest/trunk/ExternalsWithFile2/y.txt y.txt";
    }
}
