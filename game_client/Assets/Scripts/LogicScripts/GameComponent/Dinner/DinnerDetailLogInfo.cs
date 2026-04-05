
namespace GOE
{
    public enum EDinnerDetailLogType
    {
        CREATE,
        JOIN,
    }
    public class DinnerDetailLogInfo
    {
        private GDinnerTypeRefObj _m_dinnerTypeRef;
        private long _m_costId;
        private long _m_score;
        private EDinnerDetailLogType _m_logType;
        private Common.DinnerEnum.EDinnerJoinerType _m_joinerType;
        private long _m_joinerId;
        private long _m_timeMs;
        public Common.DinnerEnum.EDinnerJoinerType joinerType => _m_joinerType;
        public long joinerId => _m_joinerId;
        public long timeMs => _m_timeMs;
        
        public string makeLogContent(string _playerName)
        {
            switch (_m_logType)
            {
                case EDinnerDetailLogType.CREATE:
                    return TextTranslate.instance.getLanguage(TransKeyConst.dinner_detail_log_create_info_str, _playerName, TextTranslate.instance.getLanguage(_m_dinnerTypeRef.name));
                case EDinnerDetailLogType.JOIN:
                    var costRef = GRefdataCoreMgr.instance.dinnerJoinCostRefCore.getRef(_m_costId);
                    return TextTranslate.instance.getLanguage(TransKeyConst.dinner_detail_log_join_info_str,_playerName, TextTranslate.instance.getLanguage(costRef?.name), _m_score);
                default:
                    return string.Empty;
            }
        }
        public DinnerDetailLogInfo(EDinnerDetailLogType _logType, GDinnerTypeRefObj _dinnerTypeRef, Common.DinnerEnum.EDinnerJoinerType _joinerType, long _joinerId, long _costId, long _score, long _timeMs)
        {
            _m_dinnerTypeRef = _dinnerTypeRef;
            _m_costId = _costId;
            _m_score = _score;
            _m_joinerType = _joinerType;
            _m_joinerId = _joinerId;
            _m_logType = _logType;
            _m_timeMs = _timeMs;
        }
    
    
    }
}
