
using System;
using ALPackage;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GOE
{
    /// <summary>
    /// 游戏摇杆的通用逻辑
    /// </summary>
    public abstract class _ANPGGUISubWndGameStick<T> : _ANPGGUIBasicSubWnd<T>, _INPGameStick
        where T : NPGGUIMonoGameStick
    {
        // 当前摇杆的方向
        private Vector2 _m_currentStickDirection;
        // 当前摇杆的值
        private float _m_currentStickValue;

        // 当前是否按下了
        private bool _m_isPressed;
        
#if UNITY_EDITOR || UNITY_STANDALONE
        private bool _m_useKeyBoard;
        private Vector2 _m_lastInputValue;
        private ALCommonEnableTaskController _m_update;
#endif
        
        public _ANPGGUISubWndGameStick(T _wnd) : base(_wnd)
        {
            initWnd();
        }

        /// <summary>
        /// 当摇杆值发生了变化
        /// </summary>
        public event Action<Vector2, float> onValueChg;
        /// <summary>
        /// 当摇杆按下了
        /// </summary>
        public event Action<bool> onPress;
        /// <summary>
        /// 当前的摇杆方向【已归一化】
        /// </summary>
        public Vector2 currentStickDirection { get { return _m_currentStickDirection; } }
        /// <summary>
        /// 当前的摇杆值【0到1】
        /// </summary>
        public float currentStickValue { get { return _m_currentStickValue; } }
        /// <summary>
        /// 是否已经按下了
        /// </summary>
        public bool isPressed { get { return _m_isPressed; } }
        
        protected override void _onShowWnd()
        {
            // 设置默认值为 0
            _setCurTouchPos(Vector2.zero);
            
#if UNITY_EDITOR || UNITY_STANDALONE
            
            _m_update.setDisable();
            _m_update = ALCommonEnableTickActionMonoTask.addMonoTask(_tick);
#endif
        }

        protected override void _onHideWnd()
        {
            // 如果当前还在按下状态，就直接解除
            if (_m_isPressed)
                _onPress(false, null);
                
#if UNITY_EDITOR || UNITY_STANDALONE
            _m_update.setDisable();
#endif
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.interactiveArea != null && wnd.interactiveArea.transform is RectTransform rectTransform)
            {
                // 需要绑定按下事件和滑动事件
                ALUGUICommon.combineBtnPress(wnd.interactiveArea, _onPress);
                ALUGUICommon.combineDrag(wnd.interactiveArea, _onDrag);
            }
            
            // 设置默认值为 0
            _setCurTouchPos(Vector2.zero);
        }

        // 当手指按下了
        protected virtual void _onPress(bool _isDown, PointerEventData _eventData)
        {
            // 如果状态一样，不响应
            if (_m_isPressed == _isDown)
                return;
            
            // 记录当前是否按下了
            _m_isPressed = _isDown;
            
            if (_isDown)
            {
                if (_eventData != null)
                {
                    Vector2 stickPos = screenPosToStickPos(_eventData.position);
                    _setCurTouchPos(stickPos);
                }
            }
            else
            {
                _setCurTouchPos(Vector2.zero);
            }
            
            // 触发事件
            onPress?.Invoke(_isDown);
        }
        
        // 当手指拖动时
        protected virtual void _onDrag(PointerEventData _eventData)
        {
            // 如果当前没有按下，则不响应
            if (!_m_isPressed || _eventData == null)
                return;
            
            Vector2 stickPos = screenPosToStickPos(_eventData.position);
            _setCurTouchPos(stickPos);
        }

        // 设置现在点击的位置
        protected void _setCurTouchPos(Vector2 _touchPos)
        {
            // 如果半径不合法就不处理
            if (wnd == null || wnd.radius <= 0)
                return;
            
            // 计算当前的偏差值
            Vector2 offset = Vector2.ClampMagnitude(_touchPos, wnd.radius);
            float offsetRate = offset.magnitude / wnd.radius;
            // 限制在 0 到 1 之间
            offsetRate = Mathf.Clamp01(offsetRate);

            // 处理值变化的相关淡入淡出效果
            if (wnd.valueEffectList != null)
            {
                foreach (_ANPGGUIMonoCommonFadeObject fadeObj in wnd.valueEffectList)
                {
                    if (fadeObj == null) continue;
                    // 设置值
                    fadeObj.setValue(offsetRate);
                }
            }
            
            // 计算当前的值
            float innerDeadZone = wnd == null ? 0.1f : wnd.innerDeadZone;
            float outerDeadZone = wnd == null ? 0.9f : wnd.outerDeadZone;
            if (offsetRate <= innerDeadZone)
            {
                // 如果差值在死区内就当作 0 处理
                _m_currentStickDirection = Vector2.zero;
                _m_currentStickValue = 0;
            }
            else if (offsetRate >= outerDeadZone)
            {
                _m_currentStickDirection = offset.normalized;
                _m_currentStickValue = 1;
            }
            else
            {
                // 如果差值在死区外，重新映射当前值
                offsetRate = offsetRate.Remap(innerDeadZone, outerDeadZone, 0, 1);
                _m_currentStickDirection = offset.normalized;
                _m_currentStickValue = offsetRate;
            }
            // 触发事件
            onValueChg?.Invoke(_m_currentStickDirection, _m_currentStickValue);
            
            WinMsg.SendMsg(WinMsgType.STICK_PRESS_VALUE, _m_isPressed, _m_currentStickValue);

            // 下面设置 UI 提示，如果 wnd 为 null 就不处理
            if (wnd == null)
                return;

            // 处理值指示
            if (wnd.valueHandle != null)
            {
                wnd.valueHandle.setLocalPosition(offset, false);
            }
            
            // 处理方向指示
            if (wnd.directionHandle != null)
            {
                if (_m_currentStickValue > 0)
                {
                    // 如果当前摇杆有值，就显示方向指示
                    // ALUGUICommon.setGameObjEnable(wnd.directionHandle, true);
                    wnd.directionHandle.setLocalRotation(Quaternion.Euler(0, 0, Mathf.Atan2(_m_currentStickDirection.y, _m_currentStickDirection.x) * Mathf.Rad2Deg - 90), false);
                }
                else
                {
                    // 如果摇杆没有值，就隐藏方向指示
                    // ALUGUICommon.setGameObjEnable(wnd.directionHandle, false);
                }
            }
        }

        /// <summary>
        /// 屏幕坐标改为摇杆的偏移值
        /// </summary>
        protected abstract Vector2 screenPosToStickPos(Vector2 _screenPos);
        
#if UNITY_EDITOR || UNITY_STANDALONE
        /// <summary>
        /// 当手柄或键盘输入时
        /// </summary>
        protected virtual void _onGamepadOrKeyboardEnable() {}
        private void _tick()
        {
            Vector2 pcStickValue = Vector2.zero;
            Vector2 conStickValue = Vector2.zero;
            if (Input.GetKey(KeyCode.W))
            {
                pcStickValue += Vector2.up;
                _m_useKeyBoard = true;
            }

            if (Input.GetKey(KeyCode.A))
            {
                pcStickValue += Vector2.left;
                _m_useKeyBoard = true;
            }

            if (Input.GetKey(KeyCode.S))
            {
                pcStickValue += Vector2.down;
                _m_useKeyBoard = true;
            }

            if (Input.GetKey(KeyCode.D))
            {
                pcStickValue += Vector2.right;
                _m_useKeyBoard = true;
            }

            if (Input.GetJoystickNames().Length > 0 && pcStickValue.sqrMagnitude == 0)
            {
                conStickValue.y += Input.GetAxis("Vertical");
                conStickValue.x += Input.GetAxis("Horizontal");
                if (conStickValue.y >= 1 || conStickValue.x >= 1)
                    _m_useKeyBoard = false;
            }

            Vector2 stickValue;
            if (_m_useKeyBoard)
                stickValue = pcStickValue.normalized;
            else
                stickValue = Vector2.ClampMagnitude(conStickValue, 1);
            
            stickValue *= wnd.radius;
            
            if (NPGameUtility.Approximately(_m_lastInputValue, stickValue))
                return;

            if (_m_lastInputValue == Vector2.zero)
            {
                _onGamepadOrKeyboardEnable();
                _onPress(true, null);
            }
            else if (stickValue == Vector2.zero)
                _onPress(false, null);
            
            _m_lastInputValue = stickValue;
            _setCurTouchPos(stickValue);
        }
#endif
    }
}