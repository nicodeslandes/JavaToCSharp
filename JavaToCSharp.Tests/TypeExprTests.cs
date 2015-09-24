using com.github.javaparser;
using JavaToCSharp.Statements;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace JavaToCSharp.Tests
{
    public class TypeExprTests
    {
        [Fact]
        public void TestTypeReferenceStatement()
        {
            var java = @"myList.forEach(writer::write);";
            var csharp = @"myList.ForEach(writer.write);";

            var converted = ConvertStatement(java);
            Assert.Equal(csharp, converted);
        }

        private static string ConvertStatement(string java)
        {
            var statement = JavaParser.parseStatement(java);
            var options = new JavaConversionOptions();
            var context = new ConversionContext(options);
            var statementSyntax = StatementVisitor.VisitStatement(context, statement);

            var tree = CSharpSyntaxTree.Create(statementSyntax);
            return tree.GetText().ToString();
        }
    }
}
