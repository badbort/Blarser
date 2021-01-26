using System;
using System.Buffers;
using System.Buffers.Binary;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Blarser.WowContent.WowFiles.Chunks
{
    [FileChunk]
    public class MODF
    {
        [ChunkField]
        public MODFEntry[] Entries { get; set; }

        public static MODF Create( ref ReadOnlySequence<byte> buffer )
        {
            var modf = new MODF();

            var items = buffer.Length / 64;
            modf.Entries = new MODFEntry[items];

            for(int i = 0; i < items; i++)
            {
                modf.Entries[i] = MODFEntry.Read( ref buffer );
            }

            return modf;

            /*
            var reader = new SequenceReader<byte>( buffer );

            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            var data = pool.Rent( (int) buffer.Length );

            using var ms = new MemoryStream(data);
            using var br = new BinaryReader( ms );
            
            buffer.CopyTo(data);
            
            for(int i = 0; i < items; i++)
            {
                var item = buffer.Slice( buffer.Start, 64 );
                item.CopyTo(data);

                
                modf.Entries[i] = MODFEntry.Read( br );

                var modf2 = MODFEntry.Read( ref buffer );

                //buffer = buffer.Slice( item.End );
            }

            // Span<byte> data = stackalloc byte[4];
            // buffer.Slice( 0, 4 ).CopyTo(data);

            // if(reader.TryReadLittleEndian( out int n ))
            // modf.nameId = (uint) n;

            pool.Return(data, true);
            return modf;
            */
        }
    }

    [ChunkEntry(64)]
    public class MODFEntry
    {
        /// <summary>
        /// References an entry in the MWID chunk, specifying the model to use.
        /// </summary>
        [ChunkField]
        public UInt32 mwidEntry { get; set; }

        /// <summary>
        /// this ID should be unique for all ADTs currently loaded. Best, they are unique for the whole map.
        /// </summary>
        [ChunkField]
        public UInt32 uniqueId { get; set; }

        /// <summary>
        /// Gets the position.
        /// </summary>
        [ChunkField]
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets the Rotation. Same as in MDDF.
        /// </summary>
        [ChunkField]
        public Vector3 Rotation { get; set; }

        /// <summary>
        /// Gets the lower bounds. these two are position plus the wmo bounding box.
        /// </summary>
        [ChunkField]
        public Vector3 LowerBounds { get; set; }

        /// <summary>
        /// Gets the upper bounds. They are used for defining when if they are rendered as well as collision.
        /// </summary>
        [ChunkField]
        public Vector3 UpperBounds { get; set; }

        /// <summary>
        /// Gets the flags. Values from enum MODFFlags.
        /// </summary>
        [ChunkField]
        public UInt16 flags { get; set; }

        /// <summary>
        /// Gets the doodad set. Which WMO doodad set is used.
        /// </summary>
        [ChunkField]
        public UInt16 doodadSet { get; set; }

        /// <summary>
        /// Which WMO name set is used. Used for renaming goldshire inn to northshire inn while using the same model.
        /// </summary>
        [ChunkField]
        public UInt16 nameSet { get; set; }

        /// <summary>
        /// it reads only a WORD into the WMAPOBJDEF structure for name. I don't know about the rest.
        /// </summary>
        [ChunkField]
        public UInt16 padding { get; set; }

        /// <summary>
        /// Reads the specified entry.
        /// </summary>
        public static MODFEntry Read( BinaryReader reader )
        {
            var modfEntry = new MODFEntry();

            modfEntry.mwidEntry = reader.ReadUInt32();
            modfEntry.uniqueId = reader.ReadUInt32();
            modfEntry.Position = new Vector3( reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle() );
            modfEntry.Rotation = new Vector3( reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle() );
            modfEntry.LowerBounds = new Vector3( reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle() );
            modfEntry.UpperBounds = new Vector3( reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle() );
            modfEntry.flags = reader.ReadUInt16();
            modfEntry.doodadSet = reader.ReadUInt16();
            modfEntry.nameSet = reader.ReadUInt16();
            modfEntry.padding = reader.ReadUInt16();

            return modfEntry;
        }

        public static MODFEntry Read( ref ReadOnlySequence<byte> buffer )
        {
            Span<byte> data4 = stackalloc byte[4];
            var item = new MODFEntry();

            item.mwidEntry = ChunkSequenceReader.ReadUInt32( ref buffer, ref data4 );
            item.uniqueId = ChunkSequenceReader.ReadUInt32( ref buffer, ref data4 );
            item.Position = ChunkSequenceReader.ReadVector3( ref buffer, ref data4 );
            item.Rotation = ChunkSequenceReader.ReadVector3( ref buffer, ref data4 );
            item.LowerBounds = ChunkSequenceReader.ReadVector3( ref buffer, ref data4 );
            item.UpperBounds = ChunkSequenceReader.ReadVector3( ref buffer, ref data4 );
            item.flags = ChunkSequenceReader.ReadUInt16( ref buffer, ref data4 );
            item.doodadSet = ChunkSequenceReader.ReadUInt16( ref buffer, ref data4 );
            item.nameSet = ChunkSequenceReader.ReadUInt16( ref buffer, ref data4 );
            item.padding = ChunkSequenceReader.ReadUInt16( ref buffer, ref data4 );

            return item;
        }


        /// <summary>
        /// Reads the specified entry.
        /// </summary>
        public static MODFEntry Read( SequenceReader<byte> reader )
        {
            var modfEntry = new MODFEntry();

            // Buffer


            modfEntry.mwidEntry = reader.ReadUInt32();
            modfEntry.uniqueId = reader.ReadUInt32();
            modfEntry.Position = new Vector3( reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle() );
            modfEntry.Rotation = new Vector3( reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle() );
            modfEntry.LowerBounds = new Vector3( reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle() );
            modfEntry.UpperBounds = new Vector3( reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle() );
            // modfEntry.flags = reader.ReadUInt16();
            // modfEntry.doodadSet = reader.ReadUInt16();
            // modfEntry.nameSet = reader.ReadUInt16();
            // modfEntry.padding = reader.ReadUInt16();

            reader.Advance( 8 );
            return modfEntry;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return string.Format( "MODFEntry. Pos: {0}, {1}, {2}. Rotation: {3}", Position.X, Position.Y, Position.Z, Rotation );
        }
    }

    [ Flags ]
    public enum MODFFlags
    {
        Destroyable = 1,
    }

    public static class SequenceReaderExtensions
    {
        public static int ReadInt( this SequenceReader<byte> s )
        {
            if(!s.TryReadLittleEndian( out int val ))
                throw new InvalidOperationException();
            return val;
        }

        public static uint ReadUInt32( this SequenceReader<byte> s ) => (uint) ReadInt( s );

        public static float ReadSingle( this SequenceReader<byte> s )
        {
            int intVal = ReadInt( s );
            return Unsafe.As<int, float>( ref intVal );
        }
    }
}