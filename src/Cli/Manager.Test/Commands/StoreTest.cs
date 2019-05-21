using System;
using Xunit;
using Manager.Commands;
using FluentAssertions;

namespace Manager.Test.Commands
{
    public class StoreTest
    {
        private Store _command = new Store();

        [Fact]
        public void VerifyName()
        {
           _command.Name.Should().Be(nameof(Store));
        }
    }
}
