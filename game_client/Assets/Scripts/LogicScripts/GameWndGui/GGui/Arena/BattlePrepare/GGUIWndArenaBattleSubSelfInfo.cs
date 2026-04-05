using ALPackage;

namespace GOE
{
    /// <summary>
    /// 竞技场战斗自己信息附加窗口
    /// </summary>
    public class GGUIWndArenaBattleSubSelfInfo : _ATALBasicUISubWnd<GGUIMonoArenaBattleSubSelfInfo>
    {
        //玩家形象
        private NPGGUIWndCommonShowCase _m_wPlayerShowcase;
        //玩家头像
        private NPGGUIWndPlayerIcon _m_wPlayerIcon;
        //显示序列号
        private long _m_lShowSerialize;

        public GGUIWndArenaBattleSubSelfInfo(GGUIMonoArenaBattleSubSelfInfo _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_wPlayerIcon?.hideWnd();
            _m_wPlayerShowcase?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wPlayerIcon?.resetWnd();
            _m_wPlayerShowcase?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wPlayerIcon?.discard();
            _m_wPlayerIcon = null;
            _m_wPlayerShowcase?.discard();
            _m_wPlayerShowcase = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoPlayerIcon != null)
                _m_wPlayerIcon = new NPGGUIWndPlayerIcon(wnd.monoPlayerIcon);

            if (wnd.monoPlayerShowcase != null)
                _m_wPlayerShowcase = new NPGGUIWndCommonShowCase(wnd.monoPlayerShowcase);
        }

        //刷新显示
        private void _refreshWnd()
        {
            if (wnd == null) 
                return;

            long serialize = _m_lShowSerialize;
            long rankFixId = GRefdataCoreMgr.instance.npGeneral.arena_rank_fixed_id;

            //先重置状态
            ALUGUICommon.setLabelTxt(wnd.txtInfluence, "");
            ALUGUICommon.setLabelTxt(wnd.txtRank, TextTranslate.instance.getLanguage(TransKeyConst.arena_rankNow_num, ""));

            //设置玩家信息
            _m_wPlayerIcon?.showWnd();
            _m_wPlayerIcon?.setSelfInfo();

            //设置玩家形象
            NPGGoIndex resIndex = NPPlayer.instance?.playerInfo?.curSkinRef?.td_show;
            if (resIndex != null)
                _m_wPlayerShowcase?.showWnd(new ShowCaseCommonResUnitInfoObj(resIndex));

            //获取排名、分数
            NPPlayer.instance.rankCommonComp.reqInRankPlayerInfoByCid(rankFixId, NPPlayer.instance.playerInfo.CID, _rankInfo =>
            {
                if (wnd == null || !isShow || serialize != _m_lShowSerialize || _rankInfo == null)
                    return;

                ALUGUICommon.setLabelTxt(wnd.txtRank, TextTranslate.instance.getLanguage(TransKeyConst.arena_rankNow_num, _rankInfo.getRank()));
                ALUGUICommon.setLabelTxt(wnd.txtInfluence, TextTranslate.instance.getLanguage(TransKeyConst.arena_influence_num, _rankInfo.getScore()));
            });
        }
    }
}
