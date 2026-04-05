using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 文本类型tip
    /// </summary>
    public class NPTextTipDealer : _ATNPTipDealer<NPGGUIWndTextTip, NPGGUIMonoTextTip>
    {
        private List<string> _m_lTextList;//文本列表
        private Action<NPGGUIWndTextTip> _m_aOnPop;//弹出回调

        public NPTextTipDealer(List<string> _textList, NPCenterTipsRefObj _tipRef, Action<NPGGUIWndTextTip> _onPop, ETipDealerTagType _tag = ETipDealerTagType.NONE)
            : base(_tipRef, _tag)
        {
            _m_lTextList = _textList;
            _m_aOnPop = _onPop;
        }

        /// <summary>
        /// 弹出tip时调用
        /// </summary>
        /// <param name="_tipWnd"></param>
        protected override void _onPop()
        {
            _m_aOnPop?.Invoke(_wnd);
        }

        /// <summary>
        /// 设置窗口数据的处理
        /// 弹出tip时调用
        /// </summary>
        /// <param name="_tipWnd"></param>
        protected override void _setTipData(NPGGUIWndTextTip _tipWnd)
        {
            if (_tipWnd == null)
                return;

            _tipWnd.setTipData(_m_lTextList);
        }
    }
}
