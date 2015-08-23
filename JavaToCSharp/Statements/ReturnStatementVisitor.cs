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
    public class ReturnStatementVisitor : StatementVisitor<ReturnStmt>
    {
        public override StatementSyntax Visit(ConversionContext context, ReturnStmt returnStmt)
        {
            var expr = returnStmt.getExpr();

            if (expr == null)
                return SyntaxFactory.ReturnStatement(); // i.e. "return" in a void method

            var exprSyntax = ExpressionVisitor.VisitExpression(context, expr);

            return SyntaxFactory.ReturnStatement(exprSyntax);
        }
    }
}
