
namespace GOE
{
    [System.Serializable]
    public class GClipIndex : BasicResIndexInfo
    {
        /************
         * 资源加载路径
         **/
        protected override string customAssetPath { get { return $"clip/clip_{mainId}.unity3d"; } }
        protected override string customObjName { get { return $"clip_{mainId}_{subId}"; } }
        
        public void ParseFromString(string _str)
        {
            readIndex(_str, string.Empty);
        }

        /***************
         * 根据主id和副id获取对应的资源路径
         **/
        public static string getAssetPath(int _mainId, EIndexType _indexType = EIndexType.DEFAULT) { return $"{_indexType.assetPathRoot()}clip/clip_{_mainId}.unity3d"; }
        public static string getObjName(int _mainId, int _subId, EIndexType _indexType = EIndexType.DEFAULT) { return $"clip_{_mainId}_{_subId}"; }
    }
}