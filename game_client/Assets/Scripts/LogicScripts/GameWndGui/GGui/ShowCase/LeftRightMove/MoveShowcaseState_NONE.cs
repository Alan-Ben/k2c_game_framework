using ALPackage;

namespace GOE
{
    public partial class _NGGUIWndCommonShowCase_LeftRightMove<T_ITEM>
    {
        /// <summary>
        /// 空状态
        /// </summary>
        private class MoveShowcaseState_NONE : _AMoveShowcaseState
        {
            public MoveShowcaseState_NONE(_NGGUIWndCommonShowCase_LeftRightMove<T_ITEM> _wnd) : base(_wnd)
            {
            }
            
            public override EMoveShowCaseStateType state { get { return EMoveShowCaseStateType.NONE; } }
            
            protected override void _onEnter()
            {
                
            }

            protected override void _onExit()
            {
                
            }

            protected override void _onTick(float _deltaTime)
            {
                
            }

            public override bool canEnterState(_ATALStateBase<EMoveShowCaseStateType> _newState)
            {
                return true;
            }

            public override void resetData()
            {
                
            }
        } 
    }
}