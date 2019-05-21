using System;
using Xunit;
using Manager.Commands;
using FluentAssertions;

namespace Manager.Test.Commands
{
    public class InstallTest
    {
        private Install _command = new Install();

        [Fact]
        public void VerifyName()
        {
           _command.Name.Should().Be(nameof(Install));
        }
    }
}
