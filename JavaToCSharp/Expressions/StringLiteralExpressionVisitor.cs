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
    public class StringLiteralExpressionVisitor : ExpressionVisitor<StringLiteralExpr>
    {
        public override ExpressionSyntax Visit(ConversionContext context, StringLiteralExpr expr)
        {
            return SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(expr.toString().Trim('\"')));
        }
    }
}
