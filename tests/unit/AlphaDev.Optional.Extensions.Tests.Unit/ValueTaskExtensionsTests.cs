using System.Threading.Tasks;
using FluentAssertions;
using Optional;
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
            option.Should().Be(Option.None<string, string>("exception"));
        }

        [Fact]
        public static async Task SomeNotNullAsyncValueTaskReturnsNoneWhenTargetNull()
        {
            var option = await new ValueTask<object?>(Task.FromResult(default(object?))).SomeNotNullAsync();
            option.Should().Be(Option.None<object>());
        }

        [Fact]
        public static async Task SomeNotNullAsyncValueTaskReturnsSomeWhenTargetNotNull()
        {
            var option = await new ValueTask<string>(Task.FromResult("test")).SomeNotNullAsync();
            option.Should().Be("test".Some());
        }

        [Fact]
        public static async Task SomeNotNullAsyncValueTaskWithExceptionReturnsSomeWhenTargetNotNull()
        {
            var option = await new ValueTask<string>(Task.FromResult("test")).SomeNotNullAsync(() => new object());
            option.Should().Be("test".Some<string, object>());
        }
    }
}