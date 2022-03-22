using System;
using System.Collections.Generic;
using System.Text;

namespace QueryBuilder.Core.Statements
{
    public class DefStatement : Statement
    {
        public DefStatement(Statement statement)
        {
            Statement = statement;
        }

        public Statement Statement { get; }

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
