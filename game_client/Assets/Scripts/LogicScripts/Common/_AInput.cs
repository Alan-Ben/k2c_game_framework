using UnityEngine;
using System.Collections;

namespace GOE
{
    public abstract class _AInput
    {
        public abstract void Update();

        //按下事件处理
        public abstract void OnPress(GameObject _go, bool _press, TouchInfo _touchInfo);

        public abstract void OnClick(GameObject _go, TouchInfo _touchInfo);


        public abstract void OnDragStart(GameObject _go, TouchInfo _touchInfo, float _pressToDragTime);

        public abstract void OnDrag(GameObject _go, Vector2 _delta, TouchInfo _touchInfo);

        public abstract void OnDragEnd(GameObject _go, TouchInfo _touchInfo);

        public abstract void OnScaleChg(float _changeValue);

        public abstract void OnHold(GameObject _go, TouchInfo _touchInfo, float _holdingTime);

        //进入时的操作处理
        public abstract void OnEnter();
        //退出操作时的处理
        public abstract void OnExit();
    }
}
