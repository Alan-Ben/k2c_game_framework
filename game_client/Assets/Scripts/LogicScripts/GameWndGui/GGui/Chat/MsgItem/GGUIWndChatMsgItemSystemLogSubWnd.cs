using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChatMsgItemSystemLogSubWnd : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoChatMsgItemSystemLogSubWnd>
    {
        [NotNull]private ChatSystemLogRefObj _m_chatSystemLogRef;
        private ChatSystemLogMsgInfo _m_detailInfo;
        private NPGGuiWndTexture _m_bannerWnd;

        public GGUIWndChatMsgItemSystemLogSubWnd(ChatSystemLogMsgInfo _detailInfo, [NotNull]ChatSystemLogRefObj _chatSystemLogRef, Transform _parent) : base(_parent)
        {
            _m_detailInfo = _detailInfo;
            _m_chatSystemLogRef = _chatSystemLogRef;
        }

        protected override string _monoAssetPath => _m_chatSystemLogRef.ui_path?.asset_path;

        protected override string _monoObjName => _m_chatSystemLogRef.ui_path?.obj_name;

        protected override _AALResourceCore _resourceCore => GameResCore.instance;
        
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_bannerWnd?.discardTexture();
        }

        protected override void _onReset()
        {
            _m_bannerWnd?.discardTexture();
        }

        protected override void _onDiscard()
        {
            ALUGUICommon.uncombineBtnClick(wnd.btnJump, _onClickBtnJump);
            _m_bannerWnd?.discard();
            _m_bannerWnd = null;
        }
        
        protected override void _onWndInitDone()
        {
            if (wnd == null) return;
            if(wnd.imgBanner != null)
                _m_bannerWnd = new NPGGuiWndTexture(wnd.imgBanner);
            ALUGUICommon.combineBtnClick(wnd.btnJump, _onClickBtnJump);
        }
        
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtTitle, TextTranslate.instance.getLanguage(_m_chatSystemLogRef.title));
            ALUGUICommon.setLabelTxt(wnd.txtContent, _m_detailInfo?.getMiniContent());
            _m_bannerWnd?.setTexture(_m_chatSystemLogRef.banner);
            _m_bannerWnd?.showWnd();
        }
        
        private void _onClickBtnJump(GameObject obj)
        {
            if (_m_chatSystemLogRef.function_type != ENPFunctionType.NONE && !GCommon.isFuncUnlock(_m_chatSystemLogRef.function_type, true))
                return;
            if(!GCommon.isSimpleUnlock(_m_chatSystemLogRef.simple_unlock_id, true))
                return;
            _m_chatSystemLogRef.go_to?.dealEffect();
        }

    }
}