using UnityEngine;
using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /*********************
     * 主城部分的操作监听处理对象
     **/
    public abstract class _ACityAddInputDealer
    {
        //每帧处理
        public virtual bool onUpdate()
        {
            return false;
        }

        //按下按钮的时候的处理
        public virtual bool onPress(TouchInfo _touchInfo)
        {
            return false;
        }
        //弹起按钮的时候的处理
        public virtual bool onUnPress(TouchInfo _touchInfo)
        {
            return false;
        }

        //开始拖拽时的操作
        public virtual bool OnDragStart(GameObject _go, TouchInfo _touchInfo, float _pressToDragTime)
        {
            return false;
        }

        //拖拽过程的处理，如本对象返回true，表示已经处理了，那么外部的拖屏操作将无法处理
        public virtual bool OnDrag(GameObject _go, Vector2 _delta, TouchInfo _touchInfo)
        {
            return false;
        }

        public virtual bool OnDragEnd(GameObject _go, TouchInfo _touchInfo)
        {
            return false;
        }

        //点击操作的处理，如本对象返回true，表示已经处理了，那么外部的点击操作将无法处理
        public virtual bool OnClick(GameObject _go, TouchInfo _touchInfo)
        {
            return false;
        }
        //缩放变更的处理
        public virtual bool OnScaleChg(float _changeValue)
        {
            return false;
        }

        //在一个Go上长按时的处理
        public virtual bool OnHold(GameObject _go, TouchInfo _touchInfo, float _holdingTime)
        {
            return false;
        }

        //进入时的操作处理
        public abstract void OnEnter();
        //退出操作时的处理
        public abstract void OnExit();
    }
}
