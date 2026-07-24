using FluentAssertions;
using Xunit;

namespace Nougat.Tests;

public class SmokeTests
{
    [Fact]
    public void Sanity_true_is_true()
    {
        true.Should().BeTrue();
    }
}
