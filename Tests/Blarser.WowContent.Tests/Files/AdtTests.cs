using Blarser.WowContent.WowFiles;
using NUnit.Framework;

namespace Blarser.WowContent.Tests.Files
{
    [ TestFixture ]
    public class AdtTests
    {
        [ Test ]
        [ TestCase( "world/maps/unknown_901_33978_002/unknown_901_33978_002_25_20_lod_3508931.adt", "unknown_901_33978_002", 25, 20, "lod_3508931" ) ]
        [ TestCase( "world/maps/uldumphasedentrance/uldumphasedentrance_35_48.adt", "uldumphasedentrance", 35, 48, null ) ]
        public void Parse_adt_blocks( string adtPath, string expectedMap, int expectedX, int expectedY, string expectedSuffix )
        {
            Adt.GetADTBlock( adtPath, out var map, out var x, out var y, out var suffix );

            Assert.AreEqual( expectedMap, map );
            Assert.AreEqual( expectedX, x );
            Assert.AreEqual( expectedY, y );
            Assert.AreEqual( expectedSuffix, suffix );
        }
    }
}