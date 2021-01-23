using System;
using System.Threading;
using System.Threading.Tasks;
using Blarser.WowContent.FileSystem;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.FileProviders;

namespace Blarser.Commands.Commands
{
    public abstract class CascCommand : BaseCommand
    {
        [ Option( "--content" ) ]
        public string FileSystem { get; set; }

        [ Option( "--install" ) ]
        public string InstallPath { get; set; }

        [ Option( "--listfile" ) ]
        public string ListFile { get; set; }

        /// <inheritdoc />
        public CascCommand( IConsole console ) : base( console )
        {
        }

        protected IFileProvider GetFileProvider()
        {
            if(!string.IsNullOrWhiteSpace( FileSystem ))
                return new PhysicalFileProvider( FileSystem );

            if(!string.IsNullOrWhiteSpace( InstallPath ))
                return new CascFileProvider( false, InstallPath, ListFile, "wow" );

            return null;
        }
    }
}