using System;

namespace GOE
{
    public class NPEmptyTipDealer : _ATNPTipDealer<NPGGUIWndEmptyTip, NPGGUIMonoCommonTip>
    {
        private Action<NPGGUIWndEmptyTip> _m_aOnPop;//弹出回调

        public NPEmptyTipDealer(NPCenterTipsRefObj _tipRef, Action<NPGGUIWndEmptyTip> _onPop, ETipDealerTagType _tag = ETipDealerTagType.NONE)
            : base(_tipRef, _tag)
        {
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
        protected override void _setTipData(NPGGUIWndEmptyTip _tipWnd)
        {

        }
    }
}
