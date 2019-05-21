using System;
using Xunit;
using Manager.Commands;
using FluentAssertions;

namespace Manager.Test.Commands
{
    public class FillTest
    {
        private Fill _command = new Fill();

        [Fact]
        public void VerifyName()
        {
           _command.Name.Should().Be(nameof(Fill));
        }
    }
}
