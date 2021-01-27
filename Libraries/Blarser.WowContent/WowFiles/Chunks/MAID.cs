using System;
using System.Numerics;

namespace Blarser.WowContent.WowFiles.Chunks
{
    [FileChunk]
    public class MAID
    {
        [ChunkField]
        public MapFileDataIDs[] MapFileDataIDs { get; set; }
    }
    
    [ChunkEntry(32)]
    public struct MapFileDataIDs
    {
        [ChunkField] public uint rootADT {get;set;} // reference to fdid of mapname_xx_yy.adt
        [ChunkField] public uint obj0ADT {get;set;} // reference to fdid of mapname_xx_yy_obj0.adt
        [ChunkField] public uint obj1ADT {get;set;} // reference to fdid of mapname_xx_yy_obj1.adt
        [ChunkField] public uint tex0ADT {get;set;} // reference to fdid of mapname_xx_yy_tex0.adt
        [ChunkField] public uint lodADT {get;set;}  // reference to fdid of mapname_xx_yy_lod.adt
        [ChunkField] public uint mapTexture {get;set;} // reference to fdid of mapname_xx_yy.blp
        [ChunkField] public uint mapTextureN {get;set;} // reference to fdid of mapname_xx_yy_n.blp
        [ChunkField] public uint minimapTexture {get;set;} // reference to fdid of mapxx_yy.blp
    } 
}