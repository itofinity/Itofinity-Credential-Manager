using System;
using Xunit;
using Manager.Commands;
using FluentAssertions;

namespace Manager.Test.Commands
{
    public class GetTest
    {
        private Get _command = new Get();

        [Fact]
        public void VerifyName()
        {
           _command.Name.Should().Be(nameof(Get));
        }
    }
}
