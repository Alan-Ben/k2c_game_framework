using UnityEngine;

namespace GOE
{
    public class TravelEventConsortRole : _ITravelEventRole
    {
        private long _m_lId;
        private ConsortRefShowInfo _m_rConsortRefShowInfo;
        private _AShowCaseUnitInfoObj _m_showCaseUnitInfo;
        
        public TravelEventConsortRole(long id)
        {
            _m_lId = id;
        }
        
        public ETravelEventRoleType roleType { get { return ETravelEventRoleType.CONSORT; } }
        public long id { get { return _m_lId; } }

        public ConsortRefShowInfo consortRefShowInfo
        {
            get
            {
                if (_m_rConsortRefShowInfo == null || _m_rConsortRefShowInfo.consortId != _m_lId)
                    _m_rConsortRefShowInfo = new ConsortRefShowInfo(_m_lId);

                return _m_rConsortRefShowInfo;
            }
        }

        public string name
        {
            get
            {
                return consortRefShowInfo?.consortTransName;
            }
        }

        public _AShowCaseUnitInfoObj showCaseUnitInfo
        {
            get
            {
                if (_m_showCaseUnitInfo != null)
                    return _m_showCaseUnitInfo;

                _m_showCaseUnitInfo = new ShowCaseCommonResUnitInfoObj(consortRefShowInfo?.consortSkinShowInfo?.tdShow);
                return _m_showCaseUnitInfo;
            }
        }
        
        public Vector3 showCaseOffset { get { return Vector3.zero; } }
        
        public NPGTextureIndex midImage { get { return consortRefShowInfo?.consortSkinShowInfo?.consortCardImage; } }
    }
}