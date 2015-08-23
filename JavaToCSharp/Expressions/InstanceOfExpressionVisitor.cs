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
    public class InstanceOfExpressionVisitor : ExpressionVisitor<InstanceOfExpr>
    {
        public override ExpressionSyntax Visit(ConversionContext context, InstanceOfExpr expr)
        {
            var innerExpr = expr.getExpr();
            var exprSyntax = ExpressionVisitor.VisitExpression(context, innerExpr);

            var type = TypeHelper.ConvertType(expr.getType().toString());

            return SyntaxFactory.BinaryExpression(SyntaxKind.IsExpression, exprSyntax, SyntaxFactory.IdentifierName(type));
        }
    }
}
