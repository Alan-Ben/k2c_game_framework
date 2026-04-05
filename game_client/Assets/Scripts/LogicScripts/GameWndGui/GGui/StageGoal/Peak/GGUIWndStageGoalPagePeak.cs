using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 阶段目标-时代之巅页面
    /// </summary>
    public class GGUIWndStageGoalPagePeak : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoStageGoalPagePeak>
    {
        // 大阶段列表
        private GGUIWndStageGoalPagePeakGrid _m_wGrid;
        // 第一名玩家形象
        private NPGGUIWndCommonShowCase _m_wTopPlayerShowCase;
        // 第一名玩家头像
        private NPGGUIWndPlayerIcon _m_wPlayerIcon;
        // 显示序列号
        private long _m_lShowSerialize;

        public GGUIWndStageGoalPagePeak(Transform _parent) : base(_parent)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoStageGoalPagePeak.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoStageGoalPagePeak.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        /// <summary>
        /// show 动画是否只播放一次
        /// </summary>
        protected override bool isShowAniPlayOnlyOne { get { return true; } }


        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_STAGE_GOAL_FIRST_REACH_ADD, _onFirstReachAdd);//新增可领取大阶段首达奖励
            WinMsg.RegisterMsg(WinMsgType.ON_STAGE_GOAL_FIRST_REACH_DRAW, _onFirstReachDraw);//已经领取大阶段首达奖励
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_STAGE_GOAL_FIRST_REACH_ADD, _onFirstReachAdd);//新增可领取大阶段首达奖励
            WinMsg.UnregisterMsg(WinMsgType.ON_STAGE_GOAL_FIRST_REACH_DRAW, _onFirstReachDraw);//已经领取大阶段首达奖励
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_wGrid?.hideWnd();
            _m_wTopPlayerShowCase?.hideWnd();
            _m_wPlayerIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wGrid?.resetWnd();
            _m_wTopPlayerShowCase?.resetWnd();
            _m_wPlayerIcon?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wGrid?.discard();
            _m_wGrid = null;
            _m_wTopPlayerShowCase?.discard();
            _m_wTopPlayerShowCase = null;
            _m_wPlayerIcon?.discard();
            _m_wPlayerIcon = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoGrid != null)
                _m_wGrid = new GGUIWndStageGoalPagePeakGrid(wnd.monoGrid);

            if (wnd.monoFirstPlayer != null)
                _m_wTopPlayerShowCase = new NPGGUIWndCommonShowCase(wnd.monoFirstPlayer);

            if (wnd.monoFirstPlayerIcon != null)
                _m_wPlayerIcon = new NPGGUIWndPlayerIcon(wnd.monoFirstPlayerIcon);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            //刷新阶段列表
            _refreshStageList(true);

            //请求最新数据需要时间，先用已有数据设置一遍显示，再请求最新数据刷新界面
            _refreshCurReachInfo();

            //请求获取最新数据刷新界面
            _setNewReachInfo();
        }

        //刷新阶段列表
        private void _refreshStageList(bool _needMoveToTarget)
        {
            //设置列表
            List<StageGoalBigStepRefObj> bigStepRefList = GRefdataCoreMgr.instance.stageGoalBigStepRefCore.refList;
            _m_wGrid?.showWnd();
            _m_wGrid?.setInfo(bigStepRefList, _needMoveToTarget);
        }

        //使用已有数据刷新首达信息
        private void _refreshCurReachInfo()
        {
            if (wnd == null)
                return;

            //刷新每个阶段首达信息
            _m_wGrid?.setFirstReachInfo(NPPlayer.instance.stageGoalComp.firstReachInfoList);
            //第一名信息
            StageGoalTopPlayerInfo topPlayerInfo = NPPlayer.instance.stageGoalComp.topPlayerInfo;
            bool haveTopPlayer = topPlayerInfo != null && topPlayerInfo.cid > 0 && topPlayerInfo.simplePlayerInfo != null;
            ALUGUICommon.setGameObjEnable(wnd.goHaveTopPlayerHideList, !haveTopPlayer);
            ALUGUICommon.setGameObjEnable(wnd.goHaveTopPlayerShowList, haveTopPlayer);
            if (topPlayerInfo != null && topPlayerInfo.simplePlayerInfo != null)
            {
                _m_wTopPlayerShowCase?.showWnd(new ShowCaseCommonResUnitInfoObj(topPlayerInfo.simplePlayerInfo.skinRef?.td_show));
                _m_wPlayerIcon?.showWnd();
                _m_wPlayerIcon?.setPlayerInfo(topPlayerInfo.simplePlayerInfo);
                ALUGUICommon.setLabelTxt(wnd.txtFirstPlayerSmallStage, topPlayerInfo?.stageGoalRef?.getTitle);
            }
            else
            {
                _m_wTopPlayerShowCase?.hideWnd();
                _m_wPlayerIcon?.setName("", 0);
                _m_wPlayerIcon?.setEarnings(0);
                _m_wPlayerIcon?.hideWnd();
                ALUGUICommon.setLabelTxt(wnd.txtFirstPlayerSmallStage, "");
            }
        }

        //获取最新首达信息刷新界面
        private void _setNewReachInfo()
        {
            long curSerialize = _m_lShowSerialize;
            NPPlayer.instance.stageGoalComp.reqStageGoalFirstReachBaseInfo(_msg =>
            {
                //刷新每个阶段首达信息
                _m_wGrid?.setFirstReachInfo(NPPlayer.instance.stageGoalComp.firstReachInfoList);
                //刷新第一名玩家信息
                StageGoalTopPlayerInfo topPlayerInfo = NPPlayer.instance.stageGoalComp.topPlayerInfo;
                bool haveTopPlayer = topPlayerInfo != null && topPlayerInfo.cid > 0;
                if (haveTopPlayer)
                {
                    topPlayerInfo.getPlayerInfo(playerInfo =>
                    {
                        if (wnd == null || playerInfo == null || curSerialize != _m_lShowSerialize)
                            return;

                        ALUGUICommon.setGameObjEnable(wnd.goHaveTopPlayerHideList, false);
                        ALUGUICommon.setGameObjEnable(wnd.goHaveTopPlayerShowList, true);

                        ALUGUICommon.setLabelTxt(wnd.txtFirstPlayerSmallStage, topPlayerInfo?.stageGoalRef?.getTitle);
                        _m_wTopPlayerShowCase?.showWnd(new ShowCaseCommonResUnitInfoObj(playerInfo.skinRef?.td_show));
                        _m_wPlayerIcon?.showWnd();
                        _m_wPlayerIcon?.setPlayerInfo(playerInfo);
                    });
                }
                else
                {
                    if (wnd == null || curSerialize != _m_lShowSerialize)
                        return;

                    ALUGUICommon.setGameObjEnable(wnd.goHaveTopPlayerHideList, true);
                    ALUGUICommon.setGameObjEnable(wnd.goHaveTopPlayerShowList, false);
                    _m_wTopPlayerShowCase?.hideWnd();
                    _m_wPlayerIcon?.hideWnd();
                    ALUGUICommon.setLabelTxt(wnd.txtFirstPlayerSmallStage, "");
                }
            });
        }

        #region 消息事件
        
        // 新增可领取大阶段首达奖励
        private void _onFirstReachAdd(params object[] _objects)
        {
            //刷新阶段列表
            _refreshStageList(false);
            //请求获取最新数据刷新界面
            _setNewReachInfo();
        }

        // 已经领取大阶段首达奖励
        private void _onFirstReachDraw(params object[] _objects)
        {
            //刷新阶段列表
            _refreshStageList(false);
        }

        #endregion
    }
}