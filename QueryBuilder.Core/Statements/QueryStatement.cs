using System;
using System.Collections.Generic;
using System.Text;

namespace QueryBuilder.Core.Statements
{
    public class QueryStatement : Statement
    {
        public QueryStatement(Statement firstSt, Statement secondSt)
        {
            FirstSt = firstSt;
            SecondSt = secondSt;
        }

        public Statement FirstSt { get; }
        public Statement SecondSt { get; }

        public override void GenerarCode()
        {
            throw new NotImplementedException();
        }

        public override void ValidarSemantic()
        {
            this.FirstSt?.ValidarSemantic();
            this.SecondSt?.ValidarSemantic();
        }
    }
}
