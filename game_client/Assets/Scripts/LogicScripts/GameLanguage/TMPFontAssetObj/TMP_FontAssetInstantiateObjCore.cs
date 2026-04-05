using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;

namespace GOE
{
    public class TMP_FontAssetInstantiateObjCore
    {
        [NotNull] public Dictionary<NPCommonAssetPathInfo, TMP_FontAssetInstantiateObj> _m_dInstantiateObjDic = new Dictionary<NPCommonAssetPathInfo, TMP_FontAssetInstantiateObj>();
        
        public void getInstantiateObj(NPCommonAssetPathInfo _assetPath, Action<TMP_FontAsset> _getAction)
        {
            if (null == _assetPath || !_assetPath.enable)
            {
                _getAction?.Invoke(null);
                return;
            }

            if (!_m_dInstantiateObjDic.TryGetValue(_assetPath, out TMP_FontAssetInstantiateObj _instantiateObj) || _instantiateObj == null)
            {
                _instantiateObj = new TMP_FontAssetInstantiateObj(_assetPath);
                _m_dInstantiateObjDic[_assetPath] = _instantiateObj;
            }

            if(!_instantiateObj.inited)
                _instantiateObj.init();
            
            _instantiateObj.regInitDelegate(() =>
            {
                _getAction?.Invoke(_instantiateObj.obj);
            });
        }
        
        public void getInstantiateObj(NPCommonAssetPathInfo _assetPath, Action<TMP_FontAssetInstantiateObj> _getAction)
        {
            if (null == _assetPath || !_assetPath.enable)
            {
                _getAction?.Invoke(null);
                return;
            }

            if (!_m_dInstantiateObjDic.TryGetValue(_assetPath, out TMP_FontAssetInstantiateObj _instantiateObj) || _instantiateObj == null)
            {
                _instantiateObj = new TMP_FontAssetInstantiateObj(_assetPath);
                _m_dInstantiateObjDic[_assetPath] = _instantiateObj;
            }

            if(!_instantiateObj.inited)
                _instantiateObj.init();
            
            _instantiateObj.regInitDelegate(() =>
            {
                _getAction?.Invoke(_instantiateObj);
            });
        }

        public void discard()
        {
            foreach (var instantiateObj in _m_dInstantiateObjDic.Values)
            {
                if(instantiateObj != null)
                    instantiateObj.discard();
            }
            
            _m_dInstantiateObjDic.Clear();
        }
        
        public void discardInstantiateObj(NPCommonAssetPathInfo _assetPath)
        {
            if (null == _assetPath || !_assetPath.enable)
                return;
            
            if (_m_dInstantiateObjDic.TryGetValue(_assetPath, out TMP_FontAssetInstantiateObj _instantiateObj) && _instantiateObj != null)
            {
                _instantiateObj.discard();
                _m_dInstantiateObjDic.Remove(_assetPath);
            }
        }
    }
}