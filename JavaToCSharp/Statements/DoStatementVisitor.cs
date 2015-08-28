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
    public class DoStatementVisitor : StatementVisitor<DoStmt>
    {
        public override StatementSyntax Visit(ConversionContext context, DoStmt statement)
        {
            var condition = statement.getCondition();
            var conditionSyntax = ExpressionVisitor.VisitExpression(context, condition);

            var body = statement.getBody();
            var bodySyntax = StatementVisitor.VisitStatement(context, body);

            return SyntaxFactory.DoStatement(bodySyntax, conditionSyntax).AddComment(context, statement);
        }
    }
}
