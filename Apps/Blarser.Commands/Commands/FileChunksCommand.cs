﻿using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Blarser.WowContent.FileSystem;
using Blarser.WowContent.WowFiles;
using Blarser.WowContent.WowFiles.Chunks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;

namespace Blarser.Commands.Commands
{
    [Command(UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.Throw)]
    public class FileChunksCommand: CascCommand
    {
        [Argument(0,"Files")]
        public string[] Files { get; set; }
        
        /// <inheritdoc />
        public FileChunksCommand( IConsole console ) : base( console )
        {
        }

        /// <inheritdoc />
        protected override async Task<int> OnExecuteAsync( CommandLineApplication command, CancellationToken cancellationToken )
        {
            var provider = GetFileProvider();

            List<IFileInfo> testFiles = new List<IFileInfo>(); 
            
            foreach(var input in Files.ToList())
            {
                if(Path.HasExtension( input ) )
                {
                    IFileInfo fileInfo;

                    if(Path.IsPathRooted( input ))
                        fileInfo = new PhysicalFileInfo( new FileInfo( input ) );
                    else
                        fileInfo = provider.GetFileInfo( input );

                    testFiles.Add( fileInfo );
                }
                else if(provider.GetDirectoryContents( input ) is {Exists: true} contents)
                {
                    foreach(var directoryContent in contents.Where(f => !f.IsDirectory))
                        testFiles.Add( directoryContent );
                }
            }
            
            foreach(var fileInfo in testFiles)
            {
                if(!fileInfo.Exists)
                    Console.WriteLine( $"File {fileInfo.PhysicalPath} does not exist" );
                else
                {
                    // await using var ms = new MemoryStream();
                    await using var stream = fileInfo.CreateReadStream();

                    Console.WriteLine($"File {fileInfo.PhysicalPath} length: {stream.Length}");

                    await Test( stream, fileInfo.Name, provider, cancellationToken );
                    // await str.CopyToAsync( ms, cancellationToken );
                    //
                    // Test( ms.ToArray() );
                }
            }

            return 0;
        }

        private async Task Test( Stream stream, string fileInfoName, ICascFileProvider casc, CancellationToken cancellationToken )
        {
            Stopwatch sw = Stopwatch.StartNew();
            PipeReader reader = PipeReader.Create(stream);

            int chunks = 0;
            long size = 0;
            Dictionary<string, int> occurrences = new();
            
            try
            {

                while (true)
                {
                    ReadResult read = await reader.ReadAsync( cancellationToken );
                    ReadOnlySequence<byte> buffer = read.Buffer;

                    if(TryReadChunk( ref buffer, out string chunk, out ReadOnlySequence<byte> chunkBuffer ))
                    {
                        Console.WriteLine( $"Chunk {chunk} at index {chunkBuffer.Start.GetInteger() - 8} to {chunkBuffer.End.GetInteger()} for {chunkBuffer.Length} bytes" );
                        chunks++;
                        size += chunkBuffer.Length + 8;

                        Scan( chunkBuffer );

                        occurrences.TryGetValue( chunk, out var c );
                        occurrences[chunk] = c + 1;

                        if(chunk == "MAID")
                        {
                            
                        }
                        else if(chunk == "MOGP")
                        {
                            var buffer2 = chunkBuffer.Slice( 68 );
                            while(TryReadChunk( ref buffer2, out string subchunk, out ReadOnlySequence<byte> subChunkBuffer ))
                            {
                                Console.WriteLine( $"MOGP subchunk {subchunk} at index {subChunkBuffer.Start.GetInteger() - 8} to {subChunkBuffer.End.GetInteger()} for {subChunkBuffer.Length} bytes" );

                            }
                        }
                        else if(chunk == "MWID")
                        {
                            NullSeparatedStringsChunk.TryParse( buffer );
                        }
                        else if( chunk == "MWMO")
                        {
                            
                        }
                        else if(chunk == "MODF" && chunkBuffer.Length > 0)
                        {
                            // var modf = MODF.Create( ref chunkBuffer );
                            MODF modf = ChunkReader.ReadMODF( ref chunkBuffer );

                            if(modf.Items.Length > 0)
                                Console.WriteLine( fileInfoName + " MODF:" );

                            foreach(var modfEntry in modf.Items)
                            {
                                casc.TryGetFile( (int) modfEntry.mwidEntry, out _, out string fileName );

                                if(fileName != null)
                                    Console.WriteLine( $"\tFile: {fileName}" );
                            }
                        }
                        else if(chunk == "MOHD")
                        {
                            MOHD mohd = ChunkReader.ReadMOHD( ref chunkBuffer );
                        }
                        else if(chunk == "MOMT")
                        {
                            MOMT momt = ChunkReader.ReadMOMT( ref chunkBuffer );

                            if(momt.Materials.Length > 0)
                                Console.WriteLine( fileInfoName  + " MOMT:");
                            
                            foreach(var mat in momt.Materials)
                            {
                                if(casc.TryGetFile( (int) mat.texture_1, out _, out string texture ))
                                {
                                    Console.WriteLine("\t" + texture );
                                }
                            }

                        }
                        else if(chunk == "MOTX")
                        {
                            var motx = ChunkReader.ReadMOTX( ref chunkBuffer );
                        }
                    }

                    if (read.IsCompleted && read.Buffer.IsEmpty)
                    {
                        if (buffer.Length > 0)
                        {
                            // We have an incomplete message and there's no more data to process
                            throw new InvalidDataException("Incomplete data");
                        }
                    
                        break;
                    }
                
                    reader.AdvanceTo(buffer.Start, buffer.End);
                }
            }
            finally
            {
                await reader.CompleteAsync();
            }
            
            sw.Stop();
            // Console.WriteLine( $"Completed in {sw.ElapsedMilliseconds} ms with {chunks} chunks and {size} bytes" );
        }

