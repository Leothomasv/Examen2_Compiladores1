using QueryBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryBuilder.Core.Expressions
{
    public class BinaryExpressions : Expression
    {
        public BinaryExpressions(Token token, Expression expressionLeft, Expression expressionRight, CompilerType compilerType) : base(token, compilerType)
        {
            ExpressionLeft = expressionLeft;
            ExpressionRight = expressionRight;
        }

        public Expression ExpressionLeft { get; }
        public Expression ExpressionRight { get; }

        public override string GenerateCode()
        {
            throw new NotImplementedException();
        }

        public override CompilerType GetExpressionType()
        {
            throw new NotImplementedException();
        }
    }
}
