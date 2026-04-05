using System;
using System.Collections.Generic;
using ALPackage;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndTowerMain : _ATALBasicUIWnd<GGUIMonoTowerMain>
    {
        private static GGUIWndTowerMain _g_instance = new GGUIWndTowerMain();

        public static GGUIWndTowerMain instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndTowerMain();
                return _g_instance;
            }
        }
        
        List<GGUIWndTowerMainChapterItem> _m_chapterItems = new List<GGUIWndTowerMainChapterItem>();
        private float _m_chapterMoveRootInitPosY;
        private long _m_chapteerMoveSerializeOp;

        public GGUIWndTowerMain() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoTowerMain.assetPath; }
        protected override string _monoObjName { get => GGUIMonoTowerMain.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        public override bool needDiscardOnSwitch => true;

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_TOWER_CHG, _refreshWnd);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_TOWER_RESEARCH, _onSimulateBtnResearchClick);
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_TOWER_CHG, _refreshWnd);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_TOWER_RESEARCH, _onSimulateBtnResearchClick);
            if (_m_chapterItems != null)
            {
                foreach (var item in _m_chapterItems)
                {
                    if (item != null) item.hideWnd();
                }
            }
        }

        protected override void _onReset()
        {
        
        }

        protected override void _onDiscard()
        {
            if (_m_chapterItems != null)
            {
                foreach (var item in _m_chapterItems)
                {
                    if (item != null) item.discard();
                }
                _m_chapterItems.Clear();
                _m_chapterItems = null;
            }

            _m_chapterMoveRootInitPosY = 0;
            ALUGUICommon.uncombineBtnClick(wnd.btnLog, _onBtnLogClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnChallenge, _onBtnChallengeClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnDetail, _onBtnDetailClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnResearch, _onBtnResearchClick);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if (wnd.chapterShowMoveTrans != null)
                _m_chapterMoveRootInitPosY = wnd.chapterShowMoveTrans.localPosition.y;
            
            if (wnd.chapterItems != null)
            {
                _m_chapterItems = new List<GGUIWndTowerMainChapterItem>();
                for (int i = 0; i < wnd.chapterItems.Count; i++)
                {
                    GGUIWndTowerMainChapterItem item = new GGUIWndTowerMainChapterItem(wnd.chapterItems[i], _onChapterShow, _onChapterHide);
                    _m_chapterItems.Add(item);
                }
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnLog, _onBtnLogClick);
            ALUGUICommon.combineBtnClick(wnd.btnChallenge, _onBtnChallengeClick);
            ALUGUICommon.combineBtnClick(wnd.btnDetail, _onBtnDetailClick);     
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnResearch, _onBtnResearchClick);
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        public void _refreshWnd()
        {
            if(null == wnd)
                return;
            TowerChapterRefObj newChapter = GRefdataCoreMgr.instance.towerChapterRefCore?.getRef(NPPlayer.instance.towerComp.curChapterId);
            int newLevel = NPPlayer.instance.towerComp.curLevel;
            if (newChapter != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtEarnBonus, TextTranslate.instance.getLanguage(TransKeyConst.tower_earn_bonus_add_num, NPPlayer.instance.towerComp.towerEarningAddPer/100f));
                ALUGUICommon.setLabelTxt(wnd.txtLevelName, newChapter.getLevelName(newLevel));
                ALUGUICommon.setLabelTxt(wnd.txtDailyCoins, TextTranslate.instance.getLanguage(TransKeyConst.tower_daily_coin_get_num, newChapter.getTowerCoinCount(newLevel)));
            }
            if (_m_chapterItems != null)
            {
                long curChapterId = NPPlayer.instance.towerComp.curChapterId;
                TowerChapterRefObj curChapterRefObj = GRefdataCoreMgr.instance.towerChapterRefCore.getRef(curChapterId);

                long curLevel = NPPlayer.instance.towerComp.curLevel;
                long maxUnlockChapter = curChapterId;

                if (curChapterRefObj != null && curLevel == curChapterRefObj.level_count)
                {
                    TowerChapterRefObj nextChapterRefObj = GRefdataCoreMgr.instance.getTowerNextChapterRefObj(curChapterId);
                    if (nextChapterRefObj != null) 
                        maxUnlockChapter = nextChapterRefObj.id;
                }

                if (maxUnlockChapter == curChapterId)
                {
                    foreach (var item in _m_chapterItems)
                    {
                        if (item != null)
                        {
                            item.setUnlock(item.chapterId > maxUnlockChapter);
                            item.showWnd();
                        }
                    }
                }
                else
                {
                    foreach (var item in _m_chapterItems)
                    {
                        if (item != null)
                        {
                            if (item.chapterId < maxUnlockChapter)
                            {
                                item.setUnlock(false);
                            }
                            else if (item.chapterId == maxUnlockChapter)
                            {
                                item.setLockAndNextFight();
                            }
                            else
                            {
                                item.setUnlock(true);
                            }
                            item.showWnd();
                        }
                    }
                }
            
            }
            _setChapterMoveShow(false);
            if (wnd.chapterShowMoveTrans != null)
            {
                Vector3 localPos = wnd.chapterShowMoveTrans.localPosition;
                localPos.y = _m_chapterMoveRootInitPosY;
                wnd.chapterShowMoveTrans.localPosition = localPos ;
            }
        }
        
        public void setToCurrentChapterPos()
        {
            if (NPPlayer.instance.towerComp == null || NPPlayer.instance.towerComp.curChapterId <= 0)
                return;
            setToChapterViewPos(NPPlayer.instance.towerComp.curChapterId);
        }

        public void showChapterUnlockAnim()
        {
            if(NPPlayer.instance.towerComp.curChapterId <= 1)
                return;
            // 先设置为上一个章节的位置，再移动到当前章节的位置
            setToChapterViewPos(NPPlayer.instance.towerComp.curChapterId -1);
            GGUIWndTowerMainChapterItem chapterItem = getChapterItem(NPPlayer.instance.towerComp.curChapterId);
            if (wnd != null && chapterItem != null)
            {
                chapterItem.setUnlock(true);
                float moveTime = wnd.moveToTargetChapterTime;
                float normalizeY = calculateTargetPosNormalizePos(chapterItem);
                if (wnd.viewScrollRect != null)
                    wnd.viewScrollRect.DONormalizedPos(new Vector2(wnd.viewScrollRect.horizontalNormalizedPosition, normalizeY), moveTime).OnComplete(chapterItem.playUnlockAnimation);
            }
        }
        
        private void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TOWER_MAIN);
        }
        
        private void _onBtnLogClick(GameObject _obj)
        {
            QueueMgr.instance.addNode_InGame_MainUIAddWnd(GGUIWndTowerLog.instance, UINodeTagConst.C_TOWER_LOG);
        }
        private void _onBtnChallengeClick(GameObject _obj)
        {
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndTowerChallenge.instance, UINodeTagConst.C_TOWER_CHALLENGE, 0);

        }
        private void _onBtnDetailClick(GameObject _obj)
        {
            QueueMgr.instance.AddNode(new GNodeCommonToolTip_TowerState( 5313, wnd.toolTipsRoot, 0, 0));
        }
        
        private void _onBtnResearchClick(GameObject _obj)
        {
            QueueMgr.instance.addNode_InGame_MainUIAddWnd(GGUIWndTowerResearch.instance, UINodeTagConst.C_TOWER_RESEARCH);
        }

        /// <summary>
        /// 模拟点击研究按钮
        /// </summary>
        private void _onSimulateBtnResearchClick()
        {
            _onBtnResearchClick(null);
        }

        private void _onChapterShow(long _chapterId)
        {
            GGUIWndTowerMainChapterItem chapterItem = getChapterItem(_chapterId);
            if (wnd == null || wnd.chapterShowMoveTrans == null || wnd.chapterTargetTrans == null || chapterItem == null || chapterItem.wnd == null|| wnd.viewScrollRect.viewport == null || wnd.viewScrollRect.content == null)
                return;
            // float yMove = calculateChapterMoveNormalizePos(chapterItem);
            // float normalizeY = wnd.viewScrollRect.verticalNormalizedPosition + ( yMove)/ (wnd.viewScrollRect.content.rect.height - wnd.viewScrollRect.viewport.rect.height) ;
            // normalizeY = Mathf.Clamp01(normalizeY);
            //
            // if (wnd.viewScrollRect != null)
            //     wnd.viewScrollRect.DONormalizedPos(new Vector2(wnd.viewScrollRect.horizontalNormalizedPosition, normalizeY), wnd.chapterShowMoveCenterTime);
            _m_chapteerMoveSerializeOp = ALSerializeOpMgr.next();
            long serializeOp = _m_chapteerMoveSerializeOp;
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                if(serializeOp != _m_chapteerMoveSerializeOp)
                    return;
                _setChapterMoveShow(true, _chapterId);
            }, wnd.chapterStarShowHideGosTime);
            Vector3 dis = wnd.chapterShowMoveTrans.InverseTransformPoint(wnd.chapterTargetTrans.position) - wnd.chapterShowMoveTrans.InverseTransformPoint(chapterItem.wnd.transform.position);
            wnd.chapterShowMoveTrans.DOLocalMoveY(_m_chapterMoveRootInitPosY + dis.y, wnd.chapterShowMoveTime);
        }
        
        private void _onChapterHide()
        {
            if (wnd == null || wnd.chapterShowMoveTrans == null) return;
            
            wnd.chapterShowMoveTrans.DOLocalMoveY(_m_chapterMoveRootInitPosY, wnd.chapterHideMoveTime);
            _m_chapteerMoveSerializeOp = ALSerializeOpMgr.next();
            long serializeOp = _m_chapteerMoveSerializeOp;
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                if(serializeOp != _m_chapteerMoveSerializeOp)
                    return;
                _setChapterMoveShow(false);
            }, wnd.chapterEndShowShowGosTime);
        }

        private void _setChapterMoveShow(bool _isShow, long _chapterId = 0)
        {
            if(wnd == null)
                return;
            ALUGUICommon.setGameObjEnable(wnd.chapterShowHideGos, !_isShow);
            if (wnd.chapterShowDisableMask != null) 
                wnd.chapterShowDisableMask.enabled = !_isShow;
            if (_isShow)
            {
                if (_m_chapterItems != null)
                    foreach (GGUIWndTowerMainChapterItem chapterItem in _m_chapterItems)
                    {
                        if (chapterItem == null || chapterItem.wnd == null)
                            continue;
                        ALUGUICommon.setGameObjEnable(chapterItem.wnd.chapterDetailShowGos,
                            chapterItem.chapterId == _chapterId);
                    }
            }
            else
            {
                if (_m_chapterItems != null)
                    foreach (GGUIWndTowerMainChapterItem chapterItem in _m_chapterItems)
                    {
                        if (chapterItem == null || chapterItem.wnd == null)
                            continue;
                        ALUGUICommon.setGameObjEnable(chapterItem.wnd.chapterDetailShowGos, true);
                    }
            }
        }

        private void setToChapterViewPos(long _chapterId)
        {
            if (wnd == null || wnd.chapterItems == null || wnd.viewScrollRect == null || wnd.viewScrollRect.viewport == null || wnd.viewScrollRect.content == null)
                return;
            LayoutRebuilder.ForceRebuildLayoutImmediate(wnd.viewScrollRect.content);
            GGUIWndTowerMainChapterItem chapterItem = getChapterItem(_chapterId);

            wnd.viewScrollRect.verticalNormalizedPosition = calculateTargetPosNormalizePos(chapterItem);
        }

        private float calculateTargetPosNormalizePos(GGUIWndTowerMainChapterItem _item)
        {
            if (wnd == null || wnd.chapterItems == null || wnd.viewScrollRect == null || wnd.viewScrollRect.viewport == null || wnd.viewScrollRect.content == null || _item.wnd == null)
                return 0;
         
            RectTransform targetTrans = _item.wnd.transform as RectTransform;
            Vector3 rectCenter = targetTrans.getRectInParent().center;
            float normalizeY = (wnd.viewScrollRect.content.rect.height + rectCenter.y - wnd.viewScrollRect.viewport.rect.height/2)/ (wnd.viewScrollRect.content.rect.height - wnd.viewScrollRect.viewport.rect.height) ;
            normalizeY = Mathf.Clamp01(normalizeY);
  
            return normalizeY;
        }
    
        private GGUIWndTowerMainChapterItem getChapterItem(long _chapterId)
        {
            if (_m_chapterItems != null)
                foreach (GGUIWndTowerMainChapterItem chapterItem in _m_chapterItems)
                {
                    if (chapterItem != null && chapterItem.chapterId == _chapterId)
                    {
                        return chapterItem;
                    }
                }

            return null;
        }

    }
}