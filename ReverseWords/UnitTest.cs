using ReverseWords;
using Xunit;

namespace TestReverseWords
{
    public class UnitTest
    {
        [Fact]
        public void StringNull()
        {
            Assert.Equal(string.Empty, Program.Reverse(null));
        }

        [Fact]
        public void StringEmpty()
        {
            Assert.Equal(string.Empty, Program.Reverse(string.Empty));
        }

        [Fact]
        public void StringNoSpaces()
        {
            Assert.Equal("HelloWorld", Program.Reverse("HelloWorld"));
        }

        [Fact]
        public void StringMultipleSpaces()
        {
            Assert.Equal("Spaces  No String", Program.Reverse("String No  Spaces"));
        }
    }
}
