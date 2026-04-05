using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public partial class _NGGUIWndCommonShowCase_LeftRightMove<T_ITEM>
    {
        /// <summary>
        /// 状态机工厂
        /// </summary>
        private class MoveShowcaseStateFactory :_ATALStateFactory<_AMoveShowcaseState, EMoveShowCaseStateType>
        {
            public MoveShowcaseStateFactory(_NGGUIWndCommonShowCase_LeftRightMove<T_ITEM> _wnd) : base()
            {
                regCacheController(typeof(MoveShowcaseState_NONE), new StateCache(() => { return new MoveShowcaseState_NONE(_wnd); }));
                regCacheController(typeof(MoveShowcaseState_IDLE), new StateCache(() => { return new MoveShowcaseState_IDLE(_wnd); }));
                regCacheController(typeof(MoveShowcaseState_MOVE), new StateCache(() => { return new MoveShowcaseState_MOVE(_wnd); }));
                regCacheController(typeof(MoveShowcaseState_RESET), new StateCache(() => { return new MoveShowcaseState_RESET(_wnd); }));
                regCacheController(typeof(MoveShowcaseState_CHECK), new StateCache(() => { return new MoveShowcaseState_CHECK(_wnd); }));
                regCacheController(typeof(MoveShowcaseState_SPRING), new StateCache(() => { return new MoveShowcaseState_SPRING(_wnd); }));
            }

            private class StateCache : _TALBasicStateCacheController<_AMoveShowcaseState, EMoveShowCaseStateType>
            {
                public StateCache(Func<_AMoveShowcaseState> _createFunc) : base(_createFunc, 1, 2)
                {

                }
            }
        } 
    }
}