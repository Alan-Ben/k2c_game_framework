using UnityEngine;

namespace GOE
{
    public class TextFontResCore : _ATAssetPathResCore<Font>
    {
        private static TextFontResCore _g_instance;
        public static TextFontResCore instance { get { return _g_instance ??= new TextFontResCore(); } }
        
        protected override _ATAssetPathLoadedResInfo<Font> _createLoadedObjInfo(string _assetPath, string _objName)
        {
            return new TextFontLoadedResInfo(_assetPath, _objName);
        }
    }
}