        private static void Scan( ReadOnlySequence<byte> readOnlySequence )
        {
            Span<byte> search = stackalloc byte[4];
            Span<byte> temp = stackalloc byte[4];
            BinaryPrimitives.WriteUInt32LittleEndian( search, 210902 );

            while(readOnlySequence.Length >= 4)
            {
                readOnlySequence.Slice(0,4).CopyTo(temp);

                if(temp == search)
                {
                    
                }

                bool areSame = true;
                for(int i = 0; i < 4; i++)
                {
                    if(temp[i] != search[i])
                        areSame = false;
                }

                if(areSame)
                {
                    
                }
                
                readOnlySequence = readOnlySequence.Slice( 1 );
            }
        }

        private static bool TryReadChunk( ref ReadOnlySequence<byte> buffer, out string chunkType, out ReadOnlySequence<byte> chunkData )
        {
            // See: https://docs.microsoft.com/en-us/dotnet/standard/io/buffers
            chunkData = default;
            chunkType = default;

            // 4 bytes for chunk string
            // 4 bytes for chunk size
            if(buffer.Length < 8)
                return false;
            
            // chunkType = string.Join( null, Encoding.Default.GetString( buffer.Slice( 0, 4 ) ).Reverse() );
            // var chunkLength = BinaryPrimitives.ReadUInt32LittleEndian( buffer.Slice( 4, 4 ).FirstSpan );

            
            // var infoSlice = buffer.Slice( buffer.Start, 8 );
            // var chunkSlice = infoSlice.Slice( 0, 4);
            // var lengthSlice = buffer.Slice(4, 4);
            
            Span<byte> chunkLengthSpan = stackalloc byte[4];
            buffer.Slice( 4, 4 ).CopyTo(chunkLengthSpan);
            var chunkLength = BinaryPrimitives.ReadUInt32LittleEndian( chunkLengthSpan );
            
            // var iSegment = buffer.Start;
            // lengthSlice.TryGet( ref iSegment, out var readMemory, true );
            
            if(buffer.Length < 8 + chunkLength)
            {
                // System.Console.WriteLine($"Waiting for {chunkLength} data");
                return false;
            }
            
            chunkType = string.Join( null, Encoding.Default.GetString(  buffer.Slice( 0, 4 )  ).Reverse() );
            chunkData = buffer.Slice( 8, chunkLength );
            
            buffer = buffer.Slice( chunkData.End );
            
            // System.Console.WriteLine($"Shifting buffer to {buffer.Length}");
            return true;
        }
    }
}