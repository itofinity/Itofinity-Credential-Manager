using System;
using Xunit;
using Manager.Commands;
using FluentAssertions;

namespace Manager.Test.Commands
{
    public class DeleteTest
    {
        private Delete _command = new Delete();

        [Fact]
        public void VerifyName()
        {
           _command.Name.Should().Be(nameof(Delete));
        }
    }
}
