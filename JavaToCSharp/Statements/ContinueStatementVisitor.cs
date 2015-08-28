using com.github.javaparser.ast.stmt;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;

namespace JavaToCSharp.Statements
{
    public class ContinueStatementVisitor : StatementVisitor<ContinueStmt>
    {
        public override StatementSyntax Visit(ConversionContext context, ContinueStmt cnt)
        {
            if (!string.IsNullOrEmpty(cnt.getId()))
                context.Options.Warning("Continue with label detected, using plain continue instead. Check for correctness.", cnt.getBeginLine());

            return SyntaxFactory.ContinueStatement().AddComment(context, cnt);
        }
    }
}
