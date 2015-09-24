using com.github.javaparser.ast.expr;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;

namespace JavaToCSharp.Expressions
{
    public class ThisExpressionVisitor : ExpressionVisitor<ThisExpr>
    {
        public override ExpressionSyntax Visit(ConversionContext context, ThisExpr expr)
        {
            return SyntaxFactory.ThisExpression();
        }
    }
}
