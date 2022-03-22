using QueryBuilder.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryBuilder.Core.Statements
{
    public class SelectStatement : Statement
    {
        public SelectStatement(IEnumerable<IdExpression> param3,  Expression param2, IEnumerable<Expression> param)
        {
            this.ValidarSemantic();
            Param3 = param3;
            Param = param;
            Param2 = param2;
        }

        public IEnumerable<IdExpression> Param3 { get; }
        public IEnumerable<Expression> Param { get; }
        public Expression Param2 { get; }

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
