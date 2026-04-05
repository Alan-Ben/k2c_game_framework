using System.Text;
using Common.WeekCardObj;
using CommonEnum;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 游历
    /// </summary>
    public class WeekCardSettleShowInfo_TRAVEL:_IWeekCardSettleShowInfo
    {
        private readonly WeekCard_SettleDetailInfo _m_info;
        private readonly WeekCard_SettleInfo_Travel _m_exInfo;

        public WeekCardSettleShowInfo_TRAVEL(WeekCard_SettleDetailInfo _info)
        {
            _m_info = _info;
            _m_exInfo = new WeekCard_SettleInfo_Travel();
            _m_exInfo.readPackage(_m_info.getDetailInfo());
        }
        
        public EWeekCardSettleType getSettleType()
        {
            return _m_info.getType();
        }

        public string getShowContent()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(TextTranslate.instance.getLanguage(TransKeyConst.week_card_settle_TRAVEL, _m_exInfo.getNum()));
            if(_m_exInfo.getUngetConsortlist().Count > 0)
                sb.Append("\n");
            for (int i = 0 ; i < _m_exInfo.getUngetConsortlist().Count ; i++)
            {
                WeekCard_TravelUngetConsortInfo info = _m_exInfo.getUngetConsortlist()[i];
                sb.Append(TextTranslate.instance.getLanguage(TransKeyConst.week_card_settle_TRAVEL_like
                    ,GCommon.getItemName(ENPItemType.CONSORT, info.getConsortId())
                    , info.getLike()));
                if (i != _m_exInfo.getUngetConsortlist().Count - 1)
                    sb.Append("\n");
            }
            if(_m_exInfo.getGotConsrtlist().Count > 0)
                sb.Append("\n");
            for (int i = 0 ; i < _m_exInfo.getGotConsrtlist().Count ; i++)
            {
                WeekCard_TravelGotConsrtInfo info = _m_exInfo.getGotConsrtlist()[i];
                sb.Append(TextTranslate.instance.getLanguage(TransKeyConst.week_card_settle_TRAVEL_intimacy
                    ,GCommon.getItemName(ENPItemType.CONSORT, info.getConsortId())
                    , info.getIntimacy()));
                if (i != _m_exInfo.getGotConsrtlist().Count - 1)
                    sb.Append("\n");
            }
            
            return sb.ToString();
        }
    }
}