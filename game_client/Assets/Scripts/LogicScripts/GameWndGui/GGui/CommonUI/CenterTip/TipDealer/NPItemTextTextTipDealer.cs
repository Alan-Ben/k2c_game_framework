using System;

namespace GOE
{
    /// <summary>
    /// item+文本+文本类型tip
    /// </summary>
    public class NPItemTextTextTipDealer : _ATNPTipDealer<NPGGUIWndItemTextTextTip, NPGGUIMonoItemTextTextTip>
    {
        private _IItem _m_item;//图片索引
        private string _m_sTextOne;//文本1
        private string _m_sTextTow;//文本2
        private Action<NPGGUIWndItemTextTextTip> _m_aOnPop;//弹出回调

        public NPItemTextTextTipDealer(_IItem _item, string _textOne, string _textTow, NPCenterTipsRefObj _tipRef, Action<NPGGUIWndItemTextTextTip> _onPop, ETipDealerTagType _tag = ETipDealerTagType.NONE)
            : base(_tipRef, _tag)
        {
            _m_item = _item;
            _m_sTextOne = _textOne;
            _m_sTextTow = _textTow;
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
        protected override void _setTipData(NPGGUIWndItemTextTextTip _tipWnd)
        {
            if (_tipWnd == null)
                return;

            _tipWnd.setTipData(_m_item, _m_sTextOne,_m_sTextTow);
        }
    }
}
