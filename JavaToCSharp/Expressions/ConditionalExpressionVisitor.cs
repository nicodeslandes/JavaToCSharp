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
    public class ConditionalExpressionVisitor : ExpressionVisitor<ConditionalExpr>
    {
        public override ExpressionSyntax Visit(ConversionContext context, ConditionalExpr conditionalExpr)
        {
            var condition = conditionalExpr.getCondition();
            var conditionSyntax = ExpressionVisitor.VisitExpression(context, condition);

            var thenExpr = conditionalExpr.getThenExpr();
            var thenSyntax = ExpressionVisitor.VisitExpression(context, thenExpr);

            var elseExpr = conditionalExpr.getElseExpr();
            var elseSyntax = ExpressionVisitor.VisitExpression(context, elseExpr);

            return SyntaxFactory.ConditionalExpression(conditionSyntax, thenSyntax, elseSyntax);
        }
    }
}
