using GitSvnExternals.Core;
using GitSvnExternals.Core.Parsers;
using NFluent;
using Xunit.Extensions;

namespace GitSvnExternals.Tests.Parsers
{    
    public class NewParserTests
    {        
        private readonly NewExternalsParser _parser;

        public NewParserTests()
        {
            _parser = new NewExternalsParser();
        }

        [Theory, ClassData(typeof(NewSyntaxData))]
        public void parse_new_syntax(string lineToParse, SvnExternal expectedExternal)
        {
            var parsedExternal = _parser.ParseLine(lineToParse);

            Check.That(parsedExternal).IsEqualTo(expectedExternal);
        }

        [Theory, ClassData(typeof(NewSyntaxData.InvalidLines))]
        public void return_empty_external_when_sth_is_wrong(string lineToParse)
        {
            var parsedExternal = _parser.ParseLine(lineToParse);

            Check.That(parsedExternal).IsEqualTo(SvnExternal.Empty);
        }  
    }
}