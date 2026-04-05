using UnityEngine;
using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /*********************
     * 主城部分的操作监听处理对象
     **/
    public interface _IGameInputDealer
    {
        //每帧处理
        void onUpdate();

        //按下按钮的时候的处理
        void onPress(TouchInfo _touchInfo);
        //弹起按钮的时候的处理
        void onUnPress(TouchInfo _touchInfo);

        //开始拖拽时的操作
        void OnDragStart(GameObject _go, TouchInfo _touchInfo, float _pressToDragTime);

        //拖拽过程的处理，如本对象返回true，表示已经处理了，那么外部的拖屏操作将无法处理
        void OnDrag(GameObject _go, Vector2 _delta, TouchInfo _touchInfo);

        void OnDragEnd(GameObject _go, TouchInfo _touchInfo);

        //点击操作的处理，如本对象返回true，表示已经处理了，那么外部的点击操作将无法处理
        void OnClick(GameObject _go, TouchInfo _touchInfo);
        //缩放变更的处理
        void OnScaleChg(float _changeValue);

        //在一个Go上长按时的处理
        void OnHold(GameObject _go, TouchInfo _touchInfo, float _holdingTime);

        //进入时的操作处理
        void OnEnter();
        //退出操作时的处理
        void OnExit();
    }
}
