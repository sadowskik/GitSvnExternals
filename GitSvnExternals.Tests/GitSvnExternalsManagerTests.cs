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
        public void clones_externals_in_groups_to_avoid_unecessary_clones()
        {
            TestableSvnExternal.ResetCounters();
            _gitSvnExternalsManager.IncludeManualExternals(new SvnExternal[]
            {
                new TestableSvnExternal(willBeClonedTo: @"dir1"),
                new TestableSvnExternal(willBeClonedTo: @"dir1"),
                new TestableSvnExternal(willBeClonedTo: @"dir2"),
                new TestableSvnExternal(willBeClonedTo: @"dir2"),
                new TestableSvnExternal(willBeClonedTo: @"dir3")
            });

            _gitSvnExternalsManager.CloneAllExternals();

            Check.That(TestableSvnExternal.ClonedTimes).IsEqualTo(3);
        }

        [Fact]
        public void each_external_is_linked_to_corresponding_clone_dir()
        {
            TestableSvnExternal.ResetCounters();
            _gitSvnExternalsManager.IncludeManualExternals(new SvnExternal[]
            {
                new TestableSvnExternal(willBeClonedTo: @"dir1"),
                new TestableSvnExternal(willBeClonedTo: @"dir1"),
                new TestableSvnExternal(willBeClonedTo: @"dir2"),
                new TestableSvnExternal(willBeClonedTo: @"dir2"),
                new TestableSvnExternal(willBeClonedTo: @"dir3")
            });

            _gitSvnExternalsManager.CloneAllExternals();

            Check.That(TestableSvnExternal.LinkedTimes).IsEqualTo(5);            
        }
    }
}
