using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 前往火星主界面
    /// </summary>
    public class GGUIWndMarsGoTo : _ANPGGUIBasicResBarWnd<GGUIMonoMarsGoTo>
    {
        private static GGUIWndMarsGoTo _g_instance;
        public static GGUIWndMarsGoTo instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndMarsGoTo();
                return _g_instance;
            }
        }

        // 背景展示
        private NPGGUIWndCommonShowCase _m_wBgShowCase;
        // 倒计时控件
        private NPGGUIWndCommonCountDown _m_wCommonCD;
        // 定时任务
        private ALCommonEnableTaskController _m_refrshNavigationTimeTask;
        // 留言附加窗口
        private GGUIWndMarsGoToSubMsg _m_wSubMsg;

        public GGUIWndMarsGoTo() : base(EALUIWndLayer.NORMAL)
        {
        } 

        protected override string _monoAssetPath { get { return GGUIMonoMarsGoTo.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsGoTo.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }
        public override bool needDiscardOnSwitch { get { return true; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_MARS_ARRIVED_NEW_STAGE, _onArrivedNewStage);
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_MARS_ARRIVED_NEW_STAGE, _onArrivedNewStage);
            _m_wBgShowCase?.hideWnd();
            _m_wCommonCD?.hideWnd();
            _m_wSubMsg?.hideWnd();
            _stopRefreshTask();
        }

        protected override void _onReset()
        {
            _m_wBgShowCase?.resetWnd();
            _m_wCommonCD?.resetWnd();
            _m_wSubMsg?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wBgShowCase?.discard();
            _m_wBgShowCase = null;
            _m_wCommonCD?.discard();
            _m_wCommonCD = null;
            _m_wSubMsg?.discard();
            _m_wSubMsg = null;
    
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnLogDetail, _onClickLogDetail);
            ALUGUICommon.uncombineBtnClick(wnd.btnArriveConfirm, _onClickArriveConfirm);
            ALUGUICommon.uncombineBtnClick(wnd.btnSpeedUpTip, _onClickSpeedUpTip);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoCountDown != null)
                _m_wCommonCD = new NPGGUIWndCommonCountDown(wnd.monoCountDown);
            if (wnd.monoBgShowcase != null)
                _m_wBgShowCase = new NPGGUIWndCommonShowCase(wnd.monoBgShowcase);
            if (wnd.monoSubMsg != null)
                _m_wSubMsg = new GGUIWndMarsGoToSubMsg(wnd.monoSubMsg);

            ALUGUICommon.combineBtnClick(wnd.btnLogDetail, _onClickLogDetail);
            ALUGUICommon.combineBtnClick(wnd.btnArriveConfirm, _onClickArriveConfirm);
            ALUGUICommon.combineBtnClick(wnd.btnSpeedUpTip, _onClickSpeedUpTip);
        }

        // 刷新界面
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            // 设置显隐
            ALUGUICommon.setGameObjEnable(wnd.goDealingNewStageShowList, false);
            ALUGUICommon.setGameObjEnable(wnd.goDealingNewStageHideList, true);

            MarsStageInfo curMarsStageInfo = NPPlayer.instance.marsComp.goToSubComponent.curArrivedMarsStageInfo;
            if (curMarsStageInfo == null || curMarsStageInfo.marsGoRouteRef == null)
                return;

            // 刷新背景展示
            _m_wBgShowCase?.showWnd(new ShowCaseCommonResUnitInfoObj(curMarsStageInfo.marsGoRouteRef.bg_index));

            // 刷新节点名称和描述
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(curMarsStageInfo.marsGoRouteRef.name));
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(curMarsStageInfo.marsGoRouteRef.desc, curMarsStageInfo.marsGoRouteRef.desc_args));

            //是否有加速显隐
            long reducePer = GRefdataCoreMgr.instance.getMarsStageArriveReducePer();
            ALUGUICommon.setLabelTxt(wnd.txtSpeedUpDesc, TextTranslate.instance.getLanguage(TransKeyConst.marsGoTo_speedUpDesc_num, reducePer/100));//累计航行：{0}
            ALUGUICommon.setGameObjEnable(wnd.goSpeedUpShowList, reducePer > 0);
            ALUGUICommon.setGameObjEnable(wnd.goSpeedUpHideList, reducePer <= 0);

            //抵达火星总时间
            long arriveMarsTotalTimeSec = 0;
            GRefdataCoreMgr.instance.marsGoRouteRefCore.dealAllRef(_ref =>
            {
                if(_ref != null)
                {
                    arriveMarsTotalTimeSec += _ref.continue_secs;
                }
            });
            if (reducePer > 0)
                arriveMarsTotalTimeSec = arriveMarsTotalTimeSec * (10000 - reducePer) / 10000;
            ALUGUICommon.setLabelTxt(wnd.txtArriveTotalTime, TextTranslate.instance.getLanguage(TransKeyConst.marsGoTo_arriveMarsTotalTime_str, TimeUtil.millisecondsToTime_Two(arriveMarsTotalTimeSec * 1000)));

            // 刷新留言附加窗口
            _m_wSubMsg?.showWnd();
            _m_wSubMsg?.setInfo(curMarsStageInfo.marsGoRouteRef.stage_id);

            // 刷新倒计时
            _refreshCD();

            // 刷新航行累积时间
            _refreshNavigationTime();
            _startRefreshTask();
        }

        // 刷新倒计时
        private void _refreshCD()
        {
            MarsStageInfo curMarsStageInfo = NPPlayer.instance.marsComp.goToSubComponent.curArrivedMarsStageInfo;
            if (curMarsStageInfo == null || curMarsStageInfo.marsGoRouteRef == null)
                return;

            // 刷新倒计时
            _m_wCommonCD?.showWnd();
            _m_wCommonCD?.setInfo(TimeUtil.msToSecCeiling(curMarsStageInfo.endTimeMs - FpsAndPingMgr.instance.serverTimeTag), null);
        }

        // 刷新航行累积时间
        private void _refreshNavigationTime()
        {
            if (wnd == null)
                return;

            // 第一阶段到达时间
            long firstArrivedTimeMs = NPPlayer.instance.marsComp.goToSubComponent.firstStageArrivedTimeMs;
            string timeStr = firstArrivedTimeMs <= 0 ? "0" : TimeUtil.millisecondsToTime_Two_NoSec(FpsAndPingMgr.instance.serverTimeTag - firstArrivedTimeMs);
            ALUGUICommon.setLabelTxt(wnd.txtNavigationTime, TextTranslate.instance.getLanguage(TransKeyConst.marsGoTo_totalNavigationTime_str, timeStr));//累计航行：{0}

            // 刷新日志
            _refreshLog();
        }

        // 刷新日志
        private void _refreshLog()
        {
            // 显示log信息
            MarsStageLogInfo curLogInfo = NPPlayer.instance.marsComp.goToSubComponent.getCurLogInfo();
            if (curLogInfo != null && curLogInfo.marsGoRouteLogRef != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtLogTitle, TextTranslate.instance.getLanguage(curLogInfo.marsGoRouteLogRef.log_title, curLogInfo.marsGoRouteLogRef.log_title_args));
                ALUGUICommon.setLabelTxt(wnd.txtLogContent, TextTranslate.instance.getLanguage(curLogInfo.marsGoRouteLogRef.log_content, curLogInfo.marsGoRouteLogRef.log_content_args));
            }
        }

        // 开启定时刷新航行时间
        private void _startRefreshTask()
        {
            _m_refrshNavigationTimeTask.setDisable();
            _m_refrshNavigationTimeTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_refreshNavigationTime, 1.0f);
        }

        // 关闭定时刷新航行时间
        private void _stopRefreshTask()
        {
            _m_refrshNavigationTimeTask.setDisable();
        }

        // 处理前往新节点
        private void _dealGoToNewStage()
        {
            MarsStageInfo curMarsStageInfo = NPPlayer.instance.marsComp.goToSubComponent.curArrivedMarsStageInfo;
            if (curMarsStageInfo == null || curMarsStageInfo.marsGoRouteRef == null)
                return;

            // 下一个阶段id及配置
            long nextStageId = curMarsStageInfo.marsGoRouteRef.stage_id + 1;
            MarsGoRouteRefObj nextGoRouteRef = GRefdataCoreMgr.instance.getMarsGoRouteRefByStageId(nextStageId);

            // 进入新阶段表现流程
            ALProcess process = ALProcess.CreateProcess();
            process
                // 1、展示对话剧情
                .addDelegateProcess(_complete =>
                {
                    if (nextGoRouteRef == null || nextGoRouteRef.arrive_dialogue_id <= 0)
                        _complete?.Invoke();
                    else
                        GCommon.enterDialogueNode(nextGoRouteRef.arrive_dialogue_id, _complete);
                })
                // 2、打开确认弹窗
                .addDelegateProcess(_complete =>
                {
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsNewStageConfirm.instance, () =>
                    {
                        GGUIWndMarsNewStageConfirm.instance.showWnd();
                        GGUIWndMarsNewStageConfirm.instance.setInfo(_complete);
                    }, EUIQueueStageType.MAIN, UINodeTagConst.C_MARS_ARRIVE_NEW_STAGE_CONFIRM, false, false);
                })
                // 3、请求到达新节点，展示奖励弹窗
                .addDelegateProcess(_complete =>
                {
                    // 先设置界面显隐
                    ALUGUICommon.setGameObjEnable(wnd?.goDealingNewStageHideList, false);
                    ALUGUICommon.setGameObjEnable(wnd?.goDealingNewStageShowList, true);

                    // 先屏蔽输入，等待请求回来
                    int maskSerialize = MainCameraMono.selfInstance.openAllInputMask();
                    NPPlayer.instance.marsComp.goToSubComponent.reqArriveMarsStage((int)nextStageId, (_isSuc, _msg) =>
                    {
                        // 取消屏蔽输入
                        MainCameraMono.selfInstance.closeAllInputMask(maskSerialize);
                        if(_isSuc)
                        {
                            // 展示奖励
                            GCommon.dealGainItem(_msg.getItemList(), TransKeyConst.common_getreward_tip, _complete);
                        }
                        else
                        {
                            _complete?.Invoke();
                        }
                    });
                })
                // 4、判断是否到达火星，是则展示选择着陆点界面
                .addDelegateProcess(_complete =>
                {
                    if (NPPlayer.instance.marsComp.goToSubComponent.isArriveMars)
                    {
                        // 关闭当前界面
                        QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_GO_TO);
                        // 打开选择着陆点界面
                        QueueMgr.instance.AddNode(new GNodeMarsLandingSelect(_complete));
                    }
                    else
                        _complete?.Invoke();
                })
                // 5、刷新界面
                .addProcess(() =>
                {
                    // 是否到达了火星
                    if (NPPlayer.instance.marsComp.goToSubComponent.isArriveMars)
                    {
                        // 进入火星主界面
                        QueueMgr.instance.AddNode(new GNodeMars());
                    }
                    else
                    {
                        // 还没到达火星，刷新界面
                        _refreshWnd();
                    }
                })
                .deal();
        }

        // 到达新节点消息
        private void _onArrivedNewStage(params object[] args)
        {
            _refreshCD();
        }

        #region 点击事件

        // 点击日志详情按钮
        private void _onClickLogDetail(GameObject obj)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsGoToLog.instance, GGUIWndMarsGoToLog.instance.showWnd, EUIQueueStageType.MAIN, UINodeTagConst.C_MARS_GO_TO_LOG_DETAIL, false, false);
        }

        // 点击到达新节点按钮
        private void _onClickArriveConfirm(GameObject obj)
        {
            MarsStageInfo curMarsStageInfo = NPPlayer.instance.marsComp.goToSubComponent.curArrivedMarsStageInfo;
            if (curMarsStageInfo == null || curMarsStageInfo.marsGoRouteRef == null)
                return;

            // 完成条件未通过，不能前往新节点
            if (!GCommon.isSimpleUnlock(curMarsStageInfo.marsGoRouteRef.done_simple_unlock_id,true))
                return;

            _dealGoToNewStage();
        }

        // 点击加速提示按钮
        private void _onClickSpeedUpTip(GameObject obj)
        {
            //现金展示提示：众多火星先驱维护的航线，为后来者提供便利~
            NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.marsGoTo_speedUpTip_none);
        }

        #endregion
    }
}