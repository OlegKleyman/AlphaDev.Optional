﻿using System;
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
            var result = await Task.FromResult(Option.None<object>().WithException(() => "test"))
                                   .GetValueOrExceptionAsync();
            result.Should().Be("test");
        }

        [Fact]
        public async Task GetValueOrExceptionAsyncReturnsSomeValueWhenOptionIsSome()
        {
            var target = new object();
            var result = await Task.FromResult(target.Some().WithException(() => "test")).GetValueOrExceptionAsync();
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
    }
}