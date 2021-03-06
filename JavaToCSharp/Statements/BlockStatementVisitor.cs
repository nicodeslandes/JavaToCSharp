﻿using com.github.javaparser.ast.stmt;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;

namespace JavaToCSharp.Statements
{
    public class BlockStatementVisitor : StatementVisitor<BlockStmt>
    {
        public override StatementSyntax Visit(ConversionContext context, BlockStmt blockStmt)
        {
            var stmts = blockStmt.getStmts().ToList<Statement>();

            var syntaxes = StatementVisitor.VisitStatements(context, stmts);

            return SyntaxFactory.Block(syntaxes).AddComment(context, blockStmt);
        }
    }
}
