using System.Linq;
using AlphaDev.Optional.Extensions.Unsafe;
using FluentAssertions;
using Optional.Unsafe;
using Xunit;

namespace AlphaDev.Optional.Extensions.Tests.Unit
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void SomeNotEmptyEitherReturnsNoneWhenEnumerableIsEmpty()
        {
            var exception = new object();
            var result = Enumerable.Empty<int>().SomeNotEmpty(() => exception);
            result.ExceptionOrFailure().Should().BeSameAs(exception);
        }

        [Fact]
        public void SomeNotEmptyEitherReturnsSomeWhenEnumerableIsNotEmpty()
        {
            var target = new[] { 1 };
            var result = target.SomeNotEmpty(() => new object());
            result.ValueOrFailure().Should().BeSameAs(target);
        }

        [Fact]
        public void SomeNotEmptyReturnsNoneWhenEnumerableIsEmpty()
        {
            Enumerable.Empty<int>().SomeNotEmpty().HasValue.Should().BeFalse();
        }

        [Fact]
        public void SomeNotEmptyReturnsSomeWhenEnumerableIsNotEmpty()
        {
            var target = new[] { 1 };
            var result = target.SomeNotEmpty();
            result.ValueOrFailure().Should().BeSameAs(target);
        }
    }
}