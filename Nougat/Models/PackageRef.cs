namespace Nougat.Models;

/// <summary>Ein referenziertes NuGet-Paket aus einem Repo (moeglicherweise ohne Version).</summary>
public sealed record PackageRef(
    string Id,
    string? Version,
    string? Condition,
    string SourceRepo,
    string SourceFile
);
