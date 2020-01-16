using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Optional;
using Optional.Unsafe;
using Xunit;

namespace AlphaDev.Optional.Extensions.Tests.Unit
{
    public class OptionMaybeExtensionsTests
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
        public async Task MatchNoneAsyncDoesNotExecutesFuncWhenOptionHasSome()
        {
            var executed = false;
            await default(object).Some().MatchNoneAsync(() => Task.Run(() => executed = true));

            executed.Should().BeFalse();
        }

        [Fact]
        public async Task MatchNoneAsyncExecutesFuncWhenOptionIsNone()
        {
            var executed = false;
            await Option.None<object>().MatchNoneAsync(() => Task.Run(() => executed = true));

            executed.Should().BeTrue();
        }

        [Fact]
        public async Task MatchSomeAsyncDoesNotExecutesFuncWhenOptionIsNone()
        {
            int? result = null;
            await Option.None<int>().MatchSomeAsync(i => Task.Run(() => result = i));

            result.Should().BeNull();
        }

        [Fact]
        public async Task MatchSomeAsyncExecutesFuncWhenOptionHasSome()
        {
            int? result = null;

            await 1.Some().MatchSomeAsync(i => Task.Run(() => result = i));

            result.Should().Be(1);
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