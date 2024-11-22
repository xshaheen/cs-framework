// Copyright (c) Mahmoud Shaheen. All rights reserved.

using FluentValidation;

namespace Framework.Imaging.Contracts;

public sealed class ImagingOptions
{
    public ImageResizeMode DefaultResizeMode { get; set; } = ImageResizeMode.None;
}

public sealed class ImagingOptionsValidator : AbstractValidator<ImagingOptions>
{
    public ImagingOptionsValidator()
    {
        RuleFor(x => x.DefaultResizeMode).IsInEnum();
    }
}
