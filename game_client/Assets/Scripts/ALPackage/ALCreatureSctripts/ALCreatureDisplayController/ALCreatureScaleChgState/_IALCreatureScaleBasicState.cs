using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

#if AL_CREATURE_SYS
/******************
 * 个体对象的比例缩放状态基本对象
 **/
public interface _IALCreatureScaleBasicState
{
    /** 是否需要设置尺寸 */
    bool needChgScale();
    /** 获取当前比例信息 */
    Vector3 getCurScale();
    /** 获取目标比例 */
    Vector3 getTargetScale();
    /** 获取当前比例速度 */
    Vector3 getCurScaleSpeed();
    /** 判断状态是否有效，状态无效将切换到无效状态，避免无谓运算 */
    bool enable();
}
#endif
