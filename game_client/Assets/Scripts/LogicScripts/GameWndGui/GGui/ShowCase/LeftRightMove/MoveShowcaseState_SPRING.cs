using UnityEngine;
using ALPackage;

namespace GOE
{
    public partial class _NGGUIWndCommonShowCase_LeftRightMove<T_ITEM>
    {
        /// <summary>
        /// 回弹状态
        /// </summary>
        private class MoveShowcaseState_SPRING : _AMoveShowcaseState
        {
            //这次总的屏幕滑动距离，大于一个值选中下一个
            private float _m_totalDeltaMoveX = 0f;
            //滑动时间
            private float _m_moveTimeS = 0f;
            //当前选中单位偏移
            private int _m_indexOffset = 0;
            
            public MoveShowcaseState_SPRING(_NGGUIWndCommonShowCase_LeftRightMove<T_ITEM> _wnd) : base(_wnd)
            {
            }
            
            public override EMoveShowCaseStateType state { get { return EMoveShowCaseStateType.SPRING; } }

            //开始回正时候的位置
            private Vector3 _m_beginLeftPos;
            private Vector3 _m_beginCenterPos;
            private Vector3 _m_beginRightPos;

            public void setTotalDeltaMoveX(float _totalDeltaMoveX)
            {
                _m_totalDeltaMoveX = _totalDeltaMoveX;
            }
            
            protected override void _onEnter()
            {
                if(null == _m_showcaseInfo || null == _m_data)
                    return;

                _m_indexOffset = 0;

                //滑动到上一个
                if (_m_totalDeltaMoveX >= _m_data._m_nextOffsetDistance && !_curIsFirst())
                {
                    LeftRightMoveIndex item = null;
                    item = _m_data.right;
                    _m_data.right = _m_data.center;
                    _m_data.center = _m_data.left;
                    _m_data.left = item;
                        
                    _m_indexOffset = -1;
                    _m_data.left.setLocalPosition(_m_data.leftDefaultPos);
                }
                //滑动到下一个
                else if (_m_totalDeltaMoveX <= -_m_data._m_nextOffsetDistance && !_curIsLast())
                {
                    LeftRightMoveIndex item = null;
                    item = _m_data.left;
                    _m_data.left = _m_data.center;
                    _m_data.center = _m_data.right;
                    _m_data.right = item; 
                        
                    _m_indexOffset = 1;
                    _m_data.right.setLocalPosition(_m_data.rightDefaultPos);
                }
                
                //记录一下当前位置，后面从这个位置开始回正
                _m_beginLeftPos = _m_data.left.localPosition;
                _m_beginCenterPos = _m_data.center.localPosition;
                _m_beginRightPos = _m_data.right.localPosition;

                _m_moveTimeS = 0f;
            }

            protected override void _onExit()
            {
                _m_moveTimeS = 0f;
                _m_totalDeltaMoveX = 0f;
            }

            protected override void _onTick(float _deltaTime)
            {
                if(null == _m_data)
                   return;

                _m_moveTimeS += _deltaTime;
                if (_m_moveTimeS < _m_data._m_moveTimeS)
                {
                    float t = _m_moveTimeS / _m_data._m_moveTimeS;
                    _m_data.left.setLocalPosition(Vector3.Lerp(_m_beginLeftPos, _m_data.leftDefaultPos, t));
                    _m_data.center.setLocalPosition(Vector3.Lerp(_m_beginCenterPos, _m_data.centerDefaultPos, t));
                    _m_data.right.setLocalPosition(Vector3.Lerp(_m_beginRightPos, _m_data.rightDefaultPos, t));
                }
                else
                {
                    //设置最终位置
                    _m_data.left.setLocalPosition(_m_data.leftDefaultPos);
                    _m_data.center.setLocalPosition(_m_data.centerDefaultPos);
                    _m_data.right.setLocalPosition(_m_data.rightDefaultPos);
                    
                    //切换到矫正状态
                    _m_stateMachine.changeState<MoveShowcaseState_CHECK>((_state) =>
                    {
                        if (_state != null) 
                            _state.setIndexOffset(_m_indexOffset);
                    });
                }
            }

            public override bool canEnterState(_ATALStateBase<EMoveShowCaseStateType> _newState)
            {
                if (null != _newState && _newState.state == EMoveShowCaseStateType.CHECK)
                    return true;
                
                return false;
            }

            public override void resetData()
            {
                _m_moveTimeS = 0f;
                _m_totalDeltaMoveX = 0f;
                _m_indexOffset = 0;
                
                _m_beginLeftPos = Vector3.zero;
                _m_beginCenterPos = Vector3.zero;
                _m_beginRightPos = Vector3.zero;
            }
        } 
    }
}