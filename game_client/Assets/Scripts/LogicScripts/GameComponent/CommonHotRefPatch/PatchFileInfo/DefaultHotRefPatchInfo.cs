namespace GOE
{
    /// <summary>
    /// 默认的补丁文件，跟活动无关，用于后面通用表需要补丁的情况
    /// </summary>
    public class DefaultHotRefPatchInfo : _ACommonHotRefPatchInfo
    {
        public DefaultHotRefPatchInfo(string _fileName, string _fileMd5, string _filePath) : base(_fileName, _fileMd5, _filePath)
        {
        }

        protected override bool _checkCanPatch()
        {
            return true;
        }
    }
}