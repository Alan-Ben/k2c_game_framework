using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndTowerBattlePVE : _ATALBasicUIWnd<GGUIMonoTowerBattlePVE>
    {
        private static GGUIWndTowerBattlePVE _g_instance = new GGUIWndTowerBattlePVE();
    
        public static GGUIWndTowerBattlePVE instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndTowerBattlePVE();
                return _g_instance;
            }
        }
        private TowerChallengeResult _m_result; // 战斗结果
        private TowerChapterRefObj _m_chapter; // 章节
        private int _m_level; // 章节Stage Level

        private NPGGuiWndTexture _m_towerIcon; // 塔icon
        private NPGGUIWndCommonToggleEx _m_toggleSpeedTen; // 10倍速
        private float _m_lastToggleSpeedTenClickTime = 0 ; // 10倍速开关上次点击时间
        private GGUISubWndCommonBlood _m_bossHpProgress;
        private long _m_curBossPower; // 当前Boss血量
        private long _m_maxBossPower; // 最大Boss血量
    
        private GGUIWndSimpleVideo _m_showCaseVideoBoss;//展示视频
        private TowerBattleHPTipCacheMgr _m_hpTipCache; // 血量提示缓存

        private ALProcess _m_pProcessObj; //战斗表现步骤管理对象
        private long _m_serialize; // 战斗表现序列号
        private Action _m_aniBattleEndAction; // 战斗结束动画回调

        public GGUIWndTowerBattlePVE() : base(EALUIWndLayer.NORMAL)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoTowerBattlePVE.assetPath; }
        protected override string _monoObjName { get => GGUIMonoTowerBattlePVE.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        public override bool needDiscardOnSwitch => true;

        protected override void _onShowWnd()
        {
            showBattle();
        }
    
        protected override void _onHideWnd()
        {          
            _m_towerIcon?.hideWnd();
            _m_bossHpProgress?.hideWnd();
            _m_showCaseVideoBoss?.hideWnd();
        
            if (_m_pProcessObj != null)
                _m_pProcessObj.discard();
            _m_pProcessObj = null;
            _m_serialize = ALSerializeOpMgr.next();
        }
    
        protected override void _onReset()
        {
            _m_showCaseVideoBoss?.resetWnd();
        }
    
        protected override void _onDiscard()
        {
            _m_toggleSpeedTen?.discard();
            _m_toggleSpeedTen = null;
            
            _m_towerIcon?.discard();
            _m_towerIcon = null;
        
            _m_bossHpProgress?.discard();
            _m_bossHpProgress = null;
            
            if (_m_showCaseVideoBoss != null) 
                _m_showCaseVideoBoss.discard();
            _m_showCaseVideoBoss = null;
            
            
            if (_m_pProcessObj != null)
                _m_pProcessObj.discard();
            _m_pProcessObj = null;
            
            if (_m_hpTipCache != null)
            {
                _m_hpTipCache.pushBackAllCacheItems();
                _m_hpTipCache.discard();
            }
            
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
                _m_bossHpProgress = new GGUISubWndCommonBlood(wnd.bossHPSlider);
            }
            if (wnd.monoShowcaseBoss != null) 
                _m_showCaseVideoBoss = new GGUIWndSimpleVideo(wnd.monoShowcaseBoss);
            
            if (wnd.hpTipPrefab != null && wnd.hpTipParent != null)
            {
                _m_hpTipCache = new TowerBattleHPTipCacheMgr(wnd.hpTipParent, 1, 8);
                _m_hpTipCache.init(wnd.hpTipPrefab);
            }
            
            wnd.onEventReduceHp = _onEventReduceHp;
            wnd.onEventBattleEnd = _onEventEndBattle;
            
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
        }

        private void showBattle()
        {
            _m_pProcessObj = ALProcess.CreateProcess("tower_battle_battle");
            _m_pProcessObj
                .addProcess(_refreshWnd) //刷新界面
                .addDelegateProcess(_showCutScene) //展示战斗前的剧情
                .addDelegateProcess(_startBattle) //展示伙伴增加实力
                .addDelegateProcess(_endBattle) //展示伙伴增加实力
                .deal();
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            long totalPower = NPPlayer.instance.heroComponent.totalPower;

            ALUGUICommon.setLabelTxt(wnd.txtLevelName, _m_chapter.getLevelName(_m_level));
            ALUGUICommon.setLabelTxt(wnd.txtBossName, TextTranslate.instance.getLanguage(_m_chapter.boss_name));
            ALUGUICommon.setLabelTxt(wnd.txtPlayerPower, totalPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            if (_m_towerIcon != null)
            {
                _m_towerIcon.showWnd();
                _m_towerIcon.setTexture(_m_chapter.boss_icon);
            }
            _m_toggleSpeedTen?.setSelected(AccountSettingMgr.instance.accountSetting.towerBattleSpeedTenTimes);
            if (_m_bossHpProgress != null)
            {
                _m_bossHpProgress.showWnd();
                long bossPower = _m_chapter.getBossPower(_m_result.targetLevel);
                _m_bossHpProgress.setBloodData(bossPower, bossPower);
            }
            //展示当前章节视频
            if (null != _m_showCaseVideoBoss )
            {
                _m_showCaseVideoBoss.showWnd();
                _m_showCaseVideoBoss.setVideoClip(_m_chapter.pve_go_index.GetRandomItem());
                _m_showCaseVideoBoss.playVideo(() =>
                {
                    _m_showCaseVideoBoss.reset();
                });
            }
        }

        private void _showCutScene(Action _onComplete)
        {
            _m_serialize = ALSerializeOpMgr.next();
            long serializeOp = _m_serialize;
            ALUGUICommon.setGameObjEnable(wnd.cutSceneShowGos, true);
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                if(serializeOp != _m_serialize)
                    return; //如果序列化操作不一致，则不执行后续操作
                _onComplete?.Invoke();
                ALUGUICommon.setGameObjEnable(wnd.cutSceneShowGos, false);
            }, AccountSettingMgr.instance.accountSetting.towerBattleSpeedTenTimes ? wnd.cutSceneDelay10x : wnd.cutSceneDelay);
        }
        
        /// <summary>
        /// 开始战斗
        /// </summary>
        private void _startBattle(Action _onComplete)
        {
            if (wnd == null || _m_bossHpProgress == null || _m_chapter == null || _m_result == null)
            {
                _onComplete?.Invoke();
                return;
            }
            _m_serialize = ALSerializeOpMgr.next();
            long serializeOp = _m_serialize;
          
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                if(serializeOp != _m_serialize)
                    return; //如果序列化操作不一致，则不执行后续操作
                _onComplete?.Invoke();
            }, wnd.maxEndDelay);
            
            float speed = AccountSettingMgr.instance.accountSetting.towerBattleSpeedTenTimes ? wnd.videoSpeedTenSpeed : 1;
            if (_m_showCaseVideoBoss != null)
            {
                _m_showCaseVideoBoss.setSpeed(speed);
                _m_showCaseVideoBoss.playVideo();
            }
            wnd.battleAnim.Play(wnd.battleAniName, _onComplete);
            _m_aniBattleEndAction = _onComplete;
            
            if (wnd != null && wnd.battleAnim != null) 
                wnd.battleAnim.SetAnimSpeed(wnd.battleAniName, speed);
            
            _m_maxBossPower = _m_chapter.getBossPower(_m_result.targetLevel);
            _m_curBossPower = _m_maxBossPower;
        }

        /// <summary>
        /// 当动画触发掉血
        /// </summary>
        private void _onEventReduceHp(float _percentage)
        {
            long targetPower = (long)Math.Floor(_m_maxBossPower * _percentage);
            long thisRoundDamage = Math.Max(_m_curBossPower - targetPower, 0);
            _m_curBossPower -= thisRoundDamage;
            
            _m_bossHpProgress?.damage(thisRoundDamage);
            _showHpTip(TextTranslate.instance.getLanguage(TransKeyConst.common_reduced_num, thisRoundDamage.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            
        }

        private void _onEventEndBattle()
        {
            _m_serialize = ALSerializeOpMgr.next();
            _m_aniBattleEndAction?.Invoke();
        }

        /// <summary>
        /// 结束战斗
        /// </summary>
        private void _endBattle(Action _onComplete)
        {
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
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TOWER_BATTLE_PVE);
            _onComplete?.Invoke();
        }
        
        private void _onBtnJumpClick(GameObject _go)
        {
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
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TOWER_BATTLE_PVE);
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
        
        private void _onToggleClick(NPGGUIWndCommonToggleEx obj)
        {
            if (null == wnd || null == _m_toggleSpeedTen || null == obj)
                return;
            if(!GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.tower_accelerate_battle_simple_unlock_id, true))
                return;
            if(Time.realtimeSinceStartup - _m_lastToggleSpeedTenClickTime < wnd.toggleSpeedTenInterval)
                return;
            _m_lastToggleSpeedTenClickTime = Time.realtimeSinceStartup;
            
            _m_toggleSpeedTen.setSelected(!obj.isOn);
            AccountSettingMgr.instance.accountSetting.setTowerBattleSpeedTenTimes(_m_toggleSpeedTen.isOn);
            float speed = AccountSettingMgr.instance.accountSetting.towerBattleSpeedTenTimes ? wnd.videoSpeedTenSpeed : 1;
            _m_showCaseVideoBoss?.setSpeed(speed);
            if (wnd != null && wnd.battleAnim != null) 
                wnd.battleAnim.SetAnimSpeed(wnd.battleAniName, speed);
        }
        private string _getCommonSliderTxtStr(string _cur, string _max)
        {
            return TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, _cur, _max);
        }
    }
}