using System;
using Xunit;
using Manager.Commands;
using FluentAssertions;

namespace Manager.Test.Commands
{
    public class VersionTest
    {
        private Manager.Commands.Version _command = new Manager.Commands.Version();

        [Fact]
        public void VerifyName()
        {
           _command.Name.Should().Be(nameof(Manager.Commands.Version));
        }
    }
}
