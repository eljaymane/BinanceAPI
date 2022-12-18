using BinanceBOT.Core.Model.Exceptions;

namespace BinanceBOT.Core.Model.Abstractions
{
    public abstract class AbstractContext
    {
        public string BaseSymbol { get; set; }
        public string TargetSymbol { get; set; }

        public AbstractContext(string baseSymbol, string targetSymbol)
        {
            BaseSymbol = baseSymbol;
            TargetSymbol = targetSymbol;
        }

        public void ThrowIfNotInitialized()
        {
            if (BaseSymbol == "" || TargetSymbol == "")
            {

                throw new ContextNotInitializedException();
            }
        }
    }
}
