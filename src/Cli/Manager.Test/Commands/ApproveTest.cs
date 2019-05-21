using System;
using Xunit;
using Manager.Commands;
using FluentAssertions;

namespace Manager.Test.Commands
{
    public class ApproveTest
    {
        private Approve _command = new Approve();

        [Fact]
        public void VerifyName()
        {
           _command.Name.Should().Be(nameof(Approve));
        }
    }
}
