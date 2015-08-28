using com.github.javaparser.ast.stmt;
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
    public class SwitchStatementVisitor : StatementVisitor<SwitchStmt>
    {
        public override StatementSyntax Visit(ConversionContext context, SwitchStmt switchStmt)
        {
            var selector = switchStmt.getSelector();
            var selectorSyntax = ExpressionVisitor.VisitExpression(context, selector);

            var cases = switchStmt.getEntries().ToList<SwitchEntryStmt>();

            if (cases == null)
                return SyntaxFactory.SwitchStatement(selectorSyntax, SyntaxFactory.List<SwitchSectionSyntax>());

            var caseSyntaxes = new List<SwitchSectionSyntax>();

            foreach (var cs in cases)
            {
                var label = cs.getLabel();

                var statements = cs.getStmts().ToList<Statement>();
                var syntaxes = StatementVisitor.VisitStatements(context, statements);

                if (label == null)
                {
                    // default case
                    var defaultSyntax = SyntaxFactory.SwitchSection(
                        SyntaxFactory.List<SwitchLabelSyntax>().Add(SyntaxFactory.DefaultSwitchLabel()),
                        SyntaxFactory.List(syntaxes.AsEnumerable()));
                    caseSyntaxes.Add(defaultSyntax);
                }
                else
                {
                    var labelSyntax = ExpressionVisitor.VisitExpression(context, label);

                    var caseSyntax = SyntaxFactory.SwitchSection(
                        SyntaxFactory.List<SwitchLabelSyntax>().Add(SyntaxFactory.CaseSwitchLabel(labelSyntax)),
                        SyntaxFactory.List(syntaxes.AsEnumerable()));
                    caseSyntaxes.Add(caseSyntax);
                }
            }

            return SyntaxFactory.SwitchStatement(selectorSyntax, SyntaxFactory.List<SwitchSectionSyntax>(caseSyntaxes))
                .AddComment(context, switchStmt);
        }
    }
}
