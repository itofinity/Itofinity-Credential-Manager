using System;
using Xunit;
using Manager.Commands;
using FluentAssertions;

namespace Manager.Test.Commands
{
    public class ClearTest
    {
        private Clear _command = new Clear();

        [Fact]
        public void VerifyName()
        {
           _command.Name.Should().Be(nameof(Clear));
        }
    }
}