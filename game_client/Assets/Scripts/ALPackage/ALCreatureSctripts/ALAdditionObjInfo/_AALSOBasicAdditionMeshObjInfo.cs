using System;
using System.Collections.Generic;
using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    /**********************
     * 角色附加网格合并对象的相关属性数据
     **/
    [System.Serializable]
    public abstract class _AALSOBasicAdditionMeshObjInfo : _AALSOBasicAdditionObjInfo
    {
        /** 初始化时是否有效 */
        public bool initActive;

        public _AALSOBasicAdditionMeshObjInfo()
            : base()
        {
            initActive = true;
        }
    }
}
#endif
