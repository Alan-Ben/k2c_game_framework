using Common.WeekCardObj;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 子嗣培养
    /// </summary>
    public class WeekCardSettleShowInfo_CHILD_TRAIN:_AWeekCardSettleShowInfo_NumInfo
    {

        public WeekCardSettleShowInfo_CHILD_TRAIN(WeekCard_SettleDetailInfo _info) : base(_info)
        {
        }
        
        public override string getShowContent()
        {
            return TextTranslate.instance.getLanguage(TransKeyConst.week_card_settle_CHILD_TRAIN, _m_exInfo.getNum());
        }
    }
}