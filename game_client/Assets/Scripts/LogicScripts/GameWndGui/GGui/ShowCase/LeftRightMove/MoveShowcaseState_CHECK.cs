using UnityEngine;
using ALPackage;

namespace GOE
{
    public partial class _NGGUIWndCommonShowCase_LeftRightMove<T_ITEM>
    {
        /// <summary>
        /// 矫正状态
        /// </summary>
        private class MoveShowcaseState_CHECK : _AMoveShowcaseState
        {
            //选中值偏移量
            private int _m_indexOffset;
            
            public MoveShowcaseState_CHECK(_NGGUIWndCommonShowCase_LeftRightMove<T_ITEM> _wnd) : base(_wnd)
            {
            }
            
            public override EMoveShowCaseStateType state { get { return EMoveShowCaseStateType.CHECK; } }
            

            public void setIndexOffset(int _indexOffset)
            {
                _m_indexOffset = _indexOffset;
            }
            
            protected override void _onEnter()
            {
                _refreshSelectIndex(_m_indexOffset);
            }

            protected override void _onExit()
            {
                
            }

            protected override void _onTick(float _deltaTime)
            {
                
            }

            public override bool canEnterState(_ATALStateBase<EMoveShowCaseStateType> _newState)
            {
                if (null != _newState && _newState.state == EMoveShowCaseStateType.RESET)
                    return true;
                
                return false;
            }

            public override void resetData()
            {
                
            }
        } 
    }
}