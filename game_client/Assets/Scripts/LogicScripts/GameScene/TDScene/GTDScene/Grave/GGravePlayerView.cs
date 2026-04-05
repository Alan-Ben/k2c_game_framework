using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGravePlayerView : _AALBasicLoadObj
    {
        private NPGGoIndex _m_goIndex;
        private Transform _m_parent;
        private GameObject _m_go;
        public GGravePlayerView(NPGGoIndex goIndex, Transform _parent)
        {
            _m_goIndex = goIndex;
            _m_parent = _parent;
        }

        protected override void _loadOp()
        {
            if (null == _m_goIndex || !_m_goIndex.isValid())
            {
                _setLoadDone();
                return;
            }


            GGoResCore.instance.loadObj(_m_goIndex, (_assetHandle) =>
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
                _m_go = GameObject.Instantiate(assetGo);
                
                _m_go.transform.SetParent(_m_parent);
                _m_go.transform.localPosition = Vector3.zero;
                _m_go.transform.localScale = Vector3.one;
                _setLoadDone();
            });
        }

        protected override void _discard()
        {
            if (null != _m_go)
                ALUnityCommon.releaseGameObj(_m_go);
            _m_go = null;
        }
    }
}