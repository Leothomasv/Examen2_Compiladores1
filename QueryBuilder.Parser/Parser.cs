using System;
using QueryBuilder.Core.Enums;
using QueryBuilder.Core.Interfaces;
using QueryBuilder.Core.Models;
using QueryBuilder.Core.Statements;
using QueryBuilder.Core.Expressions;
using System.Collections.Generic;
using QueryBuilder.Core;

namespace QueryBuilder.Parser
{
    public class Parser : IParser
    {
        private readonly IScanner _scanner;
        private readonly ILogger _logger;
        private Token _lookAhead;

        public Parser(IScanner scanner, ILogger logger)
        {
            this._scanner = scanner;
            this._logger = logger;
            this._lookAhead = this._scanner.GetNextToken();
        }

        public Statement Parse()
        {
            return Code();
        }

        private Statement Code()
        {
            Match(TokenType.DefKeyword);
            Match(TokenType.TablesKeyword);
            TableDef();
            TableDefs();
            Match(TokenType.EndKeyWord);
            Match(TokenType.DefKeyword);
            Match(TokenType.RelationshipsKeyword);
            Relationships();
            Match(TokenType.EndKeyWord);
            return new CodigoStatement(Queries());
        }

        private void Relationships()
        {
            if (_lookAhead == TokenType.Identifier)
            {
                Relationship();
                Relationships();
            }
        }

        private void Relationship()
        {
            Match(TokenType.Identifier);
            Match(TokenType.Dot);
            if (_lookAhead == TokenType.ManyKeyword)
            {
                Match(TokenType.ManyKeyword);
            }
            else
            {
                Match(TokenType.OneKeyword);
            }

            Match(TokenType.LeftParens);
            Match(TokenType.Identifier);
            Match(TokenType.RightParens);
            Match(TokenType.Semicolon);
        }

        private Statement Queries()
        {
            if (_lookAhead == TokenType.Identifier)
            {
                return new QueryStatement(Query(), Queries());
            }
            else
            {
                return null;
            }
        }

        private Statement Query()
        {
            var token = _lookAhead;
            Match(TokenType.Identifier);
            Match(TokenType.Dot);
            switch (_lookAhead.TokenType)
            {
                case TokenType.AddKeyword:
                    var datos = ContextTable.GetAllProperties(token.Lexeme);
                    var statement = new AddStatement(datos, Insert());
                    return statement;
                case TokenType.UpdateKeyword:
                    datos = ContextTable.GetAllProperties(token.Lexeme);
                    Match(TokenType.UpdateKeyword);
                    Match(TokenType.LeftParens);
                    var data = Json();
                    var expr = Update();
                    var updateSt = new UpdateStatement(datos, data, expr);
                    Match(TokenType.RightParens);
                    return updateSt;
                default:
                    datos = ContextTable.GetAllProperties(token.Lexeme);
                    Match(TokenType.SelectKeyword);
                    Match(TokenType.LeftParens);
                    var @args = Args();
                    var @sel = Select();
                    Match(TokenType.RightParens);
                    var select = new SelectStatement(datos, sel, args);
                    return select;
            }
        }

        private Expression Select()
        {
            if (_lookAhead == TokenType.Semicolon)
            {
                Match(TokenType.Semicolon);
                return null;
            }
            else
            {
                Match(TokenType.Dot);
                Match(TokenType.WhereKeyword);
                Match(TokenType.LeftParens);
                var expr = LogicalOrExpr();
                Match(TokenType.RightParens);
                Match(TokenType.Semicolon);
                return expr;
            }
        }

        private IEnumerable<Expression> Args()
        {
            var @params = new List<Expression>();
            @params.Add(LogicalOrExpr());
            @params.AddRange(ArgsPrime());
            return @params;
        }

        private IEnumerable<Expression> ArgsPrime()
        {
            var @params = new List<Expression>();
            if (_lookAhead == TokenType.Comma)
            {
                Match(TokenType.Comma);
                @params.Add(LogicalOrExpr());
                @params.AddRange(ArgsPrime());
            }
            return @params;
        }


