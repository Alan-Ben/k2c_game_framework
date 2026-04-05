using System;
using ALPackage;
using TMPro;
using UnityEngine;

namespace GOE
{
    public class TMP_FontAssetLoadedResInfo : _ATAssetPathLoadedResInfo<TMP_FontAsset>
    {
        private TMP_FontAssetMaterialObj _m_oMaterialObj;
        public TMP_FontAssetLoadedResInfo(NPCommonAssetPathInfo _assetPath) : base(_assetPath)
        {
        }

        public TMP_FontAssetLoadedResInfo(string _aasetPath, string _objName) : base(_aasetPath, _objName)
        {
        }
#if UNITY_EDITOR
        protected override string _localResExName { get { return ".asset"; } }
        protected override string _localResUnitySiftStr { get { return ""; } }
#endif
        protected override _AALResourceCore _getALResourceCore()
        {
            return GameResCore.instance;
        }

        protected override _ATAssetPathResCore<TMP_FontAsset> _getObjCore()
        {
            return TMP_FontAssetResCore.instance;
        }

        protected internal override TMP_FontAsset _cloneObj()
        {
            if (obj != null)
                return TMP_FontAsset.Instantiate(obj);
            return obj;
        }

        protected internal override void _releaseCloneObj(TMP_FontAsset _obj)
        {
#if UNITY_EDITOR
            if(_obj != null)
                _obj.ClearFontAssetData(true);//Editor下释放资源时需要清除运行时数据
#endif
            if(_obj != null)
                TMP_FontAsset.Destroy(_obj);
        }

        protected override void _onInitObj(TMP_FontAsset _obj)
        {
            if(_m_oMaterialObj != null)
                _m_oMaterialObj.discard();

            _m_oMaterialObj = new TMP_FontAssetMaterialObj(_m_oObj, _m_sAssetPath);
        }

        protected override void _onDiscard()
        {
            _m_oMaterialObj?.discard();
            _m_oMaterialObj = null;
        }
        
        public void getMaterialInstantiate(string _matertal,Action<Material> _delegate)
        {
            if (_m_oMaterialObj == null)
            {
                _delegate?.Invoke(null);
                return;
            }
            
            _m_oMaterialObj.regInitedDelegate(() =>
            {
                _delegate?.Invoke(_m_oMaterialObj?.getMaterialInstantiate(_matertal));
            });
        }
    }
}