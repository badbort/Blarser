using System;
using System.Buffers;

namespace Blarser.WowContent.WowFiles.Chunks
{
    public class MWMO : NullSeparatedStringsChunk
    {
        
    }

    public class NullSeparatedStringsChunk
    {
        public static bool TryParse( ReadOnlySequence<byte> buffer )
        {
            SequencePosition position = buffer.Start;
            SequencePosition previous = position;
            var index = -1;

            // buffer.tr
            
            while(buffer.TryGet( ref position, out ReadOnlyMemory<byte> segment ))
            {
                
            }

            throw new NotImplementedException();
        }
    }
    
}