using System;
using UnityEngine;

namespace GOE
{
    public interface _INPGameStick
    {
        /// <summary>
        /// 当摇杆值发生了变化
        /// </summary>
        event Action<Vector2, float> onValueChg;
        /// <summary>
        /// 当摇杆按下了
        /// </summary>
        event Action<bool> onPress;
        /// <summary>
        /// 当前的摇杆方向【已归一化】
        /// </summary>
        Vector2 currentStickDirection { get; }
        /// <summary>
        /// 当前的摇杆值【0到1】
        /// </summary>
        float currentStickValue { get; }
    }
}