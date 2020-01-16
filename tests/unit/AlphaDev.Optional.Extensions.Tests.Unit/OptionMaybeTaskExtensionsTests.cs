using System;
using System.Threading.Tasks;
using FluentAssertions;
using Optional;
using Optional.Unsafe;
using Xunit;

namespace AlphaDev.Optional.Extensions.Tests.Unit
{
    public class OptionMaybeTaskExtensionsTests
    {
        [Fact]
        public static async Task MatchAsyncExecutesNoneActionWhenOptionIsNone()
        {
            var executedResult = false;
            await Task.FromResult(Option.None<object>())
                      .MatchAsync(i => { }, () => executedResult = true);
            executedResult.Should().BeTrue();
        }

        [Fact]
        public static async Task MatchAsyncExecutesSomeActionWhenOptionHasSome()
        {
            int? executedResult = null;
            await Task.FromResult(1.Some())
                      .MatchAsync(i => executedResult = i, () => { });
            executedResult.Should().Be(1);
        }

        [Fact]
        public static async Task MatchAsyncReturnsNoneValueWhenOptionIsNone()
        {
            var result = await Task.FromResult(Option.None<object>())
                                   .MatchAsync(i => string.Empty, () => "1");
            result.Should().Be("1");
        }

        [Fact]
        public static async Task MatchAsyncReturnsSomeWhenOptionHasSome()
        {
            var result = await Task.FromResult(1.Some())
                                   .MatchAsync(i => i.ToString(), () => string.Empty);
            result.Should().Be("1");
        }

        [Fact]
        public async Task MatchNoneAsyncDoesNotExecuteNoneTaskWhenOptionHasSome()
        {
            var executed = false;
            await Task.FromResult(1.Some()).MatchNoneAsync(() => Task.Run(() => executed = true));
            executed.Should().BeFalse();
        }

        [Fact]
        public async Task MatchNoneAsyncDoesNotExecuteNoneWhenOptionHasSome()
        {
            var executed = false;
            await Task.FromResult(1.Some()).MatchNoneAsync(() => executed = true);
            executed.Should().BeFalse();
        }

        [Fact]
        public async Task MatchNoneAsyncExecutesNoneTaskWhenOptionIsNone()
        {
            var executed = false;
            await Task.FromResult(Option.None<int>()).MatchNoneAsync(() => Task.Run(() => executed = true));
            executed.Should().BeTrue();
        }

        [Fact]
        public async Task MatchNoneAsyncExecutesNoneWhenOptionIsNone()
        {
            var executed = false;
            await Task.FromResult(Option.None<int>()).MatchNoneAsync(() => executed = true);
            executed.Should().BeTrue();
        }

        [Fact]
        public async Task MatchSomeAsyncActionDoesNotExecuteSomeTaskWhenOptionIsNone()
        {
            int? result = null;
            await Task.FromResult(Option.None<int>()).MatchSomeAsync(i => Task.Run(() => result = i));
            result.Should().NotHaveValue();
        }

        [Fact]
        public async Task MatchSomeAsyncActionDoesNotExecuteSomeWhenOptionIsNone()
        {
            int? result = null;
            await Task.FromResult(Option.None<int>()).MatchSomeAsync(i => result = i);
            result.Should().NotHaveValue();
        }

        [Fact]
        public async Task MatchSomeAsyncActionExecutesSomeTaskWhenOptionHasSome()
        {
            int? value = null;
            await Task.FromResult(1.Some()).MatchSomeAsync(i => Task.Run(() => value = i));
            value.Should().Be(1);
        }

        [Fact]
        public async Task MatchSomeAsyncActionExecutesSomeWhenOptionHasSome()
        {
            int? result = null;
            await Task.FromResult(1.Some()).MatchSomeAsync(i => result = i);
            result.Should().Be(1);
        }

        [Fact]
        public async Task NotEmptyAsyncReturnsNoneWhenEnumerableIsEmpty()
        {
            var result = await Task.FromResult(Array.Empty<object>().Some()).NotEmptyAsync();
            result.HasValue.Should().BeFalse();
        }

        [Fact]
        public async Task NotEmptyAsyncReturnsSomeWhenEnumerableHasSome()
        {
            var result = await Task.FromResult(new[] { 1 }.Some()).NotEmptyAsync();
            result.ValueOrFailure().Should().BeEquivalentTo(1);
        }
    }
}