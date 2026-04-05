namespace GOE
{
    [System.Serializable]
    public class GClothesIndex : BasicResIndexInfo
    {
        /************
         * 资源加载路径
         **/
        protected override string customAssetPath { get { return $"clothes/clothes_{mainId}/{subId}.unity3d"; } }
        protected override string customObjName { get { return $"clothes_{mainId}_{subId}"; } }
        
        public void ParseFromString(string _str)
        {
            readIndex(_str, string.Empty);
        }

        /***************
         * 根据主id和副id获取对应的资源路径
         **/
        public static string getAssetPath(int _mainId, int _subId, EIndexType _indexType = EIndexType.DEFAULT) { return $"{_indexType.assetPathRoot()}clothes/clothes_{_mainId}/{_subId}.unity3d"; }
        public static string getObjName(int _mainId, int _subId, EIndexType _indexType = EIndexType.DEFAULT) { return $"clothes_{_mainId}_{_subId}"; }
    }
}