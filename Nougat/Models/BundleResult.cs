using System;
using System.Collections.Generic;

namespace Nougat.Models;

public sealed record ConflictInfo(
    string PackageId,
    string ChosenVersion,
    IReadOnlyList<string> DiscardedVersions
);

/// <summary>Ergebnis eines Bundle-Laufs.</summary>
public sealed class BundleResult
{
    public required bool Success { get; init; }
    public int PackageCount { get; init; }
    public long TotalSizeBytes { get; init; }
    public TimeSpan Duration { get; init; }
    public IReadOnlyList<string> Warnings { get; init; } = [];
    public IReadOnlyList<PackageRef> ResolvedPackages { get; init; } = [];
    public IReadOnlyList<ConflictInfo> Conflicts { get; init; } = [];
    public string? ErrorMessage { get; init; }
}
