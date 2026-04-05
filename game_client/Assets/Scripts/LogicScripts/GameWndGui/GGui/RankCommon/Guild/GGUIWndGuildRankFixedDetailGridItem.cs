using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟排行列表item
    /// </summary>
    public class GGUIWndGuildRankFixedDetailGridItem : _AGGUIWndBaseSubRankGuildInfo<GGUIMonoGuildRankFixedDetailGridItem, GuildRankInfo>
    {
        public GGUIWndGuildRankFixedDetailGridItem(GGUIMonoGuildRankFixedDetailGridItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWndEx()
        {
        }

        protected override void _onHideWndEx()
        {
        }

        protected override void _onResetEx()
        {
        }

        protected override void _onDiscardEx()
        {
        }

        protected override void _onWndInitDoneEx()
        {
        }
    }
}
