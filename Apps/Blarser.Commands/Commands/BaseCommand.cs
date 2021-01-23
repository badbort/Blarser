using System;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Blarser.Commands.Commands
{
    public abstract class BaseCommand
    {
        protected IConsole Console { get; }

        protected BaseCommand( IConsole console )
        {
            Console = console;
        }

        protected abstract Task<int> OnExecuteAsync( CommandLineApplication command, CancellationToken cancellationToken );
    }

}