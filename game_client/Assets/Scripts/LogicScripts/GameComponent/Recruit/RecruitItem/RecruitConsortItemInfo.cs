using NPEnum;

namespace GOE
{
    public class RecruitConsortItemInfo : _ARecruitItemInfo
    {
        private long _m_lConsorId;
        /// <summary>
        /// 妃子显示数据
        /// </summary>
        private ConsortInfo _m_consortInfo;
        
        public RecruitConsortItemInfo(RecruitRefObj _recruitRefObj) : base(_recruitRefObj)
        {
            if (_recruitRefObj == null || _recruitRefObj.gain_item == null ||
                _recruitRefObj.gain_item.getItemType() != ENPItemType.CONSORT)
            {
                Debug.LogError($"[RecruitConsortItemInfo ctor] _recruitRefObj.gain_item.getItemType():{_recruitRefObj?.gain_item?.getItemType()} != ENPItemType.CONSORT");
                return;
            }

            _m_lConsorId = _recruitRefObj.gain_item.subId;
            _m_consortInfo = new ConsortInfo(_m_lConsorId);
        }

        public override ERecruitItemType recruitItemType { get { return ERecruitItemType.CONSORT; } }

        public ConsortInfo consortInfo { get { return _m_consortInfo; } }

        /// <summary>
        /// 是否已招募
        /// </summary>
        /// <returns></returns>
        public override bool isHadRecruit()
        {
            return _m_consortInfo != null && _m_consortInfo.unlockType == EGameCommonUnlockType.UNLOCK;
        }
    }
}