using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Blarser.Commands.Commands
{
    [ Command ]
    public class CascTestCommand : BaseCommand
    {
        /// <inheritdoc />
        public CascTestCommand( IConsole console ) : base( console )
        {
        }

        /// <inheritdoc />
        protected override async Task<int> OnExecuteAsync( CommandLineApplication command, CancellationToken cancellationToken )
        {
            return 0;
        }
    }
}