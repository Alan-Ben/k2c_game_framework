using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
/*********************
 * 角色行为动作信息
 **/

namespace ALPackage
{
    [System.Serializable]
    public class ALSOCreatureActionInfo : ScriptableObject
    {
        /** 识别字符串 */
        public string actionName;

        /** 默认的本行为播放事件比例 */
        public float timeScale;

        /** 初始化时的操作队列 */
        public List<ALSOBaseAnimationInfo> initAnimationList;
        /** 添加到本对象中的物件队列 */
        public List<_AALSOBasicAdditionObjInfo> initAdditionObjList;

        /** 事件队列 */
        public List<ALActionEventInfoObj> actionEventList;

        public ALSOCreatureActionInfo()
        {
            actionName = "";

            timeScale = 1f;

            initAnimationList = new List<ALSOBaseAnimationInfo>();
            initAdditionObjList = new List<_AALSOBasicAdditionObjInfo>();

            actionEventList = new List<ALActionEventInfoObj>();
        }
    }
}
#endif
