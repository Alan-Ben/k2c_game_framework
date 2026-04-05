using Common.WeekCardObj;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 政务
    /// </summary>
    public class WeekCardSettleShowInfo_ANECDOTE:_AWeekCardSettleShowInfo_NumInfo
    {
        public WeekCardSettleShowInfo_ANECDOTE(WeekCard_SettleDetailInfo _info) : base(_info)
        {
        }

        public override string getShowContent()
        {
            return TextTranslate.instance.getLanguage(TransKeyConst.week_card_settle_ANECDOTE, _m_exInfo.getNum());
        }
    }
}