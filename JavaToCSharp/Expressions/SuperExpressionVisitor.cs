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
    public class SuperExpressionVisitor : ExpressionVisitor<SuperExpr>
    {
        public override ExpressionSyntax Visit(ConversionContext context, SuperExpr expr)
        {
            return SyntaxFactory.BaseExpression();
        }
    }
}
