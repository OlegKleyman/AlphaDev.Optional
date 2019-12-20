using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Optional;
using Xunit;

namespace AlphaDev.Optional.Extensions.Tests.Unit
{
    public class TaskExtensionsTests
    {
        [Fact]
        public async Task SomeNotEmptyAsyncReturnsNoneWhenEnumerableIsEmpty()
        {
            var exception = new object();
            var result = await Task.FromResult(Enumerable.Empty<int>()).SomeNotEmptyAsync(() => exception);
            result.Should().Be(Option.None<IEnumerable<int>, object>(exception));
        }

        [Fact]
        public async Task SomeNotEmptyAsyncReturnsSomeWhenEnumerableIsNotEmpty()
        {
            var enumerable = new[] { 1 }.AsEnumerable();
            var result = await Task.FromResult(enumerable).SomeNotEmptyAsync(() => new object());
            // ReSharper disable once PossibleMultipleEnumeration - enumerable is an array
            result.Should().Be(enumerable.Some().WithException(new object()));
        }
    }
}