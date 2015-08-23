using com.github.javaparser.ast.expr;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaToCSharp.Expressions
{
    public class NullLiteralExpressionVisitor : ExpressionVisitor<NullLiteralExpr>
    {
        public override ExpressionSyntax Visit(ConversionContext context, NullLiteralExpr expr)
        {
            return Syntax.LiteralExpression(SyntaxKind.NullLiteralExpression);
        }
    }
}
