using System.Threading;
using System.Threading.Tasks;
using Blarser.WowContent.FileSystem;
using McMaster.Extensions.CommandLineUtils;

namespace Blarser.Commands.Commands
{
    [Command]
    public class CascFileCommand : CascCommand
    {
        
        /// <inheritdoc />
        public CascFileCommand( IConsole console ) : base( console )
        {
        }
        
        /// <inheritdoc />
        protected override async Task<int> OnExecuteAsync( CommandLineApplication command, CancellationToken cancellationToken )
        {
            var provider = GetFileProvider();

            var contents = provider.GetDirectoryContents( "/" );

            foreach(var file in contents)
                Console.WriteLine( $"{(file.IsDirectory ? "Directory" : "File")}: {file.PhysicalPath}" );

            var fileInfo = provider.GetFileInfo( "world/wmo/arathi/8ara_arathirockwmo_01.wmo" );
            
            return 0;
        }
    }
}