using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class TextFontInstantiateObjCore
    {
        [NotNull] public Dictionary<NPCommonAssetPathInfo, TextFontInstantiateObj> _m_dInstantiateObjDic = new Dictionary<NPCommonAssetPathInfo, TextFontInstantiateObj>();
        
        public void getInstantiateObj(NPCommonAssetPathInfo _assetPath, Action<Font> _getAction)
        {
            if (null == _assetPath || !_assetPath.enable)
            {
                _getAction?.Invoke(null);
                return;
            }

            if (!_m_dInstantiateObjDic.TryGetValue(_assetPath, out TextFontInstantiateObj _instantiateObj) || _instantiateObj == null)
            {
                _instantiateObj = new TextFontInstantiateObj(_assetPath);
                _m_dInstantiateObjDic[_assetPath] = _instantiateObj;
            }

            if(!_instantiateObj.inited)
                _instantiateObj.init();
            
            _instantiateObj.regInitDelegate(() =>
            {
                _getAction?.Invoke(_instantiateObj.obj);
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
    }
}