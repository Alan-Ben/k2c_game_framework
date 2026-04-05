using System;
using System.Collections.Generic;
using ALPackage;
using Common.TreasureHuntEnum;
using Common.TreasureHuntObj;
using GS2GC.p036_TreasureHuntOp;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 太空寻宝游戏主界面
    /// </summary>
    public class GGUIWndTreasureHuntGameMain : _ANPGGUIBasicResBarWnd<GGUIMonoTreasureHuntGameMain>
    {
        private static GGUIWndTreasureHuntGameMain _g_instance;
        public static GGUIWndTreasureHuntGameMain instance { get { return _g_instance ??= new GGUIWndTreasureHuntGameMain(); } }

        public const int oncePlayCostEnergyCount = 1;//单次游玩消耗能量
        
        private GGUISubWndTreasureHuntStationLevel _m_wStationLevel;
        private NPGGUIWndCommonItem _m_wUsingEnergy;
        private NPGGUIWndCommonItem _m_wExchangeEnergy;
        private NPGGUIWndCommonTab _m_wAkeyOncePlayToggle;
        private NPGGUIWndCommonTab _m_wAkeyMultiPlayToggle;
        private GGUIWndTreasureHuntSpecimenRoom _m_wMaterialsRoom;// 材料室 

        private bool _m_bIsUsingAdvancedEnergy = false; // 是否正在使用高级能源道具
        // 普通能源道具
        [NotNull] private NPCommonCostItem _m_iNormalEnergy = new NPCommonCostItem(GRefdataCoreMgr.instance.npGeneral.treasure_hunt_premium_energy_common_item, oncePlayCostEnergyCount);
        // 高级能源道具
        [NotNull] private NPCommonCostItem _m_iAdvancedEnergy = new NPCommonCostItem(GRefdataCoreMgr.instance.npGeneral.treasure_hunt_advanced_energy_common_item, oncePlayCostEnergyCount);
        
        // // 当前使用的能源道具, 默认普通能源道具, 单次消耗
        // [NotNull] private NPCommonCostItem _m_iUsingEnergy = new NPCommonCostItem(GRefdataCoreMgr.instance.npGeneral.treasure_hunt_premium_energy_common_item, oncePlayCostEnergyCount);

        private long _m_lShowSerialId = 0; // 窗口显示的序列号
        
        public GGUIWndTreasureHuntGameMain() : base(EALUIWndLayer.NORMAL)
        {
        }
        
        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntGameMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntGameMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoStationLevel != null)
                _m_wStationLevel = new GGUISubWndTreasureHuntStationLevel(wnd.monoStationLevel);

            if (wnd.monoUsingEnergy != null)
                _m_wUsingEnergy = new NPGGUIWndCommonItem(wnd.monoUsingEnergy);

            if (wnd.monoExchangeEnergy != null)
                _m_wExchangeEnergy = new NPGGUIWndCommonItem(wnd.monoExchangeEnergy);

            if (wnd.monoAkeyOncePlayToggle != null)
            {
                _m_wAkeyOncePlayToggle = new NPGGUIWndCommonTab(wnd.monoAkeyOncePlayToggle);
                _m_wAkeyOncePlayToggle.clickDelegate += _onClickAkeyOncePlayToggle;
                _m_wAkeyOncePlayToggle.onUnableClickDelegate += _onClickUnableAkeyOncePlayToggle;
            }

            if (wnd.monoAkeyMultiPlayToggle != null)
            {
                _m_wAkeyMultiPlayToggle = new NPGGUIWndCommonTab(wnd.monoAkeyMultiPlayToggle);
                _m_wAkeyMultiPlayToggle.clickDelegate += _onClickAkeyMultiPlayToggle;
                _m_wAkeyMultiPlayToggle.onUnableClickDelegate += _onClickUnableAkeyMultiPlayToggle;
            }

            _m_wMaterialsRoom = new GGUIWndTreasureHuntSpecimenRoom(6805);
            
            ALUGUICommon.combineBtnClick(wnd.btnChgArea, _onClickChgArea);
            ALUGUICommon.combineBtnClick(wnd.btnChgEnergy, _onClickChgEnergy);
            ALUGUICommon.combineBtnClick(wnd.btnPlay, _onClickPlay);
            ALUGUICommon.combineBtnClick(wnd.btnMaterialsRoom, _onClickMaterialsRoom);
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onClickReturn);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnChgArea, _onClickChgArea);
                ALUGUICommon.uncombineBtnClick(wnd.btnChgEnergy, _onClickChgEnergy);
                ALUGUICommon.uncombineBtnClick(wnd.btnPlay, _onClickPlay);
                ALUGUICommon.uncombineBtnClick(wnd.btnMaterialsRoom, _onClickMaterialsRoom);
                ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _onClickReturn);
            }

            _m_wStationLevel?.discard();
            _m_wStationLevel = null;

            _m_wUsingEnergy?.discard();
            _m_wUsingEnergy = null;

            _m_wExchangeEnergy?.discard();
            _m_wExchangeEnergy = null;

            if (_m_wAkeyOncePlayToggle != null)
            {
                _m_wAkeyOncePlayToggle.clickDelegate -= _onClickAkeyOncePlayToggle;
                _m_wAkeyOncePlayToggle.onUnableClickDelegate -= _onClickUnableAkeyOncePlayToggle;
                _m_wAkeyOncePlayToggle.discard();
                _m_wAkeyOncePlayToggle = null;                
            }

            if (_m_wAkeyMultiPlayToggle != null)
            {
                _m_wAkeyMultiPlayToggle.clickDelegate -= _onClickAkeyMultiPlayToggle;
                _m_wAkeyMultiPlayToggle.onUnableClickDelegate -= _onClickUnableAkeyMultiPlayToggle;
                _m_wAkeyMultiPlayToggle.discard();
                _m_wAkeyMultiPlayToggle = null;       
            }
            
            _m_wMaterialsRoom?.discard();
            _m_wMaterialsRoom = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
            
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ITEM_COUNT_CHG, _onCommonItemCountChg);
            WinMsg.RegisterMsg(WinMsgType.ON_TREASURE_HUNT_STATION_LVL_CHG, _onStationLevelChg);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_TREASURE_HUNT_GAME_PLAY, _onBtnPlay);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_TREASURE_HUNT_GAME_CHANGE_AREA, _onBtnChgArea);

        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialId = ALSerializeOpMgr.next();
            
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ITEM_COUNT_CHG, _onCommonItemCountChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_TREASURE_HUNT_STATION_LVL_CHG, _onStationLevelChg);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_TREASURE_HUNT_GAME_PLAY, _onBtnPlay);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_TREASURE_HUNT_GAME_CHANGE_AREA, _onBtnChgArea);

            _m_wStationLevel?.hideWnd();
            _m_wUsingEnergy?.hideWnd();
            _m_wExchangeEnergy?.hideWnd();
            _m_wAkeyOncePlayToggle?.hideWnd();
            _m_wAkeyMultiPlayToggle?.hideWnd();
            _m_wMaterialsRoom?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wStationLevel?.resetWnd();
            _m_wUsingEnergy?.resetWnd();
            _m_wExchangeEnergy?.resetWnd();
            _m_wAkeyOncePlayToggle?.resetWnd();
            _m_wAkeyMultiPlayToggle?.resetWnd();
            _m_wMaterialsRoom?.resetWnd();
        }
        
        private TreasureHuntGameController _m_gameController;
        
        public void setGameController(TreasureHuntGameController _gameController)
        {
            _m_gameController = _gameController;
        }

        /// <summary>
        /// 刷新游玩模式toggle
        /// </summary>
        private void _refreshPlayModeToggle()
        {
            ETreasureHuntGamePlayMode playMode = NPPlayer.instance.treasureHuntComponent.saver?.getSelectedGamePlayMode() ?? ETreasureHuntGamePlayMode.NORMAL;
            // 一键单次游玩是否开启
            bool akeyOncePlayUnlock = GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.treasure_hunt_instant_pickup_simple_unlock_id);
            // 一键多次游玩是否开启
            bool akeyMultiPlayUnlock = GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.treasure_hunt_akey_pickup_simple_unlock_id);

            if ((playMode == ETreasureHuntGamePlayMode.AKEY_ONCE_PLAY && !akeyOncePlayUnlock) 
                || (playMode == ETreasureHuntGamePlayMode.AKEY_MULTI_PLAY && !akeyMultiPlayUnlock))
            {
                playMode = ETreasureHuntGamePlayMode.NORMAL;
                NPPlayer.instance.treasureHuntComponent.saver?.setSelectedGamePlayMode(playMode);
            }

            if (_m_wAkeyOncePlayToggle != null)
            {
                _m_wAkeyOncePlayToggle.showWnd();
                _m_wAkeyOncePlayToggle.setEnable(akeyOncePlayUnlock);
                _m_wAkeyOncePlayToggle.setSelected(playMode == ETreasureHuntGamePlayMode.AKEY_ONCE_PLAY);
            }

            if (_m_wAkeyMultiPlayToggle != null)
            {
                _m_wAkeyMultiPlayToggle.showWnd();
                _m_wAkeyMultiPlayToggle.setEnable(akeyMultiPlayUnlock);
                _m_wAkeyMultiPlayToggle.setSelected(playMode == ETreasureHuntGamePlayMode.AKEY_MULTI_PLAY);
            }
            
            // 游戏模式更新后需要刷新下使用的能源道具显示
            _refreshUsingEnergyShow();
        }
        
        private void _refreshWnd()
        {
            if(wnd == null || !isShow)
                return;

            // 刷新太空舱等级
            if (_m_wStationLevel != null)
            {
                _m_wStationLevel.showWnd();
                _m_wStationLevel.setData(NPPlayer.instance.treasureHuntComponent.stationInfo);
            }

            TreasureHuntAreaRefObj areaRef = GRefdataCoreMgr.instance.treasureHuntAreaRefCore.getRef(NPPlayer.instance.treasureHuntComponent.saver.getAreaId());
            ALUGUICommon.setLabelTxt(wnd.txtNowAreaName, TextTranslate.instance.getLanguage(areaRef?.name));
            
            // 刷新游玩模式toggle
            _refreshPlayModeToggle();
        }

        /// <summary>
        /// 刷新使用的能源道具显示
        /// </summary>
        private void _refreshUsingEnergyShow()
        {
            if(wnd == null)
                return;
            
            if (NPPlayer.instance.treasureHuntComponent.saver?.getSelectedGamePlayMode() == ETreasureHuntGamePlayMode.AKEY_MULTI_PLAY)
            {
                // long costEnergyCount = GCommon.getItemCount(_m_iUsingEnergy.item);
                // costEnergyCount = Math.Min(costEnergyCount, GRefdataCoreMgr.instance.npGeneral.treasure_hunt_akey_consume_energy_limit);
                // _m_iUsingEnergy.count = costEnergyCount;
                
                // 刷新使用能源道具数量
                long costEnergyCount = GCommon.getItemCount(_m_iNormalEnergy.item);
                costEnergyCount = Math.Min(costEnergyCount, GRefdataCoreMgr.instance.npGeneral.treasure_hunt_akey_consume_energy_limit);
                _m_iNormalEnergy.count = costEnergyCount;
                
                costEnergyCount = GCommon.getItemCount(_m_iAdvancedEnergy.item);
                costEnergyCount = Math.Min(costEnergyCount, GRefdataCoreMgr.instance.npGeneral.treasure_hunt_akey_consume_energy_limit);
                _m_iAdvancedEnergy.count = costEnergyCount;
            }
            else
            {
                // _m_iUsingEnergy.count = oncePlayCostEnergyCount;

                _m_iNormalEnergy.count = oncePlayCostEnergyCount;
                _m_iAdvancedEnergy.count = oncePlayCostEnergyCount;
            }
            
            NPCommonCostItem usingEnergy = _m_bIsUsingAdvancedEnergy ? _m_iAdvancedEnergy : _m_iNormalEnergy;
            NPCommonCostItem exchangeEnergy = _m_bIsUsingAdvancedEnergy ? _m_iNormalEnergy : _m_iAdvancedEnergy;
            
            if (_m_wUsingEnergy != null)
            {
                _m_wUsingEnergy.showWnd();
                _m_wUsingEnergy.setItem(usingEnergy);
                
                ALUGUICommon.setGameObjEnable(wnd.hasUsingEnergyShowGo, GCommon.getItemCount(usingEnergy.item) > 0);
            }
            if (_m_wExchangeEnergy != null)
            {
                _m_wExchangeEnergy.showWnd();
                _m_wExchangeEnergy.setItem(exchangeEnergy);
                
                ALUGUICommon.setGameObjEnable(wnd.hasExchangeEnergyShowGo, GCommon.getItemCount(exchangeEnergy.item) > 0);
            }
        }
        
        /// <summary>
        /// 变更当前使用的能源道具
        /// </summary>
        /// <param name="_item"></param>
        private void _chgUsingEnergy(NPCommonItem _item)
        {
            // // 如果传入的道具和当前使用的道具相同，则不做任何处理
            // if(_m_iUsingEnergy.item == _item)
            //     return;
            //
            // // 只需要变化使用的道具, 不需要变化使用的道具数量
            // _m_iUsingEnergy.item = _item;
            // _refreshUsingEnergyShow();
        }

        #region toggle事件

        /// <summary>
        /// 点击一键单次游玩toggle
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickAkeyOncePlayToggle(bool _isOn)
        {
            if(_isOn)
                NPPlayer.instance.treasureHuntComponent.saver?.setSelectedGamePlayMode(ETreasureHuntGamePlayMode.AKEY_ONCE_PLAY);
            else
                NPPlayer.instance.treasureHuntComponent.saver?.setSelectedGamePlayMode(ETreasureHuntGamePlayMode.NORMAL);
            
            _refreshPlayModeToggle();
        }

        /// <summary>
        /// 点击不可用的一键单次游玩toggle
        /// </summary>
        private void _onClickUnableAkeyOncePlayToggle()
        {
            long simpleUnlockId = GRefdataCoreMgr.instance.npGeneral.treasure_hunt_instant_pickup_simple_unlock_id;
            if (GCommon.isSimpleUnlock(simpleUnlockId, true))
            {
                Debug.LogError($"[GGUIWndTreasureHuntGameMain _onClickUnableAkeyOncePlayToggle] 一键单次游玩toggle不可用，但general表配置的解锁条件 treasure_hunt_instant_pickup_simple_unlock_id:{simpleUnlockId} 可通过, 请检查");
            }
        }

        /// <summary>
        /// 点击一键多次游玩toggle
        /// </summary>
        /// <param name="_isOn"></param>
        private void _onClickAkeyMultiPlayToggle(bool _isOn)
        {
            if(_isOn)
                NPPlayer.instance.treasureHuntComponent.saver?.setSelectedGamePlayMode(ETreasureHuntGamePlayMode.AKEY_MULTI_PLAY);
            else
                NPPlayer.instance.treasureHuntComponent.saver?.setSelectedGamePlayMode(ETreasureHuntGamePlayMode.NORMAL);
            
            _refreshPlayModeToggle();
        }

        // 点击不可用的一键多次游玩toggle
        private void _onClickUnableAkeyMultiPlayToggle()
        {
            long simpleUnlockId = GRefdataCoreMgr.instance.npGeneral.treasure_hunt_akey_pickup_simple_unlock_id;
            if (GCommon.isSimpleUnlock(simpleUnlockId, true))
            {
                Debug.LogError($"[GGUIWndTreasureHuntGameMain _onClickUnableAkeyOncePlayToggle] 一键单次游玩toggle不可用，但general表配置的解锁条件 treasure_hunt_akey_pickup_simple_unlock_id:{simpleUnlockId} 可通过, 请检查");
            }
        }
        
        #endregion
        
        #region 按钮事件

        private void _onBtnChgArea()
        {
            if (wnd != null) _onClickChgArea(wnd.btnChgArea);
        }
        /// <summary>
        /// 点击切换太空区域按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickChgArea(GameObject _go)
        {
            QueueMgr.instance.AddNode(new GNodeTreasureHuntSelectArea());
        }

        /// <summary>
        /// 点击切换能源按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickChgEnergy(GameObject _go)
        {
            // GGUIWndTreasureHuntSelectEnergy.instance.setData(_m_iUsingEnergy.item, _m_iUsingEnergy.item, _chgUsingEnergy);
            // QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndTreasureHuntSelectEnergy.instance, () =>
            // {
            //     GGUIWndTreasureHuntSelectEnergy.instance.showWnd();
            // }, UINodeTagConst.C_TREASURE_HUNT_SELECT_ENERGY);
            _m_bIsUsingAdvancedEnergy = !_m_bIsUsingAdvancedEnergy;
            _refreshUsingEnergyShow();
        }

        private void _onBtnPlay()
        {
            if (wnd != null) 
                _onClickPlay(wnd.btnPlay);
        }
        /// <summary>
        /// 点击开始游戏按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickPlay(GameObject _go)
        {
            NPCommonCostItem usingEnergy = _m_bIsUsingAdvancedEnergy ? _m_iAdvancedEnergy : _m_iNormalEnergy;
            
            // 检查当前使用的能源道具是否足够
            if(!GCommon.isItemEnough(usingEnergy ,true))
                return;
            
            // 检查待处理矿石是否超过上限
            if (NPPlayer.instance.treasureHuntComponent.getPendingOreCount() >= GRefdataCoreMgr.instance.npGeneral.treasure_hunt_pending_ore_num_limit)
            {
                // 弹出通用上浮提示
                NPGUIAddSceneCenterTip.instance.showTextTip(TextTranslate.instance.getLanguage(TransKeyConst.treasureHunt_pendingOreMaxTip_none));
                return;
            }
            
            bool isAdvance = usingEnergy.item == GRefdataCoreMgr.instance.npGeneral.treasure_hunt_advanced_energy_common_item;
            ETreasureHuntGamePlayMode playMode = NPPlayer.instance.treasureHuntComponent.saver?.getSelectedGamePlayMode() ?? ETreasureHuntGamePlayMode.NORMAL;
            switch (playMode)
            {
                case ETreasureHuntGamePlayMode.NORMAL:
                    // 使用gameController开始游戏，奖励逻辑移到游戏结束状态处理
                    if (_m_gameController != null)
                    {
                        _m_gameController.startGame(isAdvance);
                    }
                    else
                    {
                        ALLog.Error("Cannot start game, game controller is null");
                    }
                    break;
                
                case ETreasureHuntGamePlayMode.AKEY_ONCE_PLAY:
                    // 一键单次游玩直接请求服务器
                    _reqPlayGame(false, isAdvance, (_msg) =>
                    {
                        if(_msg == null)
                            return;

                        TreasureHuntUtil.showCaptureResult(_msg.getResultList());
                    });
                    break;
                
                case ETreasureHuntGamePlayMode.AKEY_MULTI_PLAY:
                    // 一键多次游玩直接请求服务器
                    _reqPlayGame(true, isAdvance, (_msg) =>
                    {
                        if(_msg == null)
                            return;

                        NPUINoticeMgr.instance.addDealer(new NoticeDealer_CustomAction((_setDealDone) =>
                        {
                            GGUIWndTreasureHuntAkeyCaptureResult.instance.load();
                            GGUIWndTreasureHuntAkeyCaptureResult.instance.regLoadDoneDelegate(() =>
                            {
                                GGUIWndTreasureHuntAkeyCaptureResult.instance.setData(_msg);
                                GGUIWndTreasureHuntAkeyCaptureResult.instance.showWnd();
                            });
                        }, NPNoticeType.g_AllTypeArr, ()=>
                        {
                            GGUIWndTreasureHuntAkeyCaptureResult.instance.discard();
                        }, default, UINodeTagConst.C_TREASURE_HUNT_AKEY_CAPTURE_RESULT, true, true, true, true));
                        
                        NPPlayer.instance.treasureHuntComponent.tryShowStationUpgradePopWnd();
                    });
                    
                    break;
            }
        }
        
        /// <summary>
        /// 向服务器请求开始游戏
        /// </summary>
        private void _reqPlayGame(bool _isAkey, bool _isAdvance, Action<GS2GC_036_001_RetTreasureHuntOreCapture> _successCallback = null)
        {
            if(NPPlayer.instance.treasureHuntComponent.saver == null)
                return;
            
            long serialId = _m_lShowSerialId = ALSerializeOpMgr.next();
            ETreasureHuntCaptureType type = _isAkey ? ETreasureHuntCaptureType.MULTIPLE_AKEY : ETreasureHuntCaptureType.SINGLE_AKEY;
            // 请求开始游戏
            NPPlayer.instance.treasureHuntComponent.reqTreasureHuntOreCapture(type, _isAdvance, NPPlayer.instance.treasureHuntComponent.saver.getAreaId(), 0, (_isSuc, _msg) =>
            {
                if (!_isSuc || serialId != _m_lShowSerialId)
                    return;
                
                _successCallback?.Invoke(_msg);
            });
        }

        /// <summary>
        /// 显示捕获结果
        /// </summary>
        private void _showCaptureResult(TreasureHunt_CaptureReward _resultInfo, Action _showDone = null)
        {
            if (_resultInfo == null)
            {
                _showDone?.Invoke();
                return;
            }
            
            TreasureHuntCaptureResultBase resultBaseInfo = TreasureHuntCaptureResultBase.CreateCaptureResult(_resultInfo);
            if (resultBaseInfo == null)
            {
                _showDone?.Invoke();
                return;
            }
            
            resultBaseInfo.showCaptureResult(_showDone);
        }
        
        /// <summary>
        /// 显示捕获结果
        /// </summary>
        private void _showCaptureResult(List<TreasureHunt_CaptureResult> _resultInfoList, Action _showDone = null)
        {
            if (_resultInfoList == null)
            {
                _showDone?.Invoke();
                return;
            }

            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.regAllDoneDelegate(_showDone);
            stepCounter.chgTotalStepCount(1);

            foreach (TreasureHunt_CaptureResult item in _resultInfoList)
            {
                List<TreasureHunt_CaptureReward> rewardList = item?.getCaptureRewardList();
                if (rewardList == null)
                    continue;

                foreach (TreasureHunt_CaptureReward reward in rewardList)
                {
                    stepCounter.chgTotalStepCount(1);
                    _showCaptureResult(reward, stepCounter.addDoneStepCount);   
                }
            }
            
            stepCounter.addDoneStepCount();
        }
        
        /// <summary>
        /// 点击材料室按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickMaterialsRoom(GameObject _go)
        {
            if (_m_wMaterialsRoom == null)
                return;
            
            QueueMgr.instance.addNode_InGame_SingleWnd(_m_wMaterialsRoom, () =>
            {
                _m_wMaterialsRoom?.showWnd();
            }, UINodeTagConst.C_TREASURE_HUNT_SPECIMEN_ROOM);
        }

        /// <summary>
        /// 点击返回按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickReturn(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_GAME_MAIN);
        }

        #endregion

        #region 窗口消息

        /// <summary>
        /// 当CommonItem数量变化时
        /// </summary>
        /// <param name="_objs"></param>
        private void _onCommonItemCountChg(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 3 || !(_objs[0] is ENPItemType itemType) || 
                !(_objs[1] is long subId) || !(_objs[2] is long count))
                return;
            
            // 当变化的是当前使用能源道具
            if((itemType == _m_iNormalEnergy.getItemType() && subId == _m_iNormalEnergy.subId) || 
               (itemType == _m_iAdvancedEnergy.getItemType() && subId == _m_iAdvancedEnergy.subId))
                _refreshUsingEnergyShow();
        }

        /// <summary>
        /// 太空舱等级变化处理
        /// </summary>
        private void _onStationLevelChg(params object[] _objs)
        {
            // 当太空舱等级变化时，刷新游玩模式toggle
            _refreshPlayModeToggle();
        }
        
        #endregion
    }
}