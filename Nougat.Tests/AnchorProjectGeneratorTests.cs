using FluentAssertions;
using Nougat.Models;
using Nougat.Services;
using Xunit;

namespace Nougat.Tests;

public class AnchorProjectGeneratorTests
{
    [Fact]
    public void Generates_valid_anchor_project_with_all_packages()
    {
        var packages = new[]
        {
            new PackageRef("NLog", "6.1.4", null, "Kroste/X", "X.csproj"),
            new PackageRef("Avalonia", "12.1.0", null, "Kroste/Y", "Y.csproj"),
        };

        var xml = new AnchorProjectGenerator().GenerateCsproj(packages);

        xml.Should().Contain("<Project Sdk=\"Microsoft.NET.Sdk\"");
        xml.Should().Contain("<TargetFramework>net10.0</TargetFramework>");
        xml.Should().Contain("<NoBuild>true</NoBuild>");
        xml.Should().Contain("<NuGetAudit>false</NuGetAudit>");
        xml.Should().Contain("<ManagePackageVersionsCentrally>false</ManagePackageVersionsCentrally>");
        xml.Should().Contain("<PackageReference Include=\"NLog\" Version=\"6.1.4\"");
        xml.Should().Contain("<PackageReference Include=\"Avalonia\" Version=\"12.1.0\"");
    }

    [Fact]
    public void Generates_nuget_config_with_only_nuget_org()
    {
        var xml = new AnchorProjectGenerator().GenerateNugetConfig();
        xml.Should().Contain("<clear />");
        xml.Should().Contain("api.nuget.org/v3/index.json");
    }
}
