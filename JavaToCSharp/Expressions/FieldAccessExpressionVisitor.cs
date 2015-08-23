using com.github.javaparser.ast.expr;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;

namespace JavaToCSharp.Expressions
{
    public class FieldAccessExpressionVisitor : ExpressionVisitor<FieldAccessExpr>
    {
        public override ExpressionSyntax Visit(ConversionContext context, FieldAccessExpr fieldAccessExpr)
        {
            var scope = fieldAccessExpr.getScope();
            ExpressionSyntax scopeSyntax = null;

            if (scope != null)
            {
                scopeSyntax = ExpressionVisitor.VisitExpression(context, scope);
            }

            var field = TypeHelper.ConvertIdentifierName(fieldAccessExpr.getField());

            return SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, scopeSyntax, SyntaxFactory.IdentifierName(field));
        }
    }
}
