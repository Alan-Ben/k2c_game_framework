
using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class NPCGoCommonLoadObj : _AALBasicLoadObj
    {
        private readonly Transform _m_parent;
        private readonly NPGGoIndex _m_goIndex;

        private GameObject _m_go;
        
        public NPCGoCommonLoadObj(Transform _parent, NPGGoIndex _goIndex)
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
    }
}