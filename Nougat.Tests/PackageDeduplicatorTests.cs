using FluentAssertions;
using Nougat.Models;
using Nougat.Services;
using Xunit;

namespace Nougat.Tests;

public class PackageDeduplicatorTests
{
    private static PackageRef P(string id, string? version, string repo = "Kroste/X", string file = "X.csproj") =>
        new(id, version, null, repo, file);

    [Fact]
    public void Empty_input_yields_empty_result()
    {
        var result = new PackageDeduplicator().Deduplicate([]);
        result.Packages.Should().BeEmpty();
        result.Conflicts.Should().BeEmpty();
        result.Warnings.Should().BeEmpty();
    }

    [Fact]
    public void Removes_duplicates_without_conflict()
    {
        var packages = new[] { P("NLog", "6.1.4"), P("NLog", "6.1.4", "Kroste/Y") };
        var result = new PackageDeduplicator().Deduplicate(packages);
        result.Packages.Should().ContainSingle();
        result.Conflicts.Should().BeEmpty();
    }

    [Fact]
    public void Picks_higher_version_on_conflict_and_records_conflict()
    {
        var packages = new[]
        {
            P("NLog", "5.3.4", "Kroste/Amtsschimmel"),
            P("NLog", "6.1.4", "Kroste/DTM"),
            P("NLog", "6.0.0", "Kroste/NetScanner"),
        };
        var result = new PackageDeduplicator().Deduplicate(packages);

        result.Packages.Should().ContainSingle().Which.Version.Should().Be("6.1.4");
        result.Conflicts.Should().ContainSingle();
        var c = result.Conflicts[0];
        c.PackageId.Should().Be("NLog");
        c.ChosenVersion.Should().Be("6.1.4");
        c.ChosenRepos.Should().BeEquivalentTo(new[] { "Kroste/DTM" });
        c.DiscardedSources.Should().HaveCount(2);
        c.DiscardedSources.Should().ContainSingle(d => d.Version == "5.3.4")
            .Which.Repos.Should().BeEquivalentTo(new[] { "Kroste/Amtsschimmel" });
        c.DiscardedSources.Should().ContainSingle(d => d.Version == "6.0.0")
            .Which.Repos.Should().BeEquivalentTo(new[] { "Kroste/NetScanner" });
    }

    [Fact]
    public void Groups_multiple_repos_per_version()
    {
        var packages = new[]
        {
            P("NLog", "5.3.4", "Kroste/Amtsschimmel"),
            P("NLog", "5.3.4", "Kroste/Klemmbrett"),
            P("NLog", "6.1.4", "Kroste/DTM"),
        };
        var result = new PackageDeduplicator().Deduplicate(packages);

        var c = result.Conflicts.Should().ContainSingle().Which;
        c.ChosenRepos.Should().BeEquivalentTo(new[] { "Kroste/DTM" });
        c.DiscardedSources.Should().ContainSingle().Which
            .Repos.Should().BeEquivalentTo(new[] { "Kroste/Amtsschimmel", "Kroste/Klemmbrett" });
    }

    [Fact]
    public void Warns_on_missing_version_and_skips()
    {
        var packages = new[] { P("Ghost", null), P("NLog", "6.1.4") };
        var result = new PackageDeduplicator().Deduplicate(packages);

        result.Packages.Should().ContainSingle().Which.Id.Should().Be("NLog");
        result.Warnings.Should().ContainSingle().Which.Should().Contain("Ghost");
    }

    [Fact]
    public void Id_matching_is_case_insensitive()
    {
        var packages = new[] { P("nlog", "5.0.0"), P("NLog", "6.0.0") };
        var result = new PackageDeduplicator().Deduplicate(packages);
        result.Packages.Should().ContainSingle();
        result.Packages[0].Version.Should().Be("6.0.0");
    }
}
