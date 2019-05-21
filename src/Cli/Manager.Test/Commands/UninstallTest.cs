using System;
using Xunit;
using Manager.Commands;
using FluentAssertions;

namespace Manager.Test.Commands
{
    public class UninstallTest
    {
        private Uninstall _command = new Uninstall();

        [Fact]
        public void VerifyName()
        {
           _command.Name.Should().Be(nameof(Uninstall));
        }
    }
}
