using ALPackage;
using ChatPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChatMsgGuildPrivateInform : _ATNPGGUIWndPlayerChatMsgItem<GGUIMonoChatMsgItemGuildPrivateInform, ChatGuildPrivateInformMsgInfo>
    {
        public GGUIWndChatMsgGuildPrivateInform(ChatGuildPrivateInformMsgInfo _detailInfo, Transform _parent, [NotNull] GUICacheMgrChatMsgItem _cacheMgr) : base(_detailInfo, _parent, _cacheMgr)
        {
        }

        protected override void _loadAdditionTemplate(ALStepCounter _stepCounter)
        {
        }

        protected override void _discardAdditionTemplate()
        {
        }

        protected override long senderPlayerCid { get { return detailInfo == null ? 0 : detailInfo.sender.getCid(); } }

        protected override void _onShowWndEx()
        {
            _refreshWnd();
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

        public override float getHeight()
        {
            if (_originMono == null)
                return 0;

            RectTransform originRectTransform = _originMono.transform as RectTransform;
            if (originRectTransform == null)
                return 0;
            
            return originRectTransform.rect.height;
        }
        
        protected override void _refreshPlayerInfo(NPGGUIWndPlayerIcon _playerInfoWnd)
        {
            if (_playerInfoWnd == null || detailInfo == null)
                return;

            _playerInfoWnd.setPlayerInfo(detailInfo.sender);
        }
        
        private void _refreshWnd()
        {
            if (wnd == null || detailInfo == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtContent, detailInfo.content.getContent());
        }
    }
}