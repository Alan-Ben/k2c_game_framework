using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 效果加载自定义窗口
    /// </summary>
    public class NPGGUIWndEffectCustomAddUI : _ANPGGUIBasicWnd<NPGGUIMonoEffectCustomUI>
    {

        private long _m_lUIResId;//资源id

        public NPGGUIWndEffectCustomAddUI(long _uiResId) : base(EALUIWndLayer.ADDITION)
        {
            _m_lUIResId = _uiResId;
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_lUIResId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_lUIResId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {

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

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickBtnClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickBtnClose);
        }

        /// <summary>
        /// 点击关闭
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickBtnClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_EFFECT_CUSTOM_ADD_UI);
        }
    }
}
