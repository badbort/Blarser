using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using Blarser.WowContent.WowFiles.Chunks;

namespace Blarser.WowContent.WowFiles
{
    // ReSharper disable once PartialTypeWithSinglePart
    // [UsedImplicitly]
    public static partial class ChunkReader
    {
        public static UInt32 ReadUInt32( ref ReadOnlySequence<byte> buffer, ref Span<byte> data4 )
        {
            var data = buffer.Slice( 0, 4 );
            data.CopyTo( data4 );
            buffer = buffer.Slice( data.End );

            return BinaryPrimitives.ReadUInt32LittleEndian( data4 );
        }

        public static float ReadFloat( ref ReadOnlySequence<byte> buffer, ref Span<byte> data4 )
        {
            var data = buffer.Slice( 0, 4 );
            data.CopyTo( data4 );
            buffer = buffer.Slice( data.End );

            return BinaryPrimitives.ReadSingleLittleEndian( data4 );
        }

        public static Int16 ReadInt16( ref ReadOnlySequence<byte> buffer, ref Span<byte> data4 )
        {
            var data = buffer.Slice( 0, 2 );
            data.CopyTo( data4 );
            buffer = buffer.Slice( data.End );

            return BinaryPrimitives.ReadInt16LittleEndian( data4 );
        }
        
        public static UInt16 ReadUInt16( ref ReadOnlySequence<byte> buffer, ref Span<byte> data4 )
        {
            var data = buffer.Slice( 0, 2 );
            data.CopyTo( data4 );
            buffer = buffer.Slice( data.End );

            return BinaryPrimitives.ReadUInt16LittleEndian( data4 );
        }
        

        public static UInt64 ReadUInt64( ref ReadOnlySequence<byte> buffer, ref Span<byte> data4 )
        {
            var data = buffer.Slice( 0, 8 );
            data.CopyTo( data4 );
            buffer = buffer.Slice( data.End );

            return BinaryPrimitives.ReadUInt64LittleEndian( data4 );
        }

        public static byte ReadByte( ref ReadOnlySequence<byte> buffer, ref Span<byte> data4 )
        {
            var data = buffer.Slice( 0, 1 );
            data.CopyTo( data4 );
            buffer = buffer.Slice( data.End );

            return data4[0];
        }

        public static Vector3 ReadVector3( ref ReadOnlySequence<byte> buffer, ref Span<byte> data4 )
        {
            float x = ReadFloat( ref buffer, ref data4 );
            float y = ReadFloat( ref buffer, ref data4 );
            float z = ReadFloat( ref buffer, ref data4 );

            return new Vector3( x, y, z );
        }

        public static CAaBox ReadCAaBox( ref ReadOnlySequence<byte> buffer, ref Span<byte> data4 )
        {
            var box = new CAaBox();
            box.Min = ReadVector3( ref buffer, ref data4 );
            box.Max = ReadVector3( ref buffer, ref data4 );

            return box;
        }

        public static CArgb ReadCArgb( ref ReadOnlySequence<byte> buffer, ref Span<byte> data4 )
        {
            var box = new CArgb();

            var data = buffer.Slice( 0, 4 );
            data.CopyTo( data4 );
            buffer = buffer.Slice( data.End );

            box.R = data4[0];
            box.G = data4[1];
            box.B = data4[2];
            box.A = data4[3];

            return box;
        }

        public static CImVector ReadCImVector( ref ReadOnlySequence<byte> buffer, ref Span<byte> data4 )
        {
            var box = new CImVector();

            var data = buffer.Slice( 0, 4 );
            data.CopyTo( data4 );
            buffer = buffer.Slice( data.End );

            box.B = data4[0];
            box.G = data4[1];
            box.R = data4[2];
            box.A = data4[3];

            return box;
        }

        public static string ReadStringReverse(ref ReadOnlySequence<byte> buffer, int size )
        {
            var slice = buffer.Slice( 0, size );
            buffer = buffer.Slice( slice.End );
            return string.Join( null, Encoding.Default.GetString(  slice  ).Reverse() );
        }
        
        public static string ReadString(ref ReadOnlySequence<byte> buffer, int size )
        {
            var slice = buffer.Slice( 0, size );
            buffer = buffer.Slice( slice.End );
            return string.Join( null, Encoding.Default.GetString(  slice  ) );
        }

        public static string[] ReadStringArray( ref ReadOnlySequence<byte> buffer )
        {
            var reader = new SequenceReader<byte>(buffer);

            List<string> strings = new List<string>();
            
            while (!reader.End) // loop until we've read the entire sequence
            {
                if (reader.TryReadTo(out ReadOnlySpan<byte> itemBytes, (byte) '\0', advancePastDelimiter: true)) // we have an item to handle
                {
                    var stringLine = Encoding.Default.GetString(itemBytes);
                    
                    strings.Add(stringLine);
                }
                // else if (isCompleted) // read last item which has no final delimiter
                // {
                //     var stringLine = ReadLastItem(sequence.Slice(reader.Position));
                //     Console.WriteLine(stringLine);
                //     reader.Advance(sequence.Length); // advance reader to the end
                // }
                else // no more items in this sequence
                {
                    break;
                }
            }

            return strings.ToArray();
        }
    }
}