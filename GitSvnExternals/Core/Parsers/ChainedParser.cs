using System.Collections.Generic;
using System.Linq;

namespace GitSvnExternals.Core.Parsers
{
    public class ChainedParser : IParseExternals
    {
        private readonly IEnumerable<IParseExternals> _parsers;

        public ChainedParser(IEnumerable<IParseExternals> parsers)
        {
            _parsers = parsers;
        }

        public SvnExternal ParseLine(string line)
        {
            var result = _parsers
                .Select(parser => parser.ParseLine(line))
                .FirstOrDefault(external => external != SvnExternal.Empty);

            return result ?? SvnExternal.Empty;
        }
    }
}