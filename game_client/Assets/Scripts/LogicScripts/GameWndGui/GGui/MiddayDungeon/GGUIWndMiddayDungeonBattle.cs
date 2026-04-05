using System;
using ALPackage;
using Common.DungeonObj;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndMiddayDungeonBattle : _ATALBasicUIWnd<GGUIMonoMiddayDungeonBattle>
    {
        private static GGUIWndMiddayDungeonBattle _g_instance = new GGUIWndMiddayDungeonBattle();
    
        public static GGUIWndMiddayDungeonBattle instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndMiddayDungeonBattle();
                return _g_instance;
            }
        }

        private HeroInfo _m_curSelectHero;
        private MiddayDungeonGuildHeroInfo _m_curSelectGuildHero;
        
        private NPGGUIWndCommonShowCase _m_showCaseBoss;//展示Boss
        
        private GGUIWndSimpleVideo _m_wSimpleVideoWnd;

        private GGUIWndHeroCommonCardItem _m_heroCardWnd;
        private GGUIWndHeroIconItem _m_heroIconWnd;
        private GGUISubWndMiddayDungeonMiniBox _m_miniBoxWnd;
        private GGUISubWndMiddayDungeonBox _m_boxWnd;
        private GGUIWndLongProgress _m_bossHpProgress;
        private NPGGUIWndCommonToggleEx _m_toggleAutoBattle;
        private bool _m_isAutoBattle = false;
        private MiddayDungeonBossInfo _m_bossInfo;
        private MiddayDungeonWaveRefObj _m_curWavRef;

        private bool _m_isAttacking = false;
        private bool _m_isDealingAttack = false;

        private long _m_lVideoMonitorSerializeOp = -1;
        private float _m_autoHideRewardTime = 2;
        private long _m_lBattleSerializeOp = -1;
        
        private int _m_timeDownSer;

    
        public GGUIWndMiddayDungeonBattle() : base(EALUIWndLayer.NORMAL)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoMiddayDungeonBattle.assetPath; }
        protected override string _monoObjName { get => GGUIMonoMiddayDungeonBattle.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        public override bool needDiscardOnSwitch { get { return true; } }

        protected override void _onShowWnd()
        {
            _refreshWnd();
            _m_heroCardWnd?.showWnd();
            _m_heroIconWnd?.showWnd();
            _showCutSceneVideo();
            WinMsg.RegisterMsgAct(WinMsgType.ON_MIDDAY_DUNGEON_STATE_END, _onMiddayDungeonStateEnd);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_MIDDAY_DUNGEON_BATTLE_ATTACK, _onBtnAttack);
        }
        
        protected override void _onHideWnd()
        {
            _m_lVideoMonitorSerializeOp = ALSerializeOpMgr.next();
            _m_lBattleSerializeOp = ALSerializeOpMgr.next();
            _m_timeDownSer = ALSerializeOpMgr.next();
            _setAttacking(false);
            _setDealingAttack(false);
            _m_bossHpProgress?.hideWnd();
            _m_heroCardWnd?.hideWnd();
            _m_heroIconWnd?.hideWnd();
            _m_wSimpleVideoWnd?.hideWnd();
            WinMsg.UnregisterMsgAct(WinMsgType.ON_MIDDAY_DUNGEON_STATE_END, _onMiddayDungeonStateEnd);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_MIDDAY_DUNGEON_BATTLE_ATTACK, _onBtnAttack);
        }
    
        protected override void _onReset()
        {
            _m_bossHpProgress?.discard();
            _m_bossHpProgress = null;
            _m_heroCardWnd?.resetWnd();
            _m_heroIconWnd?.resetWnd();
            
            if (_m_showCaseBoss != null) 
                _m_showCaseBoss.resetWnd();
        }
    
        protected override void _onDiscard()
        {
            _m_bossHpProgress?.discard();
            _m_bossHpProgress = null;
            
            _m_heroCardWnd?.discard();
            _m_heroCardWnd = null;
            
            _m_heroIconWnd?.discard();
            _m_heroIconWnd = null;
            
            if (_m_showCaseBoss != null) 
                _m_showCaseBoss.discard();
            _m_showCaseBoss = null;
            
            _m_toggleAutoBattle?.discard();
            _m_toggleAutoBattle = null;
            _m_wSimpleVideoWnd?.discard();
            _m_wSimpleVideoWnd = null;
            _m_miniBoxWnd?.discard();
            _m_miniBoxWnd = null;
            _m_boxWnd?.discard();
            _m_boxWnd = null;
            
            
            
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnAttack, _onBtnAttackClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnOneKeyBattle, _onBtnOneKeyBattleClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnSelectHero, _onBtnSelectHeroClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnGuildHero, _onBtnGuildHeroClick);
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
        
            if(null != wnd.bossHPSlider)
            {
                _m_bossHpProgress = new GGUIWndLongProgress(wnd.bossHPSlider);
            }
            
            if (null != wnd.toggleAuto)
            {
                _m_toggleAutoBattle = new NPGGUIWndCommonToggleEx(wnd.toggleAuto);
                _m_toggleAutoBattle.clickDelegate += _onToggleAutoBattleClick;
                _m_toggleAutoBattle.setSelected(_m_isAutoBattle);
            }

            if (wnd.monoBox)
            {
                _m_boxWnd = new GGUISubWndMiddayDungeonBox(wnd.monoBox, _hideBoxWnd);
            }
            if (wnd.monoMiniBox)
            {
                _m_miniBoxWnd = new GGUISubWndMiddayDungeonMiniBox(wnd.monoMiniBox, _onBtnShowBoxClick);
            }

            if (wnd.monoCurHeroCard)
                _m_heroCardWnd = new GGUIWndHeroCommonCardItem(wnd.monoCurHeroCard);
            
            if(wnd.monoCurHeroIcon)
                _m_heroIconWnd = new GGUIWndHeroIconItem(wnd.monoCurHeroIcon);
            
            
            if (wnd.monoShowcaseBoss != null) 
                _m_showCaseBoss = new NPGGUIWndCommonShowCase(wnd.monoShowcaseBoss);
            
            if(wnd.monoSimpleVideo != null)
                _m_wSimpleVideoWnd = new GGUIWndSimpleVideo(wnd.monoSimpleVideo);
            _m_autoHideRewardTime = wnd.autoHideRewardTime;
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnAttack, _onBtnAttackClick);
            ALUGUICommon.combineBtnClick(wnd.btnOneKeyBattle, _onBtnOneKeyBattleClick);
            ALUGUICommon.combineBtnClick(wnd.btnSelectHero, _onBtnSelectHeroClick);
            ALUGUICommon.combineBtnClick(wnd.btnGuildHero, _onBtnGuildHeroClick);
        }

        public void setInfo()
        {
            _m_curSelectHero = null;
            _m_curSelectGuildHero = null;
            _m_isAutoBattle = false;
            _updateBossInfo();
            _autoSelectCurHero();
            _refreshWnd();
        }

        private void _updateBossInfo()
        {
            _m_bossInfo = NPPlayer.instance.middayDungeonComp.bossInfo;
            if (_m_bossInfo != null)
            {
                _m_curWavRef = GRefdataCoreMgr.instance.middayDungeonWaveRefCore.getRef(_m_bossInfo.wave);
            }
            
        }
        
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            
            if (_m_toggleAutoBattle != null) 
                _m_toggleAutoBattle.setSelected(_m_isAutoBattle);
            
            if (_m_bossInfo != null)
            {
                if (_m_curWavRef != null)
                {
                    long deductedHp = _m_bossInfo.deductedHp;
                    string bossName = TextTranslate.instance.getLanguage(_m_curWavRef.boss_name);
                    ALUGUICommon.setLabelTxt(wnd.txtBossName, bossName);
                    ALUGUICommon.setLabelTxt(wnd.txtBossNameTMP, bossName);
                    
                    if (_m_bossHpProgress != null)
                    {
                        _m_bossHpProgress.showWnd();
                        _m_bossHpProgress.initSld(0, _m_curWavRef.boss_blood, _getCommonSliderTxtStr, EValueFormatType.NORMAL);
                        _m_bossHpProgress.setNowValue(deductedHp);
                    }
                    if (null != _m_showCaseBoss )
                    {
                        _m_showCaseBoss.showWnd(new ShowCaseCommonResUnitInfoObj(_m_curWavRef.boss_go_index));
                    }
                }

                int maxWave = GRefdataCoreMgr.instance.middayDungeonWaveRefCore.refList.Count;
                int wave = Math.Min(_m_bossInfo.wave, maxWave);
                ALUGUICommon.setLabelTxt(wnd.txtCurWave, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, wave, maxWave));
                ALUGUICommon.setGameObjEnable(wnd.goShowOnNoBoss, _m_curWavRef == null);
                ALUGUICommon.setGameObjEnable(wnd.goHideOnNoBoss, _m_curWavRef != null);
            }
 
            if (_m_boxWnd != null) 
                _m_boxWnd.hideWnd();
            
            if (_m_miniBoxWnd != null)
                _m_miniBoxWnd.showWnd();
            
            _m_timeDownSer = ALSerializeOpMgr.next();
            
            if(NPPlayer.instance.middayDungeonComp.isOpen)
                _refreshEndTimeDown(_m_timeDownSer);
            
            _updateSelectedHero();
        }


        private void _showCutSceneVideo()
        {
            bool isNewDay = AccountSettingMgr.instance.dailyTagSaver.isNewDay(DailyTagConst.MIDDAY_BATTLE_CUTSCENE);

            if (!isNewDay)
            {
                if (_m_curWavRef != null)
                {
                    if (_m_wSimpleVideoWnd != null)
                    {
                        _m_wSimpleVideoWnd.setVideoClip(_m_curWavRef.videoIndex);
                        _m_wSimpleVideoWnd.playVideoLoop();
                    }
                }
                wnd?.wndAnim?.Sample(wnd.cutSceneShowAniName, 1);
                ALCommonTaskController.CommonActionAddNextFrameTask(()=>
                {
                    WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.MIDDAY_DUNGEON_BATTLE_SHOW_DONE);
                });
                return;
            }
            AccountSettingMgr.instance.dailyTagSaver.setSaveToday(DailyTagConst.MIDDAY_BATTLE_CUTSCENE);
            _m_lVideoMonitorSerializeOp = ALSerializeOpMgr.next();
            long serializeOp = _m_lVideoMonitorSerializeOp;

            if (_m_wSimpleVideoWnd != null && wnd != null)
            {
                _m_wSimpleVideoWnd.showWnd();
                _m_wSimpleVideoWnd.setVideoClip(wnd.cutSceneVideoIndex);
                _m_wSimpleVideoWnd.playVideo();
                _m_wSimpleVideoWnd.monitorPlay(() =>
                {
                    if(serializeOp != _m_lVideoMonitorSerializeOp)
                        return;
                    if (_m_curWavRef != null)
                    {
                        _m_wSimpleVideoWnd.setVideoClip(_m_curWavRef.videoIndex);
                        _m_wSimpleVideoWnd.playVideoLoop();
                    }
                });
                wnd?.wndAnim?.Sample(wnd.cutSceneShowAniName, 0);
                ALCommonTaskController.CommonActionAddMonoTask(() =>
                {
                    wnd?.wndAnim?.Play(wnd.cutSceneShowAniName, () =>
                    {
                        WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.MIDDAY_DUNGEON_BATTLE_SHOW_DONE);
                    });
                }, wnd.cutSceneAnimDelay);
            }
        }

        private void _showAttack(_IHeroCardShow _heroInfo,long _serializeOp, bool _isReqSuc, MiddayDungeon_SettleInfo _battleInfo)
        {
            // 请求失败则刷新界面
            if (!_isReqSuc || wnd == null)
            {
                _m_isAutoBattle = false;
                _m_curSelectHero = null;
                _m_curSelectGuildHero = null;
                
                _autoSelectCurHero();
                _refreshWnd();
                return;
            }
            if (_m_bossInfo != null && _m_curWavRef != null)
            {
                //  如果boss信息和当前波次一致，则更新血量，否则说明boss已死亡，血量为0 进度为100
                if (_m_bossInfo.wave == _m_curWavRef.wave)
                {
                    long deductedHp = _m_bossInfo.deductedHp;
                    
                    if (_m_bossHpProgress != null)
                    {
                        _m_bossHpProgress.setNowValueChg(deductedHp, wnd.hpReduceTime, null);
                    }
                }
                else
                {
                    if (_m_bossHpProgress != null)
                    {
                        _m_bossHpProgress.setNowValueChg(_m_bossHpProgress.rightValue, wnd.hpReduceTime, null);
                    }
                }
            }

            string bubbleKey = _battleInfo.getIsDefeat() ? 
                GRefdataCoreMgr.instance.npGeneral.midday_dungeon_battle_bubble_defeat_list.GetRandomItem() 
                : GRefdataCoreMgr.instance.npGeneral.midday_dungeon_battle_bubble_no_defeat_list.GetRandomItem();
            ALUGUICommon.setLabelTxt(wnd.txtHeroBubble, TextTranslate.instance.getLanguage(bubbleKey));
            
            wnd?.wndAnim?.Sample(wnd.cutSceneShowAniName, 1);
            _setAttacking(true);
            _setDealingAttack(true);
            if (_heroInfo != null) 
                HeroVoiceMgr.instance.playVoice(_heroInfo.id, EHeroVoiceType.FIGHT);

            wnd.wndAnim.Play(wnd.attackAniName, () =>
            {
                if (_serializeOp != _m_lBattleSerializeOp)
                    return;
                _setAttacking(false);

                _showBattleResult(_serializeOp, _battleInfo, () =>
                {
                    if (_serializeOp != _m_lBattleSerializeOp)
                        return;
                    
                    _m_curSelectHero = null;
                    _m_curSelectGuildHero = null;
                    
                    if (_m_curWavRef != null)
                    {
                        _autoSelectHero(_serializeOp, () =>
                        {
                            if (_serializeOp != _m_lBattleSerializeOp)
                                return;
                            _refreshWnd();
                            _setDealingAttack(false);
                            if (_m_isAutoBattle)
                                ALCommonTaskController.CommonActionAddMonoTask(() =>
                                {
                                    if (_serializeOp != _m_lBattleSerializeOp)
                                        return;
                                    _autoBattle(_serializeOp);
                                }, wnd.autoBattleDelayTime);
                        });
                    }// 没有boss了，弹提示结束
                    else
                    {
                        _refreshWnd();
                        
                        _setDealingAttack(false);
                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.midday_dungeon_boss_has_clear_tip) ;
                        QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MIDDAY_DUNGEON_BATTLE);
                    }
                 
                });
            });
          
        }

        private void _setAttacking(bool _isAttacking)
        {
            _m_isAttacking = _isAttacking;
            ALUGUICommon.setGameObjEnable(wnd.goShowOnAttackShow, _m_isAttacking);
            ALUGUICommon.setGameObjEnable(wnd.goHideOnAttackShow, !_m_isAttacking);
        }

        private void _setDealingAttack(bool _isDealing)
        {
            _m_isDealingAttack = _isDealing;
        }
        
        private void _updateSelectedHero()
        {
            if(wnd == null)
                return;

            //默认隐藏没有大臣次数的提示
            ALUGUICommon.setGameObjEnable(wnd.goNoHeroButHaveGuildHeroShowList, false);
            ALUGUICommon.setGameObjEnable(wnd.goNoHeroAndNoHaveGuildHeroShowList, false);

            if (_m_curSelectHero != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtCurHeroName, GCommon.getItemName(ENPItemType.HERO, _m_curSelectHero.heroRefObj.id));
                _m_heroCardWnd?.setInfo(_m_curSelectHero);
                _m_heroIconWnd?.setData(_m_curSelectHero);
            }

            if (_m_curSelectGuildHero != null)
            {
                _m_curSelectGuildHero.regPlayerInfo(_info =>
                {
                    if (_info != null) 
                        ALUGUICommon.setLabelTxt(wnd.txtCurHeroName, _info.playerName + ":" + GCommon.getItemName(ENPItemType.HERO, _info.heroRefObj.id));
                });
                _m_heroCardWnd?.setInfo(_m_curSelectGuildHero);
                _m_heroIconWnd?.setData(_m_curSelectGuildHero);
            }
            if(_m_curSelectHero == null && _m_curSelectGuildHero == null)
            {
                ALUGUICommon.setGameObjEnable(wnd.goShowOnSelectedHero, false);
                ALUGUICommon.setGameObjEnable(wnd.goHideOnSelectedHero, true);

                //没有选择大臣，判断提示显示
                NPPlayerFixedCDInfo fixedCdInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(GRefdataCoreMgr.instance.npGeneral.midday_dungeon_can_borrow_guild_hero_num_fixed_cd_id);
                //如果还有联盟援助次数，并且加入了联盟，并且还有援助大臣，则显示还有联盟援助次数提示
                if (fixedCdInfo != null && NPPlayer.instance.guildComp.isJoinGuild() && fixedCdInfo.getCount() > 0)
                    ALUGUICommon.setGameObjEnable(wnd.goNoHeroButHaveGuildHeroShowList, true);
                else
                    ALUGUICommon.setGameObjEnable(wnd.goNoHeroAndNoHaveGuildHeroShowList, true);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.goShowOnSelectedHero, true);
                ALUGUICommon.setGameObjEnable(wnd.goHideOnSelectedHero, false);
            }
        }
        

        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="_"></param>
        private void _onBtnCloseClick(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MIDDAY_DUNGEON_BATTLE);
        }

        private void _onBtnAttack()
        {
            if (wnd != null) 
                _onBtnAttackClick(wnd.btnAttack);
        }
        /// <summary>
        /// 攻击按钮
        /// </summary>
        /// <param name="_"></param>
        private void _onBtnAttackClick(GameObject _)
        {
            if(_m_isDealingAttack)
                return;
            
            _m_lBattleSerializeOp =  ALSerializeOpMgr.next();

            long serializeOp = _m_lBattleSerializeOp;
            if (_m_curSelectHero != null)
            {
                NPPlayer.instance.middayDungeonComp.reqMiddayDungeonAttack(_m_curSelectHero.id, (_isSuc, _info) =>
                {
                    _showAttack(_m_curSelectHero, serializeOp, _isSuc, _info);
                });
                return;
            }

            if (_m_curSelectGuildHero != null)
            {
                NPPlayer.instance.middayDungeonComp.reqMiddayDungeonBorrowAttack(_m_curSelectGuildHero.cid, _m_curSelectGuildHero.id, (_isSuc, _info) =>
                {
                    _showAttack(_m_curSelectGuildHero, serializeOp, _isSuc, _info);
                });
                return;
            }
            
            // 没有选择大臣则自动选择大臣，并开始自动战斗
            if (_m_isAutoBattle)
            {
                _autoBattle(serializeOp);
                return;
            }
            
            if(_m_curSelectHero == null && _m_curSelectGuildHero == null)
            {
                NPPlayerFixedCDInfo fixedCdInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(GRefdataCoreMgr.instance.npGeneral.midday_dungeon_can_borrow_guild_hero_num_fixed_cd_id);
                HeroInfo hero = NPPlayer.instance.middayDungeonComp.getSelfMatchHero(0);
                // 如果我没有大臣可以出战，并且还有借用联盟大臣次数，则显示联盟大臣选择界面，否则显示我的大臣选择界面
                if (fixedCdInfo.getCount() > 0 && hero == null)
                {
                    _onBtnGuildHeroClick(null);
                }
                else
                {
                    _onBtnSelectHeroClick(null);
                }
            }
        }

        /// <summary>
        /// 开启自动战斗
        /// </summary>
        /// <param name="_toggle"></param>
        private void _onToggleAutoBattleClick(NPGGUIWndCommonToggleEx _toggle)
        {
            _m_isAutoBattle = !_m_isAutoBattle;
            if (_m_toggleAutoBattle != null) 
                _m_toggleAutoBattle.setSelected(_m_isAutoBattle);
       
        }

        private void _autoSelectCurHero()
        {
            if (_m_curSelectHero == null)
            {
                MiddayDungeonBossInfo bossInfo = NPPlayer.instance.middayDungeonComp.bossInfo;
                if (bossInfo != null)
                {
                    MiddayDungeonWaveRefObj wavRef =
                        GRefdataCoreMgr.instance.middayDungeonWaveRefCore.getRef(bossInfo.wave);
                    if (wavRef != null)
                    {
                        long bossHp = wavRef.boss_blood - bossInfo.deductedHp;
                        _m_curSelectHero = NPPlayer.instance.middayDungeonComp.getSelfMatchHero(bossHp);
                    }
                }
            }
        }

        private void _autoSelectHero(long _serializeOp, Action _onComplete)
        {
            if (_m_curSelectHero == null)
            {
                MiddayDungeonBossInfo bossInfo = NPPlayer.instance.middayDungeonComp.bossInfo;
                if (bossInfo != null)
                {
                    MiddayDungeonWaveRefObj wavRef = GRefdataCoreMgr.instance.middayDungeonWaveRefCore.getRef(bossInfo.wave);
                    if (wavRef != null)
                    {
                        long bossHp = wavRef.boss_blood - bossInfo.deductedHp;
                        _m_curSelectHero = NPPlayer.instance.middayDungeonComp.getSelfMatchHero(bossHp);
                    }
                }

                if (_m_curSelectHero == null)
                {
                    // NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.midday_dungeon_no_hero_can_fight_tip);
                    _onComplete?.Invoke();
                    return;
                }

                _updateSelectedHero();
                if (wnd != null && wnd.changeHeroAnim != null)
                    wnd.changeHeroAnim.Play(wnd.changeHeroAniName, () =>
                    {
                        if (_serializeOp != _m_lBattleSerializeOp)
                            return;
                        _onComplete?.Invoke();
                    });
                else
                    _onComplete?.Invoke();
            }
            else
            {
                _onComplete?.Invoke();
            }
        }

        /// <summary>
        /// 自动战斗
        /// </summary>
        private void _autoBattle(long _serializeOp)
        {
            if (!_m_isAutoBattle)
                return;

            _autoSelectHero(_serializeOp, () =>
            {
                if(_m_curSelectHero == null)
                    return;
                
                if (_serializeOp != _m_lBattleSerializeOp)
                    return;
                NPPlayer.instance.middayDungeonComp.reqMiddayDungeonAttack(_m_curSelectHero.id, (_isSuc, _info) =>
                {
                    if (_serializeOp != _m_lBattleSerializeOp)
                        return;
                    _showAttack(_m_curSelectHero, _serializeOp, _isSuc, _info);
                });
            });
        }

        private void _showBattleResult(long _serializeOp, Common.DungeonObj.MiddayDungeon_SettleInfo _info, Action _onComplete)
        {
            // 如果没有击败，则直接返回
            if (_info == null || !_info.getIsDefeat())
            {
                _onComplete?.Invoke();
                return;
            }
            ALStepCounter counter = new ALStepCounter();
            counter.resetAll();
            counter.chgTotalStepCount(1);
            counter.regAllDoneDelegate(() =>
            {
                // 如果击败了boss 则刷新boss信息
                _updateBossInfo();
                wnd?.wndAnim?.Play(wnd.sucToNextBattleAniName);
                if (_m_wSimpleVideoWnd != null && _m_curWavRef != null)
                {
                    _m_wSimpleVideoWnd.setVideoClip(_m_curWavRef.videoIndex);
                    _m_wSimpleVideoWnd.playVideoLoop();
                }
                _onComplete?.Invoke();
            });
            if (_m_curWavRef != null && _m_wSimpleVideoWnd != null)
            {
                if (wnd != null && wnd.wndAnim != null) 
                    wnd.wndAnim.Play(wnd.battleSucAniName);
                
                _m_lVideoMonitorSerializeOp = ALSerializeOpMgr.next();
                long videoSerializeOp = _m_lVideoMonitorSerializeOp;
                _m_wSimpleVideoWnd.setVideoClip(_m_curWavRef.finishVideoIndex);
                _m_wSimpleVideoWnd.playVideo();
                
                GCommon.showGainRewardTip(_info.getDefeatRewardList());
                ALCommonTaskController.CommonActionAddMonoTask(() =>
                {
                    if(videoSerializeOp != _m_lVideoMonitorSerializeOp)
                        return;
                    if(_serializeOp != _m_lBattleSerializeOp)
                        return;
                    
                    if (_info.getDropBoxId() > 0)
                    {
                        counter.chgTotalStepCount(1);
                        NPUINoticeMgr.instance.addDealer(new NPNoticeDealer_MiddayDungeonBoxGet(_info.getBoxRewardList(), _info.getDropBoxId(), counter.addDoneStepCount));
                    }
                    counter.addDoneStepCount();
                }, wnd.battleSucVideoStopDelay);
            }
            else
            {
                GCommon.showGainRewardTip(_info.getDefeatRewardList());
                if (_info.getDropBoxId() > 0)
                {
                    counter.chgTotalStepCount(1);
                    NPUINoticeMgr.instance.addDealer(new NPNoticeDealer_MiddayDungeonBoxGet(_info.getBoxRewardList(), _info.getDropBoxId(), counter.addDoneStepCount));
                }
                counter.addDoneStepCount();
            }
        }
        
        
        private void _onBtnOneKeyBattleClick(GameObject _)
        {
        }
        private void _onBtnSelectHeroClick(GameObject _)
        {
            GGUIWndMiddayDungeonSelectHero.instance.setInfo(EMiddayDungeonSelectHeroType.Self, _onSelectSelfHero, _onSelectGuildHero, _m_curSelectHero, _m_curSelectGuildHero);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMiddayDungeonSelectHero.instance, GGUIWndMiddayDungeonSelectHero.instance.showWnd, UINodeTagConst.C_MIDDAY_DUNGEON_SELECT_HERO);
        }
        private void _onBtnGuildHeroClick(GameObject _)
        {
            GGUIWndMiddayDungeonSelectHero.instance.setInfo(EMiddayDungeonSelectHeroType.Guild, _onSelectSelfHero, _onSelectGuildHero, _m_curSelectHero, _m_curSelectGuildHero);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMiddayDungeonSelectHero.instance, GGUIWndMiddayDungeonSelectHero.instance.showWnd, UINodeTagConst.C_MIDDAY_DUNGEON_SELECT_HERO);
        }
        
                
        private void _onBtnShowBoxClick()
        {
            if (_m_boxWnd != null)
            {
                _m_boxWnd.showWnd();
            }
            if (_m_miniBoxWnd != null)
            {
                _m_miniBoxWnd.hideWnd();
            }
        }     

        private void _hideBoxWnd()
        {
            if (_m_boxWnd != null)
            {
                _m_boxWnd.hideWnd();
            }
            if (_m_miniBoxWnd != null)
            {
                _m_miniBoxWnd.showWnd();
            }
        }
        
        
        private void _refreshEndTimeDown(int _timeDownSer)
        {
            if (null == wnd)
                return;
           
            ALUGUICommon.setLabelTxt(wnd.txtEndCd,  TextTranslate.instance.getLanguage(TransKeyConst.midday_dungeon_end_time_cd_desc, TimeUtil.millisecondsToTime_hms(NPPlayer.instance.middayDungeonComp.endTimeMs - FpsAndPingMgr.instance.serverTimeTag)));
            if (_timeDownSer != _m_timeDownSer)
                return;
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                _refreshEndTimeDown(_timeDownSer);
            },1f);
        }
        
        private string _getCommonSliderTxtStr(string _cur, string _max)
        {
            if (wnd != null)
            {
                long progress = 100;
                if (_m_bossHpProgress != null)
                    progress = (100* _m_bossHpProgress.nowValue / _m_bossHpProgress.rightValue);
                ALUGUICommon.setLabelTxt(wnd.txtProgressTMP, TextTranslate.instance.getLanguage(TransKeyConst.midday_dungeon_fight_process_num, progress));

                if (_m_bossHpProgress != null)
                {
                    string nowValueStr = GCommon.getValueFormatStr(EValueFormatType.NORMAL, _m_bossHpProgress.rightValue - _m_bossHpProgress.nowValue);
                    if(wnd.txtBossHp != null)
                        ALUGUICommon.setLabelTxt(wnd.txtBossHp, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, nowValueStr, _max));
                }
            }
            
            return TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, _cur, _max);
        }

        private void _onSelectSelfHero(HeroInfo _selfHeroInfo)
        {
            _m_curSelectHero = _selfHeroInfo;
            _m_curSelectGuildHero = null;
            _updateSelectedHero();
        }
        private void _onSelectGuildHero(MiddayDungeonGuildHeroInfo _guildHeroInfo)
        {
            _m_curSelectHero = null;
            _m_curSelectGuildHero = _guildHeroInfo;     
            _updateSelectedHero();
        }

        /// <summary>
        ///  如果刷新了副本时间，则说明副本时间到了，要退回enter界面
        /// </summary>
        private void _onMiddayDungeonStateEnd()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MIDDAY_DUNGEON_BATTLE);
        }
    }
}