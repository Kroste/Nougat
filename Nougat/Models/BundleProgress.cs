namespace Nougat.Models;

public enum BundlePhase
{
    Idle,
    CheckingSdk,
    Analyzing,
    Deduplicating,
    Restoring,
    Assembling,
    WritingConfigs,
    Done,
    Failed,
}

/// <summary>Fortschritts-Nachricht des BundleOrchestrator an das UI.</summary>
public sealed record BundleProgress(
    BundlePhase Phase,
    double Percent,
    string StatusText,
    LogEntry? LogEntry = null
);
