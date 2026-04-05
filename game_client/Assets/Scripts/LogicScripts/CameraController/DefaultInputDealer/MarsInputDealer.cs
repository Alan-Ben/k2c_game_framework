using UnityEngine;

namespace GOE
{
    public class MarsInputDealer : CommonInputDefaultDealer
    {
        public MarsInputDealer(SceneInfoRefObj _sceneInfo) 
            : base(_sceneInfo)
        {
        }
        public MarsInputDealer(_ALogicPlane2DPosGetter _posGetter, Rect _dragRect, Vector2 _softBorderSize, WCGFloatRange _cameraScaleRange, float _airFriction = 0.3f, float _slideFriction = 10, bool _useDragTipWnd = false, Vector2 _dragTipShowDistance = default) 
            : base(_posGetter, _dragRect, _softBorderSize, _cameraScaleRange, _airFriction, _slideFriction, _useDragTipWnd, _dragTipShowDistance)
        {
        }


        protected override void _onDragStartEx(GameObject _go, TouchInfo _touchInfo, float _pressToDragTime)
        {
            base._onDragStartEx(_go, _touchInfo, _pressToDragTime);
            GGUIWndMarsHud.instance.clearMutexController();
        }
        protected override void _onDrag(GameObject _go, Vector2 _delta, TouchInfo _touchInfo)
        {
            base._onDrag(_go, _delta, _touchInfo);
            GGUIWndMarsHud.instance.showAllAutoHideController();
        }
        public override void onClickNull(TouchInfo _touchInfo)
        {
            base.onClickNull(_touchInfo);
            GGUIWndMarsHud.instance.clearMutexController();
        }
    }
}