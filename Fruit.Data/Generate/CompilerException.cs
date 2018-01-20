using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data.Generate
{
    class CompilerException : Exception
    {
        public CompilerException(CompilerResults result)
        {
            this.CompilerResults = result;
        }

        public CompilerResults CompilerResults { get; private set; }
    }
}
