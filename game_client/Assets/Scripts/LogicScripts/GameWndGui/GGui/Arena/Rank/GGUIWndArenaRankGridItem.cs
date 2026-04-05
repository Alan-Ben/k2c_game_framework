using ALPackage;

namespace GOE
{
    /// <summary>
    /// 竞技场排行榜列表item
    /// </summary>
    public class GGUIWndArenaRankGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoArenaRankGridItem>
    {
        //排行榜信息
        private NPRankCommonShowInfo _m_rankInfo;
        //玩家头像
        private NPGGUIWndPlayerIcon _m_wPlayerIcon;
        //显示序列
        private long _m_lShowSerialize;

        public GGUIWndArenaRankGridItem(GGUIMonoArenaRankGridItem  _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_wPlayerIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wPlayerIcon?.resetWnd();
        }

        protected override void _resetGridItem()
        {
            _m_wPlayerIcon?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wPlayerIcon?.discard();
            _m_wPlayerIcon = null;

            if (null == wnd)
                return;
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if(wnd.monoPlayerIcon != null)
                _m_wPlayerIcon = new NPGGUIWndPlayerIcon(wnd.monoPlayerIcon);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(NPRankCommonShowInfo _info)
        {
            _m_rankInfo = _info;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshRankInfo();
            _refreshPlayerInfo();
        }

        //刷新排行榜信息
        private void _refreshRankInfo()
        {
            if(wnd == null || _m_rankInfo == null)
                return;

            wnd.setRank(_m_rankInfo.rankSortId);
            ALUGUICommon.setLabelTxt(wnd.txtInfluence, _m_rankInfo.rankScore.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
        }

        //刷新玩家信息
        private void _refreshPlayerInfo()
        {
            if (wnd == null || _m_rankInfo == null)
                return;

            long serialize = _m_lShowSerialize;
            _m_rankInfo.getInfo(false, _info =>
            {
                if (_m_lShowSerialize != serialize || null == _info || wnd == null || !isShow)
                    return;

                _m_wPlayerIcon?.showWnd();
                _m_wPlayerIcon?.setPlayerInfo(_info.playerInfo);
            });
        }
    }
}
