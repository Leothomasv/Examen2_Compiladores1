using QueryBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryBuilder.Core.Expressions
{
    public class RelationalExpression : BinaryExpressions
    {
        private readonly Dictionary<(CompilerType, CompilerType), CompilerType> _typeRules;
        public RelationalExpression(Token token, Expression expressionLeft, Expression expressionRight) 
            : base(token, expressionLeft, expressionRight, null)
        {
            _typeRules = new Dictionary<(CompilerType, CompilerType), CompilerType>
            {
                {(CompilerType.Int, CompilerType.Int), CompilerType.Bool},
                {(CompilerType.Float, CompilerType.Int), CompilerType.Bool},
                {(CompilerType.Int, CompilerType.Float), CompilerType.Bool },
                {(CompilerType.Float, CompilerType.Float), CompilerType.Bool },
                {(CompilerType.Float, CompilerType.Float), CompilerType.Bool },
                {(CompilerType.Bool, CompilerType.Bool), CompilerType.Bool },
            };
        }

        public override string GenerateCode()
        {
            return base.GenerateCode();
        }

        public override CompilerType GetExpressionType()
        {
            var leftType = ExpressionLeft.GetExpressionType();
            var rightType = ExpressionRight.GetExpressionType();
            if (_typeRules.TryGetValue((leftType, rightType), out var resultType))
            {
                return resultType;
            }
            throw new ApplicationException($"Cannot perform relational operation on types {leftType} and {rightType}");
        }
    }
}
