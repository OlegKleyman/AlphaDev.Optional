using System.Threading.Tasks;
using AlphaDev.Optional.Extensions.Unsafe;
using FluentAssertions;
using Optional.Unsafe;
using Xunit;

namespace AlphaDev.Optional.Extensions.Tests.Unit
{
    public class ValueTaskExtensionsTests
    {
        [Fact]
        public static async Task SomeNotNullAsyncValueTaskReturnsExceptionNoneWhenTargetNull()
        {
            var option =
                await new ValueTask<string?>(Task.FromResult(default(string?))).SomeNotNullAsync(() => "exception");
            option.ExceptionOrFailure().Should().Be("exception");
        }

        [Fact]
        public static async Task SomeNotNullAsyncValueTaskReturnsNoneWhenTargetNull()
        {
            var option = await new ValueTask<object?>(Task.FromResult(default(object?))).SomeNotNullAsync();
            option.HasValue.Should().BeFalse();
        }

        [Fact]
        public static async Task SomeNotNullAsyncValueTaskReturnsSomeWhenTargetNotNull()
        {
            var option = await new ValueTask<string>(Task.FromResult("test")).SomeNotNullAsync();
            option.ValueOrFailure().Should().Be("test");
        }

        [Fact]
        public static async Task SomeNotNullAsyncValueTaskWithExceptionReturnsSomeWhenTargetNotNull()
        {
            var option = await new ValueTask<string>(Task.FromResult("test")).SomeNotNullAsync(() => new object());
            option.ValueOrFailure().Should().Be("test");
        }
    }
}