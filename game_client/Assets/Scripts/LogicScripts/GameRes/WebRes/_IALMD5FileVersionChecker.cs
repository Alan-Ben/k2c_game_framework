using System;
using System.IO;


public interface _IALMD5FileVersionChecker
{
    public enum VersionCheckResult
    {
        NONE,
        DeleteLocalFile,        // 远端文件已被删除
        UpdateFromRemote,       // 远端文件已更新
        UseLocalFile,           // 版本比对通过可以直接使用本地文件
        UseStreamingAssetsFile, // 版本比对通过可以直接使用随包文件
        FileNotExist,           // 文件压根就不存在
    }
    
    void discard();
    void checkResVersion(string _fileKey, Action<VersionCheckResult> _checkResult);
    
    void generateLocalFileMD5(FileInfo _file, string _fileKey, Action<string> _onComplete);
    void deleteLocalFileMD5(string _fileKey);
    
    void tryGetRemoteFileMD5(string _fileKey, Action<string> _got);
    void tryGetRemoteFileSizeByte(string _fileKey, Action<long> _got);

}
