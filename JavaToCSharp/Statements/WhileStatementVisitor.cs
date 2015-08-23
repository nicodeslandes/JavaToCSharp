﻿using com.github.javaparser.ast.stmt;
using JavaToCSharp.Expressions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaToCSharp.Statements
{
    public class WhileStatementVisitor : StatementVisitor<WhileStmt>
    {
        public override StatementSyntax Visit(ConversionContext context, WhileStmt whileStmt)
        {
            var expr = whileStmt.getCondition();
            var syntax = ExpressionVisitor.VisitExpression(context, expr);

            var body = whileStmt.getBody();
            var bodySyntax = StatementVisitor.VisitStatement(context, body);

            if (bodySyntax == null)
                return null;

            return Syntax.WhileStatement(syntax, bodySyntax);
        }
    }
}
