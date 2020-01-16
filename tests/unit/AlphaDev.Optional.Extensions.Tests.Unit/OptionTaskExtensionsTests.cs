using System;
using System.Threading.Tasks;
using AlphaDev.Optional.Extensions.Unsafe;
using FluentAssertions;
using Optional;
using Optional.Unsafe;
using Xunit;

namespace AlphaDev.Optional.Extensions.Tests.Unit
{
    public class OptionTaskExtensionsTests
    {
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
        public async Task NotEmptyAsyncEitherReturnsNoneWhenEnumerableIsEmpty()
        {
            var result = await Task.FromResult(Array.Empty<object>().Some().WithException(default(int)))
                                   .NotEmptyAsync(() => 1);
            result.ExceptionOrFailure().Should().Be(1);
        }

        [Fact]
        public async Task NotEmptyAsyncEitherReturnsSomeWhenEnumerableHasSome()
        {
            var result = await Task.FromResult(new[] { 1 }.Some().WithException(default(int)))
                                   .NotEmptyAsync(() => default);
            result.ValueOrFailure().Should().BeEquivalentTo(1);
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
        public static async Task MatchAsyncEitherReturnsSomeWhenOptionHasSome()
        {
            var result = await Task.FromResult(1.Some().WithException(default(object)))
                                   .MatchAsync(i => i.ToString(), o => string.Empty);
            result.Should().Be("1");
        }

        [Fact]
        public static async Task MatchAsyncEitherReturnsExceptionWhenOptionIsNone()
        {
            var result = await Task.FromResult(Option.None<object, int>(1))
                                   .MatchAsync(i => string.Empty, i => i.ToString());
            result.Should().Be("1");
        }

        [Fact]
        public static async Task MatchAsyncEitherExecutesSomeActionWhenOptionHasSome()
        {
            int? executedResult = null;
            await Task.FromResult(1.Some().WithException(default(object)))
                                   .MatchAsync(i => executedResult = i, o => {});
            executedResult.Should().Be(1);
        }

        [Fact]
        public static async Task MatchAsyncEitherExecutesNoneActionWhenOptionIsNone()
        {
            int? executedResult = null;
            await Task.FromResult(Option.None<object, int>(1))
                      .MatchAsync(i => {}, i => executedResult = i);
            executedResult.Should().Be(1);
        }

        [Fact]
        public static async Task MatchAsyncMaybeReturnsSomeWhenOptionHasSome()
        {
            var result = await Task.FromResult(1.Some())
                                   .MatchAsync(i => i.ToString(), () => string.Empty);
            result.Should().Be("1");
        }

        [Fact]
        public static async Task MatchAsyncMaybeReturnsNoneValueWhenOptionIsNone()
        {
            var result = await Task.FromResult(Option.None<object>())
                                   .MatchAsync(i => string.Empty, () => "1");
            result.Should().Be("1");
        }

        [Fact]
        public static async Task MatchAsyncMaybeExecutesSomeActionWhenOptionHasSome()
        {
            int? executedResult = null;
            await Task.FromResult(1.Some())
                      .MatchAsync(i => executedResult = i, () => { });
            executedResult.Should().Be(1);
        }

        [Fact]
        public static async Task MatchAsyncMaybeExecutesNoneActionWhenOptionIsNone()
        {
            var executedResult = false;
            await Task.FromResult(Option.None<object>())
                      .MatchAsync(i => { }, () => executedResult = true);
            executedResult.Should().BeTrue();
        }

        [Fact]
        public async Task MatchSomeAsyncMaybeActionExecutesSomeWhenOptionHasSome()
        {
            int? result = null;
            await Task.FromResult(1.Some()).MatchSomeAsync(i => result = i);
            result.Should().Be(1);
        }

        [Fact]
        public async Task MatchSomeAsyncMaybeActionDoesNotExecuteSomeWhenOptionIsNone()
        {
            int? result = null;
            await Task.FromResult(Option.None<int>()).MatchSomeAsync(i => result = i);
            result.Should().NotHaveValue();
        }

        [Fact]
        public async Task MatchSomeAsyncMaybeActionExecutesSomeTaskWhenOptionHasSome()
        {
            int? value = null;
            await Task.FromResult(1.Some()).MatchSomeAsync(i => Task.Run(() => value = i));
            value.Should().Be(1);
        }

        [Fact]
        public async Task MatchSomeAsyncMaybeActionDoesNotExecuteSomeTaskWhenOptionIsNone()
        {
            int? result = null;
            await Task.FromResult(Option.None<int>()).MatchSomeAsync(i => Task.Run(() => result = i));
            result.Should().NotHaveValue();
        }

        [Fact]
        public async Task MatchNoneAsyncMaybeExecutesNoneWhenOptionIsNone()
        {
            var executed = false;
            await Task.FromResult(Option.None<int>()).MatchNoneAsync(() => executed = true);
            executed.Should().BeTrue();
        }

        [Fact]
        public async Task MatchNoneAsyncMaybeDoesNotExecuteNoneWhenOptionHasSome()
        {
            var executed = false;
            await Task.FromResult(1.Some()).MatchNoneAsync(() => executed = true);
            executed.Should().BeFalse();
        }

        [Fact]
        public async Task MatchNoneAsyncMaybeExecutesNoneTaskWhenOptionIsNone()
        {
            var executed = false;
            await Task.FromResult(Option.None<int>()).MatchNoneAsync(() => Task.Run(() => executed = true));
            executed.Should().BeTrue();
        }

        [Fact]
        public async Task MatchNoneAsyncMaybeDoesNotExecuteNoneTaskWhenOptionHasSome()
        {
            var executed = false;
            await Task.FromResult(1.Some()).MatchNoneAsync(() => Task.Run(() => executed = true));
            executed.Should().BeFalse();
        }

        [Fact]
        public async Task MatchNoneAsyncEitherExecutesNoneTaskWhenOptionIsNone()
        {
            int? value = null;
            await Task.FromResult(Option.None<object, int>(1)).MatchNoneAsync(i => Task.Run(() => value = i));
            value.Should().Be(1);
        }

        [Fact]
        public async Task MatchNoneAsyncEitherDoesNotExecuteNoneTaskWhenOptionHasSome()
        {
            int? value = null;
            await Task.FromResult(Option.Some<object?, int>(default)).MatchNoneAsync(i => Task.Run(() => value = i));
            value.Should().NotHaveValue();
        }

        [Fact]
        public async Task MatchNoneAsyncEitherExecutesNoneWhenOptionIsNone()
        {
            int? value = null;
            await Task.FromResult(Option.None<object, int>(1)).MatchNoneAsync(i => value = i);
            value.Should().Be(1);
        }

        [Fact]
        public async Task MatchNoneAsyncEitherDoesNotExecuteNoneWhenOptionHasSome()
        {
            int? value = null;
            await Task.FromResult(Option.Some<object?, int>(default)).MatchNoneAsync(i => value = i);
            value.Should().NotHaveValue();
        }
    }
}