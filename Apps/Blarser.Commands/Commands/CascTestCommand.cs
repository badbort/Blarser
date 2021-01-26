using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Blarser.Commands.Commands
{
    [ Command ]
    public class CascTestCommand : CascCommand
    {
        [Argument(0)]
        public string File { get; set; }
        
        /// <inheritdoc />
        public CascTestCommand( IConsole console ) : base( console )
        {
        }

        /// <inheritdoc />
        protected override async Task<int> OnExecuteAsync( CommandLineApplication command, CancellationToken cancellationToken )
        {
            var provider = GetFileProvider();

            if(int.TryParse( File, out int fileNumber ))
            {
                if(provider.TryGetFile( fileNumber, out _, out var fileName ))
                {
                    File = fileName;
                    Console.WriteLine( $"Successfully resolved {fileNumber} with {File}" );
                }
                else
                {
                    Console.WriteLine( $"Failed to find associated file with id {fileNumber}" );
                    return 1;
                }
            }
            
            return 0;
        }
    }
}