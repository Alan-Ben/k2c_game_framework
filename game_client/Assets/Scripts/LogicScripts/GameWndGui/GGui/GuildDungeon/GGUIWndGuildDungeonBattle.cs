using ALPackage;
using JetBrains.Annotations;
using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟PVE战斗
    /// </summary>
    public class GGUIWndGuildDungeonBattle : _ATALBasicUIWnd<GGUIMonoGuildDungeonBattle>
    {
        [NotNull] public static GGUIWndGuildDungeonBattle instance { get { return _g_instance ??= new GGUIWndGuildDungeonBattle(); } }
        private static GGUIWndGuildDungeonBattle _g_instance;

        // <AutoGen:WndDeclaration>
        private GGUISubWndCommonBlood _m_bloodBossWnd;  // boss血条
        private NPGGUIWndCommonToggleEx _m_toggleAutoBattleWnd;  // 自动战斗开关
        // </AutoGen:WndDeclaration>
        private GGUIWndHeroCommonCardItem _m_heroCardWnd;
        private NPGGuiWndTexture _m_bgWnd;
        private NPGGuiWndTexture _m_bossIconWnd;
        private NPGGUIWndCommonShowCase _m_showCaseVideoSelf;//展示视频
        private NPGGUIWndCommonShowCase _m_showCaseVideoBoss;//展示视频
        
        private HeroInfo _m_curSelectHero; // 当前选择的大臣
        private GuildDungeonMonster _m_dungeonMonster; // 当前副本怪物信息
        private GuildDungeonInfo _m_dungeonInfo;
        private bool _m_isAutoBattle = false;
        private long _m_lAutoBattleSerializeOp = 0; // 自动战斗的序列ID，用于取消自动战斗

        private GGUIWndChapterBossHPTipCache _m_hpTipCache; // 血量提示缓存
        private bool _m_isReqAttacking = false;

        public GGUIWndGuildDungeonBattle() : base(EALUIWndLayer.NORMAL)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoGuildDungeonBattle.assetPath; }
        protected override string _monoObjName { get => GGUIMonoGuildDungeonBattle.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        public override bool needDiscardOnSwitch => true;

        protected override void _onShowWnd()
        {
            _refreshWnd();
            WinMsg.RegisterMsg(WinMsgType.ON_GUILD_DUNGEON_MONSTER_CHG, _onGuildDungeonMonsterChg);
        }


        protected override void _onHideWnd()
        {
            _m_lAutoBattleSerializeOp = ALSerializeOpMgr.next();
            _m_bgWnd?.hideWnd();
            _m_bossIconWnd?.hideWnd();
            WinMsg.UnregisterMsg(WinMsgType.ON_GUILD_DUNGEON_MONSTER_CHG, _onGuildDungeonMonsterChg);
        }
    
        protected override void _onReset()
        {
            _m_bloodBossWnd?.resetWnd();
            _m_bloodBossWnd = null;
            _m_bgWnd?.discardTexture();
            _m_bossIconWnd?.discardTexture();

            _m_toggleAutoBattleWnd?.resetWnd();
            _m_toggleAutoBattleWnd = null;
            
            
            if (_m_showCaseVideoSelf != null) 
                _m_showCaseVideoSelf.resetWnd();
            
            if (_m_showCaseVideoBoss != null) 
                _m_showCaseVideoBoss.resetWnd();
        }
    
        protected override void _onDiscard()
        {
            _m_bloodBossWnd?.discard();
            _m_bloodBossWnd = null;
            
            _m_bgWnd?.discard();
            _m_bgWnd = null;
            
            _m_bossIconWnd?.discard();
            _m_bossIconWnd = null;
            
            _m_toggleAutoBattleWnd?.discard();
            _m_toggleAutoBattleWnd = null;
            
            _m_heroCardWnd?.discard();
            _m_heroCardWnd = null;
            
            if (_m_showCaseVideoSelf != null) 
                _m_showCaseVideoSelf.discard();
            _m_showCaseVideoSelf = null;
            if (_m_showCaseVideoBoss != null) 
                _m_showCaseVideoBoss.discard();
            _m_showCaseVideoBoss = null;

            if (_m_hpTipCache != null)
            {
                _m_hpTipCache.pushBackAllCacheItems();
                _m_hpTipCache.discard();
            }

            ALUGUICommon.uncombineBtnClick(wnd.btnSelectHero, _onClickbtnSelectHero);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickbtnClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnKillClose, _onClickbtnClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnBattle, _onClickbtnBattle);
            _m_lAutoBattleSerializeOp = ALSerializeOpMgr.next();
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
                
            if (wnd.bloodBoss != null)
            {
                _m_bloodBossWnd = new GGUISubWndCommonBlood(wnd.bloodBoss);
            }
            if (wnd.toggleAutoBattle != null)
            {
                _m_toggleAutoBattleWnd = new NPGGUIWndCommonToggleEx(wnd.toggleAutoBattle);
                _m_toggleAutoBattleWnd.clickDelegate = _onToggleAutoBattle;
                _m_toggleAutoBattleWnd.setSelected(_m_isAutoBattle);

            }
            if (wnd.monoCurHeroCard)
                _m_heroCardWnd = new GGUIWndHeroCommonCardItem(wnd.monoCurHeroCard);
            
            if (wnd.monoShowcaseSelf != null) 
                _m_showCaseVideoSelf = new NPGGUIWndCommonShowCase(wnd.monoShowcaseSelf);
            if (wnd.monoShowcaseBoss != null) 
                _m_showCaseVideoBoss = new NPGGUIWndCommonShowCase(wnd.monoShowcaseBoss);
            
            if(wnd.imgBg != null)
                _m_bgWnd = new NPGGuiWndTexture(wnd.imgBg);
            
            if(wnd.imgBossIcon != null)
                _m_bossIconWnd = new NPGGuiWndTexture(wnd.imgBossIcon);

            if (wnd.hpTipPrefab != null && wnd.hpTipCacheRoot != null)
            {
                _m_hpTipCache = new GGUIWndChapterBossHPTipCache(wnd.hpTipCacheRoot, 1, 8);
                _m_hpTipCache.init(wnd.hpTipPrefab);
            }

            ALUGUICommon.combineBtnClick(wnd.btnSelectHero, _onClickbtnSelectHero);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickbtnClose);
            ALUGUICommon.combineBtnClick(wnd.btnKillClose, _onClickbtnClose);
            ALUGUICommon.combineBtnClick(wnd.btnBattle, _onClickbtnBattle);
        }
        
        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_monster"></param>
        public void setInfo(GuildDungeonMonster _monster)
        {
            _m_dungeonMonster = _monster;
            if (_m_dungeonMonster != null)
            {
                _m_curSelectHero = NPPlayer.instance.guildDungeonComp.getSelfMatchHero(_m_dungeonMonster.hp);
                _m_dungeonInfo = _m_dungeonMonster.dungeonInfo;
            }
            else
            {
                _m_curSelectHero = null;
                _m_dungeonInfo = null;
            }
            _m_lAutoBattleSerializeOp = ALSerializeOpMgr.next();
            _m_isAutoBattle = AccountSettingMgr.instance.accountSetting.guildDungeonAutoBattle;
            _refreshWnd();
            _quitBattleView();
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            
            // 更新自动战斗状态
            if (_m_toggleAutoBattleWnd != null)
            {
                _m_toggleAutoBattleWnd.setSelected(_m_isAutoBattle);
            }

            _setKillBossShow(false);
            _refreshBossInfo();
            _refreshHeroInfo();

            if (wnd != null && wnd.changeHeroAni != null && _m_curSelectHero != null)
                wnd.changeHeroAni.Play(wnd.changeHeroAniName);
            _m_isReqAttacking = false;
        }

        /// <summary>
        /// 刷新大臣信息
        /// </summary>
        private void _refreshHeroInfo()
        {
            _m_heroCardWnd?.setInfo(_m_curSelectHero);
            if(wnd == null)
                return;
            //展示当前章节视频
            if (null != _m_showCaseVideoSelf)
            {
                if (_m_curSelectHero != null)
                {
                    _m_showCaseVideoSelf.showWnd(new ShowCaseCommonResUnitInfoObj(_m_curSelectHero.getTdShow()));
                    if(!string.IsNullOrEmpty(wnd.heroDefaultAniName))
                        _m_showCaseVideoSelf?.regInitDoneDelegate(() =>
                        {
                            _m_showCaseVideoSelf?.playAnim(0, wnd.heroDefaultAniName);
                        });
                }
                else
                {
                    _m_showCaseVideoSelf.hideWnd();
                }
            }
            
            // 根据选择状态显示/隐藏UI
            bool hasSelectedHero = _m_curSelectHero != null;
            ALUGUICommon.setGameObjEnable(wnd.goShowOnSelectedHero, hasSelectedHero);
            ALUGUICommon.setGameObjEnable(wnd.goHideOnSelectedHero, !hasSelectedHero);
        }

        /// <summary>
        /// 刷新Boss信息
        /// </summary>
        private void _refreshBossInfo()
        {
            if (wnd == null || _m_dungeonMonster == null) 
                return;

            // 设置战斗背景图
            if (_m_bgWnd != null && _m_dungeonMonster.dungeonInfo != null && _m_dungeonMonster.dungeonInfo.dungeonRefObj != null)
                _m_bgWnd.setTexture(_m_dungeonMonster.dungeonInfo.dungeonRefObj.battle_bg_img);
            
            // 展示当前Boss形象
            if (_m_dungeonMonster.monsterShowRefObj != null)
            {
                _m_bossIconWnd?.setTexture(_m_dungeonMonster.monsterShowRefObj.icon);
                _m_showCaseVideoBoss?.showWnd(new ShowCaseCommonResUnitInfoObj(_m_dungeonMonster.monsterShowRefObj.boss_go_index));
            }
            
            // 设置Boss名称
            ALUGUICommon.setLabelTxt(wnd.txtBossName, TextTranslate.instance.getLanguage(_m_dungeonMonster?.monsterShowRefObj?.name));

            
            // 如果是自己在请求攻击则补刷新boss血条，避免和展示战斗的掉血重复扣血导致表现不一致
            if (_m_isReqAttacking)
                return;
            // 刷新Boss血条
            _refreshBossBlood();
        }

        /// <summary>
        /// 刷新Boss血条
        /// </summary>
        private void _refreshBossBlood()
        {
            if (_m_bloodBossWnd != null && _m_dungeonMonster != null)
            {
                _m_bloodBossWnd.showWnd();
                _m_bloodBossWnd.setBloodData(_m_dungeonMonster.hp, _m_dungeonMonster.maxHp);
            }
        }
        
        // <AutoGen:Method>
        // 自动战斗切换事件
        private void _onToggleAutoBattle(NPGGUIWndCommonToggleEx toggleWnd)
        {
            if(!GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.guild_dungeon_auto_fight_cond, true))
            {
                AccountSettingMgr.instance.accountSetting.setGuildDungeonAutoBattle(false);
                if (_m_toggleAutoBattleWnd != null)
                {
                    _m_toggleAutoBattleWnd.setSelected(false);
                }
                return;
            }
            
            _m_isAutoBattle = !_m_isAutoBattle;
            AccountSettingMgr.instance.accountSetting.setGuildDungeonAutoBattle(_m_isAutoBattle);
            if (_m_toggleAutoBattleWnd != null)
            {
                _m_toggleAutoBattleWnd.setSelected(_m_isAutoBattle);
            }
        }

        // 选择大臣点击事件
        private void _onClickbtnSelectHero(GameObject go)
        {
            GGUIWndGuildDungeonSelectHero.instance.setInfo(_m_curSelectHero, _onHeroSelect);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGuildDungeonSelectHero.instance, GGUIWndGuildDungeonSelectHero.instance.showWnd, UINodeTagConst.C_GUIlD_DUNGEON_SELECT_HERO);
        }
        
        private void _onHeroSelect(HeroInfo hero)
        {
            if (hero == null || NPPlayer.instance.guildDungeonComp.getHeroLeftFightTimes(hero.id) <= 0)
                _m_curSelectHero = null;
            else
                _m_curSelectHero = hero;
            
            _refreshHeroInfo();
            
            if (wnd != null && wnd.changeHeroAni != null)
                wnd.changeHeroAni.Play(wnd.changeHeroAniName);
        }
        
        // 关闭按钮点击事件
        private void _onClickbtnClose(GameObject go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GUIlD_DUNGEON_BATTLE);
        }

        // 攻击按钮点击事件
        private void _onClickbtnBattle(GameObject go)
        {
            // 验证必要数据
            if (_m_dungeonMonster == null || _m_dungeonInfo == null)
            {
                return;
            }
            if (_m_curSelectHero == null)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.guild_dungeon_noHeroCanFight_tip));
                return;
            }
            if (_m_isAutoBattle)
            {
                _startAutoBattle();
                return;
            }

            long oriHpValue = _m_dungeonMonster.hp;
            _enterBattleView();
            _m_isReqAttacking = true;

            NPPlayer.instance.guildDungeonComp.reqAttackDungeon(_m_dungeonInfo.instanceId, _m_dungeonMonster.monsterId, _m_curSelectHero.id,
                (_suc, _msg) =>
                {
                    _m_isReqAttacking = false;

                    if (!_suc)
                    {
                        _quitBattleView();
                        if (_m_dungeonMonster != null && _m_dungeonMonster.hp <= 0)
                        {
                            _refreshBossInfo();
                            _showMonsterBeKillTip();
                        }
                        return;
                    }
                    long damageValue = oriHpValue - _m_dungeonMonster.hp;

                    _showBattle(damageValue, () =>
                    {
                        _onHeroSelect(NPPlayer.instance.guildDungeonComp.getSelfMatchHero(_m_dungeonMonster.hp));

                        // 刷新界面显示最新状态
                        _refreshHeroInfo();
                  
                        // 处理战斗结果
                        if (_msg != null)
                        {
                            if (_msg.getIsKilled())
                            {
                                // Boss被击杀，显示奖励并显示击杀状态
                                GCommon.showGainRewardTip(_msg.getGainItemList());
                                _setKillBossShow(true);
                                _quitBattleView();
                            }
                            else
                            {
                                // Boss未被击杀，显示获得的奖励但保持窗口打开
                                GCommon.showGainRewardTip(_msg.getGainItemList());
                                _quitBattleView();
                            }
                        }
                    });
                   
                });
        }

        private void _showBattle(long _damageValue, Action _onComplete)
        {
            //播放动画
            if (wnd != null && wnd.battleAni != null)
                wnd.battleAni.Play(wnd.battleAniName, () => { _onComplete?.Invoke(); });
            else
                _onComplete?.Invoke();

            //飘血
            if (_m_hpTipCache != null)
            {
                GGUIWndChapterBossHPTip tempItemWnd = _m_hpTipCache.popItem();
                if (tempItemWnd == null)
                    return;

                tempItemWnd.showWnd();
                tempItemWnd.setInfoPlay(wnd.hpTipParent
                    , TextTranslate.instance.getLanguage(TransKeyConst.common_reduced_num, _damageValue)
                    , wnd.hpTipAniName
                    , () => { _m_hpTipCache.pushBackCacheItem(tempItemWnd); });
            }

            //血条变化
            _m_bloodBossWnd?.damage(_damageValue);
        }

        private void _startAutoBattle()
        {
            _m_lAutoBattleSerializeOp = ALSerializeOpMgr.next();
            long serializeOp = _m_lAutoBattleSerializeOp;
            _enterBattleView();
            _autoBattle(serializeOp);
        }
        
        /// <summary>
        /// 自动战斗
        /// </summary>
        private void _autoBattle(long _serializeOp)
        {
            if(_serializeOp != _m_lAutoBattleSerializeOp)
                return; // 如果序列ID不匹配，说明已经取消了自动战斗
            if (!_m_isAutoBattle)
            {
                _quitBattleView();
                return;
            }
                
            if (_m_dungeonMonster == null || _m_dungeonInfo == null)
            {
                _quitBattleView();
                return;
            }

            if (_m_curSelectHero == null)
            {
                _m_curSelectHero = NPPlayer.instance.guildDungeonComp.getSelfMatchHero(_m_dungeonMonster.hp);
                _refreshHeroInfo();
            }

            if (_m_curSelectHero == null)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.guild_dungeon_noHeroCanFight_tip));
                _refreshHeroInfo();
                _refreshBossBlood();
                _quitBattleView();
                return;
            }

            long oriHpValue = _m_dungeonMonster.hp;
            _m_isReqAttacking = true;

            NPPlayer.instance.guildDungeonComp.reqAttackDungeon(_m_dungeonInfo.instanceId, _m_dungeonMonster.monsterId, _m_curSelectHero.id,
                (_suc, _msg) =>
                {
                    _m_isReqAttacking = false;
                    if (!_suc)
                    {
                        _quitBattleView();
                        if (_m_dungeonMonster != null && _m_dungeonMonster.hp <= 0)
                        {
                            _refreshBossInfo();
                            _showMonsterBeKillTip();
                        }
                        return;
                    }

                    // 如果序列ID不匹配，说明已经取消了自动战斗
                    if (_serializeOp != _m_lAutoBattleSerializeOp)
                        return; 
                    
                    long damageValue = oriHpValue - _m_dungeonMonster.hp;

                    _showBattle(damageValue, () =>
                    {
                        // 如果序列ID不匹配，说明已经取消了自动战斗
                        if (_serializeOp != _m_lAutoBattleSerializeOp)
                            return; 


                        // 刷新界面显示最新状态
                        _m_curSelectHero = null;
                        _m_curSelectHero = NPPlayer.instance.guildDungeonComp.getSelfMatchHero(_m_dungeonMonster.hp);
                        if (_m_curSelectHero != null && wnd != null && wnd.changeHeroAni != null)
                            wnd.changeHeroAni.Play(wnd.changeHeroAniName);
                        _refreshHeroInfo();
            
                        // 处理战斗结果
                        if (_msg != null)
                        {
                            GCommon.showGainRewardTip(_msg.getGainItemList());

                            if (_msg.getIsKilled())
                            {
                                // Boss被击杀，显示奖励并显示击杀状态
                                _setKillBossShow(true);
                            }
                            else
                            {
                                // Boss未被击杀
                                ALCommonTaskController.CommonActionAddMonoTask(() =>
                                {
                                    _autoBattle(_serializeOp);
                                }, wnd.autoBattleDelayTime);

                            }
                        }
                    });
                });

        }

        /// <summary>
        /// 设置Boss被击败显示状态
        /// </summary>
        /// <param name="_isKill"></param>
        private void _setKillBossShow(bool _isKill)
        {
            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.goKillHideList, !_isKill);
                ALUGUICommon.setGameObjEnable(wnd.goKillShowList, _isKill);
            }
        }
        
        private void _enterBattleView()
        {
            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.goShowOnBattle, true);
                ALUGUICommon.setGameObjEnable(wnd.goHideOnBattle, false);
            }
        }

        private void _quitBattleView()
        {
            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.goShowOnBattle, false);
                ALUGUICommon.setGameObjEnable(wnd.goHideOnBattle, true);
            }
        }
        // </AutoGen:Method>
        
        
        private void _onGuildDungeonMonsterChg(object[] _objs)
        {
            if(_objs == null || _objs.Length < 2 || !(_objs[0] is long instanceId) || !(_objs[1] is long _monsterId))
                return;
            
            if (_m_dungeonMonster == null || _m_dungeonInfo == null || (instanceId != _m_dungeonInfo.instanceId || _monsterId != _m_dungeonMonster.monsterId))
                return;
            if (_m_dungeonMonster.hp <= 0)
            {
                // 如果是自己在请求攻击则不弹提示
                if (_m_isReqAttacking)
                    return;
                
                _refreshBossInfo();
                _showMonsterBeKillTip();
            }
            else
            {
                _refreshBossInfo();
            }
        }

        private void _showMonsterBeKillTip()
        {
            NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.guild_dungeon_monsterBeDefeatByOthers_tip) , 
                TextTranslate.instance.getLanguage(TransKeyConst.confirm) , () =>
                {
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GUIlD_DUNGEON_BATTLE);
                });
        }
    }
}