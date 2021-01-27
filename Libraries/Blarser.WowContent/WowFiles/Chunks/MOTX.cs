using System;

namespace Blarser.WowContent.WowFiles.Chunks
{
    [FileChunk]
    public class MOTX
    {
        [ChunkField]
        public String[] TextureNameList { get; set; } 
    }

    [FileChunk]
    public class MOGN
    {
        [ChunkField]
        public String[] GroupNameList { get; set; } 
    }
}