using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CASCLib;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Blarser.WowContent.FileSystem
{
    public class CascFileProvider : IFileProvider
    {
        private static readonly char[] s_pathSeparators = {Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar};
        private static readonly LocaleFlags s_localeFlags = LocaleFlags.enUS;

        private readonly CascDirectoryInfo _rootDirInfo;
        private readonly CASCFolder _rootCascFolder;
        private readonly string _listFile;

        public CASCHandler CascHandler { get; }

        public CascFileProvider( bool online, string gamePath, string listFile, string wowProduct = "wow" )
        {
            _listFile = listFile;
            CASCConfig.LoadFlags |= LoadFlags.Install;
            CASCConfig config = online ? CASCConfig.LoadOnlineStorageConfig( wowProduct, "us" ) : CASCConfig.LoadLocalStorageConfig( gamePath, wowProduct );

            var progress = new BackgroundWorkerEx();
            CascHandler = CASCHandler.OpenStorage( config, progress );

            CascHandler.Root.SetFlags( s_localeFlags, false, false );
            CascHandler.Root.LoadListFile( listFile, progress );

            _rootCascFolder = CascHandler.Root.SetFlags( s_localeFlags, false );
            _rootDirInfo = new CascDirectoryInfo( "", _rootCascFolder, null, this );
        }

        /// <inheritdoc />
        public IFileInfo GetFileInfo( string subpath )
        {
            var paths = GetParts( subpath );

            if(paths == null)
                return new NotFoundFileInfo( subpath );
            
            CascDirectoryInfo currentDir = _rootDirInfo;

            for(int i = 0; i < paths.Length; i++)
            {
                var part = paths[i];

                if(!currentDir.Contents.TryGetValue( part, out var filePart ))
                    break;

                if(filePart.IsDirectory)
                {
                    currentDir = (CascDirectoryInfo) filePart;
                }
                else
                {
                    var cascFile = (CascFileInfo) filePart;

                    if(i == paths.Length - 1)
                    {
                        // Found!
                        return cascFile;
                    }
                    else
                        break;
                }
            }

            return new NotFoundFileInfo( subpath );
        }

        /// <inheritdoc />
        public IDirectoryContents GetDirectoryContents( string subpath )
        {
            // Relative paths starting with leading slashes are okay
            subpath = subpath.TrimStart( s_pathSeparators );

            // Absolute paths not permitted.
            if(Path.IsPathRooted( subpath ))
                return new NotFoundDirectoryContents();

            var paths = subpath.Split( s_pathSeparators );

            if(paths.Length == 1 && string.IsNullOrEmpty( paths[0] ))
                return new CascDirectoryContents( this, _rootDirInfo );

            var current = _rootCascFolder;

            return new NotFoundDirectoryContents();
        }

        private string[] GetParts( string subpath )
        {
            // Relative paths starting with leading slashes are okay
            subpath = subpath.TrimStart( s_pathSeparators );

            // Absolute paths not permitted.
            if(Path.IsPathRooted( subpath ))
                return null;

            var paths = subpath.Split( s_pathSeparators );

            if(paths.Length == 1 && string.IsNullOrEmpty( paths[0] ))
                return new string[0];
            else if(paths.Length > 1 && string.IsNullOrEmpty( paths[0] ))
                paths = paths.Skip( 1 ).ToArray();

            return paths;
        }

        /// <inheritdoc />
        public IChangeToken Watch( string filter ) => NullChangeToken.Singleton;
    }

    internal class CascDirectoryContents : IDirectoryContents
    {
        private readonly CascFileProvider _provider;
        private readonly CascDirectoryInfo _cascDir;

        /// <inheritdoc />
        public bool Exists { get; } = true;

        public CascDirectoryContents( CascFileProvider provider, CascDirectoryInfo cascDir )
        {
            _cascDir = cascDir;
            _provider = provider;
        }

        /// <inheritdoc />
        public IEnumerator<IFileInfo> GetEnumerator()
        {
            foreach(var (key, value) in _cascDir.CascFolder.Entries)
            {
                if(value is CASCFolder cascFolder)
                {
                    yield return new CascDirectoryInfo( key, cascFolder, _cascDir, _provider );
                }
                else if(value is CASCFile cascFile)
                {
                    yield return new CascFileInfo( key, cascFile, _provider );
                }
                else
                {
                }
            }
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal class CascFileInfo : IFileInfo
    {
        private readonly CascFileProvider _provider;
        private readonly CASCFile _cascFile;
        private readonly string _key;
        private long? _fileSize;

        /// <inheritdoc />
        public bool Exists { get; } = true;

        /// <inheritdoc />
        public long Length => _fileSize ??= _cascFile.GetSize( _provider.CascHandler );

        /// <inheritdoc />
        public string PhysicalPath => _cascFile.FullName;

        /// <inheritdoc />
        public string Name => _cascFile.Name;

        /// <inheritdoc />
        public DateTimeOffset LastModified { get; } = DateTimeOffset.Now;

        /// <inheritdoc />
        public bool IsDirectory => false;

        public CascFileInfo( string key, CASCFile cascFile, CascFileProvider provider )
        {
            _provider = provider;
            _cascFile = cascFile;
            _key = key;
        }

        /// <inheritdoc />
        public Stream CreateReadStream() => _provider.CascHandler.OpenFile( _cascFile.Name );
    }

    internal class CascDirectoryInfo : IFileInfo
    {
        private readonly CascFileProvider _provider;
        private readonly CascDirectoryInfo _parent;
        private Dictionary<string, IFileInfo> cascContents;
        private readonly string _key;
        private string path;

        /// <inheritdoc />
        public bool Exists { get; } = true;

        /// <inheritdoc />
        public long Length => -1;

        /// <inheritdoc />
        public string PhysicalPath => path ??= GetFullPath();

        /// <inheritdoc />
        public string Name => CascFolder.Name;

        /// <inheritdoc />
        public DateTimeOffset LastModified { get; } = DateTimeOffset.Now;

        /// <inheritdoc />
        public bool IsDirectory => true;

        public CASCFolder CascFolder { get; }

        public Dictionary<string, IFileInfo> Contents => cascContents ??= new Dictionary<string, IFileInfo>( GetContents(), StringComparer.InvariantCultureIgnoreCase );

        public CascDirectoryInfo( string key, CASCFolder folder, CascDirectoryInfo parent, CascFileProvider provider )
        {
            _provider = provider;
            _parent = parent;
            _key = key;
            CascFolder = folder;
        }

        /// <inheritdoc />
        public Stream CreateReadStream() => throw new InvalidOperationException();

        private IEnumerable<KeyValuePair<string, IFileInfo>> GetContents()
        {
            foreach(var (key, value) in CascFolder.Entries)
            {
                IFileInfo fileInfo = null;
                if(value is CASCFolder childCastFolder)
                    fileInfo = new CascDirectoryInfo( key, childCastFolder, this, _provider );
                else if(value is CASCFile cascFile)
                    fileInfo = new CascFileInfo( key, cascFile, _provider );
                else
                {
                }

                if(fileInfo != null)
                    yield return new KeyValuePair<string, IFileInfo>( value.Name, fileInfo );
            }
        }

        private string GetFullPath()
        {
            StringBuilder sb = new StringBuilder();

            var current = _parent;
            sb.Append( Name );

            while(current != null)
            {
                sb.Insert( 0, current.Name + "/" );
                current = current._parent;
            }

            return sb.ToString();
        }
    }
}