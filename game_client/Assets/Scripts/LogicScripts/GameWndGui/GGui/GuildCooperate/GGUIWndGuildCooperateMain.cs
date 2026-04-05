using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 公会协作主界面
    /// </summary>
    public class GGUIWndGuildCooperateMain : _ANPGGUIBasicResBarWnd<GGUIMonoGuildCooperateMain>
    {
        private static GGUIWndGuildCooperateMain _g_instance = new GGUIWndGuildCooperateMain();
        public static GGUIWndGuildCooperateMain instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildCooperateMain();
                return _g_instance;
            }
        }

        //奖励据点预制体缓存字典
        [NotNull] private Dictionary<long, List<GGUIWndGuildCooperatePosPrefab>> _m_dRewardPosPrefabDic = new Dictionary<long, List<GGUIWndGuildCooperatePosPrefab>>();
        //背景页面字典,<区域id，页面>
        [NotNull] private Dictionary<long, GGUIWndGuildCooperateBgPage> _m_dBgPageDic = new Dictionary<long, GGUIWndGuildCooperateBgPage>();
        //当前展示的背景页面
        private GGUIWndGuildCooperateBgPage _m_wCurBgPage;
        //当前展示区域ID
        private long _m_lCurShowAreaId;
        //定时任务
        private ALCommonEnableTaskController _m_checkTask;
        //显示操作序列号
        private long _m_lShowSerialize;


        public GGUIWndGuildCooperateMain() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildCooperateMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoGuildCooperateMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_COOPERATE_DRAW_REWARD_CHG, _onGetRewardChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_COOPERATE_RESET, _onGuildCooperateReset);
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_COOPERATE_RESET_DEFAULT_AREA_SHOW, _onGuildCooperateResetDefaultAreaShow);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_GUILD_COOPERATE_CAN_GET_REWARD_POS, _simulateClickCanGetRewardPos);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_GUILD_COOPERATE_CAN_CONSTRUCT_ATTR_POS, _simulateClickCanConstructAttrPos);
            _m_lShowSerialize = ALSerializeOpMgr.next();
            if(_m_lCurShowAreaId == 0)
                _m_lCurShowAreaId = NPPlayer.instance.guildCooperateComp.curAreaId;
            _refreshWnd();
            _startCD();
            _checkIsReset();

            //设置红点已读
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_GUILD_COOPERATE_UNLOCK, 0);
            AccountSettingMgr.instance.accountSetting.addAlreadyReadGuildRedTip(RedTipConst.RED_GUILD_COOPERATE_UNLOCK);
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_GUILD_COOPERATE_UNLOCK_NEW_AREA, 0);
            AccountSettingMgr.instance.accountSetting.setGuildCooperateAreaUnlockRedTipRead($"{NPPlayer.instance.guildCooperateComp.resetCount}_{NPPlayer.instance.guildCooperateComp.curAreaId}");
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_COOPERATE_DRAW_REWARD_CHG, _onGetRewardChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_COOPERATE_RESET, _onGuildCooperateReset);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_COOPERATE_RESET_DEFAULT_AREA_SHOW, _onGuildCooperateResetDefaultAreaShow);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_GUILD_COOPERATE_CAN_GET_REWARD_POS, _simulateClickCanGetRewardPos);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_GUILD_COOPERATE_CAN_CONSTRUCT_ATTR_POS, _simulateClickCanConstructAttrPos);
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _pushBackRewardPosPrefab();
            _stopCD();

            foreach (GGUIWndGuildCooperateBgPage bgPage in _m_dBgPageDic.Values)
            {
                bgPage?.hideWnd();
            }
        }

        protected override void _onReset()
        {
            foreach (GGUIWndGuildCooperateBgPage bgPage in _m_dBgPageDic.Values)
            {
                bgPage?.resetWnd();
            }
        }

        protected override void _onDiscard()
        {
            foreach (GGUIWndGuildCooperateBgPage bgPage in _m_dBgPageDic.Values)
            {
                bgPage?.discard();
            }
            _m_dBgPageDic.Clear();
            _m_wCurBgPage = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnRank, _onClickRank);
            ALUGUICommon.uncombineBtnClick(wnd.btnLog, _onClickLog);
            ALUGUICommon.uncombineBtnClick(wnd.btnMap, _onClickMap);
            ALUGUICommon.uncombineBtnClick(wnd.btnPrevious, _onClickPrevious);
            ALUGUICommon.uncombineBtnClick(wnd.btnNext, _onClickNext);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnRank, _onClickRank);
            ALUGUICommon.combineBtnClick(wnd.btnLog, _onClickLog);
            ALUGUICommon.combineBtnClick(wnd.btnMap, _onClickMap);
            ALUGUICommon.combineBtnClick(wnd.btnPrevious, _onClickPrevious);
            ALUGUICommon.combineBtnClick(wnd.btnNext, _onClickNext);
        }

        /// <summary>
        /// 重置当前展示的区域id
        /// </summary>
        public void resetCurShowAreaId()
        {
            _m_lCurShowAreaId = 0;
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            _refreshArea();
            _refreshSwitchBtn();
            _refreshRewardTag();
            _tickCD();
        }

        /// <summary>
        /// 刷新区域展示
        /// </summary>
        private void _refreshArea()
        {
            if (wnd == null)
                return;

            GuildCooperateAreaRefObj areaRef = GRefdataCoreMgr.instance.guildCooperateAreaRefCore.getRef(_m_lCurShowAreaId);
            if (areaRef == null)
                return;

            //区域名称
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(areaRef.area_name));

            //区域背景
            if (wnd.bgParent != null)
            {
                if (_m_wCurBgPage != null && _m_wCurBgPage.areaId == areaRef.area_id)
                {
                    _m_wCurBgPage.showWnd();
                    _m_wCurBgPage.setInfo();
                    _refreshRewardPosShow();
                }
                else
                {
                    _m_wCurBgPage?.hideWnd();
                    GGUIWndGuildCooperateBgPage bgPage = null;
                    if (!_m_dBgPageDic.TryGetValue(areaRef.area_id, out bgPage))
                    {
                        bgPage = new GGUIWndGuildCooperateBgPage(areaRef.bg_ui_res_id, areaRef, wnd.bgParent);
                        bgPage.load(() =>
                        {
                            _m_wCurBgPage = bgPage;
                            _m_wCurBgPage.showWnd();
                            _m_wCurBgPage.setInfo();
                            _refreshRewardPosShow();
                        });
                        _m_dBgPageDic[areaRef.area_id] = bgPage;
                    }
                    else
                    {
                        _m_wCurBgPage = bgPage;
                        _m_wCurBgPage?.showWnd();
                        _m_wCurBgPage?.setInfo();
                        _refreshRewardPosShow();
                    }
                }
            }

            //区域是否解锁
            bool isUnlock = NPPlayer.instance.guildCooperateComp.getAreaState(_m_lCurShowAreaId) != EGuildCooperateMapAreaState.LOCK;
            ALUGUICommon.setGameObjEnable(wnd.goLockHideList, isUnlock);
            ALUGUICommon.setGameObjEnable(wnd.goLockShowList, !isUnlock);
        }

        /// <summary>
        /// 刷新切换按钮
        /// </summary>
        private void _refreshSwitchBtn()
        {
            if (wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.btnPrevious, _m_lCurShowAreaId - 1 > 0);
            ALUGUICommon.setGameObjEnable(wnd.btnNext, GRefdataCoreMgr.instance.guildCooperateAreaRefCore.getRef(_m_lCurShowAreaId + 1) != null);
        }

        /// <summary>
        /// 刷新奖励据点展示
        /// </summary>
        private void _refreshRewardPosShow()
        {
            if (wnd == null || _m_wCurBgPage == null)
                return;

            //回收旧的奖励据点预制体
            _pushBackRewardPosPrefab();

            //获取当前区域的奖励据点
            long curSerialize = _m_lShowSerialize;
            List<GuildCooperateRewardPointInfo> rewardPointInfoList = NPPlayer.instance.guildCooperateComp.getRewardPointList(_m_lCurShowAreaId);
            if (rewardPointInfoList != null)
            {
                ALStepCounter stepCounter = new ALStepCounter();
                stepCounter.chgTotalStepCount(rewardPointInfoList.Count);
                stepCounter.regAllDoneDelegate(() =>
                {
                    //联盟协作奖励据点加载完成TRIGGER
                    WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.GUILD_COOPERATE_REWARD_POS_LOAD_DONE);
                });

                //按索引排序
                rewardPointInfoList.Sort((_a, _b) => _a.index.CompareTo(_b.index));
                //创建新的奖励据点预制体
                for (int i = 0; i < rewardPointInfoList.Count; i++)
                {
                    GuildCooperateRewardPointInfo pointInfo = rewardPointInfoList[i];
                    if (pointInfo == null)
                    {
                        stepCounter.addDoneStepCount();
                        continue;
                    }

                    //获取奖励据点预制体
                    long resId = pointInfo.getRewardPointUIResId();
                    RectTransform itemParent = _m_wCurBgPage.getRewardParentByIndex(i);
                    if (itemParent == null)
                    {
                        stepCounter.addDoneStepCount();
                        continue;
                    }

                    GGuildCooperatePosPrefabCacheMgr.instance.popItem(resId, itemParent,
                    _item =>
                    {
                        if (_item == null || wnd == null || !isShow || curSerialize != _m_lShowSerialize)
                        {
                            GGuildCooperatePosPrefabCacheMgr.instance.pushBackCacheItem(resId, _item);
                            return;
                        }

                        _item.showWnd();
                        _item.setInfo(pointInfo);

                        // 保存到字典
                        if (_m_dRewardPosPrefabDic.TryGetValue(resId, out List<GGUIWndGuildCooperatePosPrefab> _prefabList))
                        {
                            _prefabList?.Add(_item);
                        }
                        else
                        {
                            List<GGUIWndGuildCooperatePosPrefab> tempList = new List<GGUIWndGuildCooperatePosPrefab>();
                            tempList.Add(_item);
                            _m_dRewardPosPrefabDic[resId] = tempList;
                        }
                        stepCounter.addDoneStepCount();
                    });
                }
            }

        }

        /// <summary>
        /// 回收奖励据点
        /// </summary>
        private void _pushBackRewardPosPrefab()
        {
            foreach (KeyValuePair<long, List<GGUIWndGuildCooperatePosPrefab>> kv in _m_dRewardPosPrefabDic)
            {
                if (kv.Value == null)
                    continue;

                foreach (GGUIWndGuildCooperatePosPrefab item in kv.Value)
                {
                    if (item == null)
                        continue;

                    GGuildCooperatePosPrefabCacheMgr.instance.pushBackCacheItem(kv.Key, item);
                }
            }
            _m_dRewardPosPrefabDic.Clear();
        }

        /// <summary>
        /// 刷新奖励标记
        /// </summary>
        private void _refreshRewardTag()
        {
            if (wnd == null)
                return;

            bool canGetReward = false;
            GRefdataCoreMgr.instance.guildCooperateAreaRefCore.dealAllRef(_ref =>
            {
                if(_ref != null && NPPlayer.instance.guildCooperateComp.getAreaState(_ref.area_id) == EGuildCooperateMapAreaState.UNLOCK_HAVE_REWARD)
                    canGetReward = true;
            });

            ALUGUICommon.setGameObjEnable(wnd.goAreaHaveRewardShowList, canGetReward);
        }

        /// <summary>
        /// 检查是否重置了
        /// </summary>
        private void _checkIsReset()
        {
            // 如果刷新时间不一致说明发生了重置
            if(NPPlayer.instance.guildCooperateComp.recordCurShowRefreshTimeMs != NPPlayer.instance.guildCooperateComp.nextRefreshTimeMs)
                _onGuildCooperateReset();
        }

        #region 引导相关

        /// <summary>
        /// 当前显示区域是否可以建造
        /// </summary>
        /// <returns></returns>
        public bool curShowAreaCanConstruct()
        {
            foreach (KeyValuePair<long, List<GGUIWndGuildCooperatePosPrefab>> kv in _m_dRewardPosPrefabDic)
            {
                if (kv.Value == null)
                    continue;

                foreach (GGUIWndGuildCooperatePosPrefab item in kv.Value)
                {
                    if (item == null)
                        continue;

                    if (item.rewardPointInfo != null && item.rewardPointInfo.getRewardType() == ECommonRewardType.NOT_GET_REWARD)
                        return true;
                }

            }
            return false;
        }

        /// <summary>
        /// 获取可领取奖励据点的位置RectTransform
        /// </summary>
        /// <returns></returns>
        public RectTransform getCanGetRewardPosRectTransform()
        {
            RectTransform targetRect = _getCanGetRewardPosItem()?.rectTransform;
            return targetRect;
        }

        /// <summary>
        /// 获取可建造属性据点的位置RectTransform
        /// </summary>
        /// <returns></returns>
        public RectTransform getCanConstructAttrPosRectTransform()
        {
            RectTransform targetRect = _getCanConstructAttrPosItem()?.rectTransform;
            return targetRect;
        }

        /// <summary>
        /// 获取可领取奖励据点的Prefab
        /// </summary>
        /// <returns></returns>
        private GGUIWndGuildCooperatePosPrefab _getCanGetRewardPosItem()
        {
            GGUIWndGuildCooperatePosPrefab targetItem = null;
            foreach (KeyValuePair<long, List<GGUIWndGuildCooperatePosPrefab>> kv in _m_dRewardPosPrefabDic)
            {
                if (kv.Value == null)
                    continue;

                foreach (GGUIWndGuildCooperatePosPrefab item in kv.Value)
                {
                    if (item == null)
                        continue;

                    if (item.rewardPointInfo != null && item.rewardPointInfo.getRewardType() == ECommonRewardType.CAN_GET_REWARD)
                    {
                        targetItem = item;
                        break;
                    }
                }

                if (targetItem != null)
                    break;
            }

            return targetItem;
        }

        /// <summary>
        /// 获取可建造属性据点item
        /// </summary>
        /// <returns></returns>
        private GGUIWndGuildCooperateAttrPosItem _getCanConstructAttrPosItem()
        {
            bool isUnlock = NPPlayer.instance.guildCooperateComp.getAreaState(_m_lCurShowAreaId) != EGuildCooperateMapAreaState.LOCK;
            if (!isUnlock)
                return null;

            GGUIWndGuildCooperatePosPrefab targetRewardPosItem = null;
            foreach (KeyValuePair<long, List<GGUIWndGuildCooperatePosPrefab>> kv in _m_dRewardPosPrefabDic)
            {
                if (kv.Value == null)
                    continue;

                foreach (GGUIWndGuildCooperatePosPrefab item in kv.Value)
                {
                    if (item == null)
                        continue;

                    if (item.rewardPointInfo != null && item.rewardPointInfo.getRewardType() == ECommonRewardType.NOT_GET_REWARD)
                    {
                        targetRewardPosItem = item;
                        break;
                    }
                }

                if (targetRewardPosItem != null)
                    break;
            }

            if (targetRewardPosItem == null)
                return null;

            return targetRewardPosItem.getCanConstruceAttrPosItem();
        }

        #endregion

        #region 倒计时

        /// <summary>
        /// 开启倒计时
        /// </summary>
        private void _startCD()
        {
            _m_checkTask.setDisable();
            _m_checkTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_tickCD, 0.2f);
        }

        /// <summary>
        /// 关闭倒计时
        /// </summary>
        private void _stopCD()
        {
            _m_checkTask.setDisable();
        }

        /// <summary>
        /// 倒计时显示
        /// </summary>
        private void _tickCD()
        {
            if (wnd == null)
                return;

            //下次刷新时间
            long leftTimeMs = NPPlayer.instance.guildCooperateComp.nextRefreshTimeMs - FpsAndPingMgr.instance.serverTimeTag;
            //重置倒计时：{0}
            ALUGUICommon.setLabelTxt(wnd.txtCD, TextTranslate.instance.getLanguage(TransKeyConst.guildCooperate_resetTimeCountDown_str, TimeUtil.millisecondsToTime_hms(leftTimeMs)));
        }

        #endregion

        #region 消息事件

        /// <summary>
        /// 领取奖励状态变更
        /// </summary>
        private void _onGetRewardChg()
        {
            _refreshRewardTag();
        }

        /// <summary>
        /// 联盟协作重置默认区域显示
        /// </summary>
        private void _onGuildCooperateResetDefaultAreaShow()
        {
            if (_m_lCurShowAreaId == NPPlayer.instance.guildCooperateComp.curAreaId)
                return;

            _m_lCurShowAreaId = NPPlayer.instance.guildCooperateComp.curAreaId;
            _refreshWnd();
        }

        /// <summary>
        /// 模拟点击可领取奖励据点打开详情
        /// </summary>
        private void _simulateClickCanGetRewardPos()
        {
            GGUIWndGuildCooperatePosPrefab targetItem = _getCanGetRewardPosItem();
            targetItem?.simulateClickDetail();
        }

        /// <summary>
        /// 模拟点击可建造属性据点打开详情
        /// </summary>
        private void _simulateClickCanConstructAttrPos()
        {
            GGUIWndGuildCooperateAttrPosItem targetItem = _getCanConstructAttrPosItem();
            targetItem?.simulateClickItem();
        }

        /// <summary>
        /// 公会协作重置
        /// </summary>
        private void _onGuildCooperateReset()
        {
            // 弹窗提示，确认后退出到联盟主界面
            NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.guildCooperate_resetNoticeContent_none), 
                TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                () =>
                {
                    QueueMgr.instance.QuitUntilCanStop(_node => _node.nodeTag == UINodeTagConst_Guild.C_GUILD_MAIN);
                },
                true,
                TransKeyConst.guildCooperate_resetNoticeTitle_none);
        }

        #endregion

        #region 点击事件

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GUIlD_COOPERATE_MAIN);
        }

        /// <summary>
        /// 点击排行按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickRank(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGuildCooperateRank.instance, GGUIWndGuildCooperateRank.instance.showWnd, UINodeTagConst.C_GUIlD_COOPERATE_RANK);
        }

        /// <summary>
        /// 点击日志按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickLog(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGuildCooperateLog.instance, GGUIWndGuildCooperateLog.instance.showWnd, UINodeTagConst.C_GUIlD_COOPERATE_LOG);
        }

        /// <summary>
        /// 点击地图按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickMap(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGuildCooperateMap.instance, () =>
            {
                GGUIWndGuildCooperateMap.instance.showWnd();
                GGUIWndGuildCooperateMap.instance.setInfo(_areaId =>
                {
                    if (_m_lCurShowAreaId != _areaId)
                    {
                        _m_lCurShowAreaId = _areaId;
                        _refreshWnd();
                    }
                });
            }, UINodeTagConst.C_GUIlD_COOPERATE_MAP);
        }

        /// <summary>
        /// 点击上一个区域按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickPrevious(GameObject _go)
        {
            if(_m_lCurShowAreaId - 1 > 0)
            {
                _m_lCurShowAreaId--;
                _refreshWnd();
            }
        }

        /// <summary>
        /// 点击下一个区域按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickNext(GameObject _go)
        {
            if (GRefdataCoreMgr.instance.guildCooperateAreaRefCore.getRef(_m_lCurShowAreaId + 1) != null)
            {
                _m_lCurShowAreaId++;
                _refreshWnd();
            }
        }

        #endregion
    }
}