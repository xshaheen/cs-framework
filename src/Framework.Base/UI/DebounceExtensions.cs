// Copyright (c) Mahmoud Shaheen. All rights reserved.

namespace Framework.UI;

[PublicAPI]
public static class DebounceExtensions
{
    public static Action Debounce(this Action action, TimeSpan interval)
    {
        ArgumentNullException.ThrowIfNull(action);

        var last = 0;

        return () =>
        {
            var current = Interlocked.Increment(ref last);

            _ = Task.Delay(interval)
                .ContinueWith(
                    task =>
                    {
                        if (current == last)
                        {
                            action();
                        }

                        task.Dispose();
                    },
                    TaskScheduler.Default
                );
        };
    }

    public static Action<T0> Debounce<T0>(this Action<T0> action, TimeSpan interval)
    {
        ArgumentNullException.ThrowIfNull(action);

        var last = 0;

        return arg0 =>
        {
            var current = Interlocked.Increment(ref last);

            _ = Task.Delay(interval)
                .ContinueWith(
                    task =>
                    {
                        if (current == last)
                        {
                            action(arg0);
                        }

                        task.Dispose();
                    },
                    TaskScheduler.Default
                );
        };
    }

    public static Action<T0, T1> Debounce<T0, T1>(this Action<T0, T1> action, TimeSpan interval)
    {
        ArgumentNullException.ThrowIfNull(action);

        var last = 0;

        return (arg0, arg1) =>
        {
            var current = Interlocked.Increment(ref last);

            _ = Task.Delay(interval)
                .ContinueWith(
                    task =>
                    {
                        if (current == last)
                        {
                            action(arg0, arg1);
                        }

                        task.Dispose();
                    },
                    TaskScheduler.Default
                );
        };
    }

    public static Action<T0, T1, T2> Debounce<T0, T1, T2>(this Action<T0, T1, T2> action, TimeSpan interval)
    {
        ArgumentNullException.ThrowIfNull(action);

        var last = 0;

        return (arg0, arg1, arg2) =>
        {
            var current = Interlocked.Increment(ref last);

            _ = Task.Delay(interval)
                .ContinueWith(
                    task =>
                    {
                        if (current == last)
                        {
                            action(arg0, arg1, arg2);
                        }

                        task.Dispose();
                    },
                    TaskScheduler.Default
                );
        };
    }

    public static Action<T0, T1, T2, T3> Debounce<T0, T1, T2, T3>(this Action<T0, T1, T2, T3> action, TimeSpan interval)
    {
        ArgumentNullException.ThrowIfNull(action);

        var last = 0;

        return (arg0, arg1, arg2, arg3) =>
        {
            var current = Interlocked.Increment(ref last);

            _ = Task.Delay(interval)
                .ContinueWith(
                    task =>
                    {
                        if (current == last)
                        {
                            action(arg0, arg1, arg2, arg3);
                        }

                        task.Dispose();
                    },
                    TaskScheduler.Default
                );
        };
    }

    public static Action<T0, T1, T2, T3, T4> Debounce<T0, T1, T2, T3, T4>(
        this Action<T0, T1, T2, T3, T4> action,
        TimeSpan interval
    )
    {
        ArgumentNullException.ThrowIfNull(action);

        var last = 0;

        return (arg0, arg1, arg2, arg3, arg4) =>
        {
            var current = Interlocked.Increment(ref last);

            _ = Task.Delay(interval)
                .ContinueWith(
                    task =>
                    {
                        if (current == last)
                        {
                            action(arg0, arg1, arg2, arg3, arg4);
                        }

                        task.Dispose();
                    },
                    TaskScheduler.Default
                );
        };
    }

    public static Action<T0, T1, T2, T3, T4, T5> Debounce<T0, T1, T2, T3, T4, T5>(
        this Action<T0, T1, T2, T3, T4, T5> action,
        TimeSpan interval
    )
    {
        ArgumentNullException.ThrowIfNull(action);

        var last = 0;

        return (arg0, arg1, arg2, arg3, arg4, arg5) =>
        {
            var current = Interlocked.Increment(ref last);

            _ = Task.Delay(interval)
                .ContinueWith(
                    task =>
                    {
                        if (current == last)
                        {
                            action(arg0, arg1, arg2, arg3, arg4, arg5);
                        }

                        task.Dispose();
                    },
                    TaskScheduler.Default
                );
        };
    }

    public static Action<T0, T1, T2, T3, T4, T5, T6> Debounce<T0, T1, T2, T3, T4, T5, T6>(
        this Action<T0, T1, T2, T3, T4, T5, T6> action,
        TimeSpan interval
    )
    {
        ArgumentNullException.ThrowIfNull(action);

        var last = 0;

        return (arg0, arg1, arg2, arg3, arg4, arg5, arg6) =>
        {
            var current = Interlocked.Increment(ref last);

            _ = Task.Delay(interval)
                .ContinueWith(
                    task =>
                    {
                        if (current == last)
                        {
                            action(arg0, arg1, arg2, arg3, arg4, arg5, arg6);
                        }

                        task.Dispose();
                    },
                    TaskScheduler.Default
                );
        };
    }

    public static Action<T0, T1, T2, T3, T4, T5, T6, T7> Debounce<T0, T1, T2, T3, T4, T5, T6, T7>(
        this Action<T0, T1, T2, T3, T4, T5, T6, T7> action,
        TimeSpan interval
    )
    {
        ArgumentNullException.ThrowIfNull(action);

        var last = 0;

        return (arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
        {
            var current = Interlocked.Increment(ref last);

            _ = Task.Delay(interval)
                .ContinueWith(
                    task =>
                    {
                        if (current == last)
                        {
                            action(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                        }

                        task.Dispose();
                    },
                    TaskScheduler.Default
                );
        };
    }

    public static Action<T0, T1, T2, T3, T4, T5, T6, T7, T8> Debounce<T0, T1, T2, T3, T4, T5, T6, T7, T8>(
        this Action<T0, T1, T2, T3, T4, T5, T6, T7, T8> action,
        TimeSpan interval
    )
    {
        ArgumentNullException.ThrowIfNull(action);

        var last = 0;

        return (arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
        {
            var current = Interlocked.Increment(ref last);

            _ = Task.Delay(interval)
                .ContinueWith(
                    task =>
                    {
                        if (current == last)
                        {
                            action(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
                        }

                        task.Dispose();
                    },
                    TaskScheduler.Default
                );
        };
    }
}
