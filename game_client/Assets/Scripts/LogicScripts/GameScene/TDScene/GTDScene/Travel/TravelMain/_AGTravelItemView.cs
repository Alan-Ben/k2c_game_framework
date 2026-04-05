using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 主游历场景item展示对象
    /// </summary>
    public abstract class _AGTravelItemView : _AALBasicLoadObj
    {
        protected Transform _m_tParent;//父节点
        
        protected Transform _m_viewTrans;
        protected GameObject _m_viewGo;

        public _AGTravelItemView(Transform _parent)
        {
            _m_tParent = _parent;
        }
        
        public Transform viewTrans  { get { return _m_viewTrans; } }

        protected override void _loadOp()
        {
            NPGGoIndex loadGoIndex = _getLoadGoIndex();
            GGoResCore.instance.loadObj(loadGoIndex,(_assetHandle) =>
            {
            
                if (_assetHandle == null || _assetHandle.loadedInfo == null)
                {
#if UNITY_EDITOR
                    UnityEngine.Debug.LogError("NPGGoIndex加载错误: " + _assetHandle.mainId + " - " + _assetHandle.subId);
#endif
                    _setLoadDone();
                    return;
                }
                //获取资源对象
                GameObject assetGo = _assetHandle.loadedInfo.obj;
                if (null == assetGo)
                {
#if UNITY_EDITOR
                    UnityEngine.Debug.LogError("NPGGoIndex加载错误: " + _assetHandle.mainId + " - " + _assetHandle.subId);
#endif
                    _setLoadDone();
                    return;
                }
                
                _m_viewGo = GameObject.Instantiate(assetGo);
                _m_viewTrans = _m_viewGo.transform;
                _m_viewTrans.SetParent(_m_tParent);
                
                _onLoaded(_m_viewGo);
            
                _setLoadDone();
            });
        }

        protected override void _discard()
        {
            hide();
            
            _onDiscard();
            ALUnityCommon.releaseGameObj(_m_viewGo);
            _m_viewGo = null;
            _m_viewTrans = null;
        }

        protected abstract void _onLoaded(GameObject _go);

        protected abstract NPGGoIndex _getLoadGoIndex();

        protected abstract void _onDiscard();

        public void show()
        {
            if (null != _m_viewGo)
                _m_viewGo.SetActive(true);

            _onShow();
        }

        protected abstract void _onShow();

        //隐藏显示
        public void hide()
        {
            if (null != _m_viewGo)
                _m_viewGo.SetActive(false);

            _onHide();
        }

        protected abstract void _onHide();
    }
}