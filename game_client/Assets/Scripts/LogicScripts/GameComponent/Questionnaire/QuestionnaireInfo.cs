using System.Collections.Generic;
using Common;
using NPCommon;

namespace GOE
{
    /// <summary>
    /// 问卷调查信息
    /// </summary>
    public class QuestionnaireInfo
    {
        private long _m_lQuestionnaireId;//问卷id
        private long _m_lStartTimeMs;//开始时间
        private long _m_lEndTimeMs;//结束时间
        private ENPCommonGetStat _m_eGetState;//可领取状态
        private string _m_sUrlLink;//链接
        private string _m_sQuestionnaireCode;//问卷编码
        private List<NPCommonCostItem> _m_lRewardList;//奖励列表

        /// <summary>
        /// 问卷id
        /// </summary>
        public long questionnaireId { get { return _m_lQuestionnaireId; } }
        /// <summary>
        /// 开始时间
        /// </summary>
        public long startTimeMs { get { return _m_lStartTimeMs; } }
        /// <summary>
        /// 结束时间
        /// </summary>
        public long endTimeMs { get { return _m_lEndTimeMs; } }
        /// <summary>
        /// 领取状态
        /// </summary>
        public ENPCommonGetStat getState { get { return _m_eGetState; } }
        /// <summary>
        /// 链接
        /// </summary>
        public string urlLink { get { return _m_sUrlLink; } }
        /// <summary>
        /// 问卷编码
        /// </summary>
        public string questionnaireCode { get { return _m_sQuestionnaireCode; } }
        /// <summary>
        /// 奖励列表
        /// </summary>
        public List<NPCommonCostItem> rewardList { get { return _m_lRewardList; } }

        public QuestionnaireInfo(Common_QuestionnaireInfo _info)
        {
            if (_info == null)
                return;

            _m_lQuestionnaireId = _info.getQuestionnaireId();
            _m_lStartTimeMs = _info.getStartTimeMs();
            _m_lEndTimeMs = _info.getEndTimeMs();
            _m_eGetState = ENPCommonGetStat.CAN_NOT_GET;
            _m_sUrlLink = _info.getUrlLink();
            _m_sQuestionnaireCode = _info.getQuestionnaireCode();
            _m_lRewardList = new List<NPCommonCostItem>();
            if (_info.getRewardList() != null)
            {
                for (int i = 0; i < _info.getRewardList().Count; i++)
                {
                    if(_info.getRewardList()[i] != null)
                        _m_lRewardList.Add(new NPCommonCostItem(_info.getRewardList()[i]));
                }
            }
        }

        /// <summary>
        /// 设置领取状态
        /// </summary>
        /// <param name="_isGet"></param>
        public void setGetState(bool _isGet)
        {
            if (_isGet)
                _m_eGetState = ENPCommonGetStat.HAS_GET;
            else
                _m_eGetState = ENPCommonGetStat.CAN_GET;
        }
    }
}