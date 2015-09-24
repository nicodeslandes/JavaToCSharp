using System.Collections.Generic;
using System.Linq;
using com.github.javaparser.ast.body;
using com.github.javaparser.ast.expr;
using JavaToCSharp.Statements;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JavaToCSharp.Expressions
{
    public class LambdaExpressionVisitor : ExpressionVisitor<LambdaExpr>
    {
        public override ExpressionSyntax Visit(ConversionContext context, LambdaExpr lambdaExpr)
        {
            var parameters = lambdaExpr.getParameters().ToList<Parameter>() ?? Enumerable.Empty<Parameter>();
            var body = lambdaExpr.getBody();
            var bodyStatementsSyntax = StatementVisitor.VisitStatements(context, new[] {body}).Single();
            var expressionStatementSyntax = bodyStatementsSyntax as ExpressionStatementSyntax;

            ParenthesizedLambdaExpressionSyntax lambdaExpression;
            if (expressionStatementSyntax != null)
            {
                lambdaExpression = SyntaxFactory.ParenthesizedLambdaExpression(expressionStatementSyntax.Expression);
            }
            else
            {
                var blockSyntax = (BlockSyntax) bodyStatementsSyntax;
                lambdaExpression = SyntaxFactory.ParenthesizedLambdaExpression(blockSyntax);
            }

            return lambdaExpression.WithParameterList(
                        SyntaxFactory.ParameterList()
                            .AddParameters(
                                parameters.Select(
                                    p => SyntaxFactory.Parameter(SyntaxFactory.ParseToken(p.getId().toString())))
                                    .ToArray())); ;
        }
    }
}