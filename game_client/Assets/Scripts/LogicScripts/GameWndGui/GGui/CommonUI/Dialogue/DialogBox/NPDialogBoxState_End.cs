using JetBrains.Annotations;
using ALPackage;

namespace GOE
{
    public partial class NPGGUIWndDialogBox
    {
        private class NPDialogBoxState_End : _ANPDialogBoxState
        {
            public NPDialogBoxState_End([NotNull] NPGGUIWndDialogBox _wnd) : base(_wnd)
            {

            }

            public override ENPDialogBoxState state { get { return ENPDialogBoxState.END; } }

            public override bool canEnterState(_ATALStateBase<ENPDialogBoxState> _newState)
            {
                return _newState != null && _newState.state == ENPDialogBoxState.WAIT;
            }

            public override void resetData()
            {

            }

            protected override void _onEnter()
            {
                wnd._onEnterEndState();
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
