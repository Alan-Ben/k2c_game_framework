using System;
using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChapterMap : _ANPGGUIBasicResBarWnd<GGUIMonoChapterMap>
    {
        private static GGUIWndChapterMap _g_instance = new GGUIWndChapterMap();

        public static GGUIWndChapterMap instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndChapterMap();
                return _g_instance;
            }
        }

        private ChapterRefObj _m_chapterRef;
        private GGUIWndChapterMapPage _m_chapterMapPage; // 章列表窗口
        private NPGGuiWndTexture _m_wUnlockBuildingImg; //解锁建筑图标
        private long _m_lWndShowSerialId;
        private NPGGuiWndTexture _m_wbgImg; //解锁建筑图标

        public GGUIWndChapterMap() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath
        {
            get { return GGUIMonoChapterMap.assetPath; }
        }

        protected override string _monoObjName
        {
            get { return GGUIMonoChapterMap.objName; }
        }

        protected override _AALResourceCore _resourceCore
        {
            get { return GameResCore.instance; }
        }


        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_OPEN_CHAPTER_AUTO_SETTING, _onSimulateClickAutoSetting);//模拟点击关卡自动设置界面
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_OPEN_CHAPTER_AUTO_SETTING, _onSimulateClickAutoSetting);//模拟点击关卡自动设置界面
            _m_lWndShowSerialId = ALSerializeOpMgr.next();

            _m_chapterMapPage?.hideWnd();

            _m_wUnlockBuildingImg?.hideWnd();
            _m_wbgImg?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_chapterMapPage?.resetWnd();

            _m_wUnlockBuildingImg?.discardTexture();
            _m_wbgImg?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_chapterMapPage?.discard();
            _m_chapterMapPage = null;

            _m_wUnlockBuildingImg?.discard();
            _m_wUnlockBuildingImg = null;

            _m_wbgImg?.discard();
            _m_wbgImg = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnStory, _onStoryBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnAuto, _onClickBtnAutoForward);
            ALUGUICommon.combineBtnClick(wnd.btnLockSimpleTutorial, _onClickLockSimpleTutorialBtn);
            ALUGUICommon.combineBtnClick(wnd.btnCancelAutoForward, _onClickBtnCancelAutoForward);


            if (wnd.unlockBuildingImg != null)
                _m_wUnlockBuildingImg = new NPGGuiWndTexture(wnd.unlockBuildingImg);

            if (wnd.bgTex != null)
                _m_wbgImg = new NPGGuiWndTexture(wnd.bgTex);
        }

        public void refreshAll()
        {
            _refreshChapterAll();
        }

        //显示章节地图
        public void showChapterMap(ChapterRefObj _chapterRef)
        {
            if (null == _chapterRef)
            {
                if (wnd != null)
                {
                    ALUGUICommon.setGameObjEnable(wnd.allPassShowGoList, true);
                    ALUGUICommon.setGameObjEnable(wnd.allPassHideGoList, false);
                }
                return;
            }

            _m_chapterRef = _chapterRef;

            _refreshChapterAll();
        }

        /// <summary>
        /// 展示自动前进表现
        /// </summary>
        public void showAutoForwardEffect(int _targetNodeIndex, float _fade, int _coefficient, long _rewardExp, long _rewardPlayerExp, List<NPCommon.NPCommon_ItemInfo> _itemList)
        {
            if(null == wnd)
                return;
            
            if(null == _m_chapterMapPage)
                return;
            
            RectTransform rect = _m_chapterMapPage.getNodeRectTransform(_targetNodeIndex);
            
            //展示粒子效果
            GCommon.showItemParticle(ENPItemType.CURRENCY, (long)ECurrency.HERO_EXP, _rewardExp,
                rect, wnd.specialParticleId);
            
            //播放特效
            _m_chapterMapPage.playNodeAutoSfx(_targetNodeIndex);
            
            //刷新展示
            _refreshChapterAll();
        }
        
        /// <summary>
        /// 展示自动boss战前进表现
        /// </summary>
        public void showAutoBossEffect(Action _playDone)
        {
            if(null == _m_chapterMapPage)
                return;
            
            _m_chapterMapPage.showBossAutoBattleState(() =>
            {
                if (_playDone != null) 
                    _playDone();
                //刷新展示
                _refreshChapterAll();
            });
        }
        
        /// <summary>
        /// 展示节点解锁动画
        /// </summary>
        /// <param name="_chapterRef"></param>
        /// <param name="_targetNodeIndex"></param>
        public void showNodeUnlock(ChapterRefObj _chapterRef, int _targetNodeIndex)
        {
            if (null == _chapterRef)
                return;

            _m_chapterRef = _chapterRef;

            long serialId = _m_lWndShowSerialId = ALSerializeOpMgr.next();
            ALProcess process = ALProcess.CreateProcess();

            //解锁新的一章
            if (_targetNodeIndex == 0)
            {
                process
                    //初始化设置
                    .addDelegateProcess((complete) =>
                    {
                        if (_m_lWndShowSerialId != serialId)
                            return;
                        _refreshChapterAll(() =>
                        {
                            //第一个item先设置未达到
                            if (_m_chapterMapPage != null)
                                _m_chapterMapPage.forceRefreshAllItemState(0, EChapterMapNodeState.UN_REACH);

                            if (complete != null)
                                complete();
                        });
                    })
                    //播放窗口动画
                    .addDelegateProcess((_complete) =>
                    {
                        if (_m_lWndShowSerialId != serialId)
                            return;
                        if (null != wnd && null != wnd.ani)
                        {
                            wnd.ani.ForcePlay(wnd.openNewChapterAniName, 0, _complete);
                        }
                        else
                        {
                            if (_complete != null)
                                _complete();
                        }
                    })
                    //播放节点解锁动画
                    .addDelegateProcess((_complete) =>
                    {
                        if (_m_lWndShowSerialId != serialId)
                            return;

                        //新章节未解锁直接结束
                        if (null != _m_chapterRef && !GCommon.isSimpleUnlock(_m_chapterRef.forward_simple_unlock_id))
                        {
                            if (_complete != null) _complete();
                            return;
                        }

                        if (null == _m_chapterMapPage)
                        {
                            if (_complete != null) _complete();
                        }
                        else
                        {
                            _m_chapterMapPage.showItemChgToUnderwayState(0, _complete);
                        }
                    })
                    .addProcess(() => { _refreshChapterAll(); });
            }
            else
            {
                process
                    //初始化设置
                    .addDelegateProcess((complete) =>
                    {
                        if (_m_lWndShowSerialId != serialId)
                            return;
                        _refreshChapterAll(complete);

                        //下一个item先设置未达到
                        if (_m_chapterMapPage != null)
                            _m_chapterMapPage.forceRefreshAllItemState(_targetNodeIndex, EChapterMapNodeState.UN_REACH);
                    })
                    //播放前一个节点完成动画
                    .addDelegateProcess((_complete) =>
                    {
                        if (_m_lWndShowSerialId != serialId)
                            return;

                        if (null == _m_chapterMapPage)
                        {
                            if (_complete != null) _complete();
                        }
                        else
                        {
                            _m_chapterMapPage.showItemChgToCompletedState(_targetNodeIndex - 1, _complete);
                        }
                    })
                    //播放新节点解锁动画
                    .addDelegateProcess((_complete) =>
                    {
                        if (_m_lWndShowSerialId != serialId)
                            return;

                        if (null == _m_chapterMapPage)
                        {
                            if (_complete != null) _complete();
                        }
                        else
                        {
                            _m_chapterMapPage.showItemChgToUnderwayState(_targetNodeIndex, _complete);
                        }
                    })
                    .addProcess(() => { _refreshChapterAll(); });
            }


            process.deal();
        }

        //刷新显示
        private void _refreshChapterAll(Action _complete = null)
        {
            if (null == _m_chapterRef || null == wnd)
            {
                if (wnd != null)
                {
                    ALUGUICommon.setGameObjEnable(wnd.allPassShowGoList, true);
                    ALUGUICommon.setGameObjEnable(wnd.allPassHideGoList, false);
                }

                if (_complete != null)
                    _complete();
                return;
            }

            wnd.ani.Sample(wnd.openNewChapterAniName, 1);


            ALUGUICommon.setLabelTxt(wnd.txtChapterName,
                TextTranslate.instance.getLanguage(_m_chapterRef.name, _m_chapterRef.nameArgs));
            ALUGUICommon.setLabelTxt(wnd.txtChapterName2,
                TextTranslate.instance.getLanguage(_m_chapterRef.name, _m_chapterRef.nameArgs));
            ALUGUICommon.setLabelTxt(wnd.txtChapterDesc,
                TextTranslate.instance.getLanguage(_m_chapterRef.desc, _m_chapterRef.descArgs));

            if (_m_wbgImg != null)
            {
                _m_wbgImg.showWnd();
                _m_wbgImg.setTexture(_m_chapterRef.chapterImg);
            }

            int goldCostReduceRate = (int)NPPlayer.instance.chapterComp.getGoldCostReduceRate();
            //消耗增加
            if (goldCostReduceRate < 0)
            {
                ALUGUICommon.setLabelTxt(wnd.txtCostDesc,
                    TextTranslate.instance.getLanguage(TransKeyConst.chapter_forward_cost_add,
                        NPPlayer.instance.heroComponent.totalPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT), Mathf.Abs(goldCostReduceRate / 100)));
            }
            //消耗减少
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtCostDesc,
                    TextTranslate.instance.getLanguage(TransKeyConst.chapter_forward_cost_reduce,
                        NPPlayer.instance.heroComponent.totalPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT), goldCostReduceRate / 100));
            }

            ALUGUICommon.setGameObjEnable(wnd.lockShowGoList,
                !GCommon.isSimpleUnlock(_m_chapterRef.forward_simple_unlock_id));
            //解锁条件描述
            NPSimpleUnlockRef simpleUnlockRef =
                GRefdataCoreMgr.instance.simpleUnlockMap.getRef(_m_chapterRef.forward_simple_unlock_id);
            if (simpleUnlockRef != null)
                ALUGUICommon.setLabelTxt(wnd.txtLockDesc,
                    TextTranslate.instance.getLanguage(simpleUnlockRef.unlock_tip, simpleUnlockRef.unlock_tip_args));

            _refreshUnlockBuilding();

            if (_m_chapterMapPage != null && _m_chapterMapPage.assetPath != _m_chapterRef.chapterMapResIndex)
            {
                _m_chapterMapPage.discard();
                _m_chapterMapPage = null;
            }

            if (_m_chapterMapPage == null)
            {
                _m_chapterMapPage = new GGUIWndChapterMapPage(_m_chapterRef.chapterMapResIndex, wnd.chapterListLoadParent);
                _m_chapterMapPage.load();
            }

            _m_chapterMapPage.regLoadDoneDelegate(() =>
            {
                _m_chapterMapPage.showWnd();
                _m_chapterMapPage.initSetInfo(_m_chapterRef);
                if (_complete != null)
                    _complete();
            });
            
            //自动前进展示的go显隐
            if (NPPlayer.instance.chapterComp.forwardLogicMgr.isInAutoForward)
            {
                ALUGUICommon.setGameObjEnable(wnd.autoForwardShowGoList, true);
                ALUGUICommon.setGameObjEnable(wnd.autoForwardHideGoList, false);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.autoForwardShowGoList, false);
                ALUGUICommon.setGameObjEnable(wnd.autoForwardHideGoList, true);
            }
        }


        /// <summary>
        /// 刷新未解锁建筑
        /// </summary>
        private void _refreshUnlockBuilding()
        {
            if(null == _m_chapterRef || null == wnd)
                return;

            ChapterBuildUnlockRefObj buildUnlockRefObj = _m_chapterRef.getChapterBuildUnlockRefObj();
            ALUGUICommon.setGameObjEnable(wnd.hasUnlockBuildingShowGoList, buildUnlockRefObj != null);
            
            if (buildUnlockRefObj != null)
            {
                if (_m_wUnlockBuildingImg != null)
                {
                    _m_wUnlockBuildingImg.showWnd();
                    _m_wUnlockBuildingImg.setTexture(buildUnlockRefObj.preview_tex_index);
                }

                ALUGUICommon.setLabelTxt(wnd.txtUnlockBuildingDesc, TextTranslate.instance.getLanguage(
                    buildUnlockRefObj.condition_desc, buildUnlockRefObj.condition_desc_params));
            }
        }

        /// <summary>
        /// 故事按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onStoryBtnClick(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndChapterStoryList.instance,
                UINodeTagConst_Chapter.C_CHAPTER_STORY_LIST, null, null, 0);
        }

        private void _onClickLockSimpleTutorialBtn(GameObject _go)
        {
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
            }
        }

        //点击自动前进
        private void _onClickBtnAutoForward(GameObject _obj)
        {
            if (null == wnd)
                return;

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndChapterAutoSetting.instance,
                () => { GGUIWndChapterAutoSetting.instance.showWnd(); }, EUIQueueStageType.MAIN,
                UINodeTagConst_Chapter.C_CHAPTER_AUTO_SETTING, false, false);
        }

        //点击取消自动前进
        private void _onClickBtnCancelAutoForward(GameObject _obj)
        {
            if (null == wnd)
                return;

            NPPlayer.instance.chapterComp.forwardLogicMgr.stopAutoForwardToChapter();
        }

        public RectTransform getCurNodeDoingRectTransform()
        {
            if (null == _m_chapterMapPage)
                return null;
            return _m_chapterMapPage.getCurNodeDoingRectTransform();

        }

        //模拟点击关卡自动设置界面
        private void _onSimulateClickAutoSetting()
        {
            _onClickBtnAutoForward(null);
        }
    }
}