// Copyright (c) Mahmoud Shaheen. All rights reserved.

using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace Framework.Constants;

public static class FrameworkDiagnostics
{
    public static ActivitySource ActivitySource { get; }

    public static Meter Meter { get; }

    static FrameworkDiagnostics()
    {
        var product = typeof(FrameworkDiagnostics).Assembly.GetAssemblyProduct() ?? "Framework";
        var version = typeof(FrameworkDiagnostics).Assembly.GetAssemblyVersion() ?? "1.0.0";

        ActivitySource = new(product, version);
        Meter = new(version, version);
    }
}
