using System.Text;
using Common.LevyEnum;
using Common.WeekCardObj;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 征收
    public class WeekCardSettleShowInfo_LEVY:_IWeekCardSettleShowInfo
    {
        private readonly WeekCard_SettleDetailInfo _m_info;
        private WeekCard_SettleInfo_LevyList _m_exInfo;
        public WeekCardSettleShowInfo_LEVY(WeekCard_SettleDetailInfo _info)
        {
            _m_info = _info;
            _m_exInfo = new WeekCard_SettleInfo_LevyList();
            _m_exInfo.readPackage(_m_info.getDetailInfo());
        }

        public EWeekCardSettleType getSettleType()
        {
            return EWeekCardSettleType.LEVY;            
        }

        public string getShowContent()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _m_exInfo.getList().Count; i++)
            {
                WeekCard_SettleInfo_LevyInfo info = _m_exInfo.getList()[i];
                switch (info.getType())
                {
                    case ELevy_Type.SILVER:
                        sb.Append(TextTranslate.instance.getLanguage(TransKeyConst.week_card_settle_LEVY_SILVER, info.getNum()));
                        break;
                    case ELevy_Type.SOLDIER:
                        sb.Append(TextTranslate.instance.getLanguage(TransKeyConst.week_card_settle_LEVY_SOLIDER, info.getNum()));
                        break;
                }
                if (i != _m_exInfo.getList().Count - 1)
                    sb.Append("\n");
            }
            //共计征收金币{0}次
            return sb.ToString();
        }
    }
}