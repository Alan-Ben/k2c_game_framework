using System;
using System.Collections.Generic;
using ALPackage;
using Common.ActivityFundObj;
using GC2GS.p017_ActivityOp;
using GS2GC.p017_ActivityOp;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 基金任务详情弹窗
    /// </summary>
    public class GGUIWndFundTaskDetail : _ATALBasicUIWnd<GGUIMonoFundTaskDetail>
    {
        public static GGUIWndFundTaskDetail instance { get { return _g_instance ??= new GGUIWndFundTaskDetail(); } }
        private static GGUIWndFundTaskDetail _g_instance;

        private GGUIWndFundTaskDetailGrid _m_wGrid;

        private FundInfoSnapshot _m_fundSnapshot;
        private int _m_showSerialize;
        private long _m_lNextRefreshTime;
        private ALCommonEnableTaskController _m_refreshTaskHandle;


        private GGUIWndFundTaskDetail()
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoFundTaskDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoFundTaskDetail.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_wGrid?.showWnd();
            _refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_wGrid?.hideWnd();
            _m_showSerialize = ALSerializeOpMgr.next();
            _stopRefreshTask();
        }
        protected override void _onReset()
        {
            _m_wGrid?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_wGrid?.discard();
            _m_wGrid = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoGrid != null)
                _m_wGrid = new GGUIWndFundTaskDetailGrid(wnd.monoGrid);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }


        /// <summary>
        /// 设置任务列表信息
        /// </summary>
        public void refreshWnd(FundInfoSnapshot _snapshot)
        {
            _m_fundSnapshot = _snapshot;
            _refreshWnd();
        }


        private void _refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_fundSnapshot == null)
                return;

            _stopRefreshTask();
            wnd.setLoadingState(true);
            int curSerialize = _m_showSerialize;
            _reqTaskInfo(_msg =>
            {
                if (curSerialize != _m_showSerialize)
                    return;

                wnd.setLoadingState(false);

                if (_msg == null)
                    return;

                //保存刷新时间并启动倒计时任务
                _m_lNextRefreshTime = _msg.getTaskNextRefreshTime();
                _updateRefreshTimeText();
                _startRefreshTask();

                //刷新任务列表
                _m_wGrid?.refreshWnd(_msg.getTaskList());
            });
        }
        private void _startRefreshTask()
        {
            _stopRefreshTask();
            if (_m_lNextRefreshTime <= 0)
                return;

            _m_refreshTaskHandle = ALCommonEnableDurationActionMonoTask.addMonoTask(_onRefreshTick, 1f);
        }
        private void _stopRefreshTask()
        {
            _m_refreshTaskHandle.setDisable();
        }
        private void _onRefreshTick()
        {
            _updateRefreshTimeText();

            long remainTime = _m_lNextRefreshTime - FpsAndPingMgr.instance.serverTimeTag;
            if (remainTime <= 0)
            {
                _stopRefreshTask();
                _refreshWnd();
            }
        }
        private void _updateRefreshTimeText()
        {
            if (wnd == null)
                return;

            long remainTime = _m_lNextRefreshTime - FpsAndPingMgr.instance.serverTimeTag;
            ALUGUICommon.setLabelTxt(wnd.txtRefreshTime,
                TextTranslate.instance.getLanguage(TransKeyConst.fund_taskRefreshTime_time, TimeUtil.millisecondsToTime_dhms(remainTime)));
        }
        private void _onClickClose(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_FUND_TASK_DETAIL);
        }
        private void _reqTaskInfo(Action<GS2GC_017_019_RetActivityFundTaskInfo> _complete)
        {
            if (_m_fundSnapshot == null)
            {
                _complete?.Invoke(null);
                return;
            }
            
            NPGSClientListener.sendRequestByLog(new GC2GS_017_019_ReqActivityFundTaskInfo(_m_fundSnapshot.fundId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_017_019_RetActivityFundTaskInfo>(_complete));
        }
    }
}
