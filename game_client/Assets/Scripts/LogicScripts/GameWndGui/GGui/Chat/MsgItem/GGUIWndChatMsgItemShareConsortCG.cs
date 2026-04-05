using ALPackage;
using ChatPackage;
using UnityEngine;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 一条妃子CG分享信息的msgItem
    /// </summary>
    public class GGUIWndChatMsgItemShareConsortCG : _ATNPGGUIWndPlayerChatMsgItem<GGUIMonoChatMsgItemConsortCG, ChatShareConsortCGMsgDetailInfo>
    {
        //图标
        private NPGGuiWndTexture _m_wIcon;

        public GGUIWndChatMsgItemShareConsortCG(ChatShareConsortCGMsgDetailInfo _detailInfo, Transform _parent, [NotNull] GUICacheMgrChatMsgItem _cacheMgr) : base(_detailInfo, _parent, _cacheMgr)
        {
        }
        
        protected override long senderPlayerCid { get { return detailInfo == null ? 0 : detailInfo.sender.getCid(); } }

        protected override void _onShowWndEx()
        {
            _refreshWnd();
        }

        protected override void _onHideWndEx()
        {
            _m_wIcon?.hideWnd();
        }

        protected override void _onResetEx()
        {
            _m_wIcon?.discardTexture();
        }

        protected override void _onDiscardEx()
        {
            _m_wIcon?.discard();
            _m_wIcon = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnDetail, _onClickDetail);
        }

        protected override void _onWndInitDoneEx()
        {
            if (wnd == null)
                return;

            if (wnd.texImage != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.texImage);

            ALUGUICommon.combineBtnClick(wnd.btnDetail, _onClickDetail);
        }

        protected override void _refreshPlayerInfo(NPGGUIWndPlayerIcon _playerInfoWnd)
        {
            if (_playerInfoWnd == null || detailInfo == null)
                return;

            _playerInfoWnd.setPlayerInfo(detailInfo.sender);
        }

        protected override void _loadAdditionTemplate(ALStepCounter _stepCounter)
        {
        }

        protected override void _discardAdditionTemplate()
        {
        }

        private void _refreshWnd()
        {
            if (wnd == null || detailInfo == null)
                return;

            ConsortCGRefObj consortCGRefObj = GRefdataCoreMgr.instance.consortCGRefCore.getRef(detailInfo.content.getCgId());
            if (consortCGRefObj == null)
                return;

            _m_wIcon?.showWnd();
            _m_wIcon?.setTexture(consortCGRefObj.cg_icon);

            ALUGUICommon.setLabelTxt(wnd.txtCGName, TextTranslate.instance.getLanguage(consortCGRefObj.name));
        }

        /// <summary>
        /// 点击详情按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickDetail(GameObject _go)
        {
            if (detailInfo == null || detailInfo.content == null)
                return;

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndShareConsortCGDetail.instance, () =>
            {
                GGUIWndShareConsortCGDetail.instance.showWnd();
                GGUIWndShareConsortCGDetail.instance.setInfo(detailInfo.content.getCgId());
            }, EUIQueueStageType.MAIN, UINodeTagConst.C_CHAT_SELECT_SHARE_CONSORT_CG_DETAIL, false, false);
        }
    }
}