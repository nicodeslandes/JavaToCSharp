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
            var parameters = lambdaExpr.getParameters().ToList<Parameter>();
            var body = lambdaExpr.getBody();
            var statementSyntax = (ExpressionStatementSyntax)StatementVisitor.VisitStatements(context, new[] {body}).Single();

            return SyntaxFactory.ParenthesizedLambdaExpression(statementSyntax.Expression)
                .WithParameterList(
                    SyntaxFactory.ParameterList()
                        .AddParameters(
                            parameters.Select(
                                p => SyntaxFactory.Parameter(SyntaxFactory.ParseToken(p.getId().toString()))).ToArray()));
        }
    }
}