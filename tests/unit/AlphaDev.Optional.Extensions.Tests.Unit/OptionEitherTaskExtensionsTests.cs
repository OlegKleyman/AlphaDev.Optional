using System;
using System.Threading.Tasks;
using AlphaDev.Optional.Extensions.Unsafe;
using FluentAssertions;
using Optional;
using Optional.Unsafe;
using Xunit;

namespace AlphaDev.Optional.Extensions.Tests.Unit
{
    public class OptionEitherTaskExtensionsTests
    {
        [Fact]
        public async Task ExceptionOrValueAsyncReturnsExceptionValueWhenOptionIsNone()
        {
            var exception = new object();
            var result = await Task.FromResult(Option.None<string>().WithException(() => exception))
                                   .ExceptionOrValueAsync();
            result.Should().Be(exception);
        }

        [Fact]
        public async Task ExceptionOrValueAsyncReturnsSomeValueWhenOptionIsSome()
        {
            var target = new object();
            var result = await Task.FromResult(target.Some().WithException(default(object))).ExceptionOrValueAsync();
            result.Should().Be(target);
        }

        [Fact]
        public async Task GetValueOrExceptionAsyncReturnsExceptionValueWhenOptionIsNone()
        {
#pragma warning disable 618 // Obsolete function should still be tested
            var result = await Task.FromResult(Option.None<object>().WithException(() => "test"))
                                   .GetValueOrExceptionAsync();
#pragma warning restore 618
            result.Should().Be("test");
        }

        [Fact]
        public async Task GetValueOrExceptionAsyncReturnsSomeValueWhenOptionIsSome()
        {
            var target = new object();
#pragma warning disable 618 // Obsolete function should still be tested
            var result = await Task.FromResult(target.Some().WithException(() => "test")).GetValueOrExceptionAsync();
#pragma warning restore 618
            result.Should().Be(target);
        }

        [Fact]
        public static async Task MatchAsyncExecutesNoneActionWhenOptionIsNone()
        {
            int? executedResult = null;
            await Task.FromResult(Option.None<object, int>(1))
                      .MatchAsync(i => { }, i => executedResult = i);
            executedResult.Should().Be(1);
        }

        [Fact]
        public static async Task MatchAsyncExecutesSomeActionWhenOptionHasSome()
        {
            int? executedResult = null;
            await Task.FromResult(1.Some().WithException(default(object)))
                      .MatchAsync(i => executedResult = i, o => { });
            executedResult.Should().Be(1);
        }

        [Fact]
        public static async Task MatchAsyncReturnsExceptionWhenOptionIsNone()
        {
            var result = await Task.FromResult(Option.None<object, int>(1))
                                   .MatchAsync(i => string.Empty, i => i.ToString());
            result.Should().Be("1");
        }

        [Fact]
        public static async Task MatchAsyncReturnsSomeWhenOptionHasSome()
        {
            var result = await Task.FromResult(1.Some().WithException(default(object)))
                                   .MatchAsync(i => i.ToString(), o => string.Empty);
            result.Should().Be("1");
        }

        [Fact]
        public async Task MatchNoneAsyncDoesNotExecuteNoneTaskWhenOptionHasSome()
        {
            int? value = null;
            await Task.FromResult(Option.Some<object?, int>(default)).MatchNoneAsync(i => Task.Run(() => value = i));
            value.Should().NotHaveValue();
        }

        [Fact]
        public async Task MatchNoneAsyncDoesNotExecuteNoneWhenOptionHasSome()
        {
            int? value = null;
            await Task.FromResult(Option.Some<object?, int>(default)).MatchNoneAsync(i => value = i);
            value.Should().NotHaveValue();
        }

        [Fact]
        public async Task MatchNoneAsyncExecutesNoneTaskWhenOptionIsNone()
        {
            int? value = null;
            await Task.FromResult(Option.None<object, int>(1)).MatchNoneAsync(i => Task.Run(() => value = i));
            value.Should().Be(1);
        }

        [Fact]
        public async Task MatchNoneAsyncExecutesNoneWhenOptionIsNone()
        {
            int? value = null;
            await Task.FromResult(Option.None<object, int>(1)).MatchNoneAsync(i => value = i);
            value.Should().Be(1);
        }

        [Fact]
        public static async Task MatchSomeAsyncActionDoesNotExecuteActionWhenNone()
        {
            var optionTask = Task.FromResult(Option.None<object>().WithException(string.Empty));
            var someExecuted = false;
            await optionTask.MatchSomeAsync(o => someExecuted = true);

            someExecuted.Should().BeFalse();
        }

        [Fact]
        public async Task MatchSomeAsyncActionExecutesSomeExecutesSomeFunctionWithValueWhenResultHasSome()
        {
            var some = new object();
            var optionTask = Task.FromResult(some.Some().WithException(string.Empty));
            object? matchSomeValue = null;
            await optionTask.MatchSomeAsync(o => matchSomeValue = o);

            matchSomeValue.Should().Be(some);
        }

        [Fact]
        public async Task MatchSomeAsyncDoesNotExecutesSomeExecutesSomeFunctionWithValueWhenResultIsNull()
        {
            var optionTask = Task.FromResult(Option.None<object>().WithException(string.Empty));
            var someExecuted = false;
            await optionTask.MatchSomeAsync(o =>
            {
                someExecuted = true;
                return Task.CompletedTask;
            });

            someExecuted.Should().BeFalse();
        }

        [Fact]
        public async Task MatchSomeAsyncExecutesSomeFunctionWithValueWhenResultHasSome()
        {
            var some = new object();
            var optionTask = Task.FromResult(some.Some().WithException(string.Empty));
            object? matchSomeValue = null;
            await optionTask.MatchSomeAsync(o =>
            {
                matchSomeValue = o;
                return Task.CompletedTask;
            });

            matchSomeValue.Should().Be(some);
        }

        [Fact]
        public async Task NotEmptyAsyncReturnsNoneWhenEnumerableIsEmpty()
        {
            var result = await Task.FromResult(Array.Empty<object>().Some().WithException(default(int)))
                                   .NotEmptyAsync(() => 1);
            result.ExceptionOrFailure().Should().Be(1);
        }

        [Fact]
        public async Task NotEmptyAsyncReturnsSomeWhenEnumerableHasSome()
        {
            var result = await Task.FromResult(new[] { 1 }.Some().WithException(default(int)))
                                   .NotEmptyAsync(() => default);
            result.ValueOrFailure().Should().BeEquivalentTo(1);
        }

        [Fact]
        public static async Task ValueOrAsyncReturnsExceptionWhenOptionHasNone()
        {
            var result = await Task.FromResult(Option.None<string, string>("ex")).ValueOrAsync(s => s);
            result.Should().Be("ex");
        }

        [Fact]
        public static async Task ValueOrAsyncReturnsValueWhenOptionHasSome()
        {
            var result = await Task.FromResult("test".Some().WithException(default(string?)))
                                   .ValueOrAsync(s => string.Empty);
            result.Should().Be("test");
        }

        [Fact]
        public async Task ValueOrExceptionAsyncReturnsExceptionValueWhenOptionIsNone()
        {
            var result = await Task.FromResult(Option.None<object>().WithException(() => "test"))
                                   .ValueOrExceptionAsync();
            result.Should().Be("test");
        }

        [Fact]
        public async Task ValueOrExceptionAsyncReturnsSomeValueWhenOptionIsSome()
        {
            var target = new object();
            var result = await Task.FromResult(target.Some().WithException(() => "test")).ValueOrExceptionAsync();
            result.Should().Be(target);
        }
    }
}