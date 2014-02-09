using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GitSvnExternals.Core;

namespace GitSvnExternals.Tests.Parsers
{
    public class NewSyntaxData : IEnumerable<object[]>
    {
        public readonly List<string> LinesToParse = new List<string>
        {            
            "/https://subversion.assembla.com/svn/svnandgittest/trunk/Externals .buildtools",
            "/https://subversion.assembla.com/svn/svnandgittest/trunk/ExternalsWithFile1/x.txt x.txt",
            "/https://subversion.assembla.com/svn/svnandgittest/trunk/ExternalsWithFile2/y.txt y.txt",
        };

        public readonly List<SvnExternal> ExpectedExternals = new List<SvnExternal>
        {            
            new DirectoryExternal(
                new Uri(
                @"https://subversion.assembla.com/svn/svnandgittest/trunk/Externals"),
                ".buildtools"),

            new FileExternal(new Uri(
                @"https://subversion.assembla.com/svn/svnandgittest/trunk/ExternalsWithFile1/x.txt"),
                "x.txt"),

            new FileExternal(
                new Uri(
                @"https://subversion.assembla.com/svn/svnandgittest/trunk/ExternalsWithFile2/y.txt"),
                "y.txt")
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            return LinesToParse                
                .Select((lineToParse, index) => new object[] {lineToParse, ExpectedExternals[index]})
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public class InvalidLines : IEnumerable<object[]>
        {
            public readonly List<string> LinesToParse = new List<string>
            {
                null,
                string.Empty,
                "nonParsabableLine",
                "# /",
            };

            public IEnumerator<object[]> GetEnumerator()
            {
                return LinesToParse.Select(x => new object[] {x}).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
                
    }
}