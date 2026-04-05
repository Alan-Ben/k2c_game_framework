using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 对话历史回顾选项
    /// </summary>
    public class GGUIWndDialogueHistoryOptionContainerItem : _ANPGGUIBasicSubWnd<GGUIMonoDialogueHistoryOptionContainerItem>
    {
        public GGUIWndDialogueHistoryOptionContainerItem(GGUIMonoDialogueHistoryOptionContainerItem _mono) : base(_mono)
        {
            initWnd();
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

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_optionRef"></param>
        /// <param name="_isSelect"></param>
        public void setInfo(NPDialogueResponseOptionRefObj _optionRef, bool _isSelect)
        {
            if (wnd == null || _optionRef == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtContent, TextTranslate.instance.getLanguage(_optionRef.option_desc));
            ALUGUICommon.setGameObjEnable(wnd.goSelectHideList, !_isSelect);
            ALUGUICommon.setGameObjEnable(wnd.goSelectShowList, _isSelect);

            if (wnd != null && wnd.txtContent != null)
                LayoutRebuilder.ForceRebuildLayoutImmediate(wnd.txtContent.rectTransform);

            _dealRightStyleShow();
        }

        /// <summary>
        /// 处理右边对话样式文本布局
        /// </summary>
        private void _dealRightStyleShow()
        {
            if (wnd == null || wnd.txtContent == null || wnd.monoSizeFix == null)
                return;

            if (wnd.txtContent.preferredWidth > wnd.monoSizeFix.maxPreferredWidth)
                wnd.txtContent.alignment = TextAnchor.UpperLeft;
            else
                wnd.txtContent.alignment = TextAnchor.UpperRight;
        }
    }
}
