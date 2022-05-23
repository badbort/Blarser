using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blarser.WowContent.WowFiles;
using McMaster.Extensions.CommandLineUtils;

namespace Blarser.Commands.Commands
{
    [ Command ]
    public class DbcCommand : CascCommand
    {
        [Argument(0,"Files")]
        public string[] Files { get; set; }

        /// <inheritdoc />
        public DbcCommand( IConsole console ) : base( console )
        {
        }

        /// <inheritdoc />
        protected override async Task<int> OnExecuteAsync( CommandLineApplication command, CancellationToken cancellationToken )
        {
            if(!(Files?.Length > 0))
            {
                Console.WriteLine( "No file args specified." );
                return 0;
            }
            
            var provider = GetFileProvider();
            
            foreach(var fileInfo in Files.Select(s => provider.GetFileInfo(s)))
            {
                if(!fileInfo.Exists)
                {
                    Console.WriteLine( $"{fileInfo.PhysicalPath} does not exist." );
                    continue;
                }
                
                await using var stream = fileInfo.CreateReadStream();
                PipeReader reader = PipeReader.Create(stream);

                wdc3_db2_header header = null;
                wdc3_section_header[] sections = null;
                field_structure[] fields = null;

                while(true)
                {
                    ReadResult read = await reader.ReadAsync( cancellationToken );
                    ReadOnlySequence<byte> buffer = read.Buffer;

                    if(header == null && !DB2Reader.TryReadHeader( ref buffer, out header ))
                        continue;

                    if(sections == null && !DB2Reader.TryReadSectionHeaders( header, ref buffer, out sections ))
                        continue;

                    
                    break;
                }
            }

            return 0;
        }
    }
}