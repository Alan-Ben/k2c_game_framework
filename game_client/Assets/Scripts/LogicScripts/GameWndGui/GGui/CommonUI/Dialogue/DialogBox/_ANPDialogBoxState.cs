using JetBrains.Annotations;
using ALPackage;

namespace GOE
{
    public partial class NPGGUIWndDialogBox
    {
        private abstract class _ANPDialogBoxState : _ATALStateBase<ENPDialogBoxState>
        {
            [NotNull] private NPGGUIWndDialogBox _m_wnd;

            protected _ANPDialogBoxState([NotNull] NPGGUIWndDialogBox _wnd) : base()
            {
                _m_wnd = _wnd;
            }

            [NotNull] protected NPGGUIWndDialogBox wnd { get { return _m_wnd; } }
        }
    }
}
