using System.Linq;
using System.Threading.Tasks;
using AlphaDev.Optional.Extensions.Unsafe;
using FluentAssertions;
using Optional.Unsafe;
using Xunit;

namespace AlphaDev.Optional.Extensions.Tests.Unit
{
    public class TaskEnumerableExtensionsTests
    {
        [Fact]
        public async Task SomeNotEmptyAsyncEitherReturnsNoneWhenEnumerableIsEmpty()
        {
            var exception = new object();
            var result = await Task.FromResult(Enumerable.Empty<int>()).SomeNotEmptyAsync(() => exception);
            result.ExceptionOrFailure().Should().BeSameAs(exception);
        }

        [Fact]
        public async Task SomeNotEmptyAsyncEitherReturnsSomeWhenEnumerableIsNotEmpty()
        {
            var target = new[] { 1 };
            var result = await Task.FromResult(target).SomeNotEmptyAsync(() => new object());
            result.ValueOrFailure().Should().BeSameAs(target);
        }

        [Fact]
        public async Task SomeNotEmptyAsyncReturnsNoneWhenEnumerableIsEmpty()
        {
            var result = await Task.FromResult(Enumerable.Empty<int>()).SomeNotEmptyAsync();
            result.HasValue.Should().BeFalse();
        }

        [Fact]
        public async Task SomeNotEmptyAsyncReturnsSomeWhenEnumerableIsNotEmpty()
        {
            var target = new[] { 1 };
            var result = await Task.FromResult(target).SomeNotEmptyAsync();
            result.ValueOrFailure().Should().BeSameAs(target);
        }
    }
}