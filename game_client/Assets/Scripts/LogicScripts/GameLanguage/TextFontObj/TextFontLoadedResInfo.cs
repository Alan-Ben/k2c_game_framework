using ALPackage;
using UnityEngine;

namespace GOE
{
    public class TextFontLoadedResInfo : _ATAssetPathLoadedResInfo<Font>
    {
        public TextFontLoadedResInfo(NPCommonAssetPathInfo _assetPath) : base(_assetPath)
        {
        }

        public TextFontLoadedResInfo(string _aasetPath, string _objName) : base(_aasetPath, _objName)
        {
        }
        
#if UNITY_EDITOR
        protected override string _localResExName { get { return ""; } }
        protected override string _localResUnitySiftStr { get { return "t:Font"; } }
#endif
        protected override _AALResourceCore _getALResourceCore()
        {
            return GameResCore.instance;
        }

        protected override _ATAssetPathResCore<Font> _getObjCore()
        {
            return TextFontResCore.instance;
        }

        protected internal override Font _cloneObj()
        {
            return obj;
        }

        protected internal override void _releaseCloneObj(Font _obj)
        {
        }

        protected override void _onInitObj(Font _obj)
        {
        }

        protected override void _onDiscard()
        {
        }
    }
}