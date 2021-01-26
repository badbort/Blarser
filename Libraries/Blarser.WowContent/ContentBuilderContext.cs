using Blarser.WowContent.FileSystem;
using Microsoft.Extensions.FileProviders;

namespace Blarser.WowContent
{
    public class ContentBuilderContext
    {
        public IFileProvider FileProvider { get; set; }
        
        public IContentLookup FileLookup { get; set; }
    }
}