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
    public class BooleanLiteralExpressionVisitor : ExpressionVisitor<BooleanLiteralExpr>
    {
        public override ExpressionSyntax Visit(ConversionContext context, BooleanLiteralExpr expr)
        {
            if (expr.getValue())
                return SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression);
            else
                return SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression);
        }
    }
}
