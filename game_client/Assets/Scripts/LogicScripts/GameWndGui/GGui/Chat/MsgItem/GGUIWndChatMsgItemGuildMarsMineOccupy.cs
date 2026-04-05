using ALPackage;
using ChatPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChatMsgItemGuildMarsMineOccupy : _ANPGUISubWndChatMsgListItem<GGUIMonoChatMsgItemGuildMarsMineOccupy, ChatGuildMarsMineOccupyMsgInfo>
    {
        public GGUIWndChatMsgItemGuildMarsMineOccupy(ChatGuildMarsMineOccupyMsgInfo _detailInfo, Transform _parent, [NotNull] GUICacheMgrChatMsgItem _cacheMgr) : base(_detailInfo, _parent, _cacheMgr)
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
            if (wnd == null)
                return;
            
            // ALUGUICommon.uncombineBtnClick(wnd.btnJump, _onBtnJumpClick);
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
            
            if (wnd == null)
                return;
            
            // ALUGUICommon.combineBtnClick(wnd.btnJump, _onBtnJumpClick);
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

            string content = TextTranslate.instance.getLanguage(
                TransKeyConst.mars_explore_guildMineUnderAttack_guildName_attackerName_name,
                detailInfo.content.getAttackGuildSimpleName(),
                detailInfo.content.getAttackCname(),
                detailInfo.content.getCname()
            );
            ALUGUICommon.setLabelTxt(wnd.txtContent, content);
        }
        private void _onBtnJumpClick(GameObject _)
        {
            // 这版本先不做
        }
    }
}
