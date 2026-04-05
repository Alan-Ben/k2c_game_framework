using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// boss战主界面
    /// </summary>
    public partial class GGUIWndChapterBossMain : _ATALBasicUIWnd<GGUIMonoChapterBossMain>, _IShakable
    {
        private static GGUIWndChapterBossMain _g_instance = new GGUIWndChapterBossMain();

        public static GGUIWndChapterBossMain instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndChapterBossMain();
                return _g_instance;
            }
        }
    
    
        //当前的表现状态，为了后面复杂的boss战流程预留
        private EChapterBossWndType _m_curWndType;
        
        private ALProcess _m_pProcessObj; //战斗表现步骤管理对象
        private long _m_serialize; 
        
        private ChapterRefObj _m_chapterRef;//关卡信息
        private NPGGUIWndPlayerIcon _m_playerInfo; //玩家信息
        private NPGGuiWndTexture _m_bossIcon; // boss的icon
        private GGUISubWndCommonBlood _m_commonBloodBoss;//1阶段血条
        private GGUISubWndCommonBlood _m_commonBloodBossSecond;//2阶段血条
 
    
        private GGUIWndChapterBossHeroInspireCache _m_inspireHeroWndache; // 大臣item缓存
        private GGUIWndChapterBossHPTipCache _m_hpTipCache; // 血量提示缓存
            
        private int _m_heroAllCount; // 英雄数量
        
        //一阶段出战大臣列表
        [NotNull]private List<HeroInfo> _m_heroListFirstStage = new List<HeroInfo>();
        //二阶段出战大臣列表
        [NotNull]private List<HeroInfo> _m_heroListSecondStage = new List<HeroInfo>();

        private long _m_firstBossPower; //一阶段boss血量
        private long _m_secondBossPower; //二阶段boss血量
        
        [NotNull]private List<GGUIWndChapterBossHeroInspire> _m_firstHeroInspireItemWndList = new List<GGUIWndChapterBossHeroInspire>();
        [NotNull]private List<GGUIWndChapterBossHeroInspire> _m_secondHeroInspireItemWndList = new List<GGUIWndChapterBossHeroInspire>();

        //奖励列表
        private NPGGUIWndCommonItemContainer _m_wRewardContainer;
        private long _m_bgMusicInstanceId;//背景音乐id
        
        // 震动任务的序列号
        private int _m_iShakeTaskSerialize;
        // 震动的原点坐标
        private Vector3 _m_vShakeOriginPos;
        
        public int taskSerialize { get { return _m_iShakeTaskSerialize; } }
        public Vector3 shakeOriginPos { get { return _m_vShakeOriginPos; } }
        public RectTransform shakeRoot { get { return wnd?.transShakeRoot; } }
        
        public GGUIWndChapterBossMain() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoChapterBossMain.assetPath; }
        protected override string _monoObjName { get => GGUIMonoChapterBossMain.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }


        protected override void _onShowWnd()
        {       
            _m_curWndType = EChapterBossWndType.NONE;
            _m_serialize = ALSerializeOpMgr.next();
            
            //播放开始的bgm
            if (NPPlayer.instance.chapterComp.chapterRefObj?.chapter_boss_bg_music_id > 0)
            {
                _m_bgMusicInstanceId = PlayAudioMgr.instance.playBackgroundMusic(NPPlayer.instance.chapterComp.chapterRefObj.chapter_boss_bg_music_id);
            }
        }

        protected override void _onHideWnd()
        {
            _m_iShakeTaskSerialize = ALSerializeOpMgr.next();
            _m_curWndType = EChapterBossWndType.NONE;

            _m_bossIcon?.hideWnd();
            
            _m_commonBloodBoss?.hideWnd();
            _m_commonBloodBossSecond?.hideWnd();
            
            if (_m_wRewardContainer != null) 
                _m_wRewardContainer.hideWnd();

            _m_inspireHeroWndache?.pushBackAllCacheItems();
            _m_firstHeroInspireItemWndList.Clear();
            _m_secondHeroInspireItemWndList.Clear();
            
            if (_m_pProcessObj != null)
                _m_pProcessObj.discard();
            _m_pProcessObj = null;
            _m_serialize = ALSerializeOpMgr.next();
            
            //恢复之前的bgm
            if(_m_bgMusicInstanceId > 0)
                PlayAudioMgr.instance.stopBackgroundMusicByInstanceID(_m_bgMusicInstanceId, true);
            _m_bgMusicInstanceId = 0;
        }

        protected override void _onReset()
        {
            _m_curWndType = EChapterBossWndType.NONE;

            _m_bossIcon?.discardTexture();

            _m_wRewardContainer?.resetWnd();
            
            _m_commonBloodBoss?.resetWnd();
            _m_commonBloodBossSecond?.resetWnd();
        
            _m_inspireHeroWndache?.pushBackAllCacheItems();
            _m_firstHeroInspireItemWndList.Clear();
            _m_secondHeroInspireItemWndList.Clear();

            if (_m_pProcessObj != null)
                _m_pProcessObj.discard();
            _m_pProcessObj = null;
            _m_serialize = ALSerializeOpMgr.next();
        }

        protected override void _onDiscard()
        {
            _m_iShakeTaskSerialize = ALSerializeOpMgr.next();
            _m_curWndType = EChapterBossWndType.NONE;
            
            _m_wRewardContainer?.discard();
            _m_wRewardContainer = null;
            
            _m_bossIcon?.discard();
            _m_bossIcon = null;
        
            _m_commonBloodBoss?.discard();
            _m_commonBloodBoss = null;
            
            _m_commonBloodBossSecond?.discard();
            _m_commonBloodBossSecond = null;
            
            _m_playerInfo?.discard();
            _m_playerInfo = null;
            
            if (_m_hpTipCache != null)
            {
                _m_hpTipCache.pushBackAllCacheItems();
                _m_hpTipCache.discard();
            }

            if (_m_inspireHeroWndache != null)
            {
                _m_inspireHeroWndache.pushBackAllCacheItems();
                _m_inspireHeroWndache.discard();
            }
         
            if (_m_pProcessObj != null)
                _m_pProcessObj.discard();
            _m_pProcessObj = null;

            _m_serialize = ALSerializeOpMgr.next();

            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnJump, _onBtnJumpClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickCloseBtn);
            }
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            if (wnd.imgBossIcon != null)
                _m_bossIcon = new NPGGuiWndTexture(wnd.imgBossIcon);
        
            if (null != wnd.monoRewardContainer)
                _m_wRewardContainer = new NPGGUIWndCommonItemContainer(wnd.monoRewardContainer);
        
            if(null != wnd.bossHPSlider)
            {
                _m_commonBloodBoss = new GGUISubWndCommonBlood(wnd.bossHPSlider);
            }

            if (wnd.bossHPSliderSecond != null)
                _m_commonBloodBossSecond = new GGUISubWndCommonBlood(wnd.bossHPSliderSecond);

            if (wnd.playerInfo !=null) _m_playerInfo = new NPGGUIWndPlayerIcon(wnd.playerInfo);

            if (wnd.hpTipPrefab != null && wnd.hpTipCacheRoot != null)
            {
                _m_hpTipCache = new GGUIWndChapterBossHPTipCache(wnd.hpTipCacheRoot, 1, 8);
                _m_hpTipCache.init(wnd.hpTipPrefab);
            }
        
            if (wnd.heroInspirePrefab != null && wnd.inspireCacheRoot != null)
            {
                _m_inspireHeroWndache = new GGUIWndChapterBossHeroInspireCache(wnd.inspireCacheRoot, 1, 8);
                _m_inspireHeroWndache.init(wnd.heroInspirePrefab);
            }
            
            if (wnd.transShakeRoot != null)
                _m_vShakeOriginPos = wnd.transShakeRoot.anchoredPosition;
            
            _m_serialize = ALSerializeOpMgr.next();
        
            ALUGUICommon.combineBtnClick(wnd.btnJump, _onBtnJumpClick);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickCloseBtn);
        }

        /// <summary>
        /// 设置爬塔战斗信息，并开始战斗
        /// </summary>
        /// <param name="_result"></param>
        public void setInfo(ChapterRefObj _chapterRef)
        {
            if(_chapterRef == null)
                return;
            _m_chapterRef = _chapterRef;
            _m_curWndType = EChapterBossWndType.NONE;
            
            _m_pProcessObj = ALProcess.CreateProcess("tower_battle_battle");
            _m_pProcessObj
                .addProcess(_checkPower) //校验战斗力
                .addProcess(_initInfo) //初始化设置
                .addProcess(_refreshWnd) //刷新界面
                .addDelegateProcess(_startDealy) //开始延迟
                .addDelegateProcess(_startBattleFirst) //开始一阶段战斗
                .addDelegateProcess(_showSecondAni) //展示2阶段表现
                .addDelegateProcess(_startBattleSecond) //开始二阶段战斗
                .addDelegateProcess(_showBubble) //展示人物气泡
                .addDelegateProcess(_endBattle) //展示结算表现
                .addProcess(_closeWnd) //关闭界面
                .deal(); 
        }

        /// <summary>
        /// 校验战力是否满足打boss
        /// </summary>
        private void _checkPower()
        {
            //boss战斗力
            long bossPower = _m_chapterRef.boss_power;
            long curPower = NPPlayer.instance.chapterComp.getTotalPower();

            //战力不满足直接关闭窗口
            if (curPower < bossPower)
            {
                _closeWnd();
            }
            
            List<HeroInfo> allHeroList = new List<HeroInfo>();
            NPPlayer.instance.heroComponent.getAllList(allHeroList);
            //按战力从小到大排序
            allHeroList.Sort((a, b) =>
            {
                return a.power.CompareTo(b.power);
            });
            //总大臣数
            _m_heroAllCount = allHeroList.Count;
            //第一阶段出战大臣数
            int firstStageHeroCount = (int)(_m_heroAllCount * wnd.firstStageHeroCountFade);
            
            _m_heroListFirstStage.Clear();
            _m_heroListSecondStage.Clear();
            _m_firstBossPower = 0;
            _m_secondBossPower = 0;
            
            long totalHeroPower = 0;
            for (int i = 0; i < _m_heroAllCount; i++)
            {
                HeroInfo heroInfo = allHeroList[i];
                long heroPower = (long) (heroInfo.power * NPPlayer.instance.chapterComp.getInspirePowerRate());
                
                //出战大臣战力大于boss战力了，直接退出
                totalHeroPower += heroPower;
                if (totalHeroPower > bossPower)
                {
                    //策划要求2阶段保底要有1个大臣
                    _m_heroListSecondStage.Add(heroInfo);
                    break;
                }
                //不能打死的话，看是第一阶段还是第二阶段的大臣
                else
                {
                    if (i < firstStageHeroCount)
                    {
                        _m_heroListFirstStage.Add(heroInfo);
                        _m_firstBossPower += heroPower;
                    }
                    else
                    {
                        _m_heroListSecondStage.Add(heroInfo);
                    }
                }
            }

            //2阶段血量等于总血量 - 1阶段血量
            _m_secondBossPower = bossPower - _m_firstBossPower;
        }

        //初始化设置
        private void _initInfo()
        {
            //播放默认视频
            if (wnd != null) 
                GGUIWndChapterMainVideoSwitch.instance.playMainAniName(wnd.videoNameStr);
        }

        private void _startDealy(Action _onComplete)
        {
            if(null == wnd)
            {
                if (_onComplete != null) 
                    _onComplete();
            }
            
            _m_serialize = ALSerializeOpMgr.next();
            long serializeOp = _m_serialize;

            if (_m_heroListFirstStage.Count <= 0)
            {
                if (_onComplete != null) 
                    _onComplete();
            }
            else
            {
                ALCommonActionMonoTask.addMonoTask(() =>
                {
                    if(serializeOp != _m_serialize)
                        return;
                
                    if (_onComplete != null) 
                        _onComplete();
                }, wnd.startVBattleDelayS);   
            }
        }
        
        /// <summary>
        /// 开始战斗
        /// </summary>
        private void _startBattleFirst(Action _onComplete)
        {
            if(null == _m_chapterRef)
            {
                if (_onComplete != null) 
                    _onComplete();
                return;
            }
            
            if (wnd == null || null == _m_inspireHeroWndache)
            {
                if (_onComplete != null) 
                    _onComplete();
                return;
            }
            
            ChapterBossStyleRefObj chapterBossStyleRefObj = _m_chapterRef.getBossStyleRefObj();
            if(null == chapterBossStyleRefObj)
            {
                if (_onComplete != null) 
                    _onComplete();
                return;
            }
            
            _m_curWndType = EChapterBossWndType.SHOW;
            //刷新显隐状态
            if (wnd != null)
                NPCommonEnumStatInfo<EChapterBossWndType>.setStat(wnd.stateShow, _m_curWndType);
            
            _m_serialize = ALSerializeOpMgr.next();
            long serializeOp = _m_serialize;
        
            _m_inspireHeroWndache.pushBackAllCacheItems();
            _m_firstHeroInspireItemWndList.Clear();
            
            //加载第一阶段大臣item
            foreach (HeroInfo heroInfo in _m_heroListFirstStage)
            {
                if(null == heroInfo)
                    continue;
                
                GGUIWndChapterBossHeroInspire inspireWnd =  _m_inspireHeroWndache.popItem();
                if (inspireWnd != null && null != inspireWnd.rectTransform)
                {
                    long power = (long)(heroInfo.power * NPPlayer.instance.chapterComp.getInspirePowerRate());
                    inspireWnd.setInfo(heroInfo, power, chapterBossStyleRefObj.getRandomTalk(), wnd.heroShowParent);
                    inspireWnd.regReduceHpCallback(_onEventReduceHpFirst);
                    inspireWnd.rectTransform.SetAsFirstSibling();
                    inspireWnd.hideWnd();
                    _m_firstHeroInspireItemWndList.Add(inspireWnd);
                }
            }
            
            if(_m_firstHeroInspireItemWndList.Count > 0)
            {
                _m_commonBloodBoss?.showWnd();
                _m_commonBloodBossSecond?.hideWnd();
            }
            else
            {
                _m_commonBloodBoss?.hideWnd();
                _m_commonBloodBossSecond?.showWnd();
            }
            
            //开始鼓舞表现
            _showInspireFirst(serializeOp, 0, _onComplete);
        }
    
        /// <summary>
        /// 显示大臣鼓舞
        /// </summary>
        /// <param name="_serialize"></param>
        private void _showInspireFirst(long _serialize, int _randomNum, Action _onComplete)
        {
            _randomNum++;
            if (wnd == null || _m_firstHeroInspireItemWndList.Count == 0 || _m_inspireHeroWndache == null || null == _m_chapterRef)
            {
                _onComplete?.Invoke();
                return;
            }

            GGUIWndChapterBossHeroInspire heroInspire = _m_firstHeroInspireItemWndList.GetFirstAndRemove();

            if (null == heroInspire)
            {
                _showInspireFirst(_serialize, _randomNum, _onComplete);
                return;
            }
            
            string aniName;
            if (_randomNum % 2 != 0)
            {
                aniName = wnd.defaultSettings.heroInspireShowAniName;
            }
            else
            {
                aniName = wnd.defaultSettings.heroInspireShowAniNameRight;
            }
            
            //播放大臣表现动画
            heroInspire.showWnd();
            heroInspire.playShowAni(aniName);

            float nextTime = wnd.defaultSettings.heroInspireInterval;
            
            // 延迟显示下一个组大臣鼓舞
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                if (_serialize != _m_serialize)
                {
                    _onComplete?.Invoke();
                    return;
                }
                _showInspireFirst(_serialize, _randomNum, _onComplete);
            }, nextTime);
        }
    
        /// <summary>
        /// 当动画触发掉血
        /// </summary>
        private void _onEventReduceHpFirst(long _power)
        {
            if(wnd == null)
                return;
            
            _m_iShakeTaskSerialize = ALSerializeOpMgr.next();
            
            //进度条
            _m_commonBloodBoss?.damage(_power);
            if (_m_commonBloodBossSecond != null && null != _m_commonBloodBoss)
            {
                ALUGUICommon.setLabelTxt(wnd.txtBossHp,
                    $"{(_m_commonBloodBossSecond.curValue + _m_commonBloodBoss.curValue).ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)}/{_m_chapterRef.boss_power.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)}");
            }
            //播放受击动画
            if (null != _m_chapterRef && null != _m_chapterRef.getBossStyleRefObj())
            {
                GGUIWndChapterMainVideoSwitch.instance.showReplaceUnit(new ShowCaseCommonResUnitInfoObj(_m_chapterRef.getBossStyleRefObj().first_hit_video_boss_go_path), wnd.hitTimeFirstS);
            }
            
            //飘血
            if (_m_hpTipCache != null)
            {
                GGUIWndChapterBossHPTip tempItemWnd = _m_hpTipCache.popItem();
                if (tempItemWnd == null)
                    return;
                
                tempItemWnd.showWnd();
                tempItemWnd.setInfoPlay(wnd.hpTipParent
                    ,TextTranslate.instance.getLanguage(TransKeyConst.common_reduced_num, _power.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT))
                    ,wnd.defaultSettings.hpTipAniName 
                    ,() => { _m_hpTipCache.pushBackCacheItem(tempItemWnd); });
            }
            
            //窗口震动
            new ShakeTask(this, wnd.fShakeIntensity, wnd.fShakeDuration, true).deal();
            GGUIWndChapterMainVideoSwitch.instance.shake();
        }
        
        //展示2阶段过场动画
        private void _showSecondAni(Action _onComplete)
        {
            if (_m_chapterRef == null || wnd == null)
            {
                _onComplete?.Invoke();
                return;
            }
            
            //没1阶段不做表现
            if (_m_heroListFirstStage.Count <= 0)
            {
                if (_onComplete != null) 
                    _onComplete();
                return;
            }
            
            //播放动画
            wnd.endAnimation.ForcePlay(wnd.secondAnimationName, 0, _onComplete);
        }
        
        /// <summary>
        /// 开始战斗二阶段
        /// </summary>
        private void _startBattleSecond(Action _onComplete)
        {
            if(null == _m_chapterRef)
            {
                if (_onComplete != null) 
                    _onComplete();
                return;
            }
            
            if (wnd == null || null == _m_inspireHeroWndache)
            {
                if (_onComplete != null) 
                    _onComplete();
                return;
            }
            
            ChapterBossStyleRefObj chapterBossStyleRefObj = _m_chapterRef.getBossStyleRefObj();
            if(null == chapterBossStyleRefObj)
            {
                if (_onComplete != null) 
                    _onComplete();
                return;
            }
            
            _m_commonBloodBoss?.hideWnd();
            _m_commonBloodBossSecond?.showWnd();
            
            //播放2阶段动画
            GGUIWndChapterMainVideoSwitch.instance.playMainAniName(wnd.bossSecondVideoNameStr);
            
            _m_serialize = ALSerializeOpMgr.next();
            long serializeOp = _m_serialize;
        
            _m_inspireHeroWndache.pushBackAllCacheItems();
            _m_secondHeroInspireItemWndList.Clear();
            
            //加载第一阶段大臣item
            foreach (HeroInfo heroInfo in _m_heroListSecondStage)
            {
                if(null == heroInfo)
                    continue;
                
                GGUIWndChapterBossHeroInspire inspireWnd =  _m_inspireHeroWndache.popItem();
                if (inspireWnd != null && null != inspireWnd.rectTransform)
                {
                    long power = (long)(heroInfo.power * NPPlayer.instance.chapterComp.getInspirePowerRate());
                    inspireWnd.setInfo(heroInfo, power, chapterBossStyleRefObj.getRandomTalk(), wnd.heroShowParent);
                    inspireWnd.regReduceHpCallback(_onEventReduceHpSecond);
                    inspireWnd.rectTransform.SetAsFirstSibling();
                    inspireWnd.hideWnd();
                    _m_secondHeroInspireItemWndList.Add(inspireWnd);
                }
            }
            
            //开始鼓舞表现
            _showInspireSecond(serializeOp, 0, _onComplete);
        }
    
        /// <summary>
        /// 显示大臣鼓舞
        /// </summary>
        /// <param name="_serialize"></param>
        private void _showInspireSecond(long _serialize, int _randomNum, Action _onComplete)
        {
            _randomNum++;
            if (wnd == null || _m_secondHeroInspireItemWndList.Count == 0 || _m_inspireHeroWndache == null || null == _m_chapterRef)
            {
                _onComplete?.Invoke();
                return;
            }

            GGUIWndChapterBossHeroInspire heroInspire = _m_secondHeroInspireItemWndList.GetFirstAndRemove();

            if (null == heroInspire)
            {
                _showInspireSecond(_serialize, _randomNum, _onComplete);
                return;
            }
            
            string aniName;
            if (_randomNum % 2 != 0)
            {
                aniName = _m_secondHeroInspireItemWndList.Count == 0 ? wnd.defaultSettings.lastHitHeroInspireShowAniName : wnd.defaultSettings.heroInspireShowAniName;
            }
            else
            {
                aniName = _m_secondHeroInspireItemWndList.Count == 0 ? wnd.defaultSettings.lastHitHeroInspireShowAniNameRight : wnd.defaultSettings.heroInspireShowAniNameRight;
            }
            
            //播放大臣表现动画
            heroInspire.showWnd();
            heroInspire.playShowAni(aniName);

            float nextTime = wnd.defaultSettings.heroInspireIntervalSecond;
            //最后一击时间单独配置
            if (_m_secondHeroInspireItemWndList.Count == 0)
            {
                nextTime = wnd.defaultSettings.lastHitTime;
            }
            
            // 延迟显示下一个组大臣鼓舞
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                if (_serialize != _m_serialize)
                {
                    _onComplete?.Invoke();
                    return;
                }
                _showInspireSecond(_serialize, _randomNum, _onComplete);
            }, nextTime);
        }
    
        /// <summary>
        /// 当动画触发掉血
        /// </summary>
        private void _onEventReduceHpSecond(long _power)
        {
            if(wnd == null)
                return;
            
            //进度条
            _m_commonBloodBossSecond?.damage(_power);

            if (_m_commonBloodBossSecond != null && null != _m_commonBloodBoss)
            {
                ALUGUICommon.setLabelTxt(wnd.txtBossHp,
                    $"{(_m_commonBloodBossSecond.curValue + _m_commonBloodBoss.curValue).ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)}/{_m_chapterRef.boss_power.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)}");
            }
            if (null != _m_chapterRef && null != _m_chapterRef.getBossStyleRefObj())
            {
                GGUIWndChapterMainVideoSwitch.instance.showReplaceUnit(new ShowCaseCommonResUnitInfoObj(_m_chapterRef.getBossStyleRefObj().second_hit_video_boss_go_path), wnd.hitTimeSecondS);
            }
            
            //飘血
            if (_m_hpTipCache != null)
            {
                GGUIWndChapterBossHPTip tempItemWnd = _m_hpTipCache.popItem();
                if (tempItemWnd == null)
                    return;
                
                tempItemWnd.showWnd();
                tempItemWnd.setInfoPlay(wnd.hpTipParent
                    ,TextTranslate.instance.getLanguage(TransKeyConst.common_reduced_num, _power.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT))
                    ,wnd.defaultSettings.hpTipAniName
                    ,() => { _m_hpTipCache.pushBackCacheItem(tempItemWnd); });
            }
            
            //窗口震动
            new ShakeTask(this, wnd.fShakeIntensity, wnd.fShakeDuration, true).deal();
            GGUIWndChapterMainVideoSwitch.instance.shake();
        }
        
        
        /// <summary>
        /// 结束战斗
        /// </summary>
        private void _endBattle(Action _onComplete)
        {
            if (_m_chapterRef == null || wnd == null)
            {
                _onComplete?.Invoke();
                return;
            }

            if (_m_curWndType == EChapterBossWndType.REWARD)
            {
                _onComplete?.Invoke();
                return;
            }
            
            _m_curWndType = EChapterBossWndType.LOADING;
            
            _m_serialize = ALSerializeOpMgr.next();
            long serializeOp = _m_serialize;
            
            NPPlayer.instance.chapterComp.reqChapterFightBoss(_m_chapterRef.chapter_id, (_msg) =>
            {
                _m_curWndType = EChapterBossWndType.REWARD;
                //刷新显隐状态
                if (wnd != null)
                    NPCommonEnumStatInfo<EChapterBossWndType>.setStat(wnd.stateShow, _m_curWndType);
                
                GGUIWndChapterMainVideoSwitch.instance.playMainAniName(wnd.endVideoNameStr);
                
                //展示奖励
                if (_m_wRewardContainer != null && null != _msg)
                {
                    _m_wRewardContainer.showWnd();
                    _m_wRewardContainer.showItemList(_msg.getRewardList().toItemDataList());
                }
                
                // 延迟结束状态
                ALCommonActionMonoTask.addMonoTask(() =>
                {
                    if(serializeOp != _m_serialize)
                        return;
                    _onComplete?.Invoke();
                }, wnd.defaultSettings.rewardStateTime);
                
            }, (_errCode) =>
            {
                _m_curWndType = EChapterBossWndType.FAIL;
                
                //上浮提示
                NPGUIAddSceneCenterTip.instance.showErrorInfo(_errCode);

                if (_onComplete != null) 
                    _onComplete();
            });
        }

        //展示人物气泡
        private void _showBubble(Action _onComplete)
        {
            if (_m_chapterRef == null || wnd == null)
            {
                _onComplete?.Invoke();
                return;
            }
            
            //切换状态
            _m_curWndType = EChapterBossWndType.END;
            //刷新显隐状态
            if (wnd != null)
                NPCommonEnumStatInfo<EChapterBossWndType>.setStat(wnd.stateShow, _m_curWndType);
            
            //播放动画
            wnd.endAnimation.ForcePlay(wnd.endAnimationName);
            
            _m_serialize = ALSerializeOpMgr.next();
            long serializeOp = _m_serialize;
            
            // 延迟结束状态
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                if(serializeOp != _m_serialize)
                    return;
                _onComplete?.Invoke();
            }, wnd.defaultSettings.endBubbleTime);
        }
        
        /// <summary>
        /// 关闭界面
        /// </summary>
        private void _closeWnd()
        {
            _m_curWndType = EChapterBossWndType.NONE;
            _m_serialize = ALSerializeOpMgr.next();

            if (null != _m_chapterRef && _m_chapterRef.boss_end_dialog > 0)
            {
                GCommon.enterDialogueNode(_m_chapterRef.boss_end_dialog, () =>
                {
                    //关闭关卡界面
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Chapter.C_CHAPTER);
                    //关闭当前界面
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Chapter.C_CHAPTER_BOSS);
                });
            }
            else
            {
                //关闭关卡界面
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Chapter.C_CHAPTER);
                //关闭当前界面
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Chapter.C_CHAPTER_BOSS);
            }
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd || null == _m_chapterRef)
                return;

            long totalPower = NPPlayer.instance.chapterComp.getTotalPower();
            ChapterBossStyleRefObj chapterBossStyleRefObj = _m_chapterRef.getBossStyleRefObj();
            
            ALUGUICommon.setLabelTxt(wnd.txtBossName, TextTranslate.instance.getLanguage(chapterBossStyleRefObj?.boss_name));
            ALUGUICommon.setLabelTxt(wnd.txtBossPower, _m_chapterRef.boss_power.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));

            ALUGUICommon.setLabelTxt(wnd.txtPlayerPower, totalPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            if (_m_bossIcon != null && null != chapterBossStyleRefObj)
            {
                //boss相关
                _m_bossIcon.showWnd();
                _m_bossIcon.setTexture(chapterBossStyleRefObj.boss_icon);
            }
            
            if (null != _m_playerInfo)
            {
                _m_playerInfo.showWnd();
                _m_playerInfo.setSelfInfo();
            }

            _m_commonBloodBoss?.setBloodData(_m_firstBossPower, _m_firstBossPower);
            _m_commonBloodBossSecond?.setBloodData(_m_secondBossPower, _m_secondBossPower);
            ALUGUICommon.setLabelTxt(wnd.txtBossHp, $"{_m_chapterRef.boss_power.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)}/{_m_chapterRef.boss_power.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)}");

            
            if (_m_heroListFirstStage.Count > 0)
            {
                _m_commonBloodBoss?.showWnd();
                _m_commonBloodBossSecond?.hideWnd();
            }
            else
            {
                _m_commonBloodBoss?.hideWnd();
                _m_commonBloodBossSecond?.showWnd();
            }
        }

        private void _onBtnJumpClick(GameObject _go)
        {
            if(null != _m_pProcessObj)
                _m_pProcessObj.discard();
            _m_pProcessObj = null;
            
            _m_serialize = ALSerializeOpMgr.next();

            if (_m_commonBloodBoss != null)
            {
                _m_commonBloodBoss.setBloodData(0, _m_firstBossPower);
            }
            
            if (_m_commonBloodBossSecond != null)
            {
                _m_commonBloodBossSecond.setBloodData(0, _m_secondBossPower);
            }
            
            ALProcess process = ALProcess.CreateProcess("tower_battle_battle_skip");
            process
                .addDelegateProcess(_showBubble) //展示人物气泡
                .addDelegateProcess(_endBattle) //展示结算表现
                .addProcess(_closeWnd) //关闭界面
                .deal(); 
        }

        private void _onClickCloseBtn(GameObject _go)
        {
            _closeWnd();
        }
        
        private string _getCommonSliderTxtStr(string _cur, string _max)
        {
            return TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, _cur, _max);
        }
    }
}