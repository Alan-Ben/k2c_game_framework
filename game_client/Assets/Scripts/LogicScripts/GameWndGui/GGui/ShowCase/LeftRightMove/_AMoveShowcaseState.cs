using JetBrains.Annotations;
using ALPackage;
using UnityEngine;

namespace GOE
{
    
    public partial class _NGGUIWndCommonShowCase_LeftRightMove<T_ITEM>
    {
        /// <summary>
        /// 状态机枚举
        /// </summary>
        private enum EMoveShowCaseStateType
        {
            NONE,//空状态
            IDLE,//待机状态
            MOVE,//移动状态
            SPRING,//回弹状态
            CHECK,//校验状态
            RESET,//重置状态
        }  
        
        /// <summary>
        /// 状态机基类
        /// </summary>
        private abstract class _AMoveShowcaseState : _ATALStateBase<EMoveShowCaseStateType>
        {
            //数据类
            protected LeftRightMoveData _m_data;
            //状态机
            [NotNull]protected MoveShowcaseStateMachine _m_stateMachine;
            //showcase
            protected ShowcaseInfo _m_showcaseInfo;
            
            protected _NGGUIWndCommonShowCase_LeftRightMove<T_ITEM> _m_wnd;
            
            protected _AMoveShowcaseState(_NGGUIWndCommonShowCase_LeftRightMove<T_ITEM> _wnd)
            {
                if(null == _wnd)
                    return;

                _m_wnd = _wnd;
                _m_data = _wnd._m_data;
                _m_stateMachine = _wnd._m_stateMachine;
                _m_showcaseInfo = _wnd._m_showcaseInfo;
            }
            
            /// <summary>
            /// 切换状态
            /// </summary>
            protected void _changeState(EMoveShowCaseStateType _showType)
            {
                if (_m_stateMachine == null)
                    return;

                switch (_showType)
                { 
                    case EMoveShowCaseStateType.NONE:
                        _m_stateMachine.changeState<MoveShowcaseState_NONE>();
                        break;
                    case EMoveShowCaseStateType.IDLE:
                        _m_stateMachine.changeState<MoveShowcaseState_IDLE>();
                        break;
                    case EMoveShowCaseStateType.MOVE:
                        _m_stateMachine.changeState<MoveShowcaseState_MOVE>();
                        break;
                    case EMoveShowCaseStateType.CHECK:
                        _m_stateMachine.changeState<MoveShowcaseState_CHECK>();
                        break;
                    case EMoveShowCaseStateType.SPRING:
                        _m_stateMachine.changeState<MoveShowcaseState_SPRING>();
                        break;
                    case EMoveShowCaseStateType.RESET:
                        _m_stateMachine.changeState<MoveShowcaseState_RESET>();
                        break;
                }
            }

            //当前是否选中第一个
            protected bool _curIsFirst()
            {
                if (null == _m_wnd)
                    return false;
                
                return _m_wnd._m_curSelectIndex == 0;
            }
            
            //当前是否选中最后一个
            protected bool _curIsLast()
            {
                if (null == _m_wnd)
                    return false;
                
                return _m_wnd._m_curSelectIndex == _m_wnd._m_itemList.Count - 1;
            }
            
            /// <summary>
            /// 刷新选中item
            /// </summary>
            /// <param name="_additionIndex">index偏移量</param>
            protected void _refreshSelectIndex(int _additionIndex)
            {
                if (null == _m_wnd)
                    return;
                
                int newIndex = _m_wnd._m_curSelectIndex + _additionIndex;
                _m_wnd.refreshSelectIndex(newIndex);
            }
        } 
    }
}