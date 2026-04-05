
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 游戏摄像头的基础控制对象
    /// </summary>
    public abstract class _AGameInputDealer : _IGameInputDealer
    {
        //按下按钮的时候的处理
        public virtual void onPress(TouchInfo _touchInfo)
        {
            //此时设置按下对象
            _AMonoOutCombatClick clickObj = null;
            if(null != _touchInfo && null != _touchInfo.pressGo)
                clickObj = _touchInfo.pressGo.GetComponentInParent<_AMonoOutCombatClick>();

            //判断点击对象是否有效
            if(null != clickObj)
            {
                //调用按下按钮效果
                clickObj.onPress();
            }
            else
            {
                _onPressNull(_touchInfo);
            }
        }

        //弹起按钮的时候的处理
        public virtual void onUnPress(TouchInfo _touchInfo)
        {
            //此时设置按下对象
            _AMonoOutCombatClick clickObj = null;
            if(null != _touchInfo && null != _touchInfo.pressGo)
                clickObj = _touchInfo.pressGo.GetComponentInParent<_AMonoOutCombatClick>();

            //判断点击对象是否有效
            if(null != clickObj)
            {
                //调用弹起按钮效果
                clickObj.onUnPress();
            }
            else
            {
                _onUnPressNull(_touchInfo);
            }
        }

        //开始拖拽时的操作
        public virtual void OnDragStart(GameObject _go, TouchInfo _touchInfo, float _pressToDragTime)
        {
        }

        public virtual void OnDrag(GameObject _go, Vector2 _delta, TouchInfo _touchInfo)
        {
        }

        public virtual void OnDragEnd(GameObject _go, TouchInfo _touchInfo)
        {
        }

        public void OnClick(GameObject _go, TouchInfo _touchInfo)
        {
            //此时设置按下对象
            _AMonoOutCombatClick clickObj = null;
            if(null != _go)
                clickObj = _go.GetComponentInParent<_AMonoOutCombatClick>();

            //判断点击对象是否有效
            if(null != clickObj)
            {
                //调用点击效果
                clickObj.onClick();
            }
            else
            {
                onClickNull(_touchInfo);
            }
        }

        //在一个Go上长按时的处理
        public void OnHold(GameObject _go, TouchInfo _touchInfo, float _holdingTime)
        {
            //此时设置按下对象
            _AMonoOutCombatClick clickObj = null;
            if (null != _go)
                clickObj = _go.GetComponentInParent<_AMonoOutCombatClick>();

            //判断点击对象是否有效
            if (null != clickObj)
            {
                //调用点击效果
                clickObj.onHolding();
            }
            else
            {
                onHoldNull(_touchInfo);
            }
        }

        //缩放变更的处理
        public virtual void OnScaleChg(float _changeValue)
        {
        }

        //退出操作时的处理
        public void OnExit()
        {
            _onExit();
        }

        //进入时的操作处理
        public void OnEnter()
        {
            _onEnter();
        }
        
        //进入时的处理的内部方法，子类实现
        protected abstract void _onEnter();
        
        //退出时的处理的内部方法，子类实现
        protected abstract void _onExit();

        //每帧处理
        public abstract void onUpdate();

        /// <summary>
        /// 在没有按到任何_ANPMonoOutCombatClick脚本对象的时候的处理函数
        /// </summary>
        protected virtual void _onPressNull(TouchInfo _touchInfo)
        {
            
        }

        /// <summary>
        /// 在没有抬起到任何_ANPMonoOutCombatClick脚本对象的时候的处理函数
        /// </summary>
        protected virtual void _onUnPressNull(TouchInfo _touchInfo)
        {
            
        }
        
        /// <summary>
        /// 在没有点击到任何_ANPMonoOutCombatClick脚本对象的时候的处理函数
        /// </summary>
        public abstract void onClickNull(TouchInfo _touchInfo);
        /// <summary>
        /// 在没有Hold到任何_ANPMonoOutCombatClick脚本对象的时候的处理函数
        /// </summary>
        public abstract void onHoldNull(TouchInfo _touchInfo);
        
    }
}
