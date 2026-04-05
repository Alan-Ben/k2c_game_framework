using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE.MiniGame
{
    public class GTDDragBoxGameBox : _IDragBoxGameBoxShow
    {
        private static readonly int animatorParameter_drag_up = Animator.StringToHash("drag_up"); // 向上拖动
        private static readonly int animatorParameter_drag_down = Animator.StringToHash("drag_down"); // 向下拖动
        private static readonly int animatorParameter_drag_left = Animator.StringToHash("drag_left"); // 向左拖动
        private static readonly int animatorParameter_drag_right = Animator.StringToHash("drag_right"); // 向右拖动
        private static readonly int animatorParameter_is_matched = Animator.StringToHash("is_matched"); // 是否匹配成功
        
        [NotNull] private GTDMonoDragBoxGameBox _m_mono;
        private DragBoxGameController _m_gameController;
        private Action<EDragBoxGameBoxDragDirection> _m_aOnBoxDrag;

        private bool _m_bIsDraging;//是否正在拖拽中
        private Vector2 _m_vStartDragPos;//开始拖拽的位置
        
        public GTDDragBoxGameBox([NotNull]GTDMonoDragBoxGameBox _mono)
        {
            _m_mono = _mono;
        }
        
        public void onInit()
        {
            if (_m_mono != null)
            {
                if (_m_mono.clickMono != null)
                {
                    _m_mono.clickMono.onDragStart += _onDragStart;
                    _m_mono.clickMono.onDragEnd += _onDragEnd;
                }
                
                if(_m_mono.boxAnimator != null)
                    _m_mono.boxAnimator.SetBool(animatorParameter_is_matched, false);//初始化设置为匹配未成功
            }

            _m_bIsDraging = false;
        }

        public void onDiscard()
        {
            if (_m_mono != null && _m_mono.clickMono != null)
            {
                _m_mono.clickMono.onDragStart -= _onDragStart;
                _m_mono.clickMono.onDragEnd -= _onDragEnd;
            }
            
            if(_m_mono.boxAnimator != null)
                _m_mono.boxAnimator.SetBool(animatorParameter_is_matched, false);//销毁时设置为匹配未成功
        }

        #region _IDragBoxGameBoxShow接口实现

        public void setGameController(DragBoxGameController _gameController, Action<EDragBoxGameBoxDragDirection> _onBoxDrag)
        {
            _m_gameController = _gameController;
            _m_aOnBoxDrag = _onBoxDrag;
        }

        public void show()
        {
            ALUGUICommon.setGameObjEnable(_m_mono, true);
        }

        public void hide()
        {
            ALUGUICommon.setGameObjEnable(_m_mono, false);
        }

        public List<EDragBoxGameBoxDragDirection> canDragDirectionList { get { return _m_mono == null ? null : _m_mono.canDragDirection; } }

        public void dragBox(EDragBoxGameBoxDragDirection _direction, bool _isMatched)
        {
            if(_m_mono == null)
                return;

            if (_m_mono.boxAnimator != null)
            {
                switch (_direction)
                {
                    case EDragBoxGameBoxDragDirection.UP:
                        _m_mono.boxAnimator.SetTrigger(animatorParameter_drag_up);
                        break;
                    
                    case EDragBoxGameBoxDragDirection.DOWN:
                        _m_mono.boxAnimator.SetTrigger(animatorParameter_drag_down);
                        break;
                    
                    case EDragBoxGameBoxDragDirection.LEFT:
                        _m_mono.boxAnimator.SetTrigger(animatorParameter_drag_left);
                        break;

                    case EDragBoxGameBoxDragDirection.RIGHT:
                        _m_mono.boxAnimator.SetTrigger(animatorParameter_drag_right);
                        break;
                }
                
                _m_mono.boxAnimator.SetBool(animatorParameter_is_matched, _isMatched);
            }
        }

        #endregion
        
        #region 拖拽事件

        /// <summary>
        /// 开始拖动
        /// </summary>
        /// <param name="_go"></param>
        /// <param name="_touchInfo"></param>
        /// <param name="_pressToDragTime"></param>
        private void _onDragStart(GameObject _go, TouchInfo _touchInfo, float _pressToDragTime)
        {
            if (_touchInfo == null)
            {
                Debug.LogError("[GTDDragBoxGameBox] TouchInfo is null!");
                return;
            }
            
            if(_m_bIsDraging)
                return;

            _m_bIsDraging = true;

            _m_vStartDragPos = _touchInfo.curPos;
        }

        /// <summary>
        /// 停止拖动
        /// </summary>
        /// <param name="_go"></param>
        /// <param name="_touchInfo"></param>
        private void _onDragEnd(GameObject _go, TouchInfo _touchInfo)
        {
            if (_touchInfo == null)
            {
                Debug.LogError("[GTDDragBoxGameBox] TouchInfo is null!");
                return;
            }
            
            if(!_m_bIsDraging)
                return;

            Vector2 dragEndPos = _touchInfo.curPos;
            Vector2 dragDirection = dragEndPos - _m_vStartDragPos;
            
            EDragBoxGameBoxDragDirection direction = default;
            if (Math.Abs(dragDirection.x / dragDirection.y) >= 1)
            {
                if (dragDirection.x >= 0)
                    direction = EDragBoxGameBoxDragDirection.RIGHT;
                else
                    direction = EDragBoxGameBoxDragDirection.LEFT;
            }
            else
            {
                if (dragDirection.y >= 0)
                    direction = EDragBoxGameBoxDragDirection.UP;
                else
                    direction = EDragBoxGameBoxDragDirection.DOWN;
            }
            
            _m_aOnBoxDrag?.Invoke(direction);
            
            _m_bIsDraging = false;
        }

        #endregion
        
        
    }
}