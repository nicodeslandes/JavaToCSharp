using com.github.javaparser.ast.stmt;
using com.github.javaparser.ast.type;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;

namespace JavaToCSharp.Statements
{
    public class TryStatementVisitor : StatementVisitor<TryStmt>
    {
        public override StatementSyntax Visit(ConversionContext context, TryStmt tryStmt)
        {
            var tryBlock = tryStmt.getTryBlock();
            var tryStatements = tryBlock.getStmts().ToList<Statement>();

            var tryConverted = StatementVisitor.VisitStatements(context, tryStatements);

            var catches = tryStmt.getCatchs().ToList<CatchClause>();

            var trySyn = SyntaxFactory.TryStatement()
                .AddBlockStatements(tryConverted.ToArray());

            if (catches != null)
            {
                foreach (var ctch in catches)
                {
                    var types = ctch.getExcept().getTypes().ToList<ReferenceType>();
                    var block = ctch.getCatchBlock();
                    var catchStatements = block.getStmts().ToList<Statement>();
                    var catchConverted = StatementVisitor.VisitStatements(context, catchStatements);
                    var catchBlockSyntax = SyntaxFactory.Block(catchConverted);

                    var type = TypeHelper.ConvertType(types[0].getType().ToString());

                    var catchDeclarationSyntax = SyntaxFactory.CatchDeclaration(
                        SyntaxFactory.ParseTypeName(type),
                        SyntaxFactory.ParseToken(ctch.getExcept().getId().toString()));

                    trySyn = trySyn.AddCatches(
                        SyntaxFactory.CatchClause()
                            .WithDeclaration(catchDeclarationSyntax)
                            .WithBlock(catchBlockSyntax)
                        );
                }
            }

            var finallyBlock = tryStmt.getFinallyBlock();

            if (finallyBlock != null)
            {
                var finallyStatements = finallyBlock.getStmts().ToList<Statement>();
                var finallyConverted = StatementVisitor.VisitStatements(context, finallyStatements);
                var finallyBlockSyntax = SyntaxFactory.Block(finallyConverted);

                trySyn = trySyn.WithFinally(SyntaxFactory.FinallyClause(finallyBlockSyntax));
            }

            return trySyn;
        }
    }
}
