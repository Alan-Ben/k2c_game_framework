
using ALPackage;
using UnityEngine;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 游戏摇杆通用的部分
    /// </summary>
    public class NPGGUIMonoGameStick : _AALBasicUIWndMono
    {
        [ALHeader("摇杆的可交互区域")]
        public GameObject interactiveArea;
        [ALHeader("摇杆的可移动半径")]
        [Min(0f)]
        public float radius;
        [ALHeader("摇杆内外死区")]
        [Range(0, 1)]
        public float innerDeadZone = 0.1f;
        [Range(0, 1)]
        public float outerDeadZone = 0.7f; 
        [ALHeader("提示当前值的显示对象")]
        public NPSmoothLocalTransformMono valueHandle;
        [ALHeader("提示当前方向值的显示对象")]
        public NPSmoothLocalTransformMono directionHandle;
        [ALHeader("摇杆处于不同值时的效果列表")]
        public List<_ANPGGUIMonoCommonFadeObject> valueEffectList;
    }
}