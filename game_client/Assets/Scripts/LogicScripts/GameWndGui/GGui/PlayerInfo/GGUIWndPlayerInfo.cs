using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;
using NPEnum;
using CommonEnum;

namespace GOE
{
    // 玩家自己的信息
    public class GGUIWndPlayerInfo : _ANPGGUIBasicResBarWnd<GGUIMonoPlayerInfo>
    {

        private static GGUIWndPlayerInfo _g_instance;
        public static GGUIWndPlayerInfo instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndPlayerInfo();
                return _g_instance;
            }
        }

        protected GGUIWndPlayerInfo()
           : base(EALUIWndLayer.NORMAL)
        {
        }

        /********************
       * 获取资源所在资源加载文件名称
       **/
        protected override string _monoAssetPath { get { return GGUIMonoPlayerInfo.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoPlayerInfo.objName; } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        /// <summary>
        /// 玩家形象
        /// </summary>
        private NPGGUIWndCommonShowCase _m_playerShowcase;

        /// <summary>
        /// 玩家基础信息
        /// </summary>
        private NPGGUIWndPlayerIcon _m_playerInfoWnd;

        //经验条
        private NPGGUIWndProgress _m_pExpProgress;
        //赚速条
        private NPGGUIWndProgress _m_pEarningsProgress;
        
        //每日奖励展示
        private GGUIWndPlayerInfo_DailyRewardShow _m_dailyRewardShow;
        //待加入顾问头像
        private NPGGuiWndTexture _m_wJoinHeroIcon;

        //是否正在处理升级
        private bool _m_bIsDealingUpgrade;

        private int _m_showSerialize;

        /// <summary>
        /// 是否正在处理升级
        /// </summary>
        public bool isDealingUpgrade { get { return _m_bIsDealingUpgrade; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (null != wnd.playerShowcase)
                _m_playerShowcase = new NPGGUIWndCommonShowCase(wnd.playerShowcase);
            if (null != wnd.playerInfoMono)
                _m_playerInfoWnd = new NPGGUIWndPlayerIcon(wnd.playerInfoMono);
            if (null != wnd.expProgress)
                _m_pExpProgress = new NPGGUIWndProgress(wnd.expProgress);
            if (null != wnd.earningsProgress)
                _m_pEarningsProgress = new NPGGUIWndProgress(wnd.earningsProgress);
            if (null != wnd.dailyRewardShow)
                _m_dailyRewardShow = new GGUIWndPlayerInfo_DailyRewardShow(wnd.dailyRewardShow);
            if (null != wnd.imgGainHeroIcon)
                _m_wJoinHeroIcon = new NPGGuiWndTexture(wnd.imgGainHeroIcon);

            //绑定
            ALUGUICommon.combineBtnClick(wnd.chgNameBtn, _onClickRenameBtn);
            ALUGUICommon.combineBtnClick(wnd.copyBtn, _onClickCopyBtn);
            ALUGUICommon.combineBtnClick(wnd.upLvlBtn, _onClickUpLvlBtn);
            ALUGUICommon.combineBtnClick(wnd.btnSkin, _onClickSkin);
            ALUGUICommon.combineBtnClick(wnd.titleBtn, _onClickTitleBtn);
            ALUGUICommon.combineBtnClick(wnd.iconBtn, _onClickIconBtn);
            ALUGUICommon.combineBtnClick(wnd.settingBtn, _onClickSettingBtn);
            ALUGUICommon.combineBtnClick(wnd.heroGainBtn, _onClickHeroGainBtn);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnVIP, _onClickVIPBtn);
        }

        protected override void _onShowWnd()
        {
            _m_dailyRewardShow?.showWnd();
            _m_wJoinHeroIcon?.showWnd();
            
            _refresh();
            WinMsg.RegisterMsgAct(WinMsgType.ON_PLAYER_PARAM_CHANGE, _playerParamChange);
            WinMsg.RegisterMsgAct(WinMsgType.ON_PLAYER_NAME_CHANGE, _playerParamChange);
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _playerResChange);
            WinMsg.RegisterMsg(WinMsgType.ON_CURRENT_TITLE_CHG, _onCurrentTitleChg);
            
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_PLAYER_LVL_UP, _onSimulateClickLvlUp);
        }

        protected override void _onHideWnd()
        {
            _m_bIsDealingUpgrade = false;
            _m_showSerialize = ALSerializeOpMgr.next();
            _m_dailyRewardShow?.hideWnd();
            _m_wJoinHeroIcon?.hideWnd();
            
            WinMsg.UnregisterMsgAct(WinMsgType.ON_PLAYER_PARAM_CHANGE, _playerParamChange);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_PLAYER_NAME_CHANGE, _playerParamChange);
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _playerResChange);
            WinMsg.UnregisterMsg(WinMsgType.ON_CURRENT_TITLE_CHG, _onCurrentTitleChg);

            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_PLAYER_LVL_UP, _onSimulateClickLvlUp);
            
            if (null != _m_playerShowcase)
                _m_playerShowcase.hideWnd();
        }

        protected override void _onReset()
        {
            _m_dailyRewardShow?.resetWnd();
            _m_wJoinHeroIcon?.discardTexture();
        }
        protected override void _onDiscard()
        {
            if (null != _m_playerShowcase)
                _m_playerShowcase.discard();
            _m_playerShowcase = null;

            if (null != _m_playerInfoWnd)
                _m_playerInfoWnd.discard();
            _m_playerInfoWnd = null;

            if(null != _m_pExpProgress)
                _m_pExpProgress.discard();
            _m_pExpProgress = null;

            if(null != _m_pEarningsProgress)
                _m_pEarningsProgress.discard();
            _m_pEarningsProgress = null;

            if(null != _m_wJoinHeroIcon)
                _m_wJoinHeroIcon.discard();
            _m_wJoinHeroIcon = null;
            
            _m_dailyRewardShow?.discard();
            _m_dailyRewardShow = null;

            if (wnd == null)
                return;

            //解绑
            ALUGUICommon.uncombineBtnClick(wnd.chgNameBtn, _onClickRenameBtn);
            ALUGUICommon.uncombineBtnClick(wnd.copyBtn, _onClickCopyBtn);
            ALUGUICommon.uncombineBtnClick(wnd.upLvlBtn, _onClickUpLvlBtn);
            ALUGUICommon.uncombineBtnClick(wnd.btnSkin, _onClickSkin);
            ALUGUICommon.uncombineBtnClick(wnd.titleBtn, _onClickTitleBtn);
            ALUGUICommon.uncombineBtnClick(wnd.iconBtn, _onClickIconBtn);
            ALUGUICommon.uncombineBtnClick(wnd.settingBtn, _onClickSettingBtn);
            ALUGUICommon.uncombineBtnClick(wnd.heroGainBtn, _onClickHeroGainBtn);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnVIP, _onClickVIPBtn);
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        private void _refresh()
        {
            if (wnd == null)
                return;

            _m_dailyRewardShow?.refreshWnd();

            //刷新可升级状态
            _refreshUpgradeState();
            //刷新玩家头像
            _refreshPlayerIcon();
            //刷新玩家形象
            _refreshShowcase();
            //刷新经验相关显示
            _refreshExp();
            //刷新待加入顾问头像
            _refreshGainHeroIcon();
        }

        /// <summary>
        /// 刷新可升级状态
        /// </summary>
        private void _refreshUpgradeState()
        {
            if (wnd == null)
                return;

            wnd.setUpgradable(NPPlayer.instance.playerInfo.canUpgrade());
        }

        /// <summary>
        /// 刷新玩家头像
        /// </summary>
        private void _refreshPlayerIcon()
        {
            if (null != _m_playerInfoWnd)
                _m_playerInfoWnd.setSelfInfo();
        }

        /// <summary>
        /// 刷新玩家形象
        /// </summary>
        private void _refreshShowcase()
        {
            if (_m_playerShowcase == null)
                return;

            NPGGoIndex resIndex = NPPlayer.instance?.playerInfo?.curSkinRef?.td_show;
            if (resIndex == null)
                return;

            _m_playerShowcase.showWnd(new ShowCaseCommonResUnitInfoObj(resIndex));
        }

        /// <summary>
        /// 刷新经验相关显示
        /// </summary>
        private void _refreshExp()
        {
            if (wnd == null)
                return;

            PlayerLvlRefObj nextLvlRefObj = NPPlayer.instance.playerInfo.nextLevelRef;
            //设置玩家经验条
            if (null != _m_pExpProgress)
            {
                long showExp = NPPlayer.instance.rescourceComp.getValue(ECurrency.P_EXP);
                if (showExp < 0)
                    showExp = 0;

                //是否满级
                if (nextLvlRefObj != null)
                    _m_pExpProgress.setProgress(showExp, nextLvlRefObj.exp, EValueFormatType.NORMAL_NOT_LARGE_STR);
                else
                {
                    _m_pExpProgress.setProgress(showExp, showExp, EValueFormatType.NORMAL_NOT_LARGE_STR);
                    _m_pExpProgress.setProgressTxt(TextTranslate.instance.getLanguage(TransKeyConst.common_max_none), true);
                }
            }
            //设置玩家赚速条
            if (null != _m_pEarningsProgress)
            {
                //是否满级
                if (nextLvlRefObj != null)
                    _m_pEarningsProgress.setProgress(NPPlayer.instance.specialItemComp.goldData.earnings, nextLvlRefObj.earnings, EValueFormatType.GOLD, null);
                else
                {
                    _m_pEarningsProgress.setProgress(NPPlayer.instance.specialItemComp.goldData.earnings, NPPlayer.instance.specialItemComp.goldData.earnings, EValueFormatType.GOLD, null);
                    _m_pEarningsProgress.setProgressTxt(TextTranslate.instance.getLanguage(TransKeyConst.common_max_none), true);
                }
            }

            //满级显隐
            ALUGUICommon.setGameObjEnable(wnd.goMaxLevelHideList, nextLvlRefObj != null);
            ALUGUICommon.setGameObjEnable(wnd.goMaxLevelShowList, nextLvlRefObj == null);
        }

        /// <summary>
        /// 刷新待加入顾问头像
        /// </summary>
        private void _refreshGainHeroIcon()
        {
            if (wnd == null)
                return;

            List<PlayerHeroUnlockShowRefObj> heroRefList = GRefdataCoreMgr.instance.getPlayerHeroUnlockShowRefListBySort();
            if (heroRefList == null || heroRefList.Count == 0 || heroRefList[0] == null)
                return;

            PlayerHeroUnlockShowRefObj targetRef = heroRefList[0];
            _m_wJoinHeroIcon?.setTexture(GCommon.getItemTexIcon(ENPItemType.HERO,targetRef.hero_id));

            //判断是否全部获取了，全部获取时隐藏入口
            bool isAllGain = true;
            for (int i = 0; i < heroRefList.Count; i++)
            {
                if (heroRefList[i] == null)
                    continue;

                if (!NPPlayer.instance.heroComponent.isHeroUnlock(heroRefList[i].hero_id))
                {
                    isAllGain = false;
                    break;
                }
            }
            ALUGUICommon.setGameObjEnable(wnd.heroGainBtn, !isAllGain);
        }

        #region 点击事件

        /// <summary>
        /// 点击改名
        /// </summary>
        /// <param name="_btn"></param>
        private void _onClickRenameBtn(GameObject _btn)
        {
            GCommon.showPlayerCommonRename();
        }

        /// <summary>
        /// 点击复制cid
        /// </summary>
        /// <param name="_btn"></param>
        private void _onClickCopyBtn(GameObject _btn)
        {
            GUIUtility.systemCopyBuffer = NPPlayer.instance.playerInfo.CID.ToString();
            NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_copySuc_str));
        }
        
        private void _onSimulateClickLvlUp(object[] _params)
        {
            _onClickUpLvlBtn(null);
        }
        
        /// <summary>
        /// 升级按钮
        /// </summary>
        /// <param name="_btn"></param>
        private void _onClickUpLvlBtn(GameObject _btn)
        {
            //判断是否满级
            if (NPPlayer.instance.playerInfo.nextLevelRef == null)
                return;

            //判断本地条件是否匹配，如不满足则直接弹出提示
            if (!GCommon.isItemEnough(ENPItemType.CURRENCY, (int)ECurrency.P_EXP, NPPlayer.instance.playerInfo.nextLevelRef.exp, true))
                return;
            
            //赚速不足直接弹出自定义获取途径弹窗
            if (NPPlayer.instance.specialItemComp.goldData.earnings < NPPlayer.instance.playerInfo.nextLevelRef.earnings)
            {
                NPGGUIWndEffectCustomAddUI effectCustomAddUIWnd = new NPGGUIWndEffectCustomAddUI(2402);
                QueueMgr.instance.addNode_InGame_SingleWnd(effectCustomAddUIWnd, effectCustomAddUIWnd.showWnd, EUIQueueStageType.MAIN, UINodeTagConst.C_EFFECT_CUSTOM_ADD_UI, false, true);
                return;
            }

            _m_bIsDealingUpgrade = true;
            //设置引导状态避免中途弹出引导
            Game.instance.openIsInTutorial(IsInTutorialConst.PLAYER_UPGRADE);
            int serialize = MainCameraMono.selfInstance.openAllInputMask();
            int wndSerialize = _m_showSerialize;
            NPPlayer.instance.playerInfoComp.reqPlayerLvlUp((_isSuc, _lvl) =>
            {
                if (wndSerialize != _m_showSerialize)
                {
                    Game.instance.closeIsInTutorial(IsInTutorialConst.PLAYER_UPGRADE);
                    MainCameraMono.selfInstance.closeAllInputMask(serialize);
                    return;
                }

                if (!_isSuc)
                {
                    Game.instance.closeIsInTutorial(IsInTutorialConst.PLAYER_UPGRADE);
                    MainCameraMono.selfInstance.closeAllInputMask(serialize);
                    return;
                }

                PlayerLvlRefObj playerLvlRefObj = GRefdataCoreMgr.instance.playerLvlCore.getRef(_lvl);

                //升级表现流程
                ALProcess process = ALProcess.CreateProcess();
                process
                .addDelegateProcess(_onDone =>
                {
                    //====播放特效====
                    if (wnd == null || wnd.upgradeSfxParent == null)
                    {
                        _onDone?.Invoke();
                        return;
                    }
                    CommonUISfxObj sfxObj = PlaySfxMgr.instance.playUISfx(wnd.upgradeSfxRefId, wnd.upgradeSfxParent);
                    if (sfxObj?.sfxRef is { duration: > 0 })
                    {
                        sfxObj.regPlayCompleteDelegate(_onDone);
                    }
                    else
                    {
                        sfxObj?.forceDiscard();
                        _onDone?.Invoke();
                    }
                })
                .addProcess(() =>
                {
                    //====尝试触发礼包====
                    MainCameraMono.selfInstance.closeAllInputMask(serialize);
                    if (null == playerLvlRefObj)
                        return;

                    //玩家等级提升时, 尝试触发等级提升推送礼包
                    NPPlayer.instance.pushGiftComp.tryTriggerPushGiftPack(playerLvlRefObj.trigger_push_gift_group_id, false);
                })
                .addDelegateProcess(_onDone =>
                {
                    //====打开升级成功界面====
                    if (null == playerLvlRefObj)
                    {
                        _onDone?.Invoke();
                        return;
                    }

                    if (playerLvlRefObj.ignore_pop_wnd)
                        _onDone?.Invoke();
                    else
                    {
                        QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndPlayerLvlUpSuc.instance, () =>
                        {
                            GGUIWndPlayerLvlUpSuc.instance.showWnd();
                            GGUIWndPlayerLvlUpSuc.instance.setData(playerLvlRefObj, _onDone);
                        }, UINodeTagConst.C_PLAYER_LVL_UP_SUC);
                    }
                })
                .addDelegateProcess(_onDone =>
                {
                    //====打开奖励展示界面====
                    if (null == playerLvlRefObj)
                    {
                        Game.instance.closeIsInTutorial(IsInTutorialConst.PLAYER_UPGRADE);
                        _onDone?.Invoke();
                        return;
                    }

                    if (playerLvlRefObj.show_reward_item_list != null && playerLvlRefObj.show_reward_item_list.Count > 0)
                    {
                        GCommon.dealGainItem(playerLvlRefObj.show_reward_item_list.toRewardItemDataList(), TransKeyConst.common_getreward_tip, () =>
                        {
                            _m_bIsDealingUpgrade = false;
                            _onDone?.Invoke();
                        });

                        //由于奖励弹窗下一帧才弹出，这里下一帧再关闭引导状态
                        ALCommonActionMonoTask.addNextFrameTask(() =>
                        {
                            Game.instance.closeIsInTutorial(IsInTutorialConst.PLAYER_UPGRADE);
                        });
                    }
                    else
                    {
                        Game.instance.closeIsInTutorial(IsInTutorialConst.PLAYER_UPGRADE);
                        _m_bIsDealingUpgrade = false;
                        _onDone?.Invoke();
                    }
                })
                .deal();
            });
        }

        /// <summary>
        /// 点击皮肤按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickSkin(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndPlayerSkin.instance, UINodeTagConst_PlayerInfo.C_MAIN_PLAYERINFO_SKIN_NODE, 0);
        }

        /// <summary>
        /// 点击称号
        /// </summary>
        /// <param name="_btn"></param>
        private void _onClickTitleBtn(GameObject _btn)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndPlayerTitleMain.instance, GGUIWndPlayerTitleMain.instance.showWnd, UINodeTagConst_PlayerInfo.C_MAIN_PLAYERINFO_TITLE_NODE);
        }

        /// <summary>
        /// 点击头像
        /// </summary>
        /// <param name="_btn"></param>
        private void _onClickIconBtn(GameObject _btn)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndPlayerInfoDress.instance, GGUIWndPlayerInfoDress.instance.showWnd, UINodeTagConst_PlayerInfo.C_MAIN_PLAYERINFO_DRESS_NODE);
        }

        /// <summary>
        /// 点击设置
        /// </summary>
        /// <param name="_btn"></param>
        private void _onClickSettingBtn(GameObject _btn)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(NPGGUIWndGameSetting.instance, NPGGUIWndGameSetting.instance.showWnd, UINodeTagConst.C_SETTING_MAIN);
        }

        /// <summary>
        /// 打开获得大臣的界面
        /// </summary>
        private void _onClickHeroGainBtn(GameObject _btn)
        {
            QueueMgr.instance.AddNode(new GNodePlayerHeroGain());
        }

        /// <summary>
        /// 打开vip预览界面
        /// </summary>
        /// <param name="_btn"></param>
        private void _onClickVIPBtn(GameObject _btn)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndVIPLevelPreview.instance, GGUIWndVIPLevelPreview.instance.showWnd, UINodeTagConst.C_VIP_LEVEL_PREVIEW);
        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_btn"></param>
        private void _onCloseBtnClick(GameObject _btn) 
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_Main_PlayerInfoNode);
        }

        #endregion

        #region 消息事件

        /// <summary>
        /// 玩家信息变动
        /// </summary>
        private void _playerParamChange()
        {
            _refresh();
        }

        /// <summary>
        /// currency变动
        /// </summary>
        private void _playerResChange(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 3)
                return;

            ECurrency currencyType = (ECurrency)_objects[0];
            long oriValue = (long)_objects[1];
            long curValue = (long)_objects[2];

            if (currencyType == ECurrency.P_EXP)
            {
                _refreshUpgradeState();
                _refreshExp();
            }
        }

        /// <summary>
        /// 当前称号变化
        /// </summary>
        /// <param name="_objects"></param>
        private void _onCurrentTitleChg(params object[] _objects)
        {
            _refreshPlayerIcon();
        }

        #endregion
    }
}
