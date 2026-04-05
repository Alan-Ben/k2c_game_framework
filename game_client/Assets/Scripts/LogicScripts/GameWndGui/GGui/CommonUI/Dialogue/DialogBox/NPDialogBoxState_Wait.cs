using JetBrains.Annotations;
using ALPackage;

namespace GOE
{
    public partial class NPGGUIWndDialogBox
    {
        /// <summary>
        /// 等待状态
        /// 进/出暂无实际功能，仅作为一句对话结束后，到下一句对话开始前的状态区分（例如点击屏蔽）
        /// </summary>
        private class NPDialogBoxState_Wait : _ANPDialogBoxState
        {
            public NPDialogBoxState_Wait([NotNull] NPGGUIWndDialogBox _wnd) : base(_wnd)
            {

            }

            public override ENPDialogBoxState state { get { return ENPDialogBoxState.WAIT; } }

            public override bool canEnterState(_ATALStateBase<ENPDialogBoxState> _newState)
            {
                return _newState != null && _newState.state == ENPDialogBoxState.PLAYING;
            }

            public override void resetData()
            {

            }

            protected override void _onEnter()
            {

            }

            protected override void _onExit()
            {

            }

            protected override void _onTick(float _deltaTime)
            {

            }
        }
    }
}
