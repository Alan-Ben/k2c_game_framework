using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 阶段任务冲榜活动任务item
    /// </summary>
    public class GGUIWndStepTaskRushRankActivityTaskItem : _ANPGGUIBasicGridItemWnd<GGUIMonoStepTaskRushRankActivityTaskItem>
    {
        // 阶段奖励信息
        private ActivityStepRewardInfo _m_stepRewardInfo;
        // 事件任务配置
        private ActivityStepRewardSetEventTaskRefObj _m_eventTaskRef;


        public GGUIWndStepTaskRushRankActivityTaskItem(GGUIMonoStepTaskRushRankActivityTaskItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _resetGridItem()
        {
        }

        protected override void _onDiscard()
        {
            _m_stepRewardInfo = null;
            _m_eventTaskRef = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnGoto, _onClickGoto);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnGoto, _onClickGoto);
        }


        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_stepRewardInfo">阶段奖励信息</param>
        /// <param name="_eventTaskRef">事件任务配置</param>
        public void setInfo(ActivityStepRewardInfo _stepRewardInfo, ActivityStepRewardSetEventTaskRefObj _eventTaskRef)
        {
            _m_stepRewardInfo = _stepRewardInfo;
            _m_eventTaskRef = _eventTaskRef;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || _m_eventTaskRef == null || _m_stepRewardInfo == null || !isShow)
                return;

            // 刷新任务名称
            ALUGUICommon.setLabelTxt(wnd.txtTaskName, TextTranslate.instance.getLanguage(_m_eventTaskRef.task_name, _m_eventTaskRef.task_name_args));

            // 刷新奖励点数
            if (wnd.txtRewardPoints != null)
            {
                string gainRewardPoint = GCommon.getValueFormatStr(_m_eventTaskRef.process_num_format, _m_eventTaskRef.trigger_gain_points);
                ALUGUICommon.setLabelTxt(wnd.txtRewardPoints, TextTranslate.instance.getLanguage(TransKeyConst.stepTaskRushRank_rewardPoints_num, gainRewardPoint));    
            }
            
            // 刷新总点数
            _refreshTotalPoints();
        }

        /// <summary>
        /// 刷新总点数
        /// 文本key: Obtained:{0} Points
        /// 有获取点数上限时中间参数展示为{当前分数}/{上限分数}, 没有点数限制时中间参数展示为{当前分数}
        /// </summary>
        private void _refreshTotalPoints()
        {
            if (wnd == null || wnd.txtTotalPoints == null || _m_eventTaskRef == null)
                return;

            long currentScore = _getCurrentEventTaskScore();
            string pointsStr;

            // 判断是否有点数上限（process_limit <= 0 表示无限制）
            if (_m_eventTaskRef.process_limit > 0)
            {
                // 有上限：显示为 当前分数/上限分数
                string currentStr = GCommon.getValueFormatStr(_m_eventTaskRef.process_num_format, currentScore);
                string limitStr = GCommon.getValueFormatStr(_m_eventTaskRef.process_num_format, _m_eventTaskRef.process_limit);
                pointsStr = TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, currentStr, limitStr);
            }
            else
            {
                // 无上限：只显示当前分数
                pointsStr = GCommon.getValueFormatStr(_m_eventTaskRef.process_num_format, currentScore);
            }

            ALUGUICommon.setLabelTxt(wnd.txtTotalPoints, TextTranslate.instance.getLanguage(TransKeyConst.stepTaskRushRank_totalPoints_num, pointsStr));
        }

        /// <summary>
        /// 获取当前事件任务的分数
        /// </summary>
        /// <returns>当前分数</returns>
        private long _getCurrentEventTaskScore()
        {
            if (_m_stepRewardInfo == null || _m_eventTaskRef == null)
                return 0;

            ActivityStepRewardEventTaskInfo eventTaskInfo = _m_stepRewardInfo.getEventTaskInfo(_m_eventTaskRef.id);
            return eventTaskInfo?.score ?? 0;
        }


        #region 按钮点击回调

        /// <summary>
        /// 点击前往按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickGoto(GameObject _go)
        {
            if (_m_eventTaskRef == null || _m_eventTaskRef.go_to == null || _m_eventTaskRef.go_to.isEmpty)
                return;

            // 执行跳转效果
            _m_eventTaskRef.go_to.dealEffect();
        }

        #endregion
    }
}
