using System;
using Xunit;
using Manager.Commands;
using FluentAssertions;

namespace Manager.Test.Commands
{
    public class EraseTest
    {
        private Erase _command = new Erase();

        [Fact]
        public void VerifyName()
        {
           _command.Name.Should().Be(nameof(Erase));
        }
    }
}
