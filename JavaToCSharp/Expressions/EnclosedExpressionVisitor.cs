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
    public class EnclosedExpressionVisitor : ExpressionVisitor<EnclosedExpr>
    {
        public override ExpressionSyntax Visit(ConversionContext context, EnclosedExpr enclosedExpr)
        {
            var expr = enclosedExpr.getInner();
            var exprSyntax = ExpressionVisitor.VisitExpression(context, expr);

            return SyntaxFactory.ParenthesizedExpression(exprSyntax);
        }
    }
}
