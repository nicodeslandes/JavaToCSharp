using Xunit;

namespace JavaToCSharp.Tests
{
    public class MethodDeclarationTests
    {
        [Fact]
        public void TestSimpleMethodDeclaration()
        {
            var java = @"public static void write() {}";
            var csharp = @"public static void Write()
{
}";

            var converted = ConversionHelper.ConvertMethodDeclaration(java);
            Assert.Equal(csharp, converted);
        }
    }
}