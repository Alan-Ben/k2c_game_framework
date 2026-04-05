using Common.WeekCardObj;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 大学学习
    /// </summary>
    public class WeekCardSettleShowInfo_COLLEGE_STUDY:_AWeekCardSettleShowInfo_NumInfo
    {

        public WeekCardSettleShowInfo_COLLEGE_STUDY(WeekCard_SettleDetailInfo _info) : base(_info)
        {
        }


        public override string getShowContent()
        {
            return TextTranslate.instance.getLanguage(TransKeyConst.week_card_settle_COLLEGE_STUDY, _m_exInfo.getNum());
        }
    }
}