using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 爬塔挑战成功 notice
    /// </summary>
    public class NPNoticeDealer_TowerBattleSuc : NPUINoticeMgr._ANPUINoticeDealer
    {
        private TowerChallengeResult _m_info;
        //窗口是否已经加载，如果已经加载的情况下。在shownotice的时候不会再加载，避免窗口驻留
        private bool _m_bWndLoaded;

        public NPNoticeDealer_TowerBattleSuc(TowerChallengeResult _info)
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
                GGUIWndTowerBattleSuc.instance.discard();
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
                GGUIWndTowerBattleSuc.instance.load(() =>
                {
                    GGUIWndTowerBattleSuc.instance.showWnd();
                    GGUIWndTowerBattleSuc.instance.setInfo(_m_info, setDealerDone);
                });
            }
            else
            {
                //已经加载的，需要刷新层级
                if (null != GGUIWndTowerBattleSuc.instance.wnd)
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndTowerBattleSuc.instance.rectTransform);
            }
        }

        public override void dealHideNotice()
        {
        }
    }
}
