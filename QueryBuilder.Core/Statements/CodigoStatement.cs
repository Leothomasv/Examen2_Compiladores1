using System;
using System.Collections.Generic;
using System.Text;

namespace QueryBuilder.Core.Statements
{
    public class CodigoStatement: Statement
    {
        public CodigoStatement(Statement stm)
        {
            Stm = stm;
        }

        public Statement Stm { get; }

        public override void GenerarCode()
        {
            throw new NotImplementedException();
        }

        public override void ValidarSemantic()
        {
            throw new NotImplementedException();
        }
    }
}
