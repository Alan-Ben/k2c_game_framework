using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public partial class _NGGUIWndCommonShowCase_LeftRightMove<T_ITEM>
    {
        /// <summary>
        /// 状态机
        /// </summary>
        private class MoveShowcaseStateMachine : _TALStateMachine<_AMoveShowcaseState, EMoveShowCaseStateType>
        {
            public MoveShowcaseStateMachine(MoveShowcaseStateFactory _factory) : base(_factory)
            {
                
            }
        } 
    }
}