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
    public class CharLiteralExpressionVisitor : ExpressionVisitor<CharLiteralExpr>
    {
        public override ExpressionSyntax Visit(ConversionContext context, CharLiteralExpr expr)
        {
            return SyntaxFactory.LiteralExpression(SyntaxKind.CharacterLiteralExpression, SyntaxFactory.Literal(expr.toString().Trim('\'')[0]));
        }
    }
}
