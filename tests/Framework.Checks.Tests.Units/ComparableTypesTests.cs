// Copyright (c) Mahmoud Shaheen. All rights reserved.

using Framework.Checks;
using Tests.Helpers;

namespace Tests;

public sealed class ComparableTypesTests
{
    [Fact]
    public void is_less_than_or_equal_to_should_return_argument_when_less_than_or_equal()
    {
        // given
        InputsTestArgument inputsTestArgument = new();
        InputsTestArgument expected = new();
        inputsTestArgument.IntValue = 1;

        // when & then
        Argument.IsLessThanOrEqualTo(inputsTestArgument, expected).Should().Be(inputsTestArgument);
    }

    [Fact]
    public void is_less_than_or_equal_to_should_throw_argument_out_of_range_exception_when_greater_than()
    {
        // given
        const int argument = 15;
        const int expected = 10;

        // when
        Action action = () => Argument.IsLessThanOrEqualTo(argument, expected);

        // then
        action
            .Should()
            .ThrowExactly<ArgumentOutOfRangeException>()
            .WithMessage($"The argument \"argument\" must be less than or equal to 10. (Parameter 'argument')");
    }

    [Fact]
    public void is_greater_than_or_equal_to_should_return_argument_when_greater_than_or_equal()
    {
        // when & then
        Argument.IsGreaterThanOrEqualTo(10, 5).Should().Be(10);
    }

    [Fact]
    public void is_greater_than_or_equal_to_should_throw_argument_out_of_range_exception_when_less_than()
    {
        // given
        const int argument = 3;
        const int expected = 5;

        // when & then
        Action action = () => Argument.IsGreaterThanOrEqualTo(argument, expected);

        action
            .Should()
            .ThrowExactly<ArgumentOutOfRangeException>()
            .WithMessage("The argument \"argument\" must be greater than or equal to 5. (Parameter 'argument')");
    }

    [Fact]
    public void is_less_than_should_return_argument_when_less_than()
    {
        // given
        InputsTestArgument argument = new()
        {
            IntValue = 1,
            DecimalValue = 1m,
            DoubleValue = 1,
            FloatValue = 1,
            TimeSpanValue = TimeSpan.Zero,
        };

        InputsTestArgument expected = new();

        // when & then
        Argument.IsLessThan(argument, expected).Should().Be(argument);
    }

    [Fact]
    public void is_less_than_should_throw_argument_out_of_range_exception_when_greater_than_or_equal()
    {
        // given
        const int argument = 5;
        const int expected = 5;

        // when
        Action action = () => Argument.IsLessThan(argument, expected);

        // then
        action
            .Should()
            .ThrowExactly<ArgumentOutOfRangeException>()
            .WithMessage($"The argument \"argument\" must be less than 5. (Parameter 'argument')");
    }

    [Fact]
    public void is_greater_than_should_return_argument_when_greater_than()
    {
        // when
        Argument.IsGreaterThan(10, 5).Should().Be(10);
    }

    [Fact]
    public void is_greater_than_should_throw_argument_out_of_range_exception_when_less_than_or()
    {
        // given
        const int argument = 3;
        const int expected = 5;

        // when & then

        Action action = () => Argument.IsGreaterThan(argument, expected);

        action
            .Should()
            .ThrowExactly<ArgumentOutOfRangeException>()
            .WithMessage("The argument \"argument\" must be greater than 5. (Parameter 'argument')");
    }

    [Fact]
    public void range_should_throw_argument_exception_when_minimum_is_greater_than_maximum()
    {
        // given
        const int minimumValue = 10;
        const int maximumValue = 5;

        // when & then
        var action = () => Argument.Range(minimumValue, maximumValue);

        action
            .Should()
            .ThrowExactly<ArgumentException>()
            .WithMessage(
                "The argument \"minimumValue\" should be less or equal than \"maximumValue\". (Parameter 'minimumValue')"
            );
    }

    [Fact]
    public void range_should_not_throw_when_minimum_is_less_than_maximum()
    {
        // given
        const int minimumValue = 5;
        const int maximumValue = 10;

        // when & then
        Argument.Range(minimumValue, maximumValue);
    }

    [Fact]
    public void is_inclusive_between_should_return_argument_when_in_range()
    {
        // when
        Argument.IsInclusiveBetween(5, 3, 10).Should().Be(5);
    }

    [Fact]
    public void is_inclusive_between_should_throw_argument_out_of_range_exception_when_out_of_range()
    {
        // given
        const int argument = 5;
        const int minimumValue = 10;
        const int maximumValue = 20;

        // when
        Action action = () => Argument.IsInclusiveBetween(argument, minimumValue, maximumValue);

        // then
        action
            .Should()
            .ThrowExactly<ArgumentOutOfRangeException>()
            .WithMessage(
                "The argument argument = 5 must be between 10 and 20 inclusively (10, 20). (Parameter 'argument')"
            );
    }

    [Fact]
    public void is_exclusive_between_should_return_argument_when_in_range()
    {
        // when & then
        Argument.IsExclusiveBetween(5, 3, 10).Should().Be(5);
    }

    [Fact]
    public void is_exclusive_between_should_throw_argument_out_of_range_exception_when_out_of_range()
    {
        // given
        const int argument = 1;
        const int minimumValue = 3;
        const int maximumValue = 10;

        // when
        Action action = () => Argument.IsExclusiveBetween(argument, minimumValue, maximumValue);

        // then
        action
            .Should()
            .ThrowExactly<ArgumentOutOfRangeException>()
            .WithMessage(
                "The argument argument = 1 must be between 3 and 10 exclusively (3, 10). (Parameter 'argument')"
            );
    }
}
