// Copyright (c) Mahmoud Shaheen. All rights reserved.

namespace Framework.Imaging;

public interface IImageCompressorContributor
{
    Task<ImageStreamCompressResult> TryCompressAsync(
        Stream stream,
        ImageCompressArgs args,
        CancellationToken cancellationToken = default
    );
}
