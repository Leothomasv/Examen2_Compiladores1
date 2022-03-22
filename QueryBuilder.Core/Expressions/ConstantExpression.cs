using QueryBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryBuilder.Core.Expressions
{
    public class ConstantExpression : Expression
    {
        private Token token;
        private Type type;

        public ConstantExpression(Token token, CompilerType compilerType) 
            : base(token, compilerType)
        {
        }

        public ConstantExpression(Token token, Type type)
        {
            this.token = token;
            this.type = type;
        }

        public override string GenerateCode()
        {
            throw new NotImplementedException();
        }

        public override CompilerType GetExpressionType()
        {
            return CompilerType;
        }
    }
}
