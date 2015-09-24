using Xunit;

namespace JavaToCSharp.Tests
{
    public class LambdaExpressionTests
    {
        [Fact]
        public void TestLambdaWithoutParameters()
        {
            var java = @"execute(() -> this.nodeRepository.get(nodeId));";
            var csharp = @"Execute(()=>this.nodeRepository.Get(nodeId));";

            var converted = ConversionHelper.ConvertStatement(java);
            Assert.Equal(csharp, converted);
        }

        [Fact]
        public void TestLambdaWithOneParameter()
        {
            var java = @"execute(a -> this.nodeRepository.get(nodeId, a));";
            var csharp = @"Execute((a)=>this.nodeRepository.Get(nodeId,a));";

            var converted = ConversionHelper.ConvertStatement(java);
            Assert.Equal(csharp, converted);
        }
    }
}