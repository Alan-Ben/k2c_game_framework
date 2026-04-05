using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 常规的输入处理
    /// </summary>
    public class CommonInputDefaultDealer : _ABasicGameCameraInputDealer
    {
        public CommonInputDefaultDealer(SceneInfoRefObj _sceneInfo) 
            : base(_sceneInfo)
        {
        }
        public CommonInputDefaultDealer(_ALogicPlane2DPosGetter _posGetter, Rect _dragRect, Vector2 _softBorderSize, WCGFloatRange _cameraScaleRange, float _airFriction = 0.3f, float _slideFriction = 10f, bool _useDragTipWnd = false, Vector2 _dragTipShowDistance = default) 
            : base(_posGetter, _dragRect, _softBorderSize, _cameraScaleRange, _airFriction, _slideFriction, _useDragTipWnd, _dragTipShowDistance)
        {
        }

        public override void onUpdate()
        {
        }

        /// <summary>
        /// 在没有点击到任何_ANPMonoOutCombatClick脚本对象的时候的处理函数
        /// </summary>
        public override void onClickNull(TouchInfo _touchInfo)
        {
            CommonOpMgr.instance.setSelectedGo(null);
        }

        /// <summary>
        /// 在没有Hold到任何_ANPMonoOutCombatClick脚本对象的时候的处理函数
        /// </summary>
        public override void onHoldNull(TouchInfo _touchInfo)
        {
        }

        protected override void _onDragStartEx(GameObject _go, TouchInfo _touchInfo, float _pressToDragTime)
        {
            WinMsg.SendMsg(WinMsgType.COMMON_INPUT_SCENE_START_DRAG, _go);
        }
    }
}
