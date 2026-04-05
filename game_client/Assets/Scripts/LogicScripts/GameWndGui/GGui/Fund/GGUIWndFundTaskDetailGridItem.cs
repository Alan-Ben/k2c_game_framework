using ALPackage;
using Common.ActivityFundObj;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 基金任务详情Grid Item
    /// </summary>
    public class GGUIWndFundTaskDetailGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoFundTaskDetailGridItem>
    {
        private ActivityFund_TaskInfo _m_taskInfo;
        private NPGGUIWndCommonItem _m_scoreItem;


        public GGUIWndFundTaskDetailGridItem(GGUIMonoFundTaskDetailGridItem _mono) : base(_mono)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
            _m_scoreItem?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_scoreItem?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_scoreItem?.resetWnd();
        }
        protected override void _resetGridItem()
        {
        }
        protected override void _onDiscard()
        {
            _m_scoreItem?.discard();
            _m_scoreItem = null;
            
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnGoTo, _onClickGoTo);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoScoreItem != null)
                _m_scoreItem = new NPGGUIWndCommonItem(wnd.monoScoreItem);

            ALUGUICommon.combineBtnClick(wnd.btnGoTo, _onClickGoTo);
        }


        public void refreshWnd(ActivityFund_TaskInfo _taskInfo)
        {
            _m_taskInfo = _taskInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || _m_taskInfo == null || !_m_bIsShow)
                return;

            ActivityFundTaskRefObj taskRef = GRefdataCoreMgr.instance.activityFundTaskRefCore.getRef(_m_taskInfo.getTaskId());
            if (taskRef == null)
                return;

            long currentCount = _m_taskInfo.getCurrentCount();
            int finishedTimes = _m_taskInfo.getFinishedTimes();
            long taskFinishLimit = taskRef.task_finish_limit;
            long doneTaskCount = taskRef.done_task_count;
            long gainScore = taskRef.gain_score;
            
            //设置奖励积分
            ALUGUICommon.setLabelTxt(wnd.txtScore, gainScore.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            _m_scoreItem?.setItem(new NPCommonCostItem(GRefdataCoreMgr.instance.npGeneral.activity_fund_battle_pass_exp_item, gainScore));

            //设置任务名称和完成次数
            string taskName = TextTranslate.instance.getLanguage(taskRef.desc, taskRef.desc_args);
            ALUGUICommon.setLabelTxt(wnd.txtNameAndTimes, TextTranslate.instance.getLanguage(TransKeyConst.fund_taskNameWithTimes_name_value_value, taskName, finishedTimes, taskFinishLimit));

            //设置进度文本
            string currentCountStr = GCommon.getValueFormatStr(taskRef.process_num_format, currentCount);
            string doneTaskCountStr = GCommon.getValueFormatStr(taskRef.process_num_format, doneTaskCount);
            ALUGUICommon.setLabelTxt(wnd.txtProgress, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, currentCountStr, doneTaskCountStr));

            //设置进度条
            if (wnd.sldProgress != null)
            {
                wnd.sldProgress.minValue = 0;
                wnd.sldProgress.maxValue = doneTaskCount == 0 ? 1 : doneTaskCount;
                wnd.sldProgress.value = currentCount;
            }
        }


        private void _onClickGoTo(GameObject _)
        {
            if (_m_taskInfo == null)
                return;

            ActivityFundTaskRefObj taskRef = GRefdataCoreMgr.instance.activityFundTaskRefCore.getRef(_m_taskInfo.getTaskId());
            taskRef?.go_to?.dealEffect();
        }
    }
}
