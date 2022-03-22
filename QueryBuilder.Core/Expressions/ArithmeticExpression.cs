using QueryBuilder.Core.Enums;
using QueryBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryBuilder.Core.Expressions
{
    public class ArithmeticExpression :BinaryExpressions
    {
        private readonly Dictionary<(CompilerType, CompilerType, TokenType), CompilerType> _Tipos;
 
        public ArithmeticExpression(Token token, Expression expressionLeft, Expression expressionRight) 
            : base(token, expressionLeft, expressionRight, null)
        {
            _Tipos = new Dictionary<(CompilerType, CompilerType, TokenType), CompilerType>
            {
                //Int
                {(CompilerType.Int, CompilerType.Int, TokenType.Plus), CompilerType.Int },
                {(CompilerType.Int, CompilerType.Int, TokenType.Minus), CompilerType.Int },
                {(CompilerType.Int, CompilerType.Int, TokenType.Division), CompilerType.Int },
                {(CompilerType.Int, CompilerType.Int, TokenType.Multiplication), CompilerType.Int },
                //Floats
                {(CompilerType.Float, CompilerType.Float, TokenType.Plus), CompilerType.Float },
                {(CompilerType.Float, CompilerType.Float, TokenType.Minus), CompilerType.Float },
                {(CompilerType.Float, CompilerType.Float, TokenType.Division), CompilerType.Float },
                {(CompilerType.Float, CompilerType.Float, TokenType.Multiplication), CompilerType.Float },
                //Floats and Ints
                {(CompilerType.Int, CompilerType.Float, TokenType.Plus), CompilerType.Float },
                {(CompilerType.Int, CompilerType.Float, TokenType.Minus), CompilerType.Float },
                {(CompilerType.Int, CompilerType.Float, TokenType.Division), CompilerType.Float },
                {(CompilerType.Int, CompilerType.Float, TokenType.Multiplication), CompilerType.Float },

                {(CompilerType.Float, CompilerType.Int, TokenType.Plus), CompilerType.Float },
                {(CompilerType.Float, CompilerType.Int, TokenType.Minus), CompilerType.Float },
                {(CompilerType.Float, CompilerType.Int, TokenType.Division), CompilerType.Float },
                {(CompilerType.Float, CompilerType.Int, TokenType.Multiplication), CompilerType.Float },
                //string
                {(CompilerType.String, CompilerType.String, TokenType.Plus), CompilerType.String },
                {(CompilerType.String, CompilerType.Int, TokenType.Plus), CompilerType.String },
                {(CompilerType.String, CompilerType.Float, TokenType.Plus), CompilerType.String },
                {(CompilerType.Int, CompilerType.String, TokenType.Plus), CompilerType.String },
                {(CompilerType.Float, CompilerType.String, TokenType.Plus), CompilerType.String },


            };
        }

        public override string GenerateCode()
        {
            return base.GenerateCode();
        }
    }
}
