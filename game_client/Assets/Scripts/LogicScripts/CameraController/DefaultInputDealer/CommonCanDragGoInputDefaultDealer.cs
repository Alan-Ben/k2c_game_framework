using UnityEngine;

namespace GOE
{
    public class CommonCanDragGoInputDefaultDealer : CommonInputDefaultDealer
    {
        public CommonCanDragGoInputDefaultDealer(SceneInfoRefObj _sceneInfo) : base(_sceneInfo)
        {
        }

        public CommonCanDragGoInputDefaultDealer(_ALogicPlane2DPosGetter _posGetter, Rect _dragRect, Vector2 _softBorderSize, WCGFloatRange _cameraScaleRange, float _airFriction = 0.3f, float _slideFriction = 10, bool _useDragTipWnd = false, Vector2 _dragTipShowDistance = default) : base(_posGetter, _dragRect, _softBorderSize, _cameraScaleRange, _airFriction, _slideFriction, _useDragTipWnd, _dragTipShowDistance)
        {
        }

        public override void OnDragStart(GameObject _go, TouchInfo _touchInfo, float _pressToDragTime)
        {
            //此时设置按下对象
            _AMonoOutCombatClickDrag_CommonTDAni clickObj = null;
            if(null != _go)
                clickObj = _go.GetComponentInParent<_AMonoOutCombatClickDrag_CommonTDAni>();

            //判断点击对象是否有效
            if(null != clickObj)
            {
                //调用点击效果
                clickObj.OnDragStart(_go, _touchInfo, _pressToDragTime);
            }
            else
            {
                base.OnDragStart(_go, _touchInfo, _pressToDragTime);
            }
        }

        public override void OnDrag(GameObject _go, Vector2 _delta, TouchInfo _touchInfo)
        {
            //此时设置按下对象
            _AMonoOutCombatClickDrag_CommonTDAni clickObj = null;
            if(null != _go)
                clickObj = _go.GetComponentInParent<_AMonoOutCombatClickDrag_CommonTDAni>();

            //判断点击对象是否有效
            if(null != clickObj)
            {
                //调用点击效果
                clickObj.OnDrag(_go, _delta, _touchInfo);
            }
            else
            {
                base.OnDrag(_go, _delta, _touchInfo);
            }
        }

        public override void OnDragEnd(GameObject _go, TouchInfo _touchInfo)
        {
            //此时设置按下对象
            _AMonoOutCombatClickDrag_CommonTDAni clickObj = null;
            if(null != _go)
                clickObj = _go.GetComponentInParent<_AMonoOutCombatClickDrag_CommonTDAni>();

            //判断点击对象是否有效
            if(null != clickObj)
            {
                //调用点击效果
                clickObj.OnDragEnd(_go, _touchInfo);
            }
            else
            {
                base.OnDragEnd(_go, _touchInfo);
            }
        }
    }
}