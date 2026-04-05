using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    ///  联盟PVE战斗成功 notice
    /// </summary>
    public class NPNoticeDealer_GuildDungeonBattleSuc : NPUINoticeMgr._ANPUINoticeDealer
    {
        private List<NPCommon.NPCommon_ItemInfo> _m_info;
        //窗口是否已经加载，如果已经加载的情况下。在shownotice的时候不会再加载，避免窗口驻留
        private bool _m_bWndLoaded;

        private Action _m_onDealDone;

        public NPNoticeDealer_GuildDungeonBattleSuc(List<NPCommon.NPCommon_ItemInfo> _info, Action _onDealerDone = null)
        {
            _m_info = new List<NPCommon.NPCommon_ItemInfo>(_info);
            _m_onDealDone = _onDealerDone;
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
                GGUIWndGuildDungeonBattleSuc.instance.discard();
                _m_bWndLoaded = false;
            }
            _m_onDealDone?.Invoke();
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
                GGUIWndGuildDungeonBattleSuc.instance.load(() =>
                {
                    GGUIWndGuildDungeonBattleSuc.instance.showWnd();
                    GGUIWndGuildDungeonBattleSuc.instance.setInfo(_m_info, setDealerDone);
                });
            }
            else
            {
                //已经加载的，需要刷新层级
                if (null != GGUIWndGuildDungeonBattleSuc.instance.wnd)
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndGuildDungeonBattleSuc.instance.rectTransform);
            }
        }

        public override void dealHideNotice()
        {
        }
    }
}
