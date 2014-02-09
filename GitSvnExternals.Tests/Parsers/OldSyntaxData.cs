using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GitSvnExternals.Core;

namespace GitSvnExternals.Tests.Parsers
{
    public class OldSyntaxData : IEnumerable<object[]>
    {
        public readonly List<string> LinesToParse = new List<string>
        {                        
            "/Maelstrom.Ping/Messages/svn://war01svn/rep/mbank_14.1/dev/BuildTools/Externals .buildtools",
            "/Maelstrom.Ping/Messages/svn://war01svn/rep/mbank_14.1/dev/Maelstrom/Maelstrom.Contracts/Maelstrom.Core_extensions.proto MaelstromExtensions.proto",
            "/Maelstrom.Ping/Messages/svn://war01svn/rep/mbank_14.1/dev/Arch/Maelstrom.Core/Messages/Messages.proto Messages.proto"
        };

        public readonly List<SvnExternal> ExpectedExternals = new List<SvnExternal>
        {            
            new DirectoryExternal(
                new Uri(
                @"svn://war01svn/rep/mbank_14.1/dev/BuildTools/Externals"),
                "Maelstrom.Ping/Messages/.buildtools"),

            new FileExternal(new Uri(
                @"svn://war01svn/rep/mbank_14.1/dev/Maelstrom/Maelstrom.Contracts/Maelstrom.Core_extensions.proto"),
                "Maelstrom.Ping/Messages/MaelstromExtensions.proto"),

            new FileExternal(
                new Uri(
                @"svn://war01svn/rep/mbank_14.1/dev/Arch/Maelstrom.Core/Messages/Messages.proto"),
                "Maelstrom.Ping/Messages/Messages.proto")
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
                "# /Maelstrom.Core.ToJson/"
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