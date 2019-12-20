using FluentAssertions;
using Optional;
using Xunit;

namespace AlphaDev.Optional.Extensions.Tests.Unit
{
    public class ObjectExtensionsTests
    {
        [Fact]
        public void SomeWhenNotNullReturnNoneWhenObjectIsNull()
        {
            ((object?) null).SomeWhenNotNull().Should().Be(Option.None<object>());
        }

        [Fact]
        public void SomeWhenNotNullReturnSomeWhenObjectIsNotNull()
        {
            "test".SomeWhenNotNull().Should().Be("test".Some());
        }

        [Fact]
        public void SomeWhenNotNullWithExceptionReturnNoneWithExceptionObjectWhenTargetObjectIsNull()
        {
            ((string?) null).SomeWhenNotNull(() => "exception").Should().Be(Option.None<string, string>("exception"));
        }

        [Fact]
        public void SomeWhenNotNullWithExceptionReturnSomeWhenTargetObjectIsNotNull()
        {
            "test".SomeWhenNotNull(() => "exception").Should().Be("test".Some().WithException(string.Empty));
        }
    }
}