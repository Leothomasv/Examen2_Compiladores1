using QueryBuilder.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryBuilder.Core.Statements
{
    public class UpdateStatement : Statement
    {
        public UpdateStatement(IEnumerable<IdExpression> param3, IEnumerable<Expression> param, Expression param2)
        {
            Param3 = param3;
            Param = param;
            Param2 = param2;
            this.ValidarSemantic();
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
