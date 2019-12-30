using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Optional.Extensions;
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
            result.Should().BeNone().Which.Should().BeSameAs(exception);
        }

        [Fact]
        public async Task SomeNotEmptyAsyncEitherReturnsSomeWhenEnumerableIsNotEmpty()
        {
            var target = new[] { 1 };
            var result = await Task.FromResult(target).SomeNotEmptyAsync(() => new object());
            result.Should().HaveSome().Which.Should().BeSameAs(target);
        }

        [Fact]
        public async Task SomeNotEmptyAsyncReturnsNoneWhenEnumerableIsEmpty()
        {
            var result = await Task.FromResult(Enumerable.Empty<int>()).SomeNotEmptyAsync();
            result.Should().BeNone();
        }

        [Fact]
        public async Task SomeNotEmptyAsyncReturnsSomeWhenEnumerableIsNotEmpty()
        {
            var target = new[] { 1 };
            var result = await Task.FromResult(target).SomeNotEmptyAsync();
            result.Should().HaveSome().Which.Should().BeSameAs(target);
        }
    }
}