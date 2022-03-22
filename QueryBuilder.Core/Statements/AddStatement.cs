using QueryBuilder.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryBuilder.Core.Statements
{
    public class AddStatement : Statement
    {
        private Statement statement;

        public AddStatement(IEnumerable<IdExpression> param, IEnumerable<Expression> param2)
        {
            Param = param;
            Param2 = param2;
            this.ValidarSemantic();

        }

        public IEnumerable<Expression> Param { get; }
        public IEnumerable<Expression> Param2 { get; }

        public override void GenerarCode()
        {
            throw new NotImplementedException();
        }

        public override void ValidarSemantic()
        {
            return string.Empty;
        }
    }
}
