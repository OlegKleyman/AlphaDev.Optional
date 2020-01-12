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
    public class OptionExtensionsTests
    {
        [Fact]
        public void FilterNotNullReturnsNoneWhenOptionContainsNull()
        {
            Option.Some<object?>(null).FilterNotNull().HasValue.Should().BeFalse();
        }

        [Fact]
        public void FilterNotNullReturnsSomeWhenOptionDoesNotContainNull()
        {
            var target = new object();
            target.Some().FilterNotNull().ValueOrFailure().Should().BeSameAs(target);
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
        public void NotEmptyEitherReturnsNoneWhenEnumerableIsEmpty()
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
        public void NotEmptyEitherReturnsSomeWhenEnumerableHasSome()
        {
            Enumerable.Range(1, 1)
                      .Some()
                      .WithException(default(object))
                      .NotEmpty(() => default)
                      .ValueOrFailure()
                      .Should()
                      .BeEquivalentTo(1);
        }

        [Fact]
        public void NotEmptyReturnsNoneWhenEnumerableIsEmpty()
        {
            Array.Empty<object>().Some().NotEmpty().HasValue.Should().BeFalse();
        }

        [Fact]
        public void NotEmptyReturnsSomeWhenEnumerableHasSome()
        {
            Enumerable.Range(1, 1).Some().NotEmpty().ValueOrFailure().Should().BeEquivalentTo(1);
        }
    }
}