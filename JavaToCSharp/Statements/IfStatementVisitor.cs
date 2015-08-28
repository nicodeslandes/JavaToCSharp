﻿using com.github.javaparser.ast.stmt;
using JavaToCSharp.Expressions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;

namespace JavaToCSharp.Statements
{
    public class IfStatementVisitor : StatementVisitor<IfStmt>
    {
        public override StatementSyntax Visit(ConversionContext context, IfStmt ifStmt)
        {
            var condition = ifStmt.getCondition();
            var conditionSyntax = ExpressionVisitor.VisitExpression(context, condition);

            var thenStmt = ifStmt.getThenStmt();
            var thenSyntax = StatementVisitor.VisitStatement(context, thenStmt);

            if (thenSyntax == null)
                return null;

            var elseStmt = ifStmt.getElseStmt();

            if (elseStmt == null)
                return SyntaxFactory.IfStatement(conditionSyntax, thenSyntax);

            var elseStatementSyntax = StatementVisitor.VisitStatement(context, elseStmt);
            var elseSyntax = SyntaxFactory.ElseClause(elseStatementSyntax);

            if (elseSyntax == null)
                return SyntaxFactory.IfStatement(conditionSyntax, thenSyntax);

            return SyntaxFactory.IfStatement(conditionSyntax, thenSyntax, elseSyntax).AddComment(context, ifStmt);
        }
    }
}
