using System;
using Xunit;
using Manager.Commands;
using FluentAssertions;

namespace Manager.Test.Commands
{
    public class RestoreTest
    {
        private Restore _command = new Restore();

        [Fact]
        public void VerifyName()
        {
           _command.Name.Should().Be(nameof(Restore));
        }
    }
}
