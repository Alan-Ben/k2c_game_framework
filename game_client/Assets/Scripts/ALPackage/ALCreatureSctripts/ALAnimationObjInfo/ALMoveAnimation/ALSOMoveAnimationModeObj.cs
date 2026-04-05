using System;
using System.Collections.Generic;
using UnityEngine;

#if AL_CREATURE_SYS
/**********************
 * 移动模式对象，其中定义了各个移动中涉及的状态控制对象
 * 
 * 程序在初始化将在指定的位置定义所有移动模式对象对应的路径
 * 逐个读取初始化进入信息存储表，表中根据模式名称进行索引
 **/

namespace ALPackage
{
    /*************************
     * 跳跃动作的相关信息对象
     **/
    [System.Serializable]
    public class ALJumpAnimationInfo
    {
        /** 起跳的时间点 */
        public float jumpUpTime;
        /** 落地后动作持续时间 */
        public float landedProcessTime;
        /** 起跳前准备的动作列表 */
        public ALSOBaseAnimationInfo readyAnimation;
        /** 起跳后在空中的循环动作对象 */
        public ALSOBaseAnimationInfo jumpingLoopAnimation;
        /** 落地后的一次性动作对象 */
        public ALSOBaseAnimationInfo landAnimation;
        /** 起跳的初始向上速度 */
        public float initJumpYSpeed;

        public ALJumpAnimationInfo()
            : base()
        {
        }
    }

    [System.Serializable]
    public class ALSOMoveAnimationModeObj : ScriptableObject
    {
        /** 用于分别模式的模式名称 */
        public string modeName;

        public ALSOMoveAnimationInfo idleAnimationInfo;
        public ALSOMoveAnimationInfo moveAnimationInfo;
        public ALJumpAnimationInfo jumpAnimationInfo;

        public ALSOMoveAnimationModeObj()
        {
        }

        public ALSOMoveAnimationInfo getAnimationInfo(ALCharacterMoveStateType _state)
        {
            if (ALCharacterMoveStateType.IDLE == _state)
                return idleAnimationInfo;
            else if (ALCharacterMoveStateType.MOVE == _state)
                return moveAnimationInfo;

            return null;
        }
    }
}
#endif
