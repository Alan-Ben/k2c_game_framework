using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 图片+文本类型tip
    /// </summary>
    public class NPIconTextTipDealer : _ATNPTipDealer<NPGGUIWndIconTextTip, NPGGUIMonoIconTextTip>
    {
        private NPGTextureIndex _m_icon;//图片索引
        private List<string> _m_sText;//文本
        private Action<NPGGUIWndIconTextTip> _m_aOnPop;//弹出回调
        
        public NPIconTextTipDealer(NPGTextureIndex _icon, string _text, NPCenterTipsRefObj _tipRef, Action<NPGGUIWndIconTextTip> _onPop, ETipDealerTagType _tag = ETipDealerTagType.NONE)
            : base(_tipRef, _tag)
        {
            _m_icon = _icon;
            _m_sText = new List<string>(){_text};;
            _m_aOnPop = _onPop;
        }
        
        public NPIconTextTipDealer(NPGTextureIndex _icon, List<string> _text, NPCenterTipsRefObj _tipRef, Action<NPGGUIWndIconTextTip> _onPop, ETipDealerTagType _tag = ETipDealerTagType.NONE)
            : base(_tipRef, _tag)
        {
            _m_icon = _icon;
            _m_sText = _text;
            _m_aOnPop = _onPop;
        }

        public event Action onClick;
        
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
        protected override void _setTipData(NPGGUIWndIconTextTip _tipWnd)
        {
            if (_tipWnd == null)
                return;

            _tipWnd.setTipData(_m_icon, _m_sText, _onClick);
        }
        
        private void _onClick()
        {
            onClick?.Invoke();
        }
    }
}
