// Copyright (c) Mahmoud Shaheen. All rights reserved.

using Microsoft.Extensions.Logging;

namespace Framework.ResourceLocks.RegularLocks;

public static partial class LoggerExtensions
{
    [LoggerMessage(
        EventId = 1,
        EventName = "LockReleaseStarted",
        Level = LogLevel.Trace,
        Message = "ReleaseAsync Start: R={Resource} Id={LockId}"
    )]
    public static partial void LogReleaseStarted(this ILogger logger, string resource, string lockId);

    [LoggerMessage(
        EventId = 2,
        EventName = "LockReleaseReleased",
        Level = LogLevel.Debug,
        Message = "Released lock: R={Resource} Id={LockId}"
    )]
    public static partial void LogReleaseReleased(this ILogger logger, string resource, string lockId);

    [LoggerMessage(
        EventId = 3,
        EventName = "RenewingLock",
        Level = LogLevel.Debug,
        Message = "Renewing lock: R={Resource} Id={LockId} for {Duration:g}"
    )]
    public static partial void LogRenewingLock(this ILogger logger, string resource, string lockId, TimeSpan? duration);

    [LoggerMessage(
        EventId = 4,
        EventName = "SubscribingToLockReleased",
        Level = LogLevel.Trace,
        Message = "Subscribing to cache lock released"
    )]
    public static partial void LogSubscribingToLockReleased(this ILogger logger);

    [LoggerMessage(
        EventId = 5,
        EventName = "SubscribedToLockReleased",
        Level = LogLevel.Trace,
        Message = "Subscribed to cache lock released"
    )]
    public static partial void LogSubscribedToLockReleased(this ILogger logger);

    [LoggerMessage(
        EventId = 6,
        EventName = "GotLockReleasedMessage",
        Level = LogLevel.Trace,
        Message = "Got lock released message: R={Resource} Id={LockId}"
    )]
    public static partial void LogGotLockReleasedMessage(this ILogger logger, string resource, string lockId);

    [LoggerMessage(
        EventId = 7,
        EventName = "AttemptingToAcquireLock",
        Level = LogLevel.Debug,
        Message = "Attempting to acquire lock: R={Resource} Id={LockId}"
    )]
    public static partial void LogAttemptingToAcquireLock(this ILogger logger, string resource, string lockId);

    [LoggerMessage(
        EventId = 8,
        EventName = "ErrorAcquiringLock",
        Level = LogLevel.Trace,
        Message = "Error acquiring lock: R={Resource} Id={LockId} after {Duration:g}"
    )]
    public static partial void LogErrorAcquiringLock(
        this ILogger logger,
        Exception exception,
        string resource,
        string lockId,
        TimeSpan duration
    );

    [LoggerMessage(
        EventId = 9,
        EventName = "FailedToAcquireLock",
        Level = LogLevel.Debug,
        Message = "Failed to acquire lock: {Resource} Id={LockId}"
    )]
    public static partial void LogFailedToAcquireLock(this ILogger logger, string resource, string lockId);

    [LoggerMessage(
        EventId = 10,
        EventName = "CancellationRequested",
        Level = LogLevel.Trace,
        Message = "Cancellation requested while acquiring lock: R={Resource} Id={LockId}"
    )]
    public static partial void LogCancellationRequested(this ILogger logger, string resource, string lockId);

    [LoggerMessage(
        EventId = 11,
        EventName = "CancellationRequestedForLock",
        Level = LogLevel.Trace,
        Message = "Cancellation requested for lock R={Resource} Id={LockId} after {Duration:g}"
    )]
    public static partial void LogCancellationRequestedAfter(
        this ILogger logger,
        string resource,
        string lockId,
        TimeSpan duration
    );

    [LoggerMessage(
        EventId = 12,
        EventName = "DelayBeforeRetry",
        Level = LogLevel.Trace,
        Message = "Will wait {Delay:g} before retrying to acquire lock: R={Resource} Id={LockId}"
    )]
    public static partial void LogDelayBeforeRetry(this ILogger logger, string resource, string lockId, TimeSpan delay);

    [LoggerMessage(
        EventId = 13,
        EventName = "LongLockAcquired",
        Level = LogLevel.Warning,
        Message = "Acquired lock in long duration R={Resource} Id={LockId} after {Duration:g}"
    )]
    public static partial void LogLongLockAcquired(
        this ILogger logger,
        string resource,
        string lockId,
        TimeSpan duration
    );

    [LoggerMessage(
        EventId = 14,
        EventName = "AcquiredLock",
        Level = LogLevel.Debug,
        Message = "Acquired lock: R={Resource} Id={LockId} after {Duration:g}"
    )]
    public static partial void LogAcquiredLock(this ILogger logger, string resource, string lockId, TimeSpan duration);

    [LoggerMessage(
        EventId = 15,
        EventName = "FailedToAcquireLockAfter",
        Level = LogLevel.Warning,
        Message = "Failed to acquire lock: R={Resource} Id={LockId} after {Duration:g}"
    )]
    public static partial void LogFailedToAcquireLockAfter(
        this ILogger logger,
        string resource,
        string lockId,
        TimeSpan duration
    );
}
