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
    public class ClassExpressionVisitor : ExpressionVisitor<ClassExpr>
    {
        public override ExpressionSyntax Visit(ConversionContext context, ClassExpr expr)
        {
            var type = TypeHelper.ConvertType(expr.getType().toString());

            return SyntaxFactory.TypeOfExpression(SyntaxFactory.ParseTypeName(type));
        }
    }
}
