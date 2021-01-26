using System.Collections.Generic;

namespace Blarser.WowContent.FileSystem
{
    public interface IContentLookup
    {
        bool TryGetFile( int fileIndex, out ulong hash, out string fileName );
    }
}