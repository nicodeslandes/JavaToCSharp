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
    public class ThrowStatementVisitor : StatementVisitor<ThrowStmt>
    {
        public override StatementSyntax Visit(ConversionContext context, ThrowStmt throwStmt)
        {
            var expr = throwStmt.getExpr();

            var exprSyntax = ExpressionVisitor.VisitExpression(context, expr);

            return SyntaxFactory.ThrowStatement(exprSyntax).AddComment(context, throwStmt);
        }
    }
}
