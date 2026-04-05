
using UnityEngine;

namespace GOE
{
    public class TravelEventNpcRole : _ITravelEventRole
    {
        private long _m_lId;
        private NPNPCRefObj _m_rNpcRefObj;
        private _AShowCaseUnitInfoObj _m_showCaseUnitInfo;
        
        public TravelEventNpcRole(long id)
        {
            _m_lId = id;
        }
        
        public ETravelEventRoleType roleType { get { return ETravelEventRoleType.NPC; } }
        public long id { get { return _m_lId; } }

        public NPNPCRefObj npcRefObj
        {
            get
            {
                if (_m_rNpcRefObj == null || _m_rNpcRefObj.id != _m_lId)
                    _m_rNpcRefObj = GRefdataCoreMgr.instance.npcRefCore.getRef(_m_lId);

                return _m_rNpcRefObj;
            }
        }

        public string name
        {
            get
            {
                return npcRefObj?.npcName;
            }
        }

        public _AShowCaseUnitInfoObj showCaseUnitInfo
        {
            get
            {
                if (_m_showCaseUnitInfo == null)
                    _m_showCaseUnitInfo = NPCUtil.toUnitInfoObj(npcRefObj);

                return _m_showCaseUnitInfo;
            }
        }

        public Vector3 showCaseOffset
        {
            get
            {
                return npcRefObj?.showCaseOffset ?? Vector3.zero;
            }
        }
        
        public NPGTextureIndex midImage { get { return npcRefObj?.npcImage; } }
    }
}