using JetBrains.Annotations;
using ALPackage;

namespace GOE
{
    public partial class ChildViewMgr
    {
        public class ChildViewMgrStateIdle : _ASimpleState<ChildViewMgrType>
        {
            [NotNull] private readonly ChildViewMgr _m_viewMgr;
            
            
            public ChildViewMgrStateIdle([NotNull] ChildViewMgr _viewMgr)
            {
                _m_viewMgr = _viewMgr;
            }


            public override ChildViewMgrType state { get { return ChildViewMgrType.Idle; } }


            protected override void _onEnter()
            {
                GGUIWndChildMain.instance.refreshWnd();
            }
            protected override void _onExit()
            {
            }
            public override bool canEnterState(ChildViewMgrType _newState)
            {
                return true;
            }
        }
    }
}