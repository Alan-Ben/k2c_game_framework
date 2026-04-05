using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GoConsortChatShotImg : _AALBasicLoadObj
    {
        private readonly Transform _m_parent;
        private readonly NPGGoIndex _m_goIndex;

        private GameObject _m_go;
        private MonoConsortChatShotImg _m_monShotImg;

        
        public GoConsortChatShotImg(Transform _parent, NPGGoIndex _goIndex)
        {
            _m_parent = _parent;
            _m_goIndex = _goIndex;
        }

        protected override void _loadOp()
        {
            GGoIndexCacheMgr.instance.popItem(_m_goIndex, _go =>
            {
                _m_go = _go;
                if (_m_go != null)
                {
                    _m_monShotImg = _m_go.GetComponent<MonoConsortChatShotImg>();
                    _m_go.transform.SetParent(_m_parent, false);
                    _m_go.transform.localRotation = Quaternion.identity;
                }
                _setLoadDone();
            });
        }

        protected override void _discard()
        {
            if (_m_go == null)
                return;
            
            GGoIndexCacheMgr.instance.pushbackItem(_m_goIndex, _m_go);
            _m_go = null;
        }
        public ShotSetting getShotSetting(EConsortChatShotType _shotType)
        {
            if (_m_monShotImg != null)
                foreach (var shot in _m_monShotImg.shotSettings)
                {
                    if (shot != null && shot.shotType == _shotType)
                    {
                        return shot;
                    }
                }
            return null;
        }

        public Vector2 getShot(EConsortChatShotType _shotType, float _normalizeScale, float _normalizePosX, float _normalizePosY,  Vector2 _imgSize, Vector2 _viewSize, out float _scale)
        {
            if (_m_monShotImg != null)
                foreach (var shot in _m_monShotImg.shotSettings)
                {
                    if (shot != null && shot.shotType == _shotType)
                    {
                        return shot.getShot(_normalizeScale, _normalizePosX, _normalizePosY, _imgSize, _viewSize,
                            out _scale);
                    }
                }
            _scale = 1;
            return Vector2.zero;
        }
    }
}