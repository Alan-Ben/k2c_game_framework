using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndTowerMainChapterItem : _ATALBasicUISubWnd<GGUIMonoTowerMainChapterItem>
    {
        public long chapterId => wnd != null ? wnd.chapterId : 0;

        public Action<long> _m_onChapterShow;
        public Action _m_onChapterHide;

        public GGUIWndTowerMainChapterItem(GGUIMonoTowerMainChapterItem _wnd, Action<long> _onChapterShow, Action _onChapterHide) : base(_wnd)
        {
            _m_onChapterShow = _onChapterShow;
            _m_onChapterHide = _onChapterHide;
            initWnd();
        }


        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_TOWER_CHG, _refreshWnd);
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_TOWER_CHG, _refreshWnd);
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onBtnClick);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnChallenge, _onBtnChanllengeClick);
        }


        /// <summary>
        /// 刷新显示
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            TowerChapterRefObj chapterRefObj = GRefdataCoreMgr.instance.towerChapterRefCore.getRef(wnd.chapterId);
            if(chapterRefObj == null)
                return;
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(chapterRefObj.name));
            string totalLevelDesc = TextTranslate.instance.getLanguage(TransKeyConst.tower_common_total_level_desc, chapterRefObj.total_level_start + 1);
            ALUGUICommon.setLabelTxt(wnd.txtLevel, totalLevelDesc);
            long curChapterId = NPPlayer.instance.towerComp.curChapterId;
            long curLevel = NPPlayer.instance.towerComp.curLevel;
            // 【优化-1】爬塔-挑战按钮显示规则优化  https://www.teambition.com/task/68ca635368f225cf182826a2
            TowerChapterRefObj curChapterRefObj = GRefdataCoreMgr.instance.towerChapterRefCore.getRef(curChapterId);
            if (curChapterRefObj != null && curLevel == curChapterRefObj.level_count)
            {
                TowerChapterRefObj nextChapterRefObj = GRefdataCoreMgr.instance.getTowerNextChapterRefObj(curChapterId);
                ALUGUICommon.setGameObjEnable(wnd.nextFightPointShowGos, nextChapterRefObj != null && wnd.chapterId == nextChapterRefObj.id);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.nextFightPointShowGos, wnd.chapterId == curChapterId);
            }
            
            ALUGUICommon.setGameObjEnable(wnd.curChapterShowGos, wnd.chapterId == curChapterId);
            bool alreadyComplete = false;
            foreach (TowerChapterRefObj chapterRef in GRefdataCoreMgr.instance.towerChapterRefCore.refList)
            {
                if (chapterRef != null )
                {
                    // 遍历章节id，如果到当前章节id了，还没碰到这个关卡，则说明还没完成
                    if (chapterRef.id == curChapterId)
                        break;
                    // 遍历章节id，如果还没到当前章节id，就碰到了这个关卡，则说明已经完成
                    if (wnd.chapterId == chapterRef.id)
                    {
                        alreadyComplete = true;
                        break;
                    }
                }
            }
            ALUGUICommon.setGameObjEnable(wnd.alreadyCompleteShowGos, alreadyComplete);
        }

        private void _onBtnClick(GameObject _go)
        {
            if (wnd == null) return;
            TowerChapterRefObj chapterRefObj = GRefdataCoreMgr.instance.towerChapterRefCore.getRef(wnd.chapterId);
            if(chapterRefObj == null)
                return;
            _m_onChapterShow?.Invoke(wnd.chapterId);
            if (chapterRefObj.if_pve_chapter)
            {
                GGUIWndTowerChapter.instance.setInfo(wnd.chapterId, _m_onChapterHide);
                QueueMgr.instance.addNode_InGame_MainUIAddWnd(GGUIWndTowerChapter.instance, EUIQueueStageType.MAIN, UINodeTagConst.C_TOWER_CHAPTER, null, null, false);
            }
            else
            {
                QueueMgr.instance.addNode_InGame_MainUIAddWnd(GGUIWndTowerChapterPVP.instance, EUIQueueStageType.MAIN, UINodeTagConst.C_TOWER_CHAPTER_PVP, null,
                    () =>
                    {
                        GGUIWndTowerChapterPVP.instance.setInfo(wnd.chapterId, _m_onChapterHide);
                    }, false);
            }
        }

        private void _onBtnChanllengeClick(GameObject _obj)
        {
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndTowerChallenge.instance, UINodeTagConst.C_TOWER_CHALLENGE, 0);
        }
        
        public void setUnlock(bool _isLock)
        {
            if (null == wnd)
                return;
            if (wnd.unlockAnimation != null)
                wnd.unlockAnimation.Sample(wnd.unlockAnimName, _isLock ? 0 : 1);
        }
        public void setLockAndNextFight()
        {
            if (null == wnd)
                return;
            if (wnd.unlockAnimation != null)
                wnd.unlockAnimation.Sample(wnd.lockAndNextFightAnimName, 0 );
        }
        public void playUnlockAnimation()
        {
            if (null == wnd || wnd.unlockAnimation == null)
                return;
            wnd.unlockAnimation.Play(wnd.unlockAnimName);
        }
    }
}