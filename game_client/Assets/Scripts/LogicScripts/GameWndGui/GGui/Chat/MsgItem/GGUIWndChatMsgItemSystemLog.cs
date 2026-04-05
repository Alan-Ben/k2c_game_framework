using ALPackage;
using ChatPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChatMsgItemSystemLog : _ANPGUISubWndChatMsgListItem<GGUIMonoChatMsgItemSystemLog, ChatSystemLogMsgInfo>
    {
        private GGUIWndChatMsgItemSystemLogSubWnd _m_subWnd;
        public GGUIWndChatMsgItemSystemLog(ChatSystemLogMsgInfo _detailInfo, Transform _parent, [NotNull] GUICacheMgrChatMsgItem _cacheMgr) : base(_detailInfo, _parent, _cacheMgr)
        {
        }

        protected override void _onShowWnd()
        {
            _m_subWnd?.showWnd();
        }

        protected override void _onHideWnd()
        {
            _m_subWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_subWnd?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_subWnd?.discard();
            _m_subWnd = null;
        }
        
        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
            if (wnd == null || detailInfo == null || detailInfo.chatSystemLogRef == null)
                return;
            _m_subWnd = new GGUIWndChatMsgItemSystemLogSubWnd(detailInfo, detailInfo.chatSystemLogRef, wnd.uiParent);
            _m_subWnd.load(_m_subWnd.showWnd);
        }

        public override float getHeight()
        {
            if (detailInfo == null || detailInfo.chatSystemLogRef == null)
                return 0;
            
            return detailInfo.chatSystemLogRef.ui_height;
        }

        protected override void _loadAdditionTemplate(ALStepCounter _stepCounter)
        {
        }

        protected override void _discardAdditionTemplate()
        {
        }
    }
}