using Xunit;

namespace GitSvnExternals.Tests
{
    public class LongRunning : TraitAttribute
    {
        public LongRunning() : base("Category", "LongRunning")
        {            
        }
    }
}