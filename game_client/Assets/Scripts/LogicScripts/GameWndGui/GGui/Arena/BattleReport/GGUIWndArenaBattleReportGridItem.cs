using ALPackage;

namespace GOE
{
    /// <summary>
    /// 竞技场战报列表item
    /// </summary>
    public class GGUIWndArenaBattleReportGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoArenaBattleReportGridItem>
    {
        //战报信息
        private ArenaBattleReportInfo _m_reportInfo;
        //玩家头像
        private NPGGUIWndPlayerIcon _m_wPlayerIcon;
        //显示序列
        private long _m_lShowSerialize;

        public GGUIWndArenaBattleReportGridItem(GGUIMonoArenaBattleReportGridItem  _wnd)
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

            if (wnd.monoPlayerIcon != null)
                _m_wPlayerIcon = new NPGGUIWndPlayerIcon(wnd.monoPlayerIcon);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(ArenaBattleReportInfo _info)
        {
            _m_reportInfo = _info;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshPlayerInfo();
            _refreshReportInfo();
        }

        //刷新玩家信息
        private void _refreshPlayerInfo()
        {
            if (wnd == null || _m_reportInfo == null)
                return;

            long serialize = _m_lShowSerialize;
            _m_reportInfo.getPlayerInfo(_playerInfo =>
            {
                if (_m_lShowSerialize != serialize || null == _playerInfo || wnd == null || !isShow)
                    return;

                _m_wPlayerIcon?.showWnd();
                _m_wPlayerIcon?.setPlayerInfo(_playerInfo);
            });
        }

        //刷新战报信息
        private void _refreshReportInfo()
        {
            if (wnd == null || _m_reportInfo == null)
                return;

            //击败我方伙伴描述
            ALUGUICommon.setLabelTxt(wnd.txtDefeatHeroCount,
                TextTranslate.instance.getLanguage(TransKeyConst.arena_defeatMyHeroCount_num,
                   _m_reportInfo.defeatHeroNum));

            //我方影响力变化描述
            ALUGUICommon.setLabelTxt(wnd.txtInfluenceChg,
                TextTranslate.instance.getLanguage(TransKeyConst.arena_myInfluenceChg_num,
                    TextTranslate.instance.getLanguage(TransKeyConst.common_reduced_num, _m_reportInfo.deductinfluence)));

            //时间
            ALUGUICommon.setLabelTxt(wnd.txtTime,
                TextTranslate.instance.getLanguage(TransKeyConst.arena_reportPassTime_str,
                    TimeUtil.getPassTimeShow(_m_reportInfo.timestamp)));
        }
    }
}
