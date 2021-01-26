using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using Blarser.WowContent.WowFiles.Chunks;

namespace Blarser.WowContent.WowFiles
{
    public static partial class ChunkSequenceReader
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

        public static UInt16 ReadUInt16( ref ReadOnlySequence<byte> buffer, ref Span<byte> data4 )
        {
            var data = buffer.Slice( 0, 2 );
            data.CopyTo( data4 );
            buffer = buffer.Slice( data.End );

            return BinaryPrimitives.ReadUInt16LittleEndian( data4 );
        }

        public static Vector3 ReadVector3( ref ReadOnlySequence<byte> buffer, ref Span<byte> data4 )
        {
            float x = ReadFloat( ref buffer, ref data4 );
            float y = ReadFloat( ref buffer, ref data4 );
            float z = ReadFloat( ref buffer, ref data4 );

            return new Vector3( x, y, z );
        }

        /// <summary>
        /// Gets the <see cref="ChunkFieldAttribute"/> <see cref="PropertyInfo"/>s in order of declaration on the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetChunkProperties<T>()
        {
            return from property in typeof(T).GetProperties()
                where Attribute.IsDefined( property, typeof(ChunkFieldAttribute) )
                orderby ((ChunkFieldAttribute) property
                    .GetCustomAttributes( typeof(ChunkFieldAttribute), false )
                    .Single()).Order
                select property;
        }
    }

    // public static partial class ChunkSequenceReader
    // {
    //     public static MODFEntry Read( ref ReadOnlySequence<byte> buffer )
    //     {
    //         Span<byte> data4 = stackalloc byte[4];
    //         var item = new MODFEntry();
    //
    //         item.mwidEntry = ReadUInt32( ref buffer, ref data4 );
    //         item.uniqueId = ReadUInt32( ref buffer, ref data4 );
    //         item.Position = ReadVector3( ref buffer, ref data4 );
    //         item.Rotation = ReadVector3( ref buffer, ref data4 );
    //         item.LowerBounds = ReadVector3( ref buffer, ref data4 );
    //         item.UpperBounds = ReadVector3( ref buffer, ref data4 );
    //         item.flags = ReadUInt16( ref buffer, ref data4 );
    //         item.doodadSet = ReadUInt16( ref buffer, ref data4 );
    //         item.nameSet = ReadUInt16( ref buffer, ref data4 );
    //         item.padding = ReadUInt16( ref buffer, ref data4 );
    //         
    //
    //         return item;
    //     }
    // }
}