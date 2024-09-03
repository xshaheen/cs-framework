#pragma warning disable IDE0130
// ReSharper disable once CheckNamespace
namespace System.Collections.Generic;

public static class QueueExtensions
{
    public static void EnqueueRange<T>(this Queue<T> queue, params T[] items)
    {
        foreach (var item in items)
        {
            queue.Enqueue(item);
        }
    }

    public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            queue.Enqueue(item);
        }
    }

    public static void EnqueueRange<T>(this Queue<T> queue, ReadOnlySpan<T> items)
    {
        foreach (var item in items)
        {
            queue.Enqueue(item);
        }
    }
}
