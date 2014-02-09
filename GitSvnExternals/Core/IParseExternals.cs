namespace GitSvnExternals.Core
{
    public interface IParseExternals
    {
        SvnExternal ParseLine(string line);
    }
}