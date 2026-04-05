using System;
using ALPackage;
using NPEnum;
using System.Collections.Generic;
using System.Linq;
using Common.ChapterEnum;
using CommonEnum;
using DG.Tweening;
using JetBrains.Annotations;
using NPCommon;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GOE
{
    /// <summary>
    /// 关卡主窗口
    /// </summary>
    public class GGUIWndChapterMain : _ANPGGUIBasicResBarWnd<GGUIMonoChapterMain>
    {
        private static GGUIWndChapterMain _g_instance = new GGUIWndChapterMain();

        public static GGUIWndChapterMain instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndChapterMain();
                return _g_instance;
            }
        }

        private GGUIWndCommentItemContainer _m_itemContainer;
        private NPGGUIWndCommonToggleEx _m_quickForwardToggle; //快速前进复选框
        private NPGGUIWndCommonItem _m_costItem; //前进消耗

        private NPGGUIWndCommonItem _m_heroExpItem; //大臣经验

        //boos头像
        private NPGGuiWndTexture _m_iconWnd;

        private NPGGuiWndTexture _m_bossTipIconWnd;

        //鼓舞子窗口
        private GGUIWndChapterMain_SubWndInspire _m_inspireWnd;
        private GGUIWndChapterMainDialogSliderTipContainer _m_dialogSliderTipContainer;

        // 进度条上的对话气泡子窗口列表
        private GGUIWndChapterMapPage _m_chapterMapPage;

        //boss对话结束回调
        private Action _m_bossDialogEndCallback;
        private long _m_bgMusicInstanceId; //背景音乐id

        //特效列表
        [NotNull] private List<CommonUISfxObj> _m_lSfxObjList = new List<CommonUISfxObj>();
        //屏幕特效，只能存在1个
        private CommonUISfxObj _m_screenSfxObj;

        //已经打开过的对话id列表,这个内存随便记录一下就行，打开过不重复打开
        [NotNull] private List<long> _m_hasOpenDialogIdList = new List<long>();

        public GGUIWndChapterMain() : base(EALUIWndLayer.NORMAL)
        {

        }

        protected override string _monoAssetPath
        {
            get { return GGUIMonoChapterMain.assetPath; }
        }

        protected override string _monoObjName
        {
            get { return GGUIMonoChapterMain.objName; }
        }

        protected override _AALResourceCore _resourceCore
        {
            get { return GameResCore.instance; }
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _onCurrencyChg); //伙伴经验变化
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_CHAPTER_FORWARD, _onSimulateClickBtnForward); //模拟点击关卡前进
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_CHAPTER_QUICK_FORWARD_TOGGLE,
                _onSimulateClickQuickForwardToggle); //模拟点击关卡快速前进开关
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_CHAPTER_QUICK_FORWARD_SKIP_DIALOG_CONFIRM,
                _onSimulateClickQuickForwardSkipDialogConfirm); //模拟点击关卡快速前进是否跳过对话"确认"按钮
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _onCostItemChg);

            //勾选快速前进
            if (_m_quickForwardToggle != null)
                _m_quickForwardToggle.setSelected(AccountSettingMgr.instance.accountSetting.isChapterQuickForward);

            //展示视频
            NPGGoIndex bgIndex = null;
            _IChapterNodeStyle chapterNodeStyle = NPPlayer.instance.chapterComp.getCurNodeStyleRef(out bgIndex);
            if (chapterNodeStyle != null)
            {
                ShowCaseCommonResUnitInfoObj[] showCaseUnitInfoObjList = new ShowCaseCommonResUnitInfoObj[2];
                showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(chapterNodeStyle.videoGoPath), 0);
                if (bgIndex != null)
                    showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(bgIndex), 1);
                GGUIWndChapterMainVideoSwitch.instance.setShowcaseInfo(showCaseUnitInfoObjList,
                    chapterNodeStyle.maskUiImage, chapterNodeStyle.videoAnimatorControllerId, true);
            }

            refreshWnd();

            //有事件尝试处理事件
            if (NPPlayer.instance.chapterComp.curChapterEventId != 0)
            {
                NPPlayer.instance.chapterComp.forwardLogicMgr.tryEnterEventState();
            }

            //有对话尝试处理对话
            if (NPPlayer.instance.chapterComp.curPointId == 0
                && NPPlayer.instance.chapterComp.chapterRefObj.enter_chapter_dialog_id != 0
                && !_m_hasOpenDialogIdList.Contains(NPPlayer.instance.chapterComp.chapterRefObj
                    .enter_chapter_dialog_id))
            {
                //打开对话的话添加
                _m_hasOpenDialogIdList.Add(NPPlayer.instance.chapterComp.chapterRefObj.enter_chapter_dialog_id);
                NPPlayer.instance.chapterComp.forwardLogicMgr.tryEnterDialogState(NPPlayer.instance.chapterComp
                    .chapterRefObj.enter_chapter_dialog_id);
            }

            //播放开始的bgm
            if (NPPlayer.instance.chapterComp.chapterRefObj?.chapter_bg_music_id > 0)
            {
                _m_bgMusicInstanceId =
                    PlayAudioMgr.instance.playBackgroundMusic(NPPlayer.instance.chapterComp.chapterRefObj
                        .chapter_bg_music_id);
            }

            if (NPPlayer.instance.chapterComp.curIsBossPoint())
            {
                GGUIWndChapterMainVideoSwitch.instance.sampleLargeAniFirstFrame(true);
            }
        }

        protected override void _onHideWnd()
        {
            if (_m_lSfxObjList != null)
            {
                for (int i = 0; i < _m_lSfxObjList.Count; i++)
                {
                    _m_lSfxObjList[i]?.forceDiscard();
                }

                _m_lSfxObjList.Clear();
            }

            if (_m_screenSfxObj != null) 
                _m_screenSfxObj.forceDiscard();
            _m_screenSfxObj = null;

            if (_m_bossDialogEndCallback != null)
                _m_bossDialogEndCallback();
            _m_bossDialogEndCallback = null;

            if(_m_dialogSliderTipContainer != null)
                _m_dialogSliderTipContainer.hideWnd();
            
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _onCurrencyChg); //伙伴经验变化
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_CHAPTER_FORWARD, _onSimulateClickBtnForward); //模拟点击关卡前进
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_CHAPTER_QUICK_FORWARD_TOGGLE,
                _onSimulateClickQuickForwardToggle); //模拟点击关卡快速前进开关
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_CHAPTER_QUICK_FORWARD_SKIP_DIALOG_CONFIRM,
                _onSimulateClickQuickForwardSkipDialogConfirm); //模拟点击关卡快速前进是否跳过对话"确认"按钮
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _onCostItemChg);

            //快速前进退出关口取消
            NPPlayer.instance.chapterComp.forwardLogicMgr.stopQuickForwardToChapter();
            NPPlayer.instance.chapterComp.forwardLogicMgr.stopContinueClickForward();
            
            //恢复之前的bgm
            if (_m_bgMusicInstanceId > 0)
                PlayAudioMgr.instance.stopBackgroundMusicByInstanceID(_m_bgMusicInstanceId, true);
            _m_bgMusicInstanceId = 0;
        }

        protected override void _onReset()
        {
            if (_m_costItem != null)
                _m_costItem.resetWnd();
            if (_m_heroExpItem != null)
                _m_heroExpItem.resetWnd();

            if (_m_dialogSliderTipContainer != null)
                _m_dialogSliderTipContainer.resetWnd();
            
            if (_m_lSfxObjList != null)
            {
                for (int i = 0; i < _m_lSfxObjList.Count; i++)
                {
                    _m_lSfxObjList[i]?.forceDiscard();
                }

                _m_lSfxObjList.Clear();
            }

            if (_m_screenSfxObj != null) 
                _m_screenSfxObj.forceDiscard();
            _m_screenSfxObj = null;
        }

        protected override void _onDiscard()
        {
            if (null == wnd)
                return;
            
            if (_m_dialogSliderTipContainer != null)
                _m_dialogSliderTipContainer.discard();
            _m_dialogSliderTipContainer = null;

            if (_m_lSfxObjList != null)
            {
                for (int i = 0; i < _m_lSfxObjList.Count; i++)
                {
                    _m_lSfxObjList[i]?.forceDiscard();
                }

                _m_lSfxObjList.Clear();
            }

            if (_m_screenSfxObj != null) 
                _m_screenSfxObj.forceDiscard();
            _m_screenSfxObj = null;

            if (null != _m_iconWnd)
                _m_iconWnd.discard();
            _m_iconWnd = null;

            if (_m_bossTipIconWnd != null)
                _m_bossTipIconWnd.discard();
            _m_bossTipIconWnd = null;

            if (_m_inspireWnd != null)
            {
                _m_inspireWnd.onClickBattle -= _onClickBossFight;
                _m_inspireWnd.discard();
                _m_inspireWnd = null;
            }

            if (null != _m_itemContainer)
                _m_itemContainer.discard();
            _m_itemContainer = null;
            if (_m_costItem != null)
                _m_costItem.discard();
            _m_costItem = null;
            if (_m_heroExpItem != null)
                _m_heroExpItem.discard();
            _m_heroExpItem = null;

            _m_quickForwardToggle?.discard();
            _m_quickForwardToggle = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnForward, _onClickBtnForward);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (null != wnd.costItem)
                _m_costItem = new NPGGUIWndCommonItem(wnd.costItem);

            if (null != wnd.heroExpItem)
                _m_heroExpItem = new NPGGUIWndCommonItem(wnd.heroExpItem);

            if (null != wnd.imgBossIcon)
                _m_iconWnd = new NPGGuiWndTexture(wnd.imgBossIcon);

            if (null != wnd.imgTipBossIcon)
                _m_bossTipIconWnd = new NPGGuiWndTexture(wnd.imgTipBossIcon);

            if (wnd.subWndInspire != null)
            {
                _m_inspireWnd = new GGUIWndChapterMain_SubWndInspire(wnd.subWndInspire);
                _m_inspireWnd.onClickBattle += _onClickBossFight;
            }

            if (wnd.dialogSliderTipContainer != null)
            {
                _m_dialogSliderTipContainer = new GGUIWndChapterMainDialogSliderTipContainer(wnd.dialogSliderTipContainer);
            }
            
            if (null != wnd.btnQuickForward)
            {
                _m_quickForwardToggle = new NPGGUIWndCommonToggleEx(wnd.btnQuickForward);
                _m_quickForwardToggle.clickDelegate += _clickQuickForward;
                _m_quickForwardToggle.setSelected(false);
            }

            ALUGUICommon.combineBtnClick(wnd.btnForward, _onClickBtnForward);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _clickClose);
            ALUGUICommon.combineBtnClick(wnd.btnBossDialog, _onClickBossDialog);

        }

        /// <summary>
        /// 点击boss对话
        /// </summary>
        /// <param name="_obj"></param>
        private void _onClickBossDialog(GameObject _obj)
        {
            long dialogId = NPPlayer.instance.chapterComp.chapterRefObj.boss_start_dialog;

            if (dialogId != 0)
            {
                GCommon.enterDialogueNode(dialogId, () =>
                {
                    refreshWnd();

                    if (_m_bossDialogEndCallback != null)
                        _m_bossDialogEndCallback();
                    _m_bossDialogEndCallback = null;
                });
            }
            else
            {
                refreshWnd();

                if (_m_bossDialogEndCallback != null)
                    _m_bossDialogEndCallback();
                _m_bossDialogEndCallback = null;
            }
        }

        /// <summary>
        /// 点击复选框
        /// </summary>
        /// <param name="obj"></param>
        private void _clickQuickForward(NPGGUIWndCommonToggleEx obj)
        {
            if (null == _m_quickForwardToggle)
                return;

            _m_quickForwardToggle?.setSelected(!obj.isOn);
            //保存settings
            AccountSettingMgr.instance.accountSetting.setIsChapterQuickForward(_m_quickForwardToggle.isOn);

            //取消勾选时候如果在快去前进状态则停止,不需要弹出
            if (!_m_quickForwardToggle.isOn)
            {
                NPPlayer.instance.chapterComp.forwardLogicMgr.stopQuickForwardToChapter();
                return;
            }

            //未保存，二次确认
            bool needConfirm =
                AccountSettingMgr.instance.warningTipSaver.needShowWarningTip(ENPWarningType.CHAPTER_QUICK_FORWARD);
            if (needConfirm)
            {
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndChapterSkipToggleDialog.instance, () =>
                {
                    //如果点击关闭是关闭自动前进
                    GGUIWndChapterSkipToggleDialog.instance.setCancelAction(() =>
                    {
                        _m_quickForwardToggle?.setSelected(false);
                        AccountSettingMgr.instance.accountSetting.setIsChapterQuickForward(false);
                        NPPlayer.instance.chapterComp.forwardLogicMgr.stopQuickForwardToChapter();
                    });
                    GGUIWndChapterSkipToggleDialog.instance.showWnd();
                }, UINodeTagConst_Chapter.C_CHAPTER_SKIP_TOGGLE_DIALOG);
            }
        }

        private void _clickClose(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Chapter.C_CHAPTER);
        }

        //点击前进
        private void _onClickBtnForward(GameObject _obj)
        {
            if (null == wnd)
                return;

            bool isQuick = null == _m_quickForwardToggle ? false : _m_quickForwardToggle.isOn;

            if (NPPlayer.instance.chapterComp.forwardLogicMgr.isInQuickForward)
            {
                return;
            }

            //通关不允许前进
            if (NPPlayer.instance.chapterComp.chapterRefObj == null)
            {
                NPGUIAddSceneCenterTip.instance.showTextSysTip(
                    TextTranslate.instance.getLanguage(TransKeyConst.chapterMax_tip));
                return;
            }

            //条件不满足尝试打开引导
            if (!GCommon.isSimpleUnlock(NPPlayer.instance.chapterComp.chapterRefObj.forward_simple_unlock_id, true))
            {
                //如果还不在引导中，触发简易引导
                if (Game.instance.isInTutorial)
                    return;

                //执行跳转效果
                NPSimpleTutorialRefObj simpleTutorialRefObj =
                    GRefdataCoreMgr.instance.simpleTutorialRefCore.getRef(NPPlayer.instance.chapterComp.chapterRefObj
                        .forward_tutorial_id);
                if (null == simpleTutorialRefObj)
                    return;

                //设置简易引导
                SimpleTutorialController.instance.setCurSimpleGuide(simpleTutorialRefObj);
                SimpleTutorialController.instance.checkStartSimpleTutorial(QueueMgr.instance._lastNode.nodeTag);
                return;
            }
            NPPlayer.instance.chapterComp.forwardLogicMgr.startContinueClickForward(Time.realtimeSinceStartupAsDouble);
            NPPlayer.instance.chapterComp.forwardLogicMgr.forwardToChapter(isQuick);
        }

        //播放前进效果
        public void playForwardEffect(bool _isQuick, long _uiSfxId, long _tdSfxId, float _fade, int _coefficient, long _rewardExp,
            long _rewardPlayerExp, long _eventId, List<NPCommon.NPCommon_ItemInfo> _itemList)
        {
            if (null == wnd)
                return;

            //播放前进暴击相关表现
            _playForwardCrit(_coefficient);
            //展示粒子效果
            _showItemParticle(_rewardExp, null);

            //播放屏幕特效
            if (_m_screenSfxObj != null) 
                _m_screenSfxObj.forceDiscard();
            _m_screenSfxObj = PlaySfxMgr.instance.playUISfx(_uiSfxId, wnd.screenSfxParent);

            //播放场景特效
            GGUIWndChapterMainVideoSwitch.instance.playSfx(_tdSfxId);
            
            //自动处理的事件奖励
            if (null != _itemList && _itemList.Count > 0)
            {
                GCommon.showGainRewardTip(_itemList, true);
            }

            //播放进度动画
            _refreshSldShow(_fade, wnd.videoSpeedTime);
        }

        //播放适配ui动画
        public void playForwardVideoAniState(EChapterVideoForwardState _forwardState)
        {
            //播放视频前进动画
            GGUIWndChapterMainVideoSwitch.instance.playForwardAni(_forwardState);
        }
        
        
        //播放前进暴击相关表现
        private void _playForwardCrit(int _coefficient)
        {
            if (null == wnd)
                return;
            
            //播放普通特效
            if (_coefficient / 10000 == 1)
            {
                CommonUISfxObj commonUISfxObj = PlaySfxMgr.instance.playUISfx(wnd.defaultSfxId, wnd.sfxParent);
                _m_lSfxObjList.Add(commonUISfxObj);
            }
            //播放暴击特效
            else
            {
                CommonUISfxObj commonUISfxObj = PlaySfxMgr.instance.playUISfx(wnd.critSfxId, wnd.sfxParent);
                _m_lSfxObjList.Add(commonUISfxObj);

                
                //暴击的话要播放动画
                if (wnd.wndCritAni != null) 
                    wnd.wndCritAni.ForcePlay(wnd.critAniName);
            }
        }
        
        //展示粒子效果
        private void _showItemParticle(long _addExp, Action _action)
        {
            if (null == wnd)
            {
                if (_action != null) _action();
                return;
            }
            
            GCommon.showItemParticle(ENPItemType.CURRENCY, (long)ECurrency.HERO_EXP, _addExp,
                wnd.particleRectTransform, wnd.specialParticleId, _action);
        }
        
        //播放准备连线动画
        public void playBossFightPerAnim(Action _doneAction)
        {
            if (null == wnd || null == wnd.wndAni)
            {
                return;
            }
            
            long dialogId = NPPlayer.instance.chapterComp.chapterRefObj.boss_start_dialog;
            if (dialogId > 0)
            {
                NPCommonEnumStatInfo<EChapterWndShowState>.setStat(wnd.wndShowState, EChapterWndShowState.BOSS_DIALOG);
            }
            
            wnd.wndAni.ForcePlay(wnd.startBossAnimName, 0f, () =>
            {
                //没有对话直接执行回调
                long dialogId = NPPlayer.instance.chapterComp.chapterRefObj.boss_start_dialog;
                if (dialogId <= 0)
                {
                    if (_doneAction != null) 
                        _doneAction();
                }
                else
                {
                    //有对话等点击对话
                    _m_bossDialogEndCallback += _doneAction;
                }
            });
        }

        public void refreshWnd()
        {
            if (null == wnd)
                return;
            
            ChapterRefObj chapterRefObj = NPPlayer.instance.chapterComp.chapterRefObj;
            if (null == chapterRefObj)
            {
                return;
            }
            
            ALUGUICommon.setLabelTxt(wnd.txtChapterName, TextTranslate.instance.getLanguage(chapterRefObj.Name, chapterRefObj.nameArgs));
            ALUGUICommon.setLabelTxt(wnd.txtTitle, GCommon.getChapterName(chapterRefObj.chapter_id, NPPlayer.instance.chapterComp.curPointId));
            ALUGUICommon.setLabelTxt(wnd.txtPower, NPPlayer.instance.chapterComp.getTotalPower());
            
            int goldCostReduceRate = (int)NPPlayer.instance.chapterComp.getGoldCostReduceRate();
            //消耗增加
            if (goldCostReduceRate < 0)
            {
                ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(TransKeyConst.chapter_forward_cost_add, NPPlayer.instance.heroComponent.totalPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT), Mathf.Abs(goldCostReduceRate / 100)));
            }
            //消耗减少
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(TransKeyConst.chapter_forward_cost_reduce, NPPlayer.instance.heroComponent.totalPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT), goldCostReduceRate / 100));
            }

            _refreshCost();
            
            //大臣经验
            if (null != _m_heroExpItem)
            {
                _m_heroExpItem.showWnd();
                _m_heroExpItem.setItem(new NPCommonCostItem(ENPItemType.CURRENCY, (int)ECurrency.HERO_EXP, GCommon.getItemCount(ENPItemType.CURRENCY, (int)ECurrency.HERO_EXP)));
            }
            
            //boss相关
            ChapterBossStyleRefObj chapterBossStyleRefObj = NPPlayer.instance.chapterComp.chapterRefObj.getBossStyleRefObj();
            if (null != chapterBossStyleRefObj)
            {
                if (null != _m_iconWnd)
                {
                    _m_iconWnd.setTexture(chapterBossStyleRefObj.boss_icon);
                    _m_iconWnd.showWnd();
                }

                if (null != _m_bossTipIconWnd)
                {
                    _m_bossTipIconWnd.setTexture(chapterBossStyleRefObj.boss_icon);
                    _m_bossTipIconWnd.showWnd();
                }
                ALUGUICommon.setLabelTxt(wnd.txtBossName, TextTranslate.instance.getLanguage(chapterBossStyleRefObj.boss_name));
            }
            
            //boos相关
            _refreshBossWnd();
            
            //如果已经解锁
            if (NPPlayer.instance.chapterComp.curIsBossPoint())
            {
                NPCommonEnumStatInfo<EChapterWndShowState>.setStat(wnd.wndShowState, EChapterWndShowState.BOSS);
                playForwardVideoAniState(EChapterVideoForwardState.BOSS);
            }
            else
            {
                NPCommonEnumStatInfo<EChapterWndShowState>.setStat(wnd.wndShowState, EChapterWndShowState.NORMAL);
            }

            if (NPPlayer.instance.chapterComp.getGoldCost()?.count <= 0)
            {
                ALUGUICommon.setGameObjEnable(wnd.noneCostShowGoList, true);
                ALUGUICommon.setGameObjEnable(wnd.noneCostHideGoList, false);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.noneCostShowGoList, false);
                ALUGUICommon.setGameObjEnable(wnd.noneCostHideGoList, true);
            }
            
            //刷新进度条
            _refreshSldShow(NPPlayer.instance.chapterComp.getTotalFade(), 0);
            
            //刷新气泡进度
            _refreshDialogTip();
            
            if (wnd != null && wnd.wndAni != null) 
                wnd.wndAni.ForcePlay(wnd.defaultWndAnimName);
            
            //暴击动画重置第一帧
            if (wnd.wndCritAni != null) 
                wnd.wndCritAni.Sample(wnd.critAniName, 1 );
        }

        private void _refreshDialogTip()
        {
            if(null == wnd)
                return;

            int pointMaxCount = NPPlayer.instance.chapterComp.chapterRefObj.point_count;
            List<long> dialogList = NPPlayer.instance.chapterComp.chapterRefObj.getDialogPointList();
            if(null == dialogList)
                return;
            
            //设置每个item位置
            List<float> posList = new List<float>();
            if (wnd.sldChapter != null && dialogList.Count != 0)
            {
                //获取进度条的长度
                RectTransform sldRect = (RectTransform) wnd.sldChapter.transform;
                float sliderWidth = sldRect != null ? sldRect.rect.width : 0f;
                for (int i = 0; i < dialogList.Count; i++)
                {
                    if (dialogList[i] == null)
                        continue;  
                    
                    //根据分数比例计算item位置
                    float itemPos = pointMaxCount != 0
                        ? dialogList[i] * 1.0f / pointMaxCount * sliderWidth
                        : 0;
                    posList.Add(itemPos);
                }
            }

            if (_m_dialogSliderTipContainer != null)
            {
                _m_dialogSliderTipContainer.showWnd();
                _m_dialogSliderTipContainer.showItemList(dialogList, posList);
            }
        }
        
        private void _refreshCost()
        {
            //当前消耗
            if (null != _m_costItem)
            {
                _m_costItem.showWnd();
                _m_costItem.setItem(NPPlayer.instance.chapterComp.getGoldCost());
            }
        }
        
        private void _onCostItemChg(params object[] _objects)
        {
            _refreshCost();
        }

        public void showDialogSliderTip(long _pointId, Action _doneAction)
        {
            if (null == wnd || null == _m_dialogSliderTipContainer)
            {
                if (_doneAction != null) 
                    _doneAction();
                return;
            }

            _m_dialogSliderTipContainer.showDialogSliderTip(_pointId, _doneAction);
        }

        //现实剧情tip表现
        public void showDialogTipEffect()
        {
            if(null == wnd)
                return;
            
            NPCommonEnumStatInfo<EChapterWndShowState>.setStat(wnd.wndShowState, EChapterWndShowState.DEAL_DIALOG);
        }
        
        //现实事件tip表现
        public void showEventTipEffect()
        {
            if(null == wnd)
                return;
            
            NPCommonEnumStatInfo<EChapterWndShowState>.setStat(wnd.wndShowState, EChapterWndShowState.DEAL_EVENT);
        }

        //处理节点移动到Boss表现
        public void dealChapterNodeMoveBossEffect()
        {
            if(null == wnd || null == wnd.wndAni)
            {
                return;
            }
            
            wnd.wndAni.ForcePlay(wnd.nodeTurnAnimNameStart, 0f, () =>
            {
                //设置下一个对象的信息
                NPGGoIndex bgIndex = null;
                _IChapterNodeStyle nodeStyleRef = NPPlayer.instance.chapterComp.getCurNodeStyleRef(out bgIndex);
                if (nodeStyleRef != null)
                {
                    ShowCaseCommonResUnitInfoObj[] showCaseUnitInfoObjList = new ShowCaseCommonResUnitInfoObj[2];
                    showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(nodeStyleRef.videoGoPath), 0);
                    if (bgIndex != null) 
                        showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(bgIndex), 1);
                    GGUIWndChapterMainVideoSwitch.instance.setShowcaseInfo(showCaseUnitInfoObjList, nodeStyleRef.maskUiImage, nodeStyleRef.videoAnimatorControllerId, false);
                }            

                //播放流转动画
                GGUIWndChapterMainVideoSwitch.instance.playTurnBossAnim(null);
            });
        }
        
        //处理节点移动表现
        public void dealChapterNodeMoveEffect()
        {
            if(null == wnd || null == wnd.wndAni)
            {
                return;
            }
            
            //设置下一个对象的信息
            NPGGoIndex bgIndex = null;
            _IChapterNodeStyle nodeStyleRef = NPPlayer.instance.chapterComp.getCurNodeStyleRef(out bgIndex);
            if (nodeStyleRef != null)
            {
                ShowCaseCommonResUnitInfoObj[] showCaseUnitInfoObjList = new ShowCaseCommonResUnitInfoObj[2];
                showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(nodeStyleRef.videoGoPath), 0);
                if (bgIndex != null) 
                    showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(bgIndex), 1);
                GGUIWndChapterMainVideoSwitch.instance.setShowcaseInfo(showCaseUnitInfoObjList, nodeStyleRef.maskUiImage, nodeStyleRef.videoAnimatorControllerId, false);
            }            

            //播放流转动画
            GGUIWndChapterMainVideoSwitch.instance.playTurnAnim(null);
        }
        
        //伙伴经验变化
        private void _onCurrencyChg(params object[] _objs)
        {
            if (_objs == null || _objs.Length == 0)
                return;

            ECurrency type = (ECurrency)_objs[0];

            if (type == ECurrency.HERO_EXP)
            {
                //大臣经验
                if (null != _m_heroExpItem)
                {
                    _m_heroExpItem.showWnd();
                    _m_heroExpItem.setItem(new NPCommonCostItem(ENPItemType.CURRENCY, (int)ECurrency.HERO_EXP, GCommon.getItemCount(ENPItemType.CURRENCY, (int)ECurrency.HERO_EXP)));
                }
            }
        }
        
        private void _onSimulateClickBtnForward(object[] _params)
        {
           _onClickBtnForward(null);
        }

        //模拟点击关卡快速前进开关
        private void _onSimulateClickQuickForwardToggle()
        {
            _clickQuickForward(_m_quickForwardToggle);
        }

        //模拟点击关卡快速前进是否跳过对话"确认"按钮
        private void _onSimulateClickQuickForwardSkipDialogConfirm()
        {
            //设置是否跳过剧情
            AccountSettingMgr.instance.accountSetting.setIsChapterQuickForwardSkipDialog(true);
            //关闭弹窗
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MES_DEALER);
        }

        //刷新进度条点显示
        private void _refreshSldShow(float _fade, float _time, Action _complete = null)
        {
            if(null == wnd)
                return;
            
            if (_time <= 0)
            {
                //刷新进度条
                ALUGUICommon.setSliderScale(wnd.sldChapter, _fade);
                //赋值百分比
                ALUGUICommon.setLabelTxt(wnd.txtSld, (1- _fade).ToString("P2"));

                if (_complete != null) 
                    _complete();
            }
            else
            {
                //移动进度条
                wnd.sldChapter.DOValue(_fade, _time).OnComplete(() =>
                {
                    _complete();
                });
                
                // 读取当前文本，去掉百分号并转为 float
                float from = 0f;
                ALCommon.TryParseFloat(wnd.txtSld?.text?.Replace("%", ""), out from);
                from /= 100f; // 如果原来是百分比格式

                DOTween.To(() => from, x => {
                    from = x;
                    ALUGUICommon.setLabelTxt(wnd.txtSld, from.ToString("P2"));
                }, 1- _fade, _time);
            }
        }

        //刷新boss窗口
        private void _refreshBossWnd()
        {
            if(null == _m_inspireWnd)
                return;
            
            _m_inspireWnd.showWnd();
            _m_inspireWnd.setInfo(NPPlayer.instance.chapterComp.chapterRefObj);
        }

        //boss战
        private void _onClickBossFight()
        {
            if (null == wnd)
                return;
            
            //当前不是boss战不允许调动
            if(!NPPlayer.instance.chapterComp.curIsBossPoint())
                return;
            
            //播放放大动画
            GGUIWndChapterMainVideoSwitch.instance.playLargeAni(true, null);
            
            NPPlayer.instance.chapterComp.forwardLogicMgr.forwardToBoss();
        }
    }
}