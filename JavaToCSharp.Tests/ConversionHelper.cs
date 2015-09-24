using com.github.javaparser;
using com.github.javaparser.ast.body;
using JavaToCSharp.Declarations;
using JavaToCSharp.Statements;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace JavaToCSharp.Tests
{
    public static class ConversionHelper
    {
        public static string ConvertStatement(string java)
        {
            var statement = JavaParser.parseStatement(java);
            var options = new JavaConversionOptions();
            var context = new ConversionContext(options);
            var statementSyntax = StatementVisitor.VisitStatement(context, statement);

            var tree = CSharpSyntaxTree.Create(statementSyntax);
            return tree.GetText().ToString();
        }

        public static string ConvertMethodDeclaration(string java)
        {
            var javaClassDeclaration = @"
            class A
            {
                " + java + @"
            }";
            var declaration = JavaParser.parseBodyDeclaration(javaClassDeclaration);
            var options = new JavaConversionOptions();
            var context = new ConversionContext(options);
            var classDeclaration = SyntaxFactory.ClassDeclaration("A");
            var statementSyntax = BodyDeclarationVisitor.VisitBodyDeclarationForClass(context,
                classDeclaration, (BodyDeclaration)declaration.getChildrenNodes().get(0))
                .NormalizeWhitespace();

            var tree = CSharpSyntaxTree.Create(statementSyntax);
            return tree.GetText().ToString();
        }
    }
}