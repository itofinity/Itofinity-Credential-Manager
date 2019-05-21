using System;
using Xunit;
using Manager.Commands;
using FluentAssertions;

namespace Manager.Test.Commands
{
    public class RejectTest
    {
        private Reject _command = new Reject();

        [Fact]
        public void VerifyName()
        {
           _command.Name.Should().Be(nameof(Reject));
        }
    }
}
