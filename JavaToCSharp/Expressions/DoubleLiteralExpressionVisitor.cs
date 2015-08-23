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
    public class DoubleLiteralExpressionVisitor : ExpressionVisitor<DoubleLiteralExpr>
    {
        public override ExpressionSyntax Visit(ConversionContext context, DoubleLiteralExpr expr)
        {
            // note: this must come before the check for StringLiteralExpr because DoubleLiteralExpr : StringLiteralExpr
            var dbl = (DoubleLiteralExpr)expr;

            if (dbl.getValue().EndsWith("f", StringComparison.OrdinalIgnoreCase))
                return SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(float.Parse(dbl.getValue().TrimEnd('f', 'F'))));
            else
                return SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(double.Parse(dbl.getValue().TrimEnd('d', 'D'))));
        }
    }
}
