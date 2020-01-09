using System.Collections;
using System.Linq;
using AlphaDev.Optional.Extensions.UtilityExtensions;
using FluentAssertions;
using Xunit;

namespace AlphaDev.Optional.Extensions.Tests.Unit.UtilityExtensions
{
    public class EnumerableExtensionsTests
    {
        [Theory]
        [InlineData(1, true)]
        [InlineData(0, false)]
        public void AnyReturnsBooleanBasedOnWhetherAnEnumerableHasElements(int count, bool expected)
        {
            IEnumerable enumerable = Enumerable.Range(0, count);
            enumerable.Any().Should().Be(expected);
        }
    }
}
