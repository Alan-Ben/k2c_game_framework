using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    [System.Serializable]
    public abstract class _AALSOBaseCreatureChildObjInfo : ScriptableObject
    {
        /** 获得对应的子物体 */
        public abstract Transform getCreatureChildObj(_AALBasicCreatureControl _creature);
    }
}
#endif
