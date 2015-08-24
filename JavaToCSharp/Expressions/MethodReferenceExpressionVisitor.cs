using com.github.javaparser.ast.expr;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JavaToCSharp.Expressions
{
    public class MethodReferenceExpressionVisitor : ExpressionVisitor<MethodReferenceExpr>
    {
        public override ExpressionSyntax Visit(ConversionContext context, MethodReferenceExpr methodReferenceExpr)
        {
            var scope = methodReferenceExpr.getScope();
            var scopeSyntax = VisitExpression(context, scope);
            return SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, scopeSyntax,
                (SimpleNameSyntax) SyntaxFactory.ParseName(methodReferenceExpr.getIdentifier()));
        }
    }
}