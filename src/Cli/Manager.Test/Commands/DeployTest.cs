using System;
using Xunit;
using Manager.Commands;
using FluentAssertions;

namespace Manager.Test.Commands
{
    public class DeployTest
    {
        private Deploy _command = new Deploy();

        [Fact]
        public void VerifyName()
        {
           _command.Name.Should().Be(nameof(Deploy));
        }
    }
}
