
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 一个游戏摇杆
    /// </summary>
    public class NPGGUISubWndCommonStick : _ANPGGUISubWndGameStick<NPGGUIMonoCommonStick>
    {
        public NPGGUISubWndCommonStick(NPGGUIMonoCommonStick _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override Vector2 screenPosToStickPos(Vector2 _screenPos)
        {
            if (wnd == null || wnd.stickCenter == null)
                return _screenPos;
            
            Vector3 worldPos = MainCameraMono.selfInstance.uiCamera.ScreenToWorldPoint(_screenPos);
            return wnd.stickCenter.InverseTransformPoint(worldPos);
        }
    }
}