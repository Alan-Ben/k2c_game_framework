
namespace ALPackage
{
    /// <summary>
    /// 一个仅有几个数据的WebResCore
    /// </summary>
    public class ALTinyWebResCore : _AALWebResCore
    {
        public ALTinyWebResCore(string _remoteRootPath, string _localRootPath, string _streamingAssetRootPath, int _failRetryCount, int _downloadMaxDealingCount = 3 )
            : base(_remoteRootPath, _localRootPath, _streamingAssetRootPath, _failRetryCount, _downloadMaxDealingCount)
        {
        }

        // 一个仅有几个数据的MD5校对器，会在对应的同目录下生成后缀多加上.md5的文件
        protected override _AALMD5FileVersionChecker _createMD5FileVersionChecker()
        {
            return new ALMD5FileVersionCheck_UseSingleFile(remoteResRootURL, localFileRootURL, streamingAssetsRootURL);
        }
    }
}