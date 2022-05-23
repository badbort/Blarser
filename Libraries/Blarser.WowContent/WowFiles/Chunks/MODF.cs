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
        public SMMapObjDef[] Items { get; set; }
    }

    [GenerateRead(64)]
    public class SMMapObjDef
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
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return string.Format( "SMMapObjDef. Pos: {0}, {1}, {2}. Rotation: {3}", Position.X, Position.Y, Position.Z, Rotation );
        }
    }
}