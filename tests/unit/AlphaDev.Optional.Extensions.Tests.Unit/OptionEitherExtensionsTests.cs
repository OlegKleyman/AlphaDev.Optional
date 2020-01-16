using System;
using System.Linq;
using System.Threading.Tasks;
using AlphaDev.Optional.Extensions.Unsafe;
using FluentAssertions;
using Optional;
using Optional.Unsafe;
using Xunit;

namespace AlphaDev.Optional.Extensions.Tests.Unit
{
    public class OptionEitherExtensionsTests
    {
        [Fact]
        public void ExceptionOrValueReturnsExceptionValueWhenOptionIsNone()
        {
            var target = new object();
            Option.None<string>().WithException(() => target).ExceptionOrValue().Should().Be(target);
        }

        [Fact]
        public void ExceptionOrValueReturnsSomeValueWhenOptionIsSome()
        {
            "test".Some().WithException(() => default(object)).ExceptionOrValue().Should().Be("test");
        }

        [Fact]
        public async Task FlatMapAsyncReturnsNoneWithInitialExceptionWhenNone()
        {
            var result = await Option.None<object?, int>(1)
                                     .FlatMapAsync(x => Task.FromResult(Option.None<string, byte>(default)),
                                         x => default);
            result.ExceptionOrFailure().Should().Be(1);
        }

        [Fact]
        public async Task FlatMapAsyncReturnsNoneWithTransformedExceptionWhenSome()
        {
            var result = await Option.Some<object?, string?>(default)
                                     .FlatMapAsync(x => Task.FromResult(Option.None<string, int>(1)),
                                         x => x.ToString());
            result.ExceptionOrFailure().Should().Be("1");
        }

        [Fact]
        public async Task FlatMapAsyncReturnsTransformedOptionWhenSome()
        {
            var result = await Option.Some<int, object?>(1)
                                     .FlatMapAsync(x => Task.FromResult(Option.Some<string, object>(x.ToString())),
                                         x => default);
            result.ValueOrFailure().Should().Be(1.ToString());
        }

        [Fact]
        public void GetValueOrExceptionReturnsExceptionValueWhenOptionIsNone()
        {
            Option.None<object>().WithException(() => "test").GetValueOrException().Should().Be("test");
        }

        [Fact]
        public void GetValueOrExceptionReturnsSomeValueWhenOptionIsSome()
        {
            var target = new object();
            target.Some().WithException(() => "test").GetValueOrException().Should().Be(target);
        }

        [Fact]
        public async Task MatchAsyncExecutesNoneWhenOptionIsNone()
        {
            int? result = null;
            await Option.None<object, int>(1)
                        .MatchAsync(_ => Task.CompletedTask, i => Task.Run(() => result = i));

            result.Should().Be(1);
        }

        [Fact]
        public async Task MatchAsyncExecutesSomeWhenOptionHasSome()
        {
            int? result = null;
            await 1.Some()
                   .WithException(() => default(object))
                   .MatchAsync(i => Task.Run(() => result = i), o => Task.CompletedTask);

            result.Should().Be(1);
        }

        [Fact]
        public async Task MatchNoneAsyncDoesNotExecutesFuncWhenOptionHasSome()
        {
            var executed = false;
            await default(object).Some().WithException(() => 1).MatchNoneAsync(_ => Task.Run(() => executed = true));

            executed.Should().BeFalse();
        }

        [Fact]
        public async Task MatchNoneAsyncExecutesFuncWhenOptionIsNone()
        {
            int? exception = null;
            await Option.None<object, int>(1).MatchNoneAsync(i => Task.Run(() => exception = i));

            exception.Should().Be(1);
        }

        [Fact]
        public async Task MatchSomeAsyncDoesNotExecutesFuncWhenOptionIsNone()
        {
            int? result = null;
            await Option.None<int, int>(1).MatchSomeAsync(i => Task.Run(() => result = i));

            result.Should().BeNull();
        }

        [Fact]
        public async Task MatchSomeAsyncExecutesFuncWhenOptionHasSome()
        {
            int? result = null;

            await 1.Some().WithException(() => 2).MatchSomeAsync(i => Task.Run(() => result = i));

            result.Should().Be(1);
        }

        [Fact]
        public void NotEmptyReturnsNoneWhenEnumerableIsEmpty()
        {
            Array.Empty<object>()
                 .Some()
                 .WithException(default(int))
                 .NotEmpty(() => 1)
                 .ExceptionOrFailure()
                 .Should()
                 .Be(1);
        }

        [Fact]
        public void NotEmptyReturnsSomeWhenEnumerableHasSome()
        {
            Enumerable.Range(1, 1)
                      .Some()
                      .WithException(default(object))
                      .NotEmpty(() => default)
                      .ValueOrFailure()
                      .Should()
                      .BeEquivalentTo(1);
        }
    }
}