using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 剧情对话记录列表选项item
    /// </summary>
    public class GGUIWndPlotDialogueOptionItem : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoPlotDialogueOptionItem>
    {
        private long _m_lResPathId;//资源路径id

        public GGUIWndPlotDialogueOptionItem(long _resPathId, Transform _parent)
            : base(_parent)
        {
            _m_lResPathId = _resPathId;
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_lResPathId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_lResPathId); } }
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
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_content"></param>
        public void setInfo(string _content)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtContent, TextTranslate.instance.getLanguage(_content));
        }
    }
}