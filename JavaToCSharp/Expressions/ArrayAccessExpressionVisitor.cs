﻿using com.github.javaparser.ast.expr;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;

namespace JavaToCSharp.Expressions
{
    public class ArrayAccessExpressionVisitor : ExpressionVisitor<ArrayAccessExpr>
    {
        public override ExpressionSyntax Visit(ConversionContext context, ArrayAccessExpr expr)
        {
            var nameExpr = expr.getName();
            var nameSyntax = ExpressionVisitor.VisitExpression(context, nameExpr);

            var indexExpr = expr.getIndex();
            var indexSyntax = ExpressionVisitor.VisitExpression(context, indexExpr);

            return SyntaxFactory.ElementAccessExpression(nameSyntax,
                SyntaxFactory.BracketedArgumentList(
                    SyntaxFactory.SeparatedList<ArgumentSyntax>().Add(SyntaxFactory.Argument(indexSyntax))));
        }
    }
}
