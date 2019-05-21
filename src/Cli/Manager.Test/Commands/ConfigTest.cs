using System;
using Xunit;
using Manager.Commands;
using FluentAssertions;

namespace Manager.Test.Commands
{
    public class ConfigTest
    {
        private Config _command = new Config();

        [Fact]
        public void VerifyName()
        {
           _command.Name.Should().Be(nameof(Config));
        }
    }
}
