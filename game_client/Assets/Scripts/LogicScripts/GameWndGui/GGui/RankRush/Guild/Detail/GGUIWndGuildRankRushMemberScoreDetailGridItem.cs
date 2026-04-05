using ALPackage;

namespace GOE
{
    /// <summary>
    /// 联盟冲榜联盟成员积分详情成员列表item
    /// </summary>
    public class GGUIWndGuildRankRushMemberScoreDetailGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoGuildRankRushMemberScoreDetailGridItem>
    {
        private long _m_lOperialize;
        public GGUIWndGuildRankRushMemberScoreDetailGridItem(GGUIMonoGuildRankRushMemberScoreDetailGridItem _wnd) : base(_wnd)
        {
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _resetGridItem()
        {
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        public void setInfo(SubRankShowInfo _info)
        {
            if (_info == null || wnd  == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtRank, itemIdx + 1);
            ALUGUICommon.setLabelTxt(wnd.txtScore, GCommon.getValueFormatStr(_info.rankScoreFormat, _info.rankScore));
            ALUGUICommon.setLabelTxt(wnd.txtName, "");

            _m_lOperialize = ALSerializeOpMgr.next();
            long curSerialize = _m_lOperialize;

            _info.getInfo(false, _info =>
            {
                if (curSerialize != _m_lOperialize || wnd == null || !isShow || _info == null)
                    return;

                ALUGUICommon.setLabelTxt(wnd.txtName, _info.playerInfo?.name);
            });
        }
    }
}
