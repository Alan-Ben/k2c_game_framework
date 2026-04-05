using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    /**********************
     * 在一个可包含事件的Action中，设置本对象的事件触发信息
     **/
    [System.Serializable]
    public class ALActionEventInfoObj
    {
        /** 事件触发时间点 */
        public float activeTime;
        /** 触发的事件效果 */
        public _AALSOBaseActionEvent eventObj;
    }
}
#endif
