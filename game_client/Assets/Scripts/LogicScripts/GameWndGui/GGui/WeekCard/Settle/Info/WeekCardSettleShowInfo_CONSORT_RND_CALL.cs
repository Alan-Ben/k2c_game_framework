using System.Text;
using Common.WeekCardObj;
using CommonEnum;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 妃子倾诉
    /// </summary>
    public class WeekCardSettleShowInfo_CONSORT_RND_CALL:_IWeekCardSettleShowInfo
    {
        private readonly WeekCard_SettleDetailInfo _m_info;
        private WeekCard_SettleInfo_ConsortRndCall _m_exInfo;

        public WeekCardSettleShowInfo_CONSORT_RND_CALL(WeekCard_SettleDetailInfo _info)
        {
            _m_info = _info;
            _m_exInfo = new WeekCard_SettleInfo_ConsortRndCall();
            _m_exInfo.readPackage(_m_info.getDetailInfo());
        }
        
        public EWeekCardSettleType getSettleType()
        {
            return _m_info.getType();
        }

        public string getShowContent()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(TextTranslate.instance.getLanguage(TransKeyConst.week_card_settle_CONSORT_RND_CALL, _m_exInfo.getNum()));
            if(_m_exInfo.getList().Count > 0)
                sb.Append("\n");
            for (int i = 0 ; i < _m_exInfo.getList().Count ; i++)
            {
                WeekCard_ConsortRndCallInfo callInfo = _m_exInfo.getList()[i];
                sb.Append(TextTranslate.instance.getLanguage(TransKeyConst.week_card_settle_CONSORT_RND_CALL_skill_point
                    ,GCommon.getItemName(ENPItemType.CONSORT, callInfo.getConsortId())
                    , callInfo.getSkillPoint()));
                if (i != _m_exInfo.getList().Count - 1)
                    sb.Append("\n");
            }

            if (_m_exInfo.getChildNum() > 0)
            {
                sb.Append("\n");
                sb.Append(TextTranslate.instance.getLanguage(TransKeyConst.week_card_settle_CONSORT_RND_CALL_child, _m_exInfo.getChildNum()));
            }
            
            return sb.ToString();
        }
    }
}