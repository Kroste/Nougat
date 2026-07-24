using System;
using System.Collections.Generic;
using System.Linq;
using Nougat.Infrastructure;
using Nougat.Models;

namespace Nougat.Services;

public sealed record DeduplicationResult(
    IReadOnlyList<PackageRef> Packages,
    IReadOnlyList<ConflictInfo> Conflicts,
    IReadOnlyList<string> Warnings
);

/// <summary>
/// Fuehrt PackageReferences aus mehreren Repos zusammen. Bei Version-Konflikten
/// gewinnt die "hoechste" Version (siehe <see cref="PackageVersionSelector"/>).
/// PackageReferences OHNE Version werden verworfen (Warning).
/// </summary>
public sealed class PackageDeduplicator
{
    public DeduplicationResult Deduplicate(IEnumerable<PackageRef> packages)
    {
        var byId = new Dictionary<string, List<PackageRef>>(StringComparer.OrdinalIgnoreCase);
        var warnings = new List<string>();

        foreach (var p in packages)
        {
            if (string.IsNullOrWhiteSpace(p.Version))
            {
                warnings.Add(
                    $"{p.SourceRepo}: '{p.Id}' hat keine Version (weder in {p.SourceFile} noch in " +
                    "Directory.Packages.props). Wird uebersprungen.");
                continue;
            }

            if (!byId.TryGetValue(p.Id, out var list))
            {
                list = [];
                byId[p.Id] = list;
            }
            list.Add(p);
        }

        var picked = new List<PackageRef>(byId.Count);
        var conflicts = new List<ConflictInfo>();

        foreach (var (id, refs) in byId)
        {
            var winner = refs[0];
            foreach (var candidate in refs.Skip(1))
            {
                var higher = PackageVersionSelector.PickHigher(winner.Version!, candidate.Version!);
                winner = higher == candidate.Version ? candidate : winner;
            }

            // Konflikt-Info sammeln (alle abweichenden Versionen)
            var distinctVersions = refs
                .Select(r => r.Version!)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (distinctVersions.Count > 1)
            {
                var discarded = distinctVersions
                    .Where(v => !string.Equals(v, winner.Version, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                conflicts.Add(new ConflictInfo(id, winner.Version!, discarded));
            }

            picked.Add(winner);
        }

        return new DeduplicationResult(picked, conflicts, warnings);
    }
}
