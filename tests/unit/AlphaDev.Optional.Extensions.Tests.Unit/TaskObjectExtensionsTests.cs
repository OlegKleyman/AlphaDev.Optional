using System;
using System.Linq;
using System.Threading.Tasks;
using AlphaDev.Optional.Extensions.Unsafe;
using FluentAssertions;
using Optional.Unsafe;
using Xunit;

namespace AlphaDev.Optional.Extensions.Tests.Unit
{
    public class TaskObjectExtensionsTests
    {
        [Fact]
        public async Task NoneAsyncEitherReturnsNone()
        {
            var target = "test";
            var result = await Task.FromResult(default(object)).NoneAsync(target);
            result.ExceptionOrFailure().Should().BeSameAs(target);
        }

        [Fact]
        public async Task NoneAsyncReturnsSome()
        {
            var result = await Task.FromResult(default(object)).NoneAsync();
            result.HasValue.Should().BeFalse();
        }

        [Fact]
        public async Task SomeAsyncEitherReturnsSome()
        {
            var target = new object();
            var result = await Task.FromResult(target).SomeAsync<object, string>();
            result.ValueOrFailure().Should().BeSameAs(target);
        }

        [Fact]
        public async Task SomeAsyncReturnsSome()
        {
            var target = new object();
            var result = await Task.FromResult(target).SomeAsync();
            result.ValueOrFailure().Should().BeSameAs(target);
        }

        [Fact]
        public async Task SomeNotEmptyAsyncEitherReturnsNoneWhenEnumerableIsEmpty()
        {
            (await Task.FromResult(1).SomeNotEmptyAsync(arg => Array.Empty<object>(), o => o.ToString()))
                .ExceptionOrFailure()
                .Should()
                .Be("1");
        }

        [Fact]
        public async Task SomeNotEmptyAsyncEitherReturnsSomeWheEnumerableIsNotEmpty()
        {
            (await Task.FromResult(1).SomeNotEmptyAsync(i => Enumerable.Repeat(1, 1), i => default(object)))
                .ValueOrFailure()
                .Should()
                .Be(1);
        }

        [Fact]
        public async Task SomeNotEmptyAsyncReturnsNoneWhenEnumerableIsEmpty()
        {
            (await Task.FromResult(default(object)).SomeNotEmptyAsync(arg => Array.Empty<object>()))
                .HasValue.Should()
                .BeFalse();
        }

        [Fact]
        public async Task SomeNotEmptyAsyncReturnsSomeWheEnumerableIsNotEmpty()
        {
            (await Task.FromResult(1).SomeNotEmptyAsync(i => Enumerable.Repeat(1, 1)))
                .ValueOrFailure()
                .Should()
                .Be(1);
        }

        [Fact]
        public async Task SomeNotNullAsyncReturnsNoneWhenTaskIsNull()
        {
            var result = await Task.FromResult((string?) null).SomeNotNullAsync();
            result.HasValue.Should().BeFalse();
        }

        [Fact]
        public async Task SomeNotNullAsyncReturnsSomeWhenTaskIsNotNull()
        {
            var result = await Task.FromResult("test").SomeNotNullAsync();
            result.ValueOrFailure().Should().Be("test");
        }

        [Fact]
        public async Task SomeNotNullAsyncWithExceptionReturnsNoneWhenTaskIsNull()
        {
            var exception = new object();
            var result = await Task.FromResult<string?>(null).SomeNotNullAsync(() => exception);
            result.ExceptionOrFailure().Should().BeSameAs(exception);
        }

        [Fact]
        public async Task SomeNotNullAsyncWithExceptionReturnsSomeWhenTaskIsNotNull()
        {
            var result = await Task.FromResult("test").SomeNotNullAsync(() => default(object));
            result.ValueOrFailure().Should().Be("test");
        }
    }
}