using System;
using System.Collections.Generic;
using ALPackage;
using GOE.EveningDungeon;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 晚间活动游戏窗口
    /// </summary>
    public class GGUIWndEveningDungeonGameMain : _ANPGGUIBasicResBarWnd<GGUIMonoEveningDungeonGameMain>
    {
        private static GGUIWndEveningDungeonGameMain _g_instance;
        public static GGUIWndEveningDungeonGameMain instance { get { return _g_instance ??= new GGUIWndEveningDungeonGameMain(); } }

        private EveningDungeonGameController _m_gameController;// 晚间活动游戏控制器

        private List<EveningDungeonHeroFightInfo> _m_lHeroFightInfoList;//大臣战斗数据列表
        private EveningDungeonHeroFightInfo _m_iCurSelectHeroInfo;//当前选中的大臣战斗数据

        private EEveningDungeonGameState _m_eCurGameState;//当前游戏状态
        
        private GGUISubWndEveningDungeonBoss _m_wBossWnd;//boss子窗口
        private GGUIWndEveningDungeonRankMini _m_wRankMini;// 排行小窗口
        private List<GGUISubWndEveningDungeonOtherPlayerAttackArea> _m_lOtherPlayerAttackAreaList;// 其他玩家攻击表现列表
        private GGUIWndHeroIconItem _m_wSelectHeroHead;//选中大臣头像
        private GGUIWndEveningDungeonGameAirshipShow _m_wAirshipShow;// 飞船形象显示
        private NPGGUIWndCommonToggleEx _m_wAutoAttackToggle;//自动攻击开关
        
        public GGUIWndEveningDungeonGameMain() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoEveningDungeonGameMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoEveningDungeonGameMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        public GGUISubWndEveningDungeonBoss bossWnd { get { return _m_wBossWnd; } }
        public EveningDungeonGameUIConfig gameUIConfig { get { return wnd == null ? null : wnd.gameUIConfig; } }
        public EveningDungeonHeroFightInfo curSelectHeroInfo { get { return _m_iCurSelectHeroInfo; } }
        
        /// <summary>
        /// 是否开启自动攻击
        /// </summary>
        public bool isAutoAttack { get { return _m_wAutoAttackToggle != null && _m_wAutoAttackToggle.isShow && _m_wAutoAttackToggle.isOn; } }
        
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoBoss != null)
                _m_wBossWnd = new GGUISubWndEveningDungeonBoss(wnd.monoBoss);

            if (wnd.monoRankMini != null)
                _m_wRankMini = new GGUIWndEveningDungeonRankMini(wnd.monoRankMini);

            if (wnd.monoOtherPlayerAttackAreaList != null)
            {
                if (_m_lOtherPlayerAttackAreaList == null)
                    _m_lOtherPlayerAttackAreaList = new List<GGUISubWndEveningDungeonOtherPlayerAttackArea>();

                GGUISubWndEveningDungeonOtherPlayerAttackArea attackAreaWnd = null;
                foreach (var attackAreaMono in wnd.monoOtherPlayerAttackAreaList)
                {
                    if (attackAreaMono == null)
                        continue;
                    
                    attackAreaWnd = new GGUISubWndEveningDungeonOtherPlayerAttackArea(attackAreaMono);
                    _m_lOtherPlayerAttackAreaList.Add(attackAreaWnd);
                }
            }
            
            if (wnd.monoSelectHeroHead != null)
                _m_wSelectHeroHead = new GGUIWndHeroIconItem(wnd.monoSelectHeroHead);

            if(wnd.monoAirshipShow != null)
                _m_wAirshipShow = new GGUIWndEveningDungeonGameAirshipShow(wnd.monoAirshipShow);
            
            if (wnd.autoAttackToggle != null)
            {
                _m_wAutoAttackToggle = new NPGGUIWndCommonToggleEx(wnd.autoAttackToggle);
                _m_wAutoAttackToggle.clickDelegate += _onAutoAttackToggleClick;
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnOpenSelectHeroWnd, _onOpenSelectHeroWndBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnFinalAttackRecord, _btnFinalAttackRecordClick);
            ALUGUICommon.combineBtnClick(wnd.btnFight, _onFightBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onReturnBtnClick);
        }

        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnOpenSelectHeroWnd, _onOpenSelectHeroWndBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnFinalAttackRecord, _btnFinalAttackRecordClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnFight, _onFightBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _onReturnBtnClick);
            }
    
            _m_wBossWnd?.discard();
            _m_wBossWnd = null;
            
            _m_wRankMini?.discard();
            _m_wRankMini = null;

            if (_m_lOtherPlayerAttackAreaList != null)
            {
                foreach (var attackAreaWnd in _m_lOtherPlayerAttackAreaList)
                {
                    if(attackAreaWnd != null)
                        attackAreaWnd.discard();
                }
                _m_lOtherPlayerAttackAreaList.Clear();
                _m_lOtherPlayerAttackAreaList = null;
            }
            
            _m_wSelectHeroHead?.discard();
            _m_wSelectHeroHead = null;

            _m_wAirshipShow?.discard();
            _m_wAirshipShow = null;
            
            if (_m_wAutoAttackToggle != null)
            {
                _m_wAutoAttackToggle.clickDelegate -= _onAutoAttackToggleClick;
                _m_wAutoAttackToggle.discard();
                _m_wAutoAttackToggle = null;       
            }
        }
        
        protected override void _onShowWnd()
        {
            // 获取大臣战斗数据列表
            _m_lHeroFightInfoList = NPPlayer.instance.eveningDungeonComp.getAllHeroFightInfoList();
            _m_lHeroFightInfoList?.Sort(EveningDungeonHeroFightInfo.sort);// 排序
            refreshCurSelectHero(null);// 刷新当前选中大臣

            if (_m_lOtherPlayerAttackAreaList != null)
            {
                foreach (var attackAreaWnd in _m_lOtherPlayerAttackAreaList)
                {
                    if(attackAreaWnd != null)
                        attackAreaWnd.showWnd();
                }
            }
            
            if (_m_wRankMini != null)
            {
                _m_wRankMini.showWnd();
                _m_wRankMini.refreshRank(true);
            }
            
            if (_m_wAutoAttackToggle != null)
            {
                _m_wAutoAttackToggle.showWnd();
                // 每次显示窗口时都要设置为默认值
                _m_wAutoAttackToggle.setState(false);
            }
            
            WinMsg.RegisterMsgAct(WinMsgType.ON_EVENING_DUNGEON_USED_HERO_CHG, _onHeroUsedInfoChg);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_EVENING_DUNGEON_GAME_FIGHT, _onBtnFight);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_EVENING_DUNGEON_USED_HERO_CHG, _onHeroUsedInfoChg);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_EVENING_DUNGEON_GAME_FIGHT, _onBtnFight);

            _m_lHeroFightInfoList?.Clear();
            _m_lHeroFightInfoList = null;

            _m_iCurSelectHeroInfo = null;
            
            _m_wRankMini?.hideWnd();
            _m_wSelectHeroHead?.hideWnd();
            _m_wAirshipShow?.hideWnd();
            _m_wAutoAttackToggle?.hideWnd();
            
            if (_m_lOtherPlayerAttackAreaList != null)
            {
                foreach (var attackAreaWnd in _m_lOtherPlayerAttackAreaList)
                {
                    if(attackAreaWnd != null)
                        attackAreaWnd.hideWnd();
                }
            }
        }

        protected override void _onReset()
        {
            _m_wRankMini?.resetWnd();
            _m_wSelectHeroHead?.resetWnd();
            _m_wAirshipShow?.resetWnd();
            _m_wAutoAttackToggle?.resetWnd();
            
            if (_m_lOtherPlayerAttackAreaList != null)
            {
                foreach (var attackAreaWnd in _m_lOtherPlayerAttackAreaList)
                {
                    if(attackAreaWnd != null)
                        attackAreaWnd.resetWnd();
                }
            }
        }

        /// <summary>
        /// 设置游戏控制器
        /// </summary>
        /// <param name="_gameController"></param>
        public void setGameController(EveningDungeonGameController _gameController)
        {
            _m_gameController = _gameController;
        }

        public void getOtherPlayerAttackArea(List<GGUISubWndEveningDungeonOtherPlayerAttackArea> _areaList)
        {
            if(_areaList == null || _m_lOtherPlayerAttackAreaList == null)
                return;
            
            _areaList.Clear();
            _areaList.AddRange(_m_lOtherPlayerAttackAreaList);
        }
        
        /// <summary>
        /// 刷新当前选中大臣
        /// </summary>
        public void refreshCurSelectHero(Action _onChgDone)
        {
            // 若当前没有选中的大臣 或 选中大臣已经出战次数达到最大值
            if (_m_iCurSelectHeroInfo == null || _m_iCurSelectHeroInfo.heroCardShow == null || _m_iCurSelectHeroInfo.nowFightCount >= _m_iCurSelectHeroInfo.fightMaxCount)
            {
                _m_iCurSelectHeroInfo = null;

                if (_m_lHeroFightInfoList != null)
                {
                    foreach (var heroFightInfo in _m_lHeroFightInfoList)
                    {
                        // 按照大臣列表顺序找到第一个可出战的大臣
                        if (heroFightInfo != null && heroFightInfo.heroCardShow != null && heroFightInfo.nowFightCount < heroFightInfo.fightMaxCount)
                        {
                            _m_iCurSelectHeroInfo = heroFightInfo;
                            break;
                        }
                    }
                }
            }

            if (_m_iCurSelectHeroInfo == null || _m_iCurSelectHeroInfo.heroCardShow == null)
            {
                if (wnd != null)
                {
                    ALUGUICommon.setGameObjEnable(wnd.hasSelectHeroShowList, false);
                    ALUGUICommon.setGameObjEnable(wnd.noSelectHeroShowList, true);
                }
                
                _m_wAirshipShow?.airshipDeparture(() =>
                {
                    if(isShow)
                        _m_wAirshipShow?.showNoShip();
                });
                _onChgDone?.Invoke();
            }
            else
            {
                if (_m_wSelectHeroHead != null)
                {
                    _m_wSelectHeroHead.showWnd();
                    _m_wSelectHeroHead.setData(_m_iCurSelectHeroInfo.heroCardShow);
                }
                if (wnd != null)
                {
                    string atkKey = string.IsNullOrEmpty(wnd.txtSelectHeroATKKey) ? TransKeyConst.common_value : wnd.txtSelectHeroATKKey;
                    ALUGUICommon.setLabelTxt(wnd.txtSelectHeroATK, TextTranslate.instance.getLanguage(atkKey, _m_iCurSelectHeroInfo.fightATK.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
                    
                    ALUGUICommon.setGameObjEnable(wnd.hasSelectHeroShowList, true);
                    ALUGUICommon.setGameObjEnable(wnd.noSelectHeroShowList, false);
                }

                if (_m_wAirshipShow != null)
                {
                    _m_wAirshipShow.showWnd();
                    _m_wAirshipShow.airshipEntry(_m_iCurSelectHeroInfo.heroId, _onChgDone);
                }
                else
                {
                    _onChgDone?.Invoke();
                }
            }
        }

        /// <summary>
        /// 刷新状态显示
        /// </summary>
        /// <param name="_gameState"></param>
        public void refreshStateShow(EEveningDungeonGameState _gameState)
        {
            if (wnd == null)
                return;

            _m_eCurGameState = _gameState;

            if (wnd.stateShowList != null)
            {
                NPCommonEnumAniStatInfo<EEveningDungeonGameState>.setStat(wnd.stateShowList, _gameState);
            }
        }
        
        /// <summary>
        /// 停止自动攻击
        /// </summary>
        public void stopAutoAttack()
        {
            if (_m_wAutoAttackToggle != null)
            {
                _m_wAutoAttackToggle.setState(false);
            }
        }

        /// <summary>
        /// 飞船攻击表现
        /// </summary>
        /// <param name="_showDone"></param>
        public void showAirshipAttack(Action _showDone)
        {
            if (_m_wAirshipShow == null)
            {
                _showDone?.Invoke();
                return;
            }
            
            _m_wAirshipShow.airshipAttack(_showDone);
        }
        
        /// <summary>
        /// 飞船离场表现
        /// </summary>
        /// <param name="_showDone"></param>
        public void showAirshipDeparture(Action _showDone)
        {
            if (_m_wAirshipShow == null)
            {
                _showDone?.Invoke();
                return;
            }
            
            _m_wAirshipShow.airshipDeparture(_showDone);
        }
        
        /// <summary>
        /// 自动攻击开关被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onAutoAttackToggleClick(NPGGUIWndCommonToggleEx _toggleWnd)
        {
            if(_m_wAutoAttackToggle == null)
                return;

            bool setIsOn = !_m_wAutoAttackToggle.isOn;
            
            // 判断自动攻击功能是否解锁
            if(setIsOn && !GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.evening_dungeon_auto_fight_simple_unlock_id, true))
                return;
            
            _m_wAutoAttackToggle.setSelected(setIsOn);
            // if (setIsOn)
            //     _onFightBtnClick(null);
        }
        
        /// <summary>
        /// 当打开选择大臣窗口按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onOpenSelectHeroWndBtnClick(GameObject _go)
        {
            if(_m_eCurGameState != EEveningDungeonGameState.IDLE)
                return;
            
            //打开选择大臣窗口前, 先刷新一下当前选中大臣
            refreshCurSelectHero(null);
            
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndEveningDungeonFightSelectHero.instance, () =>
            {
                GGUIWndEveningDungeonFightSelectHero.instance.showWnd();
                List<_IEveningDungeonHeroFightInfo> heroFightInfoList = new List<_IEveningDungeonHeroFightInfo>();
                if (_m_lHeroFightInfoList != null)
                    heroFightInfoList.AddRange(_m_lHeroFightInfoList);
                GGUIWndEveningDungeonFightSelectHero.instance.setData(new List<_IEveningDungeonHeroFightInfo>(heroFightInfoList), _m_iCurSelectHeroInfo,
                    (_selectHero) =>
                    {
                        _m_iCurSelectHeroInfo = _selectHero as EveningDungeonHeroFightInfo;
                        refreshCurSelectHero(null);
                    });
            }, UINodeTagConst.C_EVENING_DUNGEON_SELECT_HERO);
        }

        /// <summary>
        /// 尾刀记录按钮被点击
        /// </summary>
        private void _btnFinalAttackRecordClick(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndEveningDungeonFinalAttackRecord.instance, () =>
            {
                GGUIWndEveningDungeonFinalAttackRecord.instance.showWnd();
            }, UINodeTagConst.C_EVENING_DUNGEON_FINAL_ATTACK_RECORD);
        }
        private void _onBtnFight()
        {
            if (wnd != null) 
                _onFightBtnClick(wnd.btnFight);
        }
        /// <summary>
        /// 当战斗按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onFightBtnClick(GameObject _go)
        {
            if(_m_iCurSelectHeroInfo == null || 
               (_m_wAirshipShow != null && _m_wAirshipShow.curAirshipState != EEveningDungeonGameAirshipState.IDLE))
               return;
            
            _m_gameController?.attack();
        }

        /// <summary>
        /// 点击返回按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onReturnBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_EVENING_DUNGEON_GAME_MAIN);
        }
        
        /// <summary>
        /// 大臣使用过的数据发生改变
        /// </summary>
        private void _onHeroUsedInfoChg()
        {
            if(_m_lHeroFightInfoList == null)
                return;
            
            // 更新大臣战斗数据列表
            _m_lHeroFightInfoList.ForEach((_heroFightInfo) =>
            {
                _heroFightInfo?.updateNowFightCount();
            });
            
            // 排序
            _m_lHeroFightInfoList?.Sort(EveningDungeonHeroFightInfo.sort);
        }
    }
}