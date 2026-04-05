using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 招募商店信息
    /// </summary>
    public class RecruitShopInfo
    {
        private RecruitShopRefObj _m_recruitShopRefObj;//招募商店配表对象
        private List<_ARecruitItemInfo> _m_lRecruitItemInfoList;//招募项信息列表
        
        private List<long> _m_lHadRecruitIdList;//已招募id列表
        
        public RecruitShopInfo(RecruitShopRefObj _refObj)
        {
            _m_recruitShopRefObj = _refObj;
            
            _m_lRecruitItemInfoList = new List<_ARecruitItemInfo>();
            if (_m_recruitShopRefObj != null && _m_recruitShopRefObj.recruitRefObjList != null)
            {
                foreach (var recruitRefObj in _m_recruitShopRefObj.recruitRefObjList)
                {
                    if(recruitRefObj == null)
                        continue;
                    
                    _ARecruitItemInfo recruitItemInfo = _ARecruitItemInfo.getRecruitItemInfo(recruitRefObj);
                    if(recruitItemInfo != null)
                        _m_lRecruitItemInfoList.Add(recruitItemInfo);
                }
            }
        }
        
        public RecruitShopRefObj recruitShopRefObj { get { return _m_recruitShopRefObj; } }
        public List<_ARecruitItemInfo> recruitItemInfoList { get { return _m_lRecruitItemInfoList; } }
        public long shopId { get { return _m_recruitShopRefObj == null ? 0 : _m_recruitShopRefObj.id; } }
        public List<long> hadRecruitIdList { get { return _m_lHadRecruitIdList; } }

        public int hadRecruitCount
        {
            get
            {
                if(_m_lRecruitItemInfoList == null || _m_lRecruitItemInfoList.Count <= 0)
                    return 0;

                int recruitCount = 0;
                foreach (var recruitItemInfo in _m_lRecruitItemInfoList)
                {
                    if(isHadRecruit(recruitItemInfo))
                        recruitCount++;
                }

                return recruitCount;
            }
        }

        public void getRecruitItemInfoList(List<_ARecruitItemInfo> _recruitItemInfoList)
        {
            if(_recruitItemInfoList == null)
                return;
            
            _recruitItemInfoList.Clear();
            
            if(_m_lRecruitItemInfoList != null)
                _recruitItemInfoList.AddRange(_m_lRecruitItemInfoList);
        }
        
        /// <summary>
        /// 增加已招募id
        /// </summary>
        /// <param name="_id"></param>
        public void addRecruitId(long _id)
        {
            if (_m_lHadRecruitIdList == null)
                _m_lHadRecruitIdList = new List<long>();
            
            _m_lHadRecruitIdList.Add(_id);
        }
        
        /// <summary>
        /// 是否已招募
        /// </summary>
        /// <param name="_recruitRefObj"></param>
        /// <returns></returns>
        public bool isHadRecruit(_ARecruitItemInfo _recruitItemInfo)
        {
            if (_recruitItemInfo == null)
                return false;
            
            // 判断 是否有已招募记录 或 直接使用item数据判断是否已招募
            return (_m_lHadRecruitIdList != null && _m_lHadRecruitIdList.Contains(_recruitItemInfo.recruitId)) || (_recruitItemInfo.isHadRecruit());
        }
        
        /// <summary>
        /// 获取招募状态
        /// </summary>
        /// <returns></returns>
        public ERecruitState getRecruitState(_ARecruitItemInfo _recruitItemInfo, bool _needShowTip)
        {
            if (_recruitItemInfo == null || _recruitItemInfo.recruitRefObj == null)
                return ERecruitState.NONE;

            if (!_recruitItemInfo.hadUnlockRecruit())
            {
                if(_needShowTip)
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(_recruitItemInfo.recruitRefObj.condition_desc, _recruitItemInfo.recruitRefObj.condition_desc_args_list));
                
                return ERecruitState.LOCK;
            }
            
            if (isHadRecruit(_recruitItemInfo))
                return ERecruitState.RECRUITED;

            if (_recruitItemInfo.recruitRefObj.canGotoRecruit())
                return ERecruitState.CAN_GO_TO_RECRUIT;
            
            if (!GCommon.isItemEnough(_recruitItemInfo.recruitRefObj.cost_item, _needShowTip))
                return ERecruitState.UNLOCK_CANNOT_RECRUIT;
                
            return ERecruitState.CAN_EXCHANGE_RECRUIT;
        }
    }
}