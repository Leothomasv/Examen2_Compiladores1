using System;
using System.Collections.Generic;
using System.Text;

namespace QueryBuilder.Core.Statements
{
    public abstract class Statement
    {
        public abstract void ValidarSemantic();
        public abstract void GenerarCode();
    }
}