        private Expression Update()
        {
            if (_lookAhead == TokenType.Semicolon)
            {
                Match(TokenType.Semicolon);
            }
            else
            {
                Match(TokenType.Dot);
                Match(TokenType.WhereKeyword);
                Match(TokenType.LeftParens);
                var expression = LogicalOrExpr();
                Match(TokenType.RightParens);
                Match(TokenType.Semicolon);
                return expression;
            }
            return null;
        }

        private IEnumerable<Expression> Json()
        {
            var datos = new List<Expression>();
            Match(TokenType.LeftBrace);
            datos.Add(JsonElementBlock());
            Match(TokenType.RightBrace);
            return datos;
        }

        private IEnumerable<Expression> JsonElementsOptional()
        {
            if (_lookAhead == TokenType.Identifier)
            {
                return JsonElements();
            }
            return null;
        }

        private IEnumerable<Expression> JsonElements()
        {
            var datos = new List<Expression>();
            datos.Add(JsonElementBlock());
            JsonElementBlock();
            while (_lookAhead == TokenType.Comma)
            {
                Match(TokenType.Comma); 
                datos.Add(JsonElementBlock());
            }
            return datos;
        }

        private Expression JsonElementBlock()
        {
            Match(TokenType.Identifier);
            Match(TokenType.Colon);
            return LogicalOrExpr();
        }

        private IEnumerable<Expression> Insert()
        {
            var datos = new List<Expression>();
            Match(TokenType.AddKeyword);
            Match(TokenType.LeftParens);
            datos.AddRange(Json());
            Match(TokenType.RightParens);
            Match(TokenType.Semicolon);
            return datos;
        }

        private void TableDefs()
        {
            if (_lookAhead == TokenType.Identifier)
            {
                TableDef();
                TableDefs();
            }
        }

        private void TableDef()
        {
            var token = this._lookAhead;
            Match(TokenType.Identifier);
            Match(TokenType.LeftBrace);
            var @params = new List<IdExpression>();
            @params.AddRange(TableColumns());
            Match(TokenType.RightBrace);
            ContextTable.Add(token.Lexeme, @params);
        }

        private  IEnumerable<IdExpression> TableColumns()
        {
            var @params = new List<IdExpression>();
            while (_lookAhead == TokenType.LeftBracket || _lookAhead == TokenType.Identifier)
            {
                switch (_lookAhead)
                {
                    case { TokenType: TokenType.LeftBracket }:
                        var token = this._lookAhead;
                        Match(TokenType.LeftBracket);
                        Match(TokenType.PrimaryKeyword);
                        Match(TokenType.RightBracket);
                        Match(TokenType.Identifier);
                        Match(TokenType.Colon);
                        var type =Type();
                        var id = new IdExpression(token, type);
                        @params.Add(id);
                        Match(TokenType.Semicolon);
                        break;
                    default:
                        token = this._lookAhead;
                        Match(TokenType.Identifier);
                        Match(TokenType.Colon);
                        type = Type();
                        id = new IdExpression(token, type);
                        @params.Add(id);
                        Match(TokenType.Semicolon);
                        break;
                }
            }
            return @params;
        }
        private CompilerType Type()
        {
            switch (_lookAhead)
            {
                case { TokenType: TokenType.IntKeyword }:
                    Match(TokenType.IntKeyword);
                    return CompilerType.Int;
                case { TokenType: TokenType.FloatKeyword }:
                    Match(TokenType.FloatKeyword);
                    return CompilerType.Float;
                case { TokenType: TokenType.BoolKeyword }:
                    Match(TokenType.BoolKeyword);
                    return CompilerType.Bool;
                default:
                    Match(TokenType.StringKeyword);
                    return CompilerType.String;
            }
        }

