// Copyright (c) Mahmoud Shaheen. All rights reserved.

#pragma warning disable IDE0130
// ReSharper disable once CheckNamespace
namespace System.Collections.Generic;

[PublicAPI]
public static class QueueExtensions
{
    public static Queue<T> ToQueue<T>(this IEnumerable<T> items)
    {
        var queue = new Queue<T>();

        foreach (var item in items)
        {
            queue.Enqueue(item);
        }

        return queue;
    }

    public static void EnqueueRange<T>(this Queue<T> queue, params T[] items)
    {
        foreach (var item in items)
        {
            queue.Enqueue(item);
        }
    }

    public static void EnqueueRange<T>(this Queue<T> queue, params IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            queue.Enqueue(item);
        }
    }

    public static void EnqueueRange<T>(this Queue<T> queue, params ReadOnlySpan<T> items)
    {
        foreach (var item in items)
        {
            queue.Enqueue(item);
        }
    }
}
