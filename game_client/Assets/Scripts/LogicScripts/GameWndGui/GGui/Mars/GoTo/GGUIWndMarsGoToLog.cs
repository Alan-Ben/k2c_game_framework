using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星航行日志界面
    /// </summary>
    public class GGUIWndMarsGoToLog : _ANPGGUIBasicWnd<GGUIMonoMarsGoToLog>
    {
        private static GGUIWndMarsGoToLog _g_instance;
        public static GGUIWndMarsGoToLog instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndMarsGoToLog();
                return _g_instance;
            }
        }

        // 日志列表
        private GGUIWndMarsGoToLogGrid _m_wLogGrid;

        public GGUIWndMarsGoToLog() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoMarsGoToLog.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsGoToLog.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override bool isShowAniPlayOnlyOne { get { return true; } }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wLogGrid?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wLogGrid?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wLogGrid?.discard();
            _m_wLogGrid = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose2, _onBtnCloseClick);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoLogGrid != null)
                _m_wLogGrid = new GGUIWndMarsGoToLogGrid(wnd.monoLogGrid);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnClose2, _onBtnCloseClick);
        }

        // 刷新界面
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            MarsStageInfo curMarsStageInfo = NPPlayer.instance.marsComp.goToSubComponent.curArrivedMarsStageInfo;
            if (curMarsStageInfo == null || curMarsStageInfo.marsGoRouteRef == null)
                return;

            // 显示日志列表
            List<MarsStageLogInfo> logList = NPPlayer.instance.marsComp.goToSubComponent.getAlreadyTriggerLogInfoList();
            logList?.Sort((_a, _b) => -(_a.triggerTimeMs.CompareTo(_b.triggerTimeMs)));
            if (_m_wLogGrid != null && logList != null)
            {
                _m_wLogGrid.showWnd();
                _m_wLogGrid.setInfo(logList);
            }

            //设置阶段名称
            if (wnd.processStateList != null)
            {
                for (int i = 0; i < wnd.processStateList.Count; i++)
                {
                    if (wnd.processStateList[i] == null)
                        continue;

                    MarsGoRouteRefObj stageRef = GRefdataCoreMgr.instance.getMarsGoRouteRefByStageId(i + 1);
                    if (stageRef != null)
                        ALUGUICommon.setLabelTxt(wnd.processStateList[i].txtName, TextTranslate.instance.getLanguage(stageRef.name));
                }
            }

            //设置航行进度
            wnd.setProcessState(curMarsStageInfo.marsGoRouteRef.stage_id);
        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        private void _onBtnCloseClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_GO_TO_LOG_DETAIL);
        }
    }
}