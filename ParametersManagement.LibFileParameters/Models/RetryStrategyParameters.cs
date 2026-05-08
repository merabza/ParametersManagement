using System;
using System.ComponentModel.DataAnnotations;
using Polly;
using SystemTools.SystemToolsShared;

namespace ParametersManagement.LibFileParameters.Models;

public class RetryStrategyParameters : ItemData
{
    [Range(1, int.MaxValue)] public int MaxRetryAttempts { get; set; }

    public DelayBackoffType BackoffType { get; set; }

    public bool UseJitter { get; set; }

    [Range(typeof(TimeSpan), "00:00:00", "1.00:00:00")]
    public TimeSpan Delay { get; set; }

    [Range(typeof(TimeSpan), "00:00:00", "1.00:00:00")]
    public TimeSpan? MaxDelay { get; set; }

    public override string GetItemKey()
    {
        return $"{MaxRetryAttempts} {BackoffType} {UseJitter} {Delay} {MaxDelay}";
    }
}
