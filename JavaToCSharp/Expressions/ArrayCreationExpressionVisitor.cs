using com.github.javaparser.ast.expr;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;

namespace JavaToCSharp.Expressions
{
    public class ArrayCreationExpressionVisitor : ExpressionVisitor<ArrayCreationExpr>
    {
        public override ExpressionSyntax Visit(ConversionContext context, ArrayCreationExpr expr)
        {
            var type = TypeHelper.ConvertType(expr.getType().toString());

            var rankDimensions = expr.getDimensions().ToList<Expression>();

            var initializer = expr.getInitializer();

            var rankSyntaxes = new List<ExpressionSyntax>();

            if (rankDimensions != null)
            {
                foreach (var dimension in rankDimensions)
                {
                    var rankSyntax = ExpressionVisitor.VisitExpression(context, dimension);
                    rankSyntaxes.Add(rankSyntax);
                }
            }

            if (initializer == null)
                return SyntaxFactory.ArrayCreationExpression(SyntaxFactory.ArrayType(SyntaxFactory.ParseTypeName(type)))
                    .AddTypeRankSpecifiers(SyntaxFactory.ArrayRankSpecifier(SyntaxFactory.SeparatedList(rankSyntaxes, Enumerable.Repeat(SyntaxFactory.Token(SyntaxKind.CommaToken), rankSyntaxes.Count - 1))));

            // todo: support multi-dimensional and jagged arrays

            var values = initializer.getValues().ToList<Expression>();

            var syntaxes = new List<ExpressionSyntax>();

            foreach (var value in values)
            {
                var syntax = ExpressionVisitor.VisitExpression(context, value);
                syntaxes.Add(syntax);
            }

            var initSyntax = SyntaxFactory.InitializerExpression(SyntaxKind.ArrayInitializerExpression, SyntaxFactory.SeparatedList(syntaxes, Enumerable.Repeat(SyntaxFactory.Token(SyntaxKind.CommaToken), syntaxes.Count - 1)));

            return SyntaxFactory.ArrayCreationExpression(SyntaxFactory.ArrayType(SyntaxFactory.ParseTypeName(type)), initSyntax);
        }
    }
}
