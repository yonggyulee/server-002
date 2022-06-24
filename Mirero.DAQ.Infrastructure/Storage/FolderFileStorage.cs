using Google.Protobuf.WellKnownTypes;

namespace Mirero.DAQ.Infrastructure.Storage;

public class FolderFileStorage : IFileStorage
{
    private string _NormalizePath(string path) {
        if (string.IsNullOrEmpty(path))
            return path;
            
        if (Path.DirectorySeparatorChar == '\\')
            path = path.Replace('/', Path.DirectorySeparatorChar);
        else if (Path.DirectorySeparatorChar == '/')
            path = path.Replace('\\', Path.DirectorySeparatorChar);

        return path;
    }

    public void DeleteFolder(string uri)
    {
        if (!Directory.Exists(uri))
            throw new IOException($"{nameof(uri)}({uri}) not found.");
        
        Directory.Delete(uri, true);
    }

    public void MoveFolder(string uri, string newUri)
    {
        if (!Directory.Exists(uri))
            throw new IOException($"{nameof(uri)}({uri}) not found.");

        Directory.Move(uri, newUri);
    }

    public bool FolderExists(string path)
    {
        return Directory.Exists(path);
    }

    public async Task<byte[]> GetFileBufferAsync(string uri, string filename, CancellationToken cancellationToken = default)
    {
        if (!Directory.Exists(uri))
            throw new IOException($"{nameof(uri)}({uri}) not found.");

        var filePath = Path.Combine(uri, filename);
        
        if (!File.Exists(filePath))
            throw new IOException($"{nameof(uri)}({uri}) not found.");

        await using var fs = File.OpenRead(filePath);
        var buffer = new byte[fs.Length];
        await fs.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
        
        return buffer;
    }

    public Task<FileInfo> GetFileInfoAsync(string uri)
    {
        throw new NotImplementedException();
    }

    public bool FileExists(string path)
    {
        return File.Exists(path);
    }
    
    public async Task CreateFolderAsync(string folderPath, CancellationToken cancellationToken)
    {
        if (Directory.Exists(folderPath))
            throw new IOException($"{nameof(folderPath)}({folderPath}) already exists.");

        Directory.CreateDirectory(folderPath);
    }

    public async Task SaveFileAsync(string filePath, byte[] buffer, CancellationToken cancellationToken = default)
    {
        if (File.Exists(filePath))
            throw new IOException($"{nameof(filePath)}({filePath}) already exists.");

        var directoryName = Path.GetDirectoryName(filePath) ?? throw new ArgumentException(nameof(filePath));

        if (!Directory.Exists(directoryName))
            Directory.CreateDirectory(directoryName);

        await using var fileStream = CreateFileStream(filePath);
        await fileStream.WriteAsync(buffer, cancellationToken);
    }

    //public async Task SaveFileAsync(string filePath, byte[] buffer, bool isInspectDir, CancellationToken cancellationToken = default)
    //{
    //    try
    //    {
    //        await SaveFileAsync(filePath, buffer, cancellationToken);
    //    }
    //    catch
    //    {
    //        if (!isInspectDir) throw;
    //        var directoryName = Path.GetDirectoryName(filePath) ?? throw new ArgumentException(nameof(filePath));
    //        if (Directory.EnumerateFiles(directoryName).Count() == 0)
    //        {
    //            Directory.Delete(directoryName);
    //        }
    //        throw;
    //    }
    //}

    public async Task SaveFileAsync(string uri, string filename, byte[] buffer,
        CancellationToken cancellationToken = default)
    {
        await SaveFileAsync(Path.Combine(uri, filename), buffer, cancellationToken);
    }

    public async Task SaveFilesAsync(IEnumerable<string> uris, IEnumerable<byte[]> buffers,
        CancellationToken cancellationToken = default)
    {
        var filePathList = uris.ToList();
        var bufferList = buffers.ToList();

        if (filePathList.Count != bufferList.Count)
        {
            throw new NotImplementedException();
        }

        var i = 0;
        try
        {
            for (i = 0; i < filePathList.Count; i++)
            {
                await SaveFileAsync(filePathList[i], bufferList[i], cancellationToken);
            }
        }
        catch
        {
            for (var j = 0; j < i; j++)
            {
                await DeleteFileAsync(filePathList[j], cancellationToken);
            }

            throw;
        }
    }

    public async Task SaveFilesAsync(string uri, IEnumerable<string> filenames, IEnumerable<byte[]> buffers,
        CancellationToken cancellationToken = default)
    {
        var filenameList = filenames.ToList();
        var bufferList = buffers.ToList();

        if (filenameList.Count != bufferList.Count)
        {
            throw new NotImplementedException();
        }

        var i = 0;
        try
        {
            for (i = 0; i < filenameList.Count; i++)
            {
                await SaveFileAsync(uri, filenameList[i], bufferList[i], cancellationToken);
            }
        }
        catch
        {
            for (var j = 0; j < i; j++)
            {
                await DeleteFileAsync(uri, filenameList[j], cancellationToken);
            }

            throw;
        }
    }

    public Task RenameFileAsync(string originUri, string newFilename, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(originUri))
            throw new ArgumentNullException(nameof(originUri));
        
        if (string.IsNullOrEmpty(newFilename))
            throw new ArgumentNullException("newFilename");

        string originFilename = Path.GetFileName(originUri);
 
        // if the file name is changed 
        if (!string.Equals( originFilename, newFilename, StringComparison.CurrentCulture )) 
        { 
            string folder = Path.GetDirectoryName( originUri ); 
            string newPath = Path.Combine( folder, newFilename ); 
            bool changeCase = string.Equals( originFilename, newFilename, StringComparison.CurrentCultureIgnoreCase );
 
            // if renamed file already exists and not just changing case 
            if (File.Exists( newPath ) && !changeCase) 
            { 
                throw new IOException( String.Format( "File already exists:n{0}", newPath ) ); 
            } 
            else if (changeCase)
            {
                // Move fails when changing case, so need to perform two moves
                string tempPath = Path.Combine( folder, Guid.NewGuid().ToString() );
                Directory.Move( originUri, tempPath );
                Directory.Move( tempPath, newPath );
            }
            else
            {
                Directory.Move( originUri, newPath );
            }
        } 
        return Task.FromResult(true);
    }

    public async Task CopyFileAsync(string sourceUri, string targetUri, CancellationToken cancellationToken = default)
    {
        if (File.Exists(targetUri))
        {
            throw new IOException($"{nameof(targetUri)}({targetUri}) already exists.");
        }
        await Task.Run(() => File.Copy(sourceUri, targetUri), cancellationToken);
    }

    // public async Task CopyFolderAsync(string sourceUri, string targetUri, CancellationToken cancellationToken = default)
    // {
    //     var sourcePath = GetUriToPath(sourceUri);
    //     var targetPath = GetUriToPath(targetUri);
    //
    //     if (!FolderExists(sourcePath))
    //         throw new DirectoryNotFoundException(nameof(sourceUri));
    //     if (FolderExists(targetPath))
    //         throw new NotImplementedException(nameof(targetUri));
    //
    //     Directory.CreateDirectory(targetPath);
    //
    //     var files = Directory.GetFiles(sourcePath);
    //     var folders = Directory.GetDirectories(sourcePath);
    //
    //     var createdFileList = new List<string>();
    //     var createdFolderList = new List<string>();
    //
    //     try
    //     {
    //         lock (new object())
    //         {
    //             foreach (var file in files)
    //             {
    //                 var name = Path.GetFileName(file);
    //                 var dest = Path.Combine(targetPath, name);
    //                 File.Copy(file, dest);
    //                 createdFileList.Add(dest);
    //             }
    //
    //             foreach (var folder in folders)
    //             {
    //                 var name = Path.GetFileName(folder);
    //                 var dest = Path.Combine(targetPath, name);
    //                 createdFolderList.Add(dest);
    //                 CopyFolderAsync(folder, dest, cancellationToken);
    //             }
    //         }
    //     }
    //     catch
    //     {
    //         foreach (var file in createdFileList)
    //         {
    //             File.Delete(file);
    //         }
    //
    //         foreach (var folder in createdFolderList)
    //         {
    //             Directory.Delete(folder);
    //         }
    //
    //         throw;
    //     }
    // }

    public async Task<byte[]> DeleteFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        if (!File.Exists(filePath))
            throw new IOException($"{nameof(filePath)}({filePath}) not found.");

        var deletedFileBuffer = await File.ReadAllBytesAsync(filePath, cancellationToken);
        
        File.Delete(filePath);

        return deletedFileBuffer;
    }
    
    public Task<byte[]> DeleteFileAsync(string uri, string filename, CancellationToken cancellationToken = default)
    {
        return DeleteFileAsync(Path.Combine(uri, filename), cancellationToken);
    }
    
    public async Task<Empty> DeleteFile(string filePath, CancellationToken cancellationToken = default)
    {
        if (!File.Exists(filePath))
            throw new IOException($"{nameof(filePath)}({filePath}) not found.");
        
        File.Delete(filePath);

        return new Empty();
    }

    // 여러 경로의 파일을 제거
    public async Task<IEnumerable<byte[]>> DeleteFilesAsync(IEnumerable<string> uris,
        CancellationToken cancellation = default)
    {
        var deletedFileBuffers = new List<byte[]>();
        var filePathList = uris.ToList();

        var i = 0;
        try
        {
            for (i = 0; i < filePathList.Count; i++)
            {
                deletedFileBuffers.Add(await DeleteFileAsync(filePathList[i], cancellation));
            }
        }
        catch
        {
            for (var j = 0; j < i; j++)
            {
                await SaveFileAsync(filePathList[j], deletedFileBuffers[j], cancellation);
            }

            throw;
        }

        return deletedFileBuffers;
    }

    // 폴더 내에서 filenames의 파일명들과 일치하는 파일만 제거
    public async Task<IEnumerable<byte[]>> DeleteFilesAsync(string uri, IEnumerable<string> filenames,
        CancellationToken cancellation = default)
    {
        var deletedFileBuffers = new List<byte[]>();
        var filenameList = filenames.ToList();

        var i = 0;
        try
        {
            for (i = 0; i < filenameList.Count; i++)
            {
                deletedFileBuffers.Add(await DeleteFileAsync(uri, filenameList[i], cancellation));
            }
        }
        catch
        {
            for (var j = 0; j < i; j++)
            {
                await SaveFileAsync(uri, filenameList[j], deletedFileBuffers[j], cancellation);
            }

            throw;
        }

        return deletedFileBuffers;
    }

    // 폴더 내에서 searchPattern과 일치하는 파일 모두 제거
    public Task<int> DeleteFilesAsync(string uri, string? searchPattern = null, CancellationToken cancellation = default)
    {
        if (!Directory.Exists(uri))
            throw new IOException($"{nameof(uri)}({uri}) not found.");

        var count = 0;

        if (searchPattern == null || String.IsNullOrEmpty(searchPattern) || searchPattern == "*")
        {
            if (Directory.Exists(uri))
            {
                count += Directory.EnumerateFiles(uri, "*,*", SearchOption.AllDirectories).Count();
                Directory.Delete(uri, true);
            }

            return Task.FromResult(count);
        }

        searchPattern = _NormalizePath(searchPattern);
        string path = Path.Combine(uri, searchPattern);
        if (path[^1] == Path.DirectorySeparatorChar || path.EndsWith(Path.DirectorySeparatorChar + "*"))
        {
            string? directory = Path.GetDirectoryName(path);
            if (Directory.Exists(directory))
            {
                count += Directory.EnumerateFiles(directory, "*,*", SearchOption.AllDirectories).Count();
                Directory.Delete(directory, true);
                return Task.FromResult(count);
            }

            return Task.FromResult(0);
        }

        if (Directory.Exists(path))
        {
            count += Directory.EnumerateFiles(path, "*,*", SearchOption.AllDirectories).Count();
            Directory.Delete(path, true);
            return Task.FromResult(count);
        }

        foreach (string file in Directory.EnumerateFiles(uri, searchPattern, SearchOption.AllDirectories))
        {
            File.Delete(file);
            count++;
        }

        return Task.FromResult(count);
    }

    // private string GetUriToPath(string uri)
    // {
    //     while (uri.StartsWith("\\") || uri.StartsWith('/') || uri.StartsWith('.'))
    //     {
    //         uri = uri.Remove(0, 1);
    //     }
    //
    //     var path = Path.Combine(_basePath, uri);
    //
    //     return path.NormalizePath();
    // }

    public void MoveFile(string uri, string newUri, bool isInspectDir = false)
    {
        if (!FileExists(uri))
        {
            throw new IOException($"{nameof(uri)}({uri}) not found.");
        }

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(newUri)!);
            File.Move(uri, newUri);
            if (!isInspectDir) return;
            var dirUri = Path.GetDirectoryName(uri)!;
            if (Directory.EnumerateFiles(dirUri).Count() == 0)
            {
                Directory.Delete(dirUri);
            }
        }
        catch
        {
            if (!isInspectDir) throw;
            var dirUri = Path.GetDirectoryName(newUri)!;
            if (Directory.EnumerateFiles(dirUri).Count() == 0)
            {
                Directory.Delete(dirUri);
            }
            throw;
        }
    }

    public FileStream CreateFileStream(string filePath)
    {

        var dirUri = Path.GetDirectoryName(filePath);
        if (dirUri != null)
        {
            Directory.CreateDirectory(dirUri);
        }

        return new FileStream(filePath, FileMode.CreateNew);

    }

    public FileStream OpenReadFileStream(string filePath)
    {
        var dirUri = Path.GetDirectoryName(filePath);
        if (dirUri != null)
        {
            Directory.CreateDirectory(dirUri);
        }

        return File.OpenRead(filePath);
    }

    public Task<Dictionary<string, int>> GetUsageAsync(string uri, CancellationToken cancellation = default)
    {
        if (!Directory.Exists(uri))
            throw new IOException($"{nameof(uri)}({uri}) not found.");
        
        DriveInfo drive = new DriveInfo(uri);

        int totalSize = Convert.ToInt32(drive.TotalSize / 1024 / 1024 / 1024);
        int freeSize = Convert.ToInt32(drive.AvailableFreeSpace / 1024 / 1024 / 1024);
        int usage = totalSize - freeSize;

        var driveSize = new Dictionary<string, int>();
        driveSize.Add("totalSize", totalSize);
        driveSize.Add("usage", usage);

        return Task.FromResult(driveSize);
    }
}