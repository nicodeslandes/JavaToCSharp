using com.github.javaparser.ast.stmt;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaToCSharp.Statements
{
    public class LabeledStatementVisitor : StatementVisitor<LabeledStmt>
    {
        public override StatementSyntax Visit(ConversionContext context, LabeledStmt labeledStmt)
        {
            var statement = labeledStmt.getStmt();
            var syntax = StatementVisitor.VisitStatement(context, statement);

            if (syntax == null)
                return null;

            return Syntax.LabeledStatement(labeledStmt.getLabel(), syntax);
        }
    }
}
