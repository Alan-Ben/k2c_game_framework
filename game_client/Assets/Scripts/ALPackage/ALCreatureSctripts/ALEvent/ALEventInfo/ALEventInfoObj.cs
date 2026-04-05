using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    /**********************
     * 在一个可包含事件的基本动作中，设置本对象的事件触发信息
     **/
    [System.Serializable]
    public class ALEventInfoObj
    {
        /** 事件触发时间点 */
        public float activeTime;
        /** 触发的事件效果 */
        public _AALSOBaseEvent eventObj;
    }
}
#endif
