using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Optional.Extensions;
using Optional;
using Xunit;

namespace AlphaDev.Optional.Extensions.Tests.Unit
{
    public class OptionExtensionsTests
    {
        [Fact]
        public void FilterNotNullReturnsNoneWhenOptionContainsNull()
        {
            Option.Some<object?>(null).FilterNotNull().Should().BeNone();
        }

        [Fact]
        public void FilterNotNullReturnsSomeWhenOptionDoesNotContainNull()
        {
            var target = new object();
            target.Some().FilterNotNull().Should().HaveSome().Which.Should().BeSameAs(target);
        }

        [Fact]
        public async Task FlatMapAsyncReturnsNoneWithInitialExceptionWhenNone()
        {
            var result = await Option.None<object?, int>(1)
                                     .FlatMapAsync(x => Task.FromResult(Option.None<string, byte>(default)),
                                         x => default);
            result.Should().BeNone().Which.Should().Be(1);
        }

        [Fact]
        public async Task FlatMapAsyncReturnsNoneWithTransformedExceptionWhenSome()
        {
            var result = await Option.Some<object?, string?>(default)
                                     .FlatMapAsync(x => Task.FromResult(Option.None<string, int>(1)),
                                         x => x.ToString());
            result.Should().BeNone().Which.Should().Be("1");
        }

        [Fact]
        public async Task FlatMapAsyncReturnsTransformedOptionWhenSome()
        {
            var result = await Option.Some<int, object?>(1)
                                     .FlatMapAsync(x => Task.FromResult(Option.Some<string, object>(x.ToString())),
                                         x => default);
            result.Should().HaveSome().Which.Should().Be(1.ToString());
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
                 .Should()
                 .BeNone()
                 .Which.Should()
                 .Be(1);
        }

        [Fact]
        public void NotEmptyEitherReturnsSomeWhenEnumerableHasSome()
        {
            Enumerable.Range(1, 1)
                      .Some()
                      .WithException(default(object))
                      .NotEmpty(() => default)
                      .Should()
                      .HaveSome()
                      .Which.Should()
                      .BeEquivalentTo(1);
        }

        [Fact]
        public void NotEmptyReturnsNoneWhenEnumerableIsEmpty()
        {
            Array.Empty<object>().Some().NotEmpty().Should().BeNone();
        }

        [Fact]
        public void NotEmptyReturnsSomeWhenEnumerableHasSome()
        {
            Enumerable.Range(1, 1).Some().NotEmpty().Should().HaveSome().Which.Should().BeEquivalentTo(1);
        }
    }
}