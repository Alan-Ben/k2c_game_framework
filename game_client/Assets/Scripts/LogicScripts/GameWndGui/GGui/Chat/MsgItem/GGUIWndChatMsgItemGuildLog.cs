using ALPackage;
using ChatPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChatMsgItemGuildLog : _ANPGUISubWndChatMsgListItem<GGUIMonoChatMsgItemGuildLog, ChatGuildLogMsgInfo>
    {
        public GGUIWndChatMsgItemGuildLog(ChatGuildLogMsgInfo _detailInfo, Transform _parent, [NotNull] GUICacheMgrChatMsgItem _cacheMgr) : base(_detailInfo, _parent, _cacheMgr)
        {
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
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

            ALUGUICommon.setLabelTxt(wnd.txtContent, TextTranslate.instance.getLanguage(detailInfo.guildLogDetailInfo?.content));
        }
    }
}