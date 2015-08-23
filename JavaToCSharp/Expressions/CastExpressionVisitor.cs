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
    public class CastExpressionVisitor : ExpressionVisitor<CastExpr>
    {
        public override ExpressionSyntax Visit(ConversionContext context, CastExpr castExpr)
        {
            var expr = castExpr.getExpr();
            var exprSyntax = ExpressionVisitor.VisitExpression(context, expr);

            var type = TypeHelper.ConvertType(castExpr.getType().toString());

            return SyntaxFactory.CastExpression(SyntaxFactory.ParseTypeName(type), exprSyntax);
        }
    }
}
