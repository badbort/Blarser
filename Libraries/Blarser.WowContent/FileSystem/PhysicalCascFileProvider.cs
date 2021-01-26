using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;

namespace Blarser.WowContent.FileSystem
{
    public class PhysicalCascFileProvider : PhysicalFileProvider, ICascFileProvider
    {
        private readonly Dictionary<int, string> _files;

        /// <inheritdoc />
        public PhysicalCascFileProvider( string root, string listFile = null ) : base( root )
        {
            _files = listFile != null ? GetFiles( listFile ) : null;
        }

        public static Dictionary<int, string> GetFiles( string file )
        {
            using var fileStream = File.OpenRead( file );
            // PipeReader pipeReader = PipeReader.Create( fileStream );
            // SequenceReader<byte> reader = new SequenceReader<byte>(pipeReader.)

            using var sr = new StreamReader( fileStream );

            Dictionary<int, string> files = new();

            while(!sr.EndOfStream)
            {
                var line = sr.ReadLine();

                var parts = line.Split( ';' );

                var id = int.Parse( parts[0] );
                files[id] = parts[1];
            }

            return files;
        }

        /// <inheritdoc />
        public bool TryGetFile( int fileIndex, out ulong hash, out string fileName )
        {
            hash = default;
            fileName = default;

            if(_files == null)
                return false;

            if(!_files.TryGetValue( fileIndex, out fileName ))
                return false;

            return true;
        }
    }
}