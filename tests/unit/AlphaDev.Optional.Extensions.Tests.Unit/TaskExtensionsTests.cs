using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Optional.Extensions;
using Xunit;

namespace AlphaDev.Optional.Extensions.Tests.Unit
{
    public class TaskExtensionsTests
    {
        [Fact]
        public async Task NoneAsyncEitherReturnsNone()
        {
            var target = "test";
            var result = await Task.FromResult(default(object)).NoneAsync(target);
            result.Should().BeNone().Which.Should().BeSameAs(target);
        }

        [Fact]
        public async Task NoneAsyncReturnsSome()
        {
            var result = await Task.FromResult(default(object)).NoneAsync();
            result.Should().BeNone();
        }

        [Fact]
        public async Task SomeAsyncEitherReturnsSome()
        {
            var target = new object();
            var result = await Task.FromResult(target).SomeAsync<object, string>();
            result.Should().HaveSome().Which.Should().BeSameAs(target);
        }

        [Fact]
        public async Task SomeAsyncReturnsSome()
        {
            var target = new object();
            var result = await Task.FromResult(target).SomeAsync();
            result.Should().HaveSome().Which.Should().BeSameAs(target);
        }

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

        [Fact]
        public async Task SomeNotNullAsyncReturnsNoneWhenTaskIsNull()
        {
            var result = await Task.FromResult((string?) null).SomeNotNullAsync();
            result.Should().BeNone();
        }

        [Fact]
        public async Task SomeNotNullAsyncReturnsSomeWhenTaskIsNotNull()
        {
            var result = await Task.FromResult("test").SomeNotNullAsync();
            result.Should().HaveSome().Which.Should().Be("test");
        }

        [Fact]
        public async Task SomeNotNullAsyncWithExceptionReturnsNoneWhenTaskIsNull()
        {
            var exception = new object();
            var result = await Task.FromResult<string?>(null).SomeNotNullAsync(() => exception);
            result.Should().BeNone().Which.Should().BeSameAs(exception);
        }

        [Fact]
        public async Task SomeNotNullAsyncWithExceptionReturnsSomeWhenTaskIsNotNull()
        {
            var result = await Task.FromResult("test").SomeNotNullAsync(() => default(object));
            result.Should().HaveSome().Which.Should().Be("test");
        }
    }
}