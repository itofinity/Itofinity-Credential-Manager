using Itofinity.Refit.Cli.Utils.Commands;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Itofinity.Refit.Cli.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using Itofinity.Refit.Cli.Utils.Options;
using System.Threading.Tasks;
using Spi.Credentials;
using Manager.Model;
using Security.Common;
using Common.Gcmw;
using Spi.Hosts;

namespace Manager.Commands
{
    // TODO Swap around, make Erase the implementation and Delete the overload

    [Export(typeof(ICommandDefinition))]
    public class Delete : Erase
    {
        public override string Name { get; } = nameof(Delete);
    }
}