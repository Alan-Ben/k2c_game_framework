using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    ///  联盟副本升级成功 notice
    /// </summary>
    public class NPNoticeDealer_GuildDungeonUpgradeSuc : NPUINoticeMgr._ANPUINoticeDealer
    {
        private GuildDungeonRefObj _m_info;
        private long _m_oldLvl;
        private long _m_newLvl;
        //窗口是否已经加载，如果已经加载的情况下。在shownotice的时候不会再加载，避免窗口驻留
        private bool _m_bWndLoaded;

        public NPNoticeDealer_GuildDungeonUpgradeSuc(GuildDungeonRefObj _info, long _oldLvl, long _newLvl)
        {
            _m_info = _info;
            _m_oldLvl = _oldLvl;
            _m_newLvl = _newLvl;
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
                GGUIWndGuildDungeonUpgradeSuc.instance.discard();
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
                GGUIWndGuildDungeonUpgradeSuc.instance.load(() =>
                {
                    GGUIWndGuildDungeonUpgradeSuc.instance.showWnd();
                    GGUIWndGuildDungeonUpgradeSuc.instance.setInfo(_m_info, _m_oldLvl, _m_newLvl, setDealerDone);
                });
            }
            else
            {
                //已经加载的，需要刷新层级
                if (null != GGUIWndGuildDungeonUpgradeSuc.instance.wnd)
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndGuildDungeonUpgradeSuc.instance.rectTransform);
            }
        }

        public override void dealHideNotice()
        {
        }
    }
}
