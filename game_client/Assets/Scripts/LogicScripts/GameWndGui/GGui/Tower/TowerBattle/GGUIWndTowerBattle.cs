using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndTowerBattle : _ATALBasicUIWnd<GGUIMonoTowerBattle>
    {
        private static GGUIWndTowerBattle _g_instance = new GGUIWndTowerBattle();

        public static GGUIWndTowerBattle instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndTowerBattle();
                return _g_instance;
            }
        }
    
    
        private class AttackInfo
        {
            public long curHp;
            public long power;
            public int heroCount;
            public long heroCurPower;
            public bool isEnd;
            public AttackInfo(long _curHp, long _power, int _heroCount, long _heroCurPower, bool _isEnd = false)
            {
                curHp = _curHp;
                power = _power;
                heroCount = _heroCount;
                heroCurPower = _heroCurPower;
                isEnd = _isEnd;
            }
        }
    
        private ALProcess _m_pProcessObj; //战斗表现步骤管理对象
        private long _m_serialize; 
    
        private TowerChallengeResult _m_result; // 战斗结果
        private TowerChapterRefObj _m_chapter; // 章节
        private int _m_level; // 章节Stage Level
    
        private NPGGUIWndPlayerIcon _m_playerInfo; //玩家信息
        private NPGGuiWndTexture _m_towerIcon; // 塔icon
        private NPGGUIWndCommonToggleEx _m_toggleSpeedTen; // 10倍速
        private GGUIWndLongProgress _m_bossHpProgress;
        private GGUIWndLongProgress _m_heroCountProgress;
        private GGUIWndLongProgress _m_heroPowerProgress;
    
        private GGUIWndTowerBattleInspireCache _m_inspireWndLeftCache; // 左气泡缓存
        private GGUIWndTowerBattleInspireCache _m_inspireWndRightCache;// 右气泡缓存
        private TowerBattleHPTipCacheMgr _m_hpTipCache; // 血量提示缓存
    
        private int _m_heroAllCount; // 英雄数量
        private long _m_BossFullHP; // boss满血量
        private long _m_BossCurHP; // 当前boss血量
        
        private long _m_HeroCurPower; // 大臣当前剩余战力
        private List<HeroInfo> _m_heroList;
        private TowerBattleInspireGroup _m_curInspireGroup;
        private List<AttackInfo> _m_attackInfoList;

        public GGUIWndTowerBattle() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoTowerBattle.assetPath; }
        protected override string _monoObjName { get => GGUIMonoTowerBattle.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }


        protected override void _onShowWnd()
        {       
            _m_serialize = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            _m_towerIcon?.hideWnd();
        
            _m_bossHpProgress?.hideWnd();
            _m_heroCountProgress?.hideWnd();
            _m_heroPowerProgress?.hideWnd();
        
            _m_inspireWndLeftCache?.pushBackAllCacheItems();
            _m_inspireWndRightCache?.pushBackAllCacheItems();
            if (_m_curInspireGroup != null)
            {
                ALUGUICommon.setGameObjEnable(_m_curInspireGroup, false);
                _m_curInspireGroup.onEventReduceHp = null;
                _m_curInspireGroup = null;
            }
        
            _m_serialize = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
            _m_towerIcon?.discardTexture();
        
            _m_bossHpProgress?.discard();
            _m_bossHpProgress = null;
            _m_heroCountProgress?.discard();
            _m_heroCountProgress = null;
            _m_heroPowerProgress?.discard();
            _m_heroPowerProgress = null;
        
            _m_inspireWndLeftCache?.pushBackAllCacheItems();
            _m_inspireWndRightCache?.pushBackAllCacheItems();
            if (_m_curInspireGroup != null)
            {
                ALUGUICommon.setGameObjEnable(_m_curInspireGroup, false);
                _m_curInspireGroup.onEventReduceHp = null;
                _m_curInspireGroup = null;
            }
                 
            if (_m_pProcessObj != null)
                _m_pProcessObj.discard();
            _m_pProcessObj = null;
            _m_serialize = ALSerializeOpMgr.next();
        }

        protected override void _onDiscard()
        {
            _m_heroList = null;
        
            _m_towerIcon?.discard();
            _m_towerIcon = null;
        
            _m_bossHpProgress?.discard();
            _m_bossHpProgress = null;
            
            _m_playerInfo?.discard();
            _m_playerInfo = null;

            _m_heroCountProgress?.discard();
            _m_heroCountProgress = null;
            _m_heroPowerProgress?.discard();
            _m_heroPowerProgress = null;

            if (_m_hpTipCache != null)
            {
                _m_hpTipCache.pushBackAllCacheItems();
                _m_hpTipCache.discard();
            }

            if (_m_inspireWndLeftCache != null)
            {
                _m_inspireWndLeftCache.pushBackAllCacheItems();
                _m_inspireWndLeftCache.discard();
            }
            if (_m_inspireWndRightCache != null)
            {
                _m_inspireWndRightCache.pushBackAllCacheItems();
                _m_inspireWndRightCache.discard();
            }

            _m_serialize = ALSerializeOpMgr.next();
                
            _m_inspireWndLeftCache?.pushBackAllCacheItems();
            _m_inspireWndRightCache?.pushBackAllCacheItems();
            if (_m_curInspireGroup != null)
            {
                ALUGUICommon.setGameObjEnable(_m_curInspireGroup, false);
                _m_curInspireGroup.onEventReduceHp = null;
                _m_curInspireGroup = null;
            }
         
            if (_m_pProcessObj != null)
                _m_pProcessObj.discard();
            _m_pProcessObj = null;

            _m_heroList = null;
            _m_attackInfoList = null;
            _m_serialize = ALSerializeOpMgr.next();

            ALUGUICommon.uncombineBtnClick(wnd.btnJump, _onBtnJumpClick);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            if (wnd.texIcon != null)
                _m_towerIcon = new NPGGuiWndTexture(wnd.texIcon);
        
            if (null != wnd.toggleSpeedTen)
            {
                _m_toggleSpeedTen = new NPGGUIWndCommonToggleEx(wnd.toggleSpeedTen);
                _m_toggleSpeedTen.clickDelegate += _onToggleClick;
                _m_toggleSpeedTen.setSelected(false);
            }
        
            if(null != wnd.bossHPSlider)
            {
                _m_bossHpProgress = new GGUIWndLongProgress(wnd.bossHPSlider);
            }
            
            if (wnd.playerInfo !=null) _m_playerInfo = new NPGGUIWndPlayerIcon(wnd.playerInfo);

            if (null != wnd.heroCountSlider)
            {
                _m_heroCountProgress = new GGUIWndLongProgress(wnd.heroCountSlider);
            }
            
            if (null != wnd.heroPowerSlider)
            {
                _m_heroPowerProgress = new GGUIWndLongProgress(wnd.heroPowerSlider);
            }

            if (wnd.hpTipPrefab != null && wnd.hpTipParent != null)
            {
                _m_hpTipCache = new TowerBattleHPTipCacheMgr(wnd.hpTipParent, 1, 8);
                _m_hpTipCache.init(wnd.hpTipPrefab);
            }
        
            if (wnd.inspireLeftPrefab != null && wnd.inspireCacheRoot != null)
            {
                _m_inspireWndLeftCache = new GGUIWndTowerBattleInspireCache(wnd.inspireCacheRoot, 1, 8);
                _m_inspireWndLeftCache.init(wnd.inspireLeftPrefab);
            }
            if (wnd.inspireRightPrefab != null && wnd.inspireCacheRoot != null)
            {
                _m_inspireWndRightCache = new GGUIWndTowerBattleInspireCache(wnd.inspireCacheRoot, 1, 8);
                _m_inspireWndRightCache.init(wnd.inspireRightPrefab);
            }

            if (wnd.inspireGroupList != null)
                foreach (TowerBattleInspireGroup inspireGroup in wnd.inspireGroupList)
                {
                    ALUGUICommon.setGameObjEnable(inspireGroup, false);
                }
        
            _m_heroList = new List<HeroInfo>();
            _m_attackInfoList = new List<AttackInfo>();
        
            _m_serialize = ALSerializeOpMgr.next();
        
            ALUGUICommon.combineBtnClick(wnd.btnJump, _onBtnJumpClick);
        }

        /// <summary>
        /// 设置爬塔战斗信息，并开始战斗
        /// </summary>
        /// <param name="_result"></param>
        public void setInfo(TowerChallengeResult _result)
        {
            if(_result == null)
                return;
            _m_result = _result;
            _m_chapter = GRefdataCoreMgr.instance.towerChapterRefCore?.getRef(_m_result.targetChapterId);
            _m_level = _m_result.targetLevel;
            _m_heroList.Clear();
            _m_attackInfoList.Clear();
            NPPlayer.instance.heroComponent.getAllList(_m_heroList);
            _m_heroAllCount = _m_heroList.Count;
            _m_BossCurHP = _m_BossFullHP = _m_chapter.getBossPower(_m_result.targetLevel);
            _m_HeroCurPower = NPPlayer.instance.heroComponent.totalPower;
            _m_pProcessObj = ALProcess.CreateProcess("tower_battle_battle");
            _m_pProcessObj
                .addDelegateProcess(showCutscene) //展示最终结果
                .addProcess(_refreshWnd) //刷新界面
                .addDelegateProcess(_startBattle) //展示伙伴增加实力
                .addDelegateProcess(_endBattle) //展示伙伴增加实力
                .deal(); 
        }

        /// <summary>
        /// 显示战斗开场
        /// </summary>
        /// <param name="_onComplete"></param>
        private void showCutscene(Action _onComplete)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndTowerBattleShow.instance, () =>
            {
                GGUIWndTowerBattleShow.instance.showWnd();
                GGUIWndTowerBattleShow.instance.refreshWnd(_m_result, _onComplete);
            }, UINodeTagConst.C_TOWER_BATTLE_SHOW);
        }

        /// <summary>
        /// 开始战斗
        /// </summary>
        private void _startBattle(Action _onComplete)
        {
            if (wnd == null) return;
            _m_serialize = ALSerializeOpMgr.next();
            long serializeOp = _m_serialize;
            float firstDelay = AccountSettingMgr.instance.accountSetting.towerBattleSpeedTenTimes
                ? wnd.firstInspire10xDelay
                : wnd.firstInspireDelay;
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                _showInspire(serializeOp, _onComplete);
                GTowerBattleSceneMgr.instance.showTalkSFX();
            }, firstDelay);
        }
    
        /// <summary>
        /// 显示大臣鼓舞
        /// </summary>
        /// <param name="_serialize"></param>
        private void _showInspire(long _serialize, Action _onComplete)
        {
            if (wnd == null || _m_heroList == null || _m_inspireWndLeftCache == null || _m_inspireWndRightCache == null)
            {
                _onComplete?.Invoke();
                return;
            }
        
            _m_inspireWndLeftCache.pushBackAllCacheItems();
            _m_inspireWndRightCache.pushBackAllCacheItems();

            if (_m_curInspireGroup != null)
            {
                ALUGUICommon.setGameObjEnable(_m_curInspireGroup, false);
                _m_curInspireGroup.onEventReduceHp = null;
                _m_curInspireGroup = null;
            }
            _m_curInspireGroup = wnd.inspireGroupList.GetRandomItem();
            if (_m_curInspireGroup == null || _m_curInspireGroup.inspireParentList == null)
            {
                _onComplete?.Invoke();
                return;
            }        
            _m_curInspireGroup.onEventReduceHp = _onEventReduceHp;

            if (_m_curInspireGroup.showAnim != null)
                _m_curInspireGroup.showAnim.Play(AccountSettingMgr.instance.accountSetting.towerBattleSpeedTenTimes
                    ? _m_curInspireGroup.showAnim10xName
                    : _m_curInspireGroup.showAnimName);
            int count = Math.Min(_m_curInspireGroup.inspireParentList.Count, _m_heroList.Count);
            // 每个战斗组重置攻击信息
            if(_m_attackInfoList != null)
                _m_attackInfoList.Clear();
            // 给这个组设置英雄信息，并延迟执行掉血
            for (var i = 0; i < count; i++)
            {
                TowerBattleInspire inspire = _m_curInspireGroup.inspireParentList[i];
                HeroInfo heroInfo = _m_heroList.GetFirstAndRemove();
            
                if (inspire == null || heroInfo == null) continue;
                GGUIWndTowerBattleInspire inspireWnd = inspire.isLeft ? _m_inspireWndLeftCache.popItem() : _m_inspireWndRightCache.popItem();
                if (inspireWnd != null)
                {
                    inspireWnd.setInfo(heroInfo, inspire.parent, wnd.getRandomInspireDesc());
                    inspireWnd.showWnd();
                }


                long power = Math.Min(_m_BossCurHP, heroInfo.power);
                _m_BossCurHP = _m_BossCurHP - power;
                _m_HeroCurPower = _m_HeroCurPower - power;

                if (_m_BossCurHP <= 0)
                {
                    // 增加攻击信息到列表中，用于后续动画显示
                    if (_m_attackInfoList != null)
                        _m_attackInfoList.Add(new AttackInfo(_m_BossCurHP, heroInfo.power, _m_heroList.Count, _m_HeroCurPower, true));
                    // 如果boss血量小于等于0，停止增加攻击信息
                    break;
                }
                else
                {
                    // 增加攻击信息到列表中，用于后续动画显示
                    if (_m_attackInfoList != null)
                        _m_attackInfoList.Add(new AttackInfo(_m_BossCurHP, heroInfo.power, _m_heroList.Count, _m_HeroCurPower));
                }
            }

            ALUGUICommon.setGameObjEnable(_m_curInspireGroup, true);

            float groupDelay = AccountSettingMgr.instance.accountSetting.towerBattleSpeedTenTimes
                ? wnd.inspireGroup10xDelay.getRandomValue()
                : wnd.inspireGroupDelay.getRandomValue();
        
            // 延迟显示下一个组大臣鼓舞
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                if (_serialize != _m_serialize)
                    return;
                if (_m_BossCurHP <= 0)
                {
                    _m_BossCurHP = 0;
                    _onComplete?.Invoke();
                    return;
                }
                if (_m_heroList == null || _m_heroList.Count <= 0)
                {
                    _onComplete?.Invoke();
                    return;
                }
                _showInspire(_serialize, _onComplete);
            }, groupDelay);
        }
    
        /// <summary>
        /// 结束战斗
        /// </summary>
        private void _endBattle(Action _onComplete)
        {
            GTowerBattleSceneMgr.instance.hideTalkSFX();
            if (_m_result == null)
            {
                _onComplete?.Invoke();
                return;
            }
            if (_m_result.isSuc)
            {
                NPUINoticeMgr.instance.addDealer(new NPNoticeDealer_TowerBattleSuc(_m_result));
                if(_m_result.oldChapterId != _m_result.targetChapterId)
                    NPUINoticeMgr.instance.addDealer(new NPNoticeDealer_TowerBattleCrossChapter(_m_result.targetChapterId));
            }
            else
            {
                NPUINoticeMgr.instance.addDealer(new NPNoticeDealer_TowerBattleFail(_m_result));
            }
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TOWER_BATTLE);
            _onComplete?.Invoke();
        }
    
        /// <summary>
        /// 显示Boss掉血Tip
        /// </summary>
        /// <param name="_hpTip"></param>
        private void _showHpTip(string _hpTip)
        {
            if(_m_hpTipCache == null)
                return;
            GGUIWndTowerBattleHPTip tempItemWnd = _m_hpTipCache.popItem();
            if (tempItemWnd == null)
                return;
            tempItemWnd.showWnd(_hpTip, () => { _m_hpTipCache.pushBackCacheItem(tempItemWnd); });
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd || null == _m_chapter)
                return;

            long totalPower = NPPlayer.instance.heroComponent.totalPower;
            ALUGUICommon.setLabelTxt(wnd.txtLevelName, _m_chapter.getLevelName(_m_level));
            ALUGUICommon.setLabelTxt(wnd.txtBossName, TextTranslate.instance.getLanguage(_m_chapter.boss_name));
            ALUGUICommon.setLabelTxt(wnd.txtPlayerPower, totalPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            ALUGUICommon.setLabelTxt(wnd.txtHeroNum, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, _m_heroAllCount, _m_heroAllCount));
            if (_m_towerIcon != null)
            {
                _m_towerIcon.showWnd();
                _m_towerIcon.setTexture(_m_chapter.boss_icon);
            }
            
            if (null != _m_playerInfo)
            {
                _m_playerInfo.showWnd();
                _m_playerInfo.setSelfInfo();
            }
            _m_toggleSpeedTen?.setSelected(AccountSettingMgr.instance.accountSetting.towerBattleSpeedTenTimes);
            bool canSpeedTen = GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.tower_accelerate_battle_simple_unlock_id);
            ALUGUICommon.setGameObjEnable(wnd.goSpeedTenShowList, canSpeedTen);
            ALUGUICommon.setGameObjEnable(wnd.goSpeedTenHideList, !canSpeedTen);
        
            bool canSkip = GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.tower_skip_battle_simple_unlock_id);
            ALUGUICommon.setGameObjEnable(wnd.goSkipShowList, canSkip);
            ALUGUICommon.setGameObjEnable(wnd.goSkipHideList, !canSkip);
        

            if (_m_bossHpProgress != null)
            {
                _m_bossHpProgress.showWnd();
                _m_bossHpProgress.initSld(0, _m_BossFullHP, _getCommonSliderTxtStr, EValueFormatType.NORMAL);
                _m_bossHpProgress.setNowValue(_m_BossFullHP);
            }
            
            if(_m_heroCountProgress != null)
            {
                _m_heroCountProgress.showWnd();
                _m_heroCountProgress.initSld(0, _m_heroAllCount, _getCommonSliderTxtStr, EValueFormatType.NORMAL);
                _m_heroCountProgress.setNowValue(_m_heroAllCount);
            }

            if (_m_heroPowerProgress != null)
            {
                _m_heroPowerProgress.showWnd();
                _m_heroPowerProgress.initSld(0, totalPower, _getCommonSliderTxtStr, EValueFormatType.NORMAL);
                _m_heroPowerProgress.setNowValue(totalPower);
            }

        }
    
        /// <summary>
        /// 当动画触发掉血
        /// </summary>
        private void _onEventReduceHp()
        {
            if(wnd == null)
                return;
            AttackInfo attackInfo = _m_attackInfoList.GetFirstAndRemove();
            if(attackInfo == null)
                return;
       
            float aniTime = AccountSettingMgr.instance.accountSetting.towerBattleSpeedTenTimes ? wnd.bossHPSlider10xDuration : wnd.bossHPSliderDuration;
        
            _m_bossHpProgress?.setNowValueChg(attackInfo.curHp, aniTime, null);
            _m_heroCountProgress?.setNowValueChg(attackInfo.heroCount, aniTime, null);
            _m_heroPowerProgress?.setNowValueChg(attackInfo.heroCurPower, aniTime, null);
        
            _showHpTip(TextTranslate.instance.getLanguage(TransKeyConst.common_reduced_num, attackInfo.power.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
                
            ALUGUICommon.setLabelTxt(wnd.txtHeroNum, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, attackInfo.heroCount, _m_heroAllCount));
        }

        private void _onBtnJumpClick(GameObject _go)
        {
            GTowerBattleSceneMgr.instance.hideTalkSFX();
            if (null == _m_result)
                return;
            if(!GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.tower_skip_battle_simple_unlock_id, true))
                return;
            if (_m_result.isSuc)
            {
                NPUINoticeMgr.instance.addDealer(new NPNoticeDealer_TowerBattleSuc(_m_result));
                if(_m_result.oldChapterId != _m_result.targetChapterId)
                    NPUINoticeMgr.instance.addDealer(new NPNoticeDealer_TowerBattleCrossChapter(_m_result.targetChapterId));
            }
            else
            {
                NPUINoticeMgr.instance.addDealer(new NPNoticeDealer_TowerBattleFail(_m_result));
            }
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TOWER_BATTLE);
        }
    
        private void _onToggleClick(NPGGUIWndCommonToggleEx obj)
        {
            if (null == _m_toggleSpeedTen || null == obj)
                return;
            if(!GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.tower_accelerate_battle_simple_unlock_id, true))
                return;
            _m_toggleSpeedTen.setSelected(!obj.isOn);
            AccountSettingMgr.instance.accountSetting.setTowerBattleSpeedTenTimes(_m_toggleSpeedTen.isOn);
            //TODO:设置战斗速度
        }
    
        private string _getCommonSliderTxtStr(string _cur, string _max)
        {
            return TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, _cur, _max);
        }
    }
}