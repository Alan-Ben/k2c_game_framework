namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class RecruitNormalItemInfo : _ARecruitItemInfo
    {
        public RecruitNormalItemInfo(RecruitRefObj _recruitRefObj) : base(_recruitRefObj)
        {
        }

        public override ERecruitItemType recruitItemType { get { return ERecruitItemType.NORMAL; } }

        public override bool isHadRecruit()
        {
            return _m_rRecruitRefObj != null && _m_rRecruitRefObj.gain_item != null && GCommon.getItemCount(_m_rRecruitRefObj.gain_item.item) > 0;
        }
    }
}