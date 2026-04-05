using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// TowerBattleCrossChapter notice
    /// </summary>
    public class NPNoticeDealer_TowerBattleCrossChapter : NPUINoticeMgr._ANPUINoticeDealer
    {
        private long _m_info;
        //窗口是否已经加载，如果已经加载的情况下。在shownotice的时候不会再加载，避免窗口驻留
        private bool _m_bWndLoaded;

        public NPNoticeDealer_TowerBattleCrossChapter(long _info)
        {
            _m_info = _info;
        }
        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return true; } }
        public override bool isNoticeFullScreen { get { return false; } }
        public override bool isOnlyUINode { get { return true; } }
        protected override void _onDealerDone()
        {
            if (_m_bWndLoaded)
            {
                GGUIWndTowerBattleCrossChapter.instance.discard();
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TOWER_CHALLENGE);
                QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndTowerMain.instance, UINodeTagConst.C_TOWER_MAIN, null,
                    () =>
                    {
                        GGUIWndTowerMain.instance.showChapterUnlockAnim();
                    }, 0);
                

                _m_bWndLoaded = false;
            }
        }

        public override void dealShowNotice()
        {
            if (_m_info == null)
            {
                setDealerDone();
                return;
            }

            if (!_m_bWndLoaded)
            {
                _m_bWndLoaded = true;
                GGUIWndTowerBattleCrossChapter.instance.load(() =>
                {
                    GGUIWndTowerBattleCrossChapter.instance.showWnd();
                    GGUIWndTowerBattleCrossChapter.instance.setInfo(_m_info, setDealerDone);
                });
            }
            else
            {
                //已经加载的，需要刷新层级
                if (null != GGUIWndTowerBattleCrossChapter.instance.wnd)
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndTowerBattleCrossChapter.instance.rectTransform);
            }
        }

        public override void dealHideNotice()
        {
        }
    }
}
