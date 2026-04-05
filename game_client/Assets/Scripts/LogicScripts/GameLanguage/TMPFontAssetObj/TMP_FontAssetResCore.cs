using TMPro;

namespace GOE
{
    public class TMP_FontAssetResCore : _ATAssetPathResCore<TMP_FontAsset>
    {
        private static TMP_FontAssetResCore _g_instance;
        public static TMP_FontAssetResCore instance { get { return _g_instance ??= new TMP_FontAssetResCore(); } }

        protected override _ATAssetPathLoadedResInfo<TMP_FontAsset> _createLoadedObjInfo(string _assetPath, string _objName)
        {
            return new TMP_FontAssetLoadedResInfo(_assetPath, _objName);
        }
    }
}