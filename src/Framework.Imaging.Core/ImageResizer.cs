﻿// Copyright (c) Mahmoud Shaheen. All rights reserved.

using Framework.Checks;
using Microsoft.Extensions.Options;

namespace Framework.Imaging;

public sealed class ImageResizer(
    IEnumerable<IImageResizerContributor> contributors,
    IOptions<ImagingOptions> optionsAccessor
) : IImageResizer
{
    private readonly IEnumerable<IImageResizerContributor> _contributors = contributors.Reverse();
    private readonly ImagingOptions _imagingOptions = optionsAccessor.Value;

    public async Task<ImageStreamResizeResult> ResizeAsync(
        Stream stream,
        ImageResizeArgs args,
        CancellationToken cancellationToken = default
    )
    {
        Argument.IsNotNull(stream);

        _ChangeDefaultResizeMode(args);

        if (!stream.CanRead)
        {
            return ImageStreamResizeResult.CannotRead();
        }

        if (!stream.CanSeek)
        {
            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream, cancellationToken);
            _SeekToBegin(memoryStream);
            stream = memoryStream;
        }

        foreach (var resizerContributor in _contributors)
        {
            var result = await resizerContributor.TryResizeAsync(stream, args, cancellationToken);

            _SeekToBegin(stream);

            if (result.State is ImageProcessState.Unsupported)
            {
                continue;
            }

            return result;
        }

        return ImageStreamResizeResult.NotSupported();
    }

    private void _ChangeDefaultResizeMode(ImageResizeArgs resizeArgs)
    {
        if (resizeArgs.Mode == ImageResizeMode.Default)
        {
            resizeArgs.Mode = _imagingOptions.DefaultResizeMode;
        }
    }

    private static void _SeekToBegin(Stream stream)
    {
        if (stream.CanSeek)
        {
            stream.Seek(0, SeekOrigin.Begin);
        }
    }
}
