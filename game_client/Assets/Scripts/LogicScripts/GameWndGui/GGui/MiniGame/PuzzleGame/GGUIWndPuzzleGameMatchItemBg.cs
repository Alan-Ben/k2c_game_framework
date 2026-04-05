using GOE.MiniGame;
using UnityEngine;

namespace GOE
{
    public class GGUIWndPuzzleGameMatchItemBg : _ANPGGUIBasicSubWnd<GGUIMonoPuzzleGameMatchItemBg>
    {
        private EPuzzleGameMatchItemBgState _m_eItemState;

        public long matchItemId { get { return wnd == null ? 0 : wnd.matchItemId; } }
        public RectTransform matchRectTransform { get { return wnd == null ? null : wnd.matchRectTransform; } }
        
        /// <summary>
        /// item的世界坐标位置
        /// </summary>
        public Vector3 position
        {
            get { return rectTransform == null ? Vector3.zero : rectTransform.position; }
        }
        
        public GGUIWndPuzzleGameMatchItemBg(GGUIMonoPuzzleGameMatchItemBg _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
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
        
        public void setItemState(EPuzzleGameMatchItemBgState _state, bool _forceChg = false)
        {
            if (wnd == null || !isShow)
            {
                Debug.LogError_EditorOnly($"[GGUIWndPuzzleGameMatchItemBg] setGameState 在wnd == null或窗口未显示时, 就尝试设置窗口状态为:{_state}");
                return;
            }

            if (_m_eItemState == _state && !_forceChg)
                return;

            _m_eItemState = _state;
            wnd.setState(_m_eItemState);
        }
    }
}