using System;

namespace BoxesPojectShared.Entities
{
    public class SymbolAttribute : Attribute
    {
        public SymbolAttribute(string symbol)
        {
            Symbol = symbol;
        }

        public string Symbol { get; }
    }
}