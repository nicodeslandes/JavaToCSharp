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
    public class IntegerLiteralExpressionVisitor : ExpressionVisitor<IntegerLiteralExpr>
    {
        public override ExpressionSyntax Visit(ConversionContext context, IntegerLiteralExpr expr)
        {
            string value = expr.toString();

            if (value.StartsWith("0x"))
                return SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(value, Convert.ToInt32(value.Substring(2), 16)));
            else
                return SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(int.Parse(expr.toString())));
        }
    }
}
