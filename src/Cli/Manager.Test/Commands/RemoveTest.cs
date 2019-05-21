using System;
using Xunit;
using Manager.Commands;
using FluentAssertions;

namespace Manager.Test.Commands
{
    public class RemoveTest
    {
        private Remove _command = new Remove();

        [Fact]
        public void VerifyName()
        {
           _command.Name.Should().Be(nameof(Remove));
        }
    }
}
