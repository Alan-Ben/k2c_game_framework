using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟申请列表item
    /// </summary>
    public class GGUIWndGuildOthersApplyListGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoGuildOthersApplyListGridItem>
    {
        //玩家信息附加窗口
        private NPGGUIWndPlayerIcon _m_wPlayInfo;
        //申请信息
        private GuildJoinRequestInfo _m_requestInfo;
        //是否请求最新数据
        private bool _m_bReqNewPlayerInfo;
        //显示序列
        private long _m_lShowSerialize;

        public GGUIWndGuildOthersApplyListGridItem(GGUIMonoGuildOthersApplyListGridItem _wnd) : base(_wnd)
        {
        }

        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_wPlayInfo?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wPlayInfo?.resetWnd();
        }

        protected override void _resetGridItem()
        {
            _m_wPlayInfo?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wPlayInfo?.discard();
            _m_wPlayInfo = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnAgree, _onClickAgree);
            ALUGUICommon.uncombineBtnClick(wnd.btnRefuse, _onClickRefuse);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoPlayerIcon != null)
                _m_wPlayInfo = new NPGGUIWndPlayerIcon(wnd.monoPlayerIcon);

            ALUGUICommon.combineBtnClick(wnd.btnAgree, _onClickAgree);
            ALUGUICommon.combineBtnClick(wnd.btnRefuse, _onClickRefuse);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_reqNewPlayerInfo"></param>
        public void setInfo(GuildJoinRequestInfo _info, bool _reqNewPlayerInfo)
        {
            _m_requestInfo = _info;
            _m_bReqNewPlayerInfo = _reqNewPlayerInfo;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (_m_requestInfo == null)
                return;

            //默认先隐藏
            _m_wPlayInfo?.hideWnd();
            long serialze = _m_lShowSerialize;
            _m_requestInfo.getPlayerDetailInfo(_playerInfo =>
            {
                if (serialze != _m_lShowSerialize)
                    return;

                if (_m_wPlayInfo != null)
                {
                    _m_wPlayInfo.showWnd();
                    _m_wPlayInfo.setPlayerInfo(_playerInfo);
                }
            },_m_bReqNewPlayerInfo);
        }

        #region 点击事件

        //点击同意
        private void _onClickAgree(GameObject _go)
        {
            if (_m_requestInfo == null)
                return;

            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if (guildInfo == null)
                return;

            GuildLevelRefObj levelRef = GRefdataCoreMgr.instance.guildLevelRefCore.getRef(guildInfo.level);
            if (levelRef == null)
                return;

            //人数是否达到上限
            if (guildInfo.memberCount == levelRef.member_limit)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_guildMemberCountReachedLimit_none);
                return;
            }

            //请求同意
            NPPlayer.instance.guildComp.reqProcessGuildJoinRequest(_m_requestInfo.dbId, true, null);
        }

        //点击拒绝
        private void _onClickRefuse(GameObject _go)
        {
            if (_m_requestInfo == null)
                return;

            //请求拒绝
            NPPlayer.instance.guildComp.reqProcessGuildJoinRequest(_m_requestInfo.dbId, false, null);
        }

        #endregion
    }
}
