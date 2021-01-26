using System;

namespace Blarser.WowContent.WowFiles.Chunks
{
    [ AttributeUsage( AttributeTargets.Class | AttributeTargets.Struct ) ]
    public class ChunkEntryAttribute : Attribute
    {
        /// <summary>
        /// Length in bytes for a single entry/record. This is used in advance to determine how many instances there are.
        /// </summary>
        public int RecordLength { get; }

        public ChunkEntryAttribute( int recordSize ) => RecordLength = recordSize;
    }
}