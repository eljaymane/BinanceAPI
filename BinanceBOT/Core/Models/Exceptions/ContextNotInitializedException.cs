using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceBOT.Core.Model.Exceptions
{
    public class ContextNotInitializedException : Exception
    {
        private static string message = "The context was not initialized. Please run Initialize() method !";
        public ContextNotInitializedException() : base(message)
        {

        }
    }
}
