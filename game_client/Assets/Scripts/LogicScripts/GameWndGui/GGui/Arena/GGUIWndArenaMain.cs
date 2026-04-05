using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场主页面
    /// </summary>
    public class GGUIWndArenaMain : _ANPGGUIBasicResBarWnd<GGUIMonoArenaMain>
    {
        private static GGUIWndArenaMain _g_instance;
        public static GGUIWndArenaMain instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndArenaMain();
                return _g_instance;
            }
        }

        //名人榜子窗口
        private GGUIWndArenaSubCelebrity _m_wSubCelebrityList;
        //贸易站收集子窗口
        private GGUIWndArenaStationCollection _m_wSubCollection;
        //名人榜信息列表
        private List<ArenaCelebrityInfo> _m_lCelebrityList;
        //任务刷新定时器
        private ALCommonEnableTaskController _m_tcTickTaskController;
        //显示的名人榜索引
        private int _m_iShowCelebrityIndex;
        //屏蔽输入的序列号
        private int _m_iInputMaskSerialize;

        public GGUIWndArenaMain() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoArenaMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoArenaMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override bool isShowAniPlayOnlyOne { get { return true; } }
        
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_STATION_CHG, _onStationChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_ARENA_BASE_INFO_CHG, _onArenaInfoChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_GET_ARENA_CELEBRITY_LIST, _onGetCelebrityList);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_ARENA_RANDOM_ATTACK, _onSimulateClickRandomAttack);
            WinMsg.RegisterMsgAct(WinMsgType.SET_ARENA_CELEBRITY_ONE_BOT, _onSetArenaCelebrityOneBot);

            //刷新窗口
            _refreshWnd();
            //默认隐藏名人榜附加窗口
            _m_wSubCelebrityList?.hideWnd();
            //获取名人榜信息
            _m_lCelebrityList = NPPlayer.instance.arenaComp.celebrityList;
            //先重置任务
            _m_tcTickTaskController.setDisable();
            //开启任务刷新名人榜显示
            if(wnd != null && wnd.switchCelebrityDescTimeSec > 0)
                _m_tcTickTaskController = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_onTickRefresh, wnd.switchCelebrityDescTimeSec);

            //更新一次名人榜数据
            NPPlayer.instance.arenaComp.reqCelebrityRank(0);

            //竞技场主页面显示完成TRIGGER
            WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.ARENA_MAIN_SHOW);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_STATION_CHG, _onStationChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_ARENA_BASE_INFO_CHG, _onArenaInfoChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GET_ARENA_CELEBRITY_LIST, _onGetCelebrityList);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_ARENA_RANDOM_ATTACK, _onSimulateClickRandomAttack);
            WinMsg.UnregisterMsgAct(WinMsgType.SET_ARENA_CELEBRITY_ONE_BOT, _onSetArenaCelebrityOneBot);

            _m_wSubCelebrityList?.hideWnd();
            _m_wSubCollection?.hideWnd();
            _m_tcTickTaskController.setDisable();
        }

        protected override void _onReset()
        {
            _m_wSubCelebrityList?.resetWnd();
            _m_wSubCollection?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wSubCelebrityList?.discard();
            _m_wSubCelebrityList = null;
            _m_wSubCollection?.discard();
            _m_wSubCollection = null;
            _m_tcTickTaskController.setDisable();

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnRank, _onClickRank);//点击排行榜
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);//点击关闭
            ALUGUICommon.uncombineBtnClick(wnd.btnReport, _onClickReport);//点击战报
            ALUGUICommon.uncombineBtnClick(wnd.btnSetting, _onClickSetting);//点击设置
            ALUGUICommon.uncombineBtnClick(wnd.btnStation, _onClickStation);//点击贸易站
            ALUGUICommon.uncombineBtnClick(wnd.btnAddCount, _onClickAddCount);//点击增加次数
            ALUGUICommon.uncombineBtnClick(wnd.btnRandomAttack, _onClickRandomAttack);//点击随机谈判
            ALUGUICommon.uncombineBtnClick(wnd.btnCelebrityList, _onClickCelebrityList);//点击名人榜
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoSubCelebrity != null)
                _m_wSubCelebrityList = new GGUIWndArenaSubCelebrity(wnd.monoSubCelebrity);

            if (wnd.monoStationCollection != null)
                _m_wSubCollection = new GGUIWndArenaStationCollection(wnd.monoStationCollection);

            ALUGUICommon.combineBtnClick(wnd.btnRank, _onClickRank);//点击排行榜
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);//点击关闭
            ALUGUICommon.combineBtnClick(wnd.btnReport, _onClickReport);//点击战报
            ALUGUICommon.combineBtnClick(wnd.btnSetting, _onClickSetting);//点击设置
            ALUGUICommon.combineBtnClick(wnd.btnStation, _onClickStation);//点击贸易站
            ALUGUICommon.combineBtnClick(wnd.btnAddCount, _onClickAddCount);//点击增加次数
            ALUGUICommon.combineBtnClick(wnd.btnRandomAttack, _onClickRandomAttack);//点击随机谈判
            ALUGUICommon.combineBtnClick(wnd.btnCelebrityList, _onClickCelebrityList);//点击名人榜
        }

        //检查是否正在谈判中
        public void checkIsInBattle()
        {
            //是否正在谈判中，正在谈判中需要打开对应的界面
            if (NPPlayer.instance.arenaComp.isInBattle())
            {
                //是否已经选择了伙伴
                if (NPPlayer.instance.arenaComp.getBattleSelectHeroId() > 0)
                {
                    //打开谈判界面
                    QueueMgr.instance.AddNode(new GMainQueueArenaBattleNode());
                }
                else
                {
                    //打开谈判准备界面
                    QueueMgr.instance.AddNode(new GMainQueueArenaBattlePrepareNode(() =>
                    {
                        //设置随机谈判数据
                        GGUIWndArenaBattlePrepare.instance.setRandomAttackInfo();
                    }));
                }
            }
        }

        /// <summary>
        /// 打开名人榜
        /// </summary>
        public void showCelebrityList()
        {
            _onClickCelebrityList(null);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshStationInfo();
            _refreshAttackInfo();
            _refreshCelebrityShow();
        }

        //刷新贸易站信息
        private void _refreshStationInfo()
        {
            if (wnd == null)
                return;

            StationInfo stationInfo = NPPlayer.instance.stationComp.stationInfo;
            if (stationInfo == null)
                return;

            //贸易站收集子窗口
            _m_wSubCollection?.showWnd();
            //贸易站等级
            ALUGUICommon.setLabelTxt(wnd.txtStationLevel, TextTranslate.instance.getLanguage(TransKeyConst.arena_stationLevel_num, stationInfo.level));
        }

        //刷新攻击信息
        private void _refreshAttackInfo()
        {
            if (wnd == null)
                return;

            ArenaInfo arenaInfo = NPPlayer.instance.arenaComp.arenaInfo;
            if (arenaInfo == null)
                return;

            long freeCount = GRefdataCoreMgr.instance.npGeneral.arena_random_attack_free_limit;
            long leftCount = arenaInfo.getCurLeftAttackCount();
            ALUGUICommon.setLabelTxt(wnd.txtAttackCount, TextTranslate.instance.getLanguage(TransKeyConst.arena_randomAttackCount_num_num, leftCount, freeCount));
        }

        //刷新名人榜显示
        private void _refreshCelebrityShow()
        {
            if (wnd == null)
                return;

            if (_m_lCelebrityList == null || _m_lCelebrityList.Count == 0 || _m_iShowCelebrityIndex < 0 || _m_lCelebrityList.Count <= _m_iShowCelebrityIndex)
            {
                ALUGUICommon.setLabelTxt(wnd.txtCelebrityListDesc, "");
            }
            else
            {
                ArenaCelebrityInfo info = _m_lCelebrityList[_m_iShowCelebrityIndex];
                if (info != null)
                {
                    //{0}击败{1}的{2}名伙伴
                    ALUGUICommon.setLabelTxt(wnd.txtCelebrityListDesc,
                        TextTranslate.instance.getLanguage(TransKeyConst.arena_celebrityDefeatHeroDesc_str_str_num,
                            info.attackerName, info.defenderName, info.defeatHeroNum));
                }
                else
                    ALUGUICommon.setLabelTxt(wnd.txtCelebrityListDesc, "");
            }
        }

        //定时刷新
        private void _onTickRefresh()
        {
            if(wnd == null)
                return;

            if (_m_lCelebrityList == null || _m_lCelebrityList.Count == 0)
            {
                ALUGUICommon.setGameObjEnable(wnd.goNoCelebrityHideList, false);
                ALUGUICommon.setGameObjEnable(wnd.goNoCelebrityShowList, true);
                return;
            }

            ALUGUICommon.setGameObjEnable(wnd.goNoCelebrityHideList, true);
            ALUGUICommon.setGameObjEnable(wnd.goNoCelebrityShowList, false);

            _m_iShowCelebrityIndex++;
            if(_m_iShowCelebrityIndex >= _m_lCelebrityList.Count || _m_iShowCelebrityIndex >= wnd.showCelebrityDescRanking)
                _m_iShowCelebrityIndex = 0;

            _refreshCelebrityShow();
        }

        #region 点击事件

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_MAIN);
        }

        //点击排行榜
        private void _onClickRank(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndArenaRank.instance, GGUIWndArenaRank.instance.showWnd, UINodeTagConst.C_ARENA_RANK);
        }

        //点击随机谈判
        private void _onClickRandomAttack(GameObject _go)
        {
            //次数不足，弹出购买次数弹窗
            if (!NPPlayer.instance.arenaComp.canRendomAttack())
            {
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndArenaAddRandomAttackCount.instance, GGUIWndArenaAddRandomAttackCount.instance.showWnd, UINodeTagConst.C_ARENA_ADD_RANDOM_COUNT);
                return;
            }

            //先屏蔽操作
            _m_iInputMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            NPPlayer.instance.arenaComp.reqRandomAttackPlayer((_isSuc) =>
            {
                MainCameraMono.selfInstance.closeAllInputMask(_m_iInputMaskSerialize);

                //未找到对手，不处理
                if (!_isSuc)
                    return;

                //打开谈判准备界面
                QueueMgr.instance.AddNode(new GMainQueueArenaBattlePrepareNode(() =>
                {
                    //设置随机谈判数据
                    GGUIWndArenaBattlePrepare.instance.setRandomAttackInfo();
                }));
            });
        }

        //点击战报
        private void _onClickReport(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndArenaBattleReport.instance, () =>
            {
                GGUIWndArenaBattleReport.instance.showWnd();
                GGUIWndArenaBattleReport.instance.setInfo(EArenaBattleReportTabType.REPORT);
            }, UINodeTagConst.C_ARENA_BATTLE_REPORT);
        }

        //点击设置
        private void _onClickSetting(GameObject _go)
        {
            if (!GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.arena_convenient_setting_simple_unlock_id, true))
                return;

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndArenaConvenientSetting.instance, GGUIWndArenaConvenientSetting.instance.showWnd, UINodeTagConst.C_ARENA_CONVENTENT_SETTING);
        }

        //点击升级贸易站
        private void _onClickStation(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndArenaStationInfo.instance, GGUIWndArenaStationInfo.instance.showWnd, UINodeTagConst.C_ARENA_STATION_INFO);
        }

        //点击增加谈判次数
        private void _onClickAddCount(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndArenaAddRandomAttackCount.instance, GGUIWndArenaAddRandomAttackCount.instance.showWnd, UINodeTagConst.C_ARENA_ADD_RANDOM_COUNT);
        }

        //点击名人榜
        private void _onClickCelebrityList(GameObject _go)
        {
            _m_wSubCelebrityList?.showWnd();
            _m_wSubCelebrityList?.setInfo(() =>
            {
                _m_wSubCelebrityList?.hideWnd();
            });
        }

        #endregion

        #region 消息事件

        //贸易站信息变化
        private void _onStationChg()
        {
            _refreshStationInfo();
        }

        //竞技场信息变更
        private void _onArenaInfoChg()
        {
            _refreshAttackInfo();
        }

        //获取名人榜信息
        private void _onGetCelebrityList()
        {
            _m_lCelebrityList = NPPlayer.instance.arenaComp.celebrityList;
            _refreshCelebrityShow();
        }

        //模拟点击随机谈判
        private void _onSimulateClickRandomAttack()
        {
            _onClickRandomAttack(null);
        }

        //设置名人榜机器人
        private void _onSetArenaCelebrityOneBot()
        {
            NPPlayer.instance.arenaComp.setCelebrityOneBot();
            _m_lCelebrityList = NPPlayer.instance.arenaComp.celebrityList;
            _onTickRefresh();
        }

        #endregion
    }
}