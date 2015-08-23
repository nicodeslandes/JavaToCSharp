using com.github.javaparser.ast.expr;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.github.javaparser.ast.body;
using JavaToCSharp.Declarations;
using JavaToCSharp.Statements;
using Microsoft.CodeAnalysis.CSharp;

namespace JavaToCSharp.Expressions
{
    public class LambdaExpressionVisitor : ExpressionVisitor<LambdaExpr>
    {
        public override ExpressionSyntax Visit(ConversionContext context, LambdaExpr lambdaExpr)
        {
            var parameters = lambdaExpr.getParameters().ToList<Parameter>();
            var body = lambdaExpr.getBody();
            var statementSyntax = StatementVisitor.VisitStatements(context, new[] {body});

            return SyntaxFactory.ParenthesizedLambdaExpression(statementSyntax.Single())
                .WithParameterList(
                    SyntaxFactory.ParameterList()
                        .AddParameters(
                            parameters.Select(
                                p => SyntaxFactory.Parameter(SyntaxFactory.ParseToken(p.getId().toString()))).ToArray()));
        }
    }
    
    public class MethodReferenceExpressionVisitor : ExpressionVisitor<MethodReferenceExpr>
    {
        public override ExpressionSyntax Visit(ConversionContext context, MethodReferenceExpr methodReferenceExpr)
        {
            var scope = methodReferenceExpr.getScope();

            return VisitExpression(context, scope);
        }
    }

    public class MethodCallExpressionVisitor : ExpressionVisitor<MethodCallExpr>
    {
        public override ExpressionSyntax Visit(ConversionContext context, MethodCallExpr methodCallExpr)
        {
            var scope = methodCallExpr.getScope();
            ExpressionSyntax scopeSyntax = null;

            if (scope != null)
            {
                scopeSyntax = ExpressionVisitor.VisitExpression(context, scope);
            }

            var methodName = TypeHelper.Capitalize(methodCallExpr.getName());
            methodName = TypeHelper.ReplaceCommonMethodNames(methodName);

            ExpressionSyntax methodExpression;

            if (scopeSyntax == null)
            {
                methodExpression = SyntaxFactory.IdentifierName(methodName);
            }
            else
            {
                methodExpression = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, scopeSyntax, SyntaxFactory.IdentifierName(methodName));
            }

            var args = methodCallExpr.getArgs().ToList<Expression>();

            if (args == null || args.Count == 0)
                return SyntaxFactory.InvocationExpression(methodExpression);

            var argSyntaxes = new List<ArgumentSyntax>();

            foreach (var arg in args)
            {
                var argSyntax = ExpressionVisitor.VisitExpression(context, arg);
                argSyntaxes.Add(SyntaxFactory.Argument(argSyntax));
            }

            return SyntaxFactory.InvocationExpression(methodExpression, SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(argSyntaxes, Enumerable.Repeat(SyntaxFactory.Token(SyntaxKind.CommaToken), argSyntaxes.Count - 1))));
        }
    }
}
