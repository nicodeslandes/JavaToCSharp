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
    public class ThisExpressionVisitor : ExpressionVisitor<ThisExpr>
    {
        public override ExpressionSyntax Visit(ConversionContext context, ThisExpr expr)
        {
            return SyntaxFactory.ThisExpression();
        }
    }
}
