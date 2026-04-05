using ALPackage;

namespace GOE
{
    public partial class ChildViewMgr
    {
        public class ChildViewMgrStateNone : _ASimpleState<ChildViewMgrType>
        {
            public override ChildViewMgrType state { get { return ChildViewMgrType.None; } }


            protected override void _onEnter()
            {
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