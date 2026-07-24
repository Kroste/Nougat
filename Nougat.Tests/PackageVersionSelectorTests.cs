using FluentAssertions;
using Nougat.Infrastructure;
using Xunit;

namespace Nougat.Tests;

public class PackageVersionSelectorTests
{
    [Theory]
    [InlineData("1.0.0", "2.0.0", "2.0.0")]
    [InlineData("2.0.0", "1.0.0", "2.0.0")]
    [InlineData("1.0.0", "1.0.0", "1.0.0")]
    [InlineData("10.0.7", "5.0.0", "10.0.7")]
    [InlineData("12.1.0", "12.0.4", "12.1.0")]
    [InlineData("2.0.0-preview.1", "2.0.0", "2.0.0")]
    [InlineData("2.0.0-preview.2", "2.0.0-preview.1", "2.0.0-preview.2")]
    public void PickHigher_returns_higher_semver(string a, string b, string expected)
    {
        PackageVersionSelector.PickHigher(a, b).Should().Be(expected);
    }

    [Fact]
    public void PickHigher_range_string_uses_lower_bound()
    {
        // [7.2.2,8.0.0) wird normalisiert zu 7.2.2
        PackageVersionSelector.PickHigher("[7.2.2,8.0.0)", "7.2.1").Should().Be("[7.2.2,8.0.0)");
        PackageVersionSelector.PickHigher("[7.2.2,8.0.0)", "7.2.3").Should().Be("7.2.3");
    }

    [Fact]
    public void PickHigher_empty_returns_other()
    {
        PackageVersionSelector.PickHigher("", "1.0.0").Should().Be("1.0.0");
        PackageVersionSelector.PickHigher("1.0.0", "").Should().Be("1.0.0");
    }

    [Fact]
    public void PickHigher_four_segment_version_falls_back_to_system_version()
    {
        PackageVersionSelector.PickHigher("10.0.7.1", "10.0.7.2").Should().Be("10.0.7.2");
    }
}
