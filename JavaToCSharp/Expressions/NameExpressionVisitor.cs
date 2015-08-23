using com.github.javaparser.ast.expr;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaToCSharp.Expressions
{
    public class NameExpressionVisitor : ExpressionVisitor<NameExpr>
    {
        public override ExpressionSyntax Visit(ConversionContext context, NameExpr nameExpr)
        {
            return Syntax.IdentifierName(TypeHelper.ConvertIdentifierName(nameExpr.getName()));
        }
    }
}
