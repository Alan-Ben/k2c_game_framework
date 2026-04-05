using UnityEngine;
using ALPackage;

namespace GOE
{
    public partial class _NGGUIWndCommonShowCase_LeftRightMove<T_ITEM>
    {
        /// <summary>
        /// 移动状态
        /// </summary>
        private class MoveShowcaseState_MOVE : _AMoveShowcaseState
        {
            //总的移动水平距离
            private float _m_totalDeltaMoveX;
            
            public MoveShowcaseState_MOVE(_NGGUIWndCommonShowCase_LeftRightMove<T_ITEM> _wnd) : base(_wnd)
            {
            }

            public float totalDeltaMoveX { get { return _m_totalDeltaMoveX; } }
            public override EMoveShowCaseStateType state { get { return EMoveShowCaseStateType.MOVE; } }
            
            //当拖拽时候
            public void onDrag(float _deltaMoveX)
            {
                if(null == _m_data)
                    return;
                
                if(null == _m_showcaseInfo || null == _m_showcaseInfo.showcaseCameraController)
                    return;
                
                _m_totalDeltaMoveX += _deltaMoveX;
                
                //对应td的位移
                Vector3 delta = _m_showcaseInfo.showcaseCameraController.getOnlyGroundPos(new Vector2(_deltaMoveX, 0)) -
                                _m_showcaseInfo.showcaseCameraController.getOnlyGroundPos(Vector2.zero);
                
                //左边没有或者右边没有的情况下，说明是边界，位移减半
                if (null == _m_data.left.infoObj || null == _m_data.right.infoObj)
                {
                    delta = delta / 2f;
                }
                
                //左边对象位置
                _m_data.left.setLocalPosition(_m_data.left.localPosition + delta);
                //中间对象位置
                _m_data.center.setLocalPosition(_m_data.center.localPosition + delta);
                //右边对象位置
                _m_data.right.setLocalPosition(_m_data.right.localPosition + delta);
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

            public override bool canEnterState(_ATALStateBase<EMoveShowCaseStateType> _newState)
            {
                if (null != _newState && _newState.state == EMoveShowCaseStateType.SPRING)
                    return true;
                
                return false;
            }

            public override void resetData()
            {
                
            }
        } 
    }
}