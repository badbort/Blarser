namespace Blarser.WowContent.WowFiles.Chunks
{
    public class MAID
    {
        public MapFileDataIDs[] MapFileDataIDs { get; set; }
    }
    
    public struct MapFileDataIDs
    {
        uint rootADT; // reference to fdid of mapname_xx_yy.adt
        uint obj0ADT; // reference to fdid of mapname_xx_yy_obj0.adt
        uint obj1ADT; // reference to fdid of mapname_xx_yy_obj1.adt
        uint tex0ADT; // reference to fdid of mapname_xx_yy_tex0.adt
        uint lodADT;  // reference to fdid of mapname_xx_yy_lod.adt
        uint mapTexture; // reference to fdid of mapname_xx_yy.blp
        uint mapTextureN; // reference to fdid of mapname_xx_yy_n.blp
        uint minimapTexture; // reference to fdid of mapxx_yy.blp
    } 
}