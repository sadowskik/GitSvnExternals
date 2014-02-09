using System;
using System.IO;
using GitSvnExternals.Core;
using Moq;
using NFluent;
using Xunit;

namespace GitSvnExternals.Tests
{    
    public class GitSvnExternalsManagerTests
    {
        private const string TestRepoPath = @"C:\Projects\testsvngit2";
                
        private readonly GitSvnExternalsManager _gitSvnExternalsManager;
        private readonly Mock<IParseExternals> _parserMock;

        public GitSvnExternalsManagerTests()
        {
            var mockedRunner = new RunnerMock {ReturnedSvnExternals = "nonEmptyLine"};
            _parserMock = new Mock<IParseExternals>();

            _gitSvnExternalsManager = new GitSvnExternalsManager(
                repoPath: TestRepoPath,
                commandRunner: mockedRunner,
                parser: _parserMock.Object);
        }

        [Fact]        
        public void checks_if_dir_is_a_git_repository()
        {            
            Check.That(_gitSvnExternalsManager.IsGitSvnRepo).IsTrue();
        }

        [Fact]
        public void retrives_svn_externals()
        {            
            _parserMock
                .Setup(x => x.ParseLine(It.IsAny<string>()))
                .Returns(SvnExternal.Empty);

            Check.That(_gitSvnExternalsManager.Externals).IsEmpty();
        }

        [Fact]
        public void ignores_empty_externals()
        {
            var parsedExternal = new DirectoryExternal(
                remotePath: new Uri(@"https://testurl.com"),
                localPath: ".testdir2");

            _parserMock
                .Setup(x => x.ParseLine(It.IsAny<string>()))
                .Returns(parsedExternal);
            
            Check.That(_gitSvnExternalsManager.Externals).ContainsExactly(parsedExternal);
        }

        [Fact]
        public void combines_retrived_externals_with_manually_added()
        {
            var parsedExternal = new DirectoryExternal(
                remotePath: new Uri(@"https://testurl.com"),
                localPath: ".testdir2");

            var manualExternal = new DirectoryExternal(
                remotePath: new Uri(@"https://testurl2.com"),
                localPath: ".testDir2");

            _parserMock
                .Setup(x => x.ParseLine(It.IsAny<string>()))
                .Returns(parsedExternal);

            _gitSvnExternalsManager.IncludeManualExternals(new[] {manualExternal});

            Check.That(_gitSvnExternalsManager.Externals).ContainsExactly(parsedExternal, manualExternal);
        }

        [Fact]
        public void when_manual_external_is_empty_ignore_it()
        {
            var parsedExternal = new DirectoryExternal(
                remotePath: new Uri(@"https://testurl.com"),
                localPath: ".testdir2");

            var manualExternal = SvnExternal.Empty;

            _parserMock
                .Setup(x => x.ParseLine(It.IsAny<string>()))
                .Returns(parsedExternal);

            _gitSvnExternalsManager.IncludeManualExternals(new[] { manualExternal });

            Check.That(_gitSvnExternalsManager.Externals).ContainsExactly(parsedExternal);
        }

        [Fact]
        public void stores_externals_in_specific_directory()
        {            
            _gitSvnExternalsManager.Clone(new DirectoryExternal(
                remotePath: new Uri(@"https://subversion.assembla.com/svn/svnandgittest/trunk/Externals"),
                localPath: ".buildtools"));

            Check.That(Directory.Exists(TestRepoPath + @"\git_externals")).IsTrue();
        }                
    }
}
