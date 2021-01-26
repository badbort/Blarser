using Microsoft.Extensions.FileProviders;

namespace Blarser.WowContent.FileSystem
{
    public interface ICascFileProvider :IFileProvider, IContentLookup
    {
    }
}