        private Expression LogicalOrExpr()
        {
            var expression = LogicalAndExpr();
            while (this._lookAhead.TokenType == TokenType.LogicalOr)
            {
                var token = this._lookAhead;
                this.Move();
                expression = new LogicalExpression(token, expression, LogicalOrExpr());
            }
            return expression;
        }

        private Expression LogicalAndExpr()
        {
            var expr = Eq();
            while (this._lookAhead.TokenType == TokenType.LogicalAnd)
            {
                var token = this._lookAhead;
                this.Move();
                expr = new LogicalExpression(token, expr, Eq());
            }
            return expr;
        }

        private Expression Eq()
        {
            var expr = Rel();
            while (this._lookAhead.TokenType == TokenType.Equal)
            {
                var token = this._lookAhead;
                this.Move();
                expr = new RelationalExpression(token, expr, Rel());
            }
            return expr;
        }


        private Expression Rel()
        {
            var expr = Expr();
            while (this._lookAhead.TokenType == TokenType.LessThan ||
                   this._lookAhead.TokenType == TokenType.GreaterThan ||
                   this._lookAhead.TokenType == TokenType.LessOrEqualThan ||
                   this._lookAhead.TokenType == TokenType.GreaterOrEqualThan)
            {
                var token = this._lookAhead;
                this.Move();
                expr = new ArithmeticExpression(token, expr, Expr());
            }
            return expr;
        }

        private Expression Expr()
        {
            var expression = Term();
            while (this._lookAhead.TokenType == TokenType.Plus || this._lookAhead.TokenType == TokenType.Minus)
            {
                var token = this._lookAhead;
                this.Move();
                expression = new ArithmeticExpression(token, expression, Term());
            }
            return expression;
        }

        private Expression Term()
        {
            var expression = Factor();
            while (this._lookAhead.TokenType == TokenType.Multiplication ||
                   this._lookAhead.TokenType == TokenType.Division)
            {
                var token = this._lookAhead;
                this.Move();
                expression = new ArithmeticExpression(token, expression, Factor());
            }
            return expression;
        }

        private Expression Factor()
        {
            switch (this._lookAhead.TokenType)
            {
                case TokenType.LeftParens:
                    this.Match(TokenType.LeftParens);
                    var expr = LogicalOrExpr();
                    this.Match(TokenType.RightParens);
                    return expr;
                case TokenType.Identifier:
                    var token = this._lookAhead;
                    Match(TokenType.Identifier);
                    return new ConstantExpression(token, token.GetType());
                case TokenType.IntConstant:
                    token = this._lookAhead;
                    this.Match(TokenType.IntConstant);
                    return new ConstantExpression(token, Core.CompilerType.Int);
                case TokenType.FloatConstant:
                    token = this._lookAhead;
                    this.Match(TokenType.FloatConstant);
                    return new ConstantExpression(token, Core.CompilerType.Float);
                case TokenType.TrueKeyword:
                    token = this._lookAhead;
                    this.Match(TokenType.TrueKeyword);
                    return new ConstantExpression(token, Core.CompilerType.Bool);
                case TokenType.FalseKeyword:
                    token = this._lookAhead;
                    this.Match(TokenType.FalseKeyword);
                    return new ConstantExpression(token, Core.CompilerType.Bool);
                default:
                    token = this._lookAhead;
                    this.Match(TokenType.StringLiteral);
                    return new ConstantExpression(token, Core.CompilerType.String);
            }
        }

        private void Move()
        {
            this._lookAhead = this._scanner.GetNextToken();
        }

        private void Match(TokenType expectedTokenType)
        {
            if (this._lookAhead != expectedTokenType)
            {
                this._logger.Error(
                    $"Syntax Error! expected token {expectedTokenType} but found {this._lookAhead.TokenType} on line {this._lookAhead.Line} and column {this._lookAhead.Column}");
                throw new ApplicationException(
                    $"Syntax Error! expected token {expectedTokenType} but found {this._lookAhead.TokenType} on line {this._lookAhead.Line} and column {this._lookAhead.Column}");
            }

            this.Move();
        }
    }
}
