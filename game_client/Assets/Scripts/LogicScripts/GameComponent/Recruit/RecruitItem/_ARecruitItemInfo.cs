using NPEnum;

namespace GOE
{
    /// <summary>
    /// 召唤的道具信息
    /// </summary>
    public abstract class _ARecruitItemInfo
    {
        protected RecruitRefObj _m_rRecruitRefObj;//招募配表对象

        public _ARecruitItemInfo(RecruitRefObj _recruitRefObj)
        {
            _m_rRecruitRefObj = _recruitRefObj;
        }

        public RecruitRefObj recruitRefObj { get { return _m_rRecruitRefObj; } }
        
        public long recruitId { get { return _m_rRecruitRefObj?.id ?? 0; } }
        
        public abstract ERecruitItemType recruitItemType { get; }

        /// <summary>
        /// 是否已招募
        /// </summary>
        /// <returns></returns>
        public abstract bool isHadRecruit();

        /// <summary>
        /// 是否解锁招募
        /// </summary>
        /// <returns></returns>
        public bool hadUnlockRecruit()
        {
            if (_m_rRecruitRefObj == null)
                return false;
            
            if (_m_rRecruitRefObj.condition != null && _m_rRecruitRefObj.condition.hasCondition && !_m_rRecruitRefObj.condition.IsEnable(null))
                return false;

            return true;
        }
        
        /// <summary>
        /// 获取招募item信息
        /// </summary>
        /// <param name="_recruitRefObj"></param>
        /// <returns></returns>
        public static _ARecruitItemInfo getRecruitItemInfo(RecruitRefObj _recruitRefObj)
        {
            if (_recruitRefObj == null)
                return null;

            if (_recruitRefObj.gain_item == null)
                return new RecruitNormalItemInfo(_recruitRefObj);
            
            switch (_recruitRefObj.gain_item.getItemType())
            {
                case ENPItemType.HERO:
                    return new RecruitHeroItemInfo(_recruitRefObj);
                case ENPItemType.CONSORT:
                    return new RecruitConsortItemInfo(_recruitRefObj);
                default:
                    return new RecruitNormalItemInfo(_recruitRefObj);
            }
        }
    }
}