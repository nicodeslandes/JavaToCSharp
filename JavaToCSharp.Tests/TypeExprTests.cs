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

            var converted = ConversionHelper.ConvertStatement(java);
            Assert.Equal(csharp, converted);
        }
    }
}
