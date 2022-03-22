using QueryBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryBuilder.Core.Expressions
{
    public class LogicalExpression : BinaryExpressions
    {
        private readonly Dictionary<(CompilerType, CompilerType), CompilerType> _typeRules;
        public LogicalExpression(Token token, Expression expressionLeft, Expression expressionRight)
            : base(token, expressionLeft, expressionRight, null)
        {
            _typeRules = new Dictionary<(CompilerType, CompilerType), CompilerType>
            {
                {(CompilerType.Bool, CompilerType.Bool), CompilerType},
            };
        }

        public override string GenerateCode()
        {
            return string.Empty;
        }
    }
}
