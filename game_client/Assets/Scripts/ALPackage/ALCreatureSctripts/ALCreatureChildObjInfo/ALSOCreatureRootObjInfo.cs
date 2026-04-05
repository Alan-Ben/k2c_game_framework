using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    [System.Serializable]
    public class ALSOCreatureRootObjInfo : _AALSOBaseCreatureChildObjInfo
    {
        public ALSOCreatureRootObjInfo()
        {
        }

        /** 获得对应的子物体 */
        public override Transform getCreatureChildObj(_AALBasicCreatureControl _creature)
        {
            return _creature.transform;
        }

        public override string ToString()
        {
            return " Root";
        }
    }
}
#endif
