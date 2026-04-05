using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 自由位置的摇杆
    /// </summary>
    public class NPGGUIMonoFreeStick : NPGGUIMonoGameStick
    {
        [ALHeader("真正的摇杆的中心")]
        public NPSmoothLocalTransformMono gameStickRoot;

        [ALHeader("空闲时显示的对象列表")]
        public List<GameObject> idleShowObjs;
        [ALHeader("按下时显示的对象列表")]
        public List<GameObject> pressShowObjs;
        [ALHeader("特效父节点")]
        public Transform sfxParent;
        [ALHeader("摇杆消失时的特效")]
        public long stickOverSfxId;

        [ALHeader("是否使用摇杆位置修正")]
        public bool useStickPosFix = true;
        [ALHeader("允许进行修正的范围")]
        public RectTransform fixArea;
    }
}