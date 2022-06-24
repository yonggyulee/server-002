using Google.Protobuf.WellKnownTypes;

namespace Mirero.DAQ.Infrastructure.Storage;

public interface IFileStorage
{
    Task CreateFolderAsync(string folderPath, CancellationToken cancellationToken = default);
    void DeleteFolder(string uri);
    void MoveFolder(string uri, string newUri);
    bool FolderExists(string uri);
    Task<byte[]> GetFileBufferAsync(string uri, string filename, CancellationToken cancellationToken = default);
    Task<FileInfo> GetFileInfoAsync(string uri);
    bool FileExists(string uri);
    Task SaveFileAsync(string filePath, byte[] buffer, CancellationToken cancellationToken = default);
    Task SaveFileAsync(string uri, string filename, byte[] buffer, CancellationToken cancellationToken = default);
    Task SaveFilesAsync(IEnumerable<string> uris, IEnumerable<byte[]> buffers, CancellationToken cancellationToken = default);
    Task SaveFilesAsync(string uri, IEnumerable<string> filenames, IEnumerable<byte[]> buffers, CancellationToken cancellationToken = default);
    Task RenameFileAsync(string uri, string newFilename, CancellationToken cancellationToken = default);
    Task CopyFileAsync(string sourceUri, string targetUri, CancellationToken cancellationToken = default);
    //Task CopyFolderAsync(string sourceUri, string targetUri, CancellationToken cancellationToken = default);
    Task<byte[]> DeleteFileAsync(string filePath, CancellationToken cancellationToken = default);
    Task<byte[]> DeleteFileAsync(string uri, string filename, CancellationToken cancellationToken = default);
    Task<IEnumerable<byte[]>> DeleteFilesAsync(IEnumerable<string> uris, CancellationToken cancellation = default);
    Task<IEnumerable<byte[]>> DeleteFilesAsync(string uri, IEnumerable<string> filenames, CancellationToken cancellation = default);
    Task<int> DeleteFilesAsync(string uri, string searchPattern, CancellationToken cancellation = default);
    void MoveFile(string uri, string newUri, bool isInspectDir = false);
    FileStream CreateFileStream(string filePath);
    FileStream OpenReadFileStream(string filePath);
    Task<Dictionary<string, int>> GetUsageAsync(string uri, CancellationToken cancellation = default);
    Task<Empty> DeleteFile(string filePath, CancellationToken cancellationToken = default);
}