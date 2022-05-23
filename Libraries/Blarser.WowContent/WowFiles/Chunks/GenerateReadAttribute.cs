using System;

namespace Blarser.WowContent.WowFiles.Chunks
{
    [ AttributeUsage( AttributeTargets.Class | AttributeTargets.Struct ) ]
    public class GenerateReadAttribute : Attribute
    {
        /// <summary>
        /// Length in bytes for a single entry/record. This is used in advance to determine how many instances there are.
        /// </summary>
        public int RecordLength { get; }
        
        public bool GenerateArray { get; }

        public GenerateReadAttribute( int size, bool generateArray = false)
        {
            RecordLength = size;
            GenerateArray = generateArray;
        }
    }
}