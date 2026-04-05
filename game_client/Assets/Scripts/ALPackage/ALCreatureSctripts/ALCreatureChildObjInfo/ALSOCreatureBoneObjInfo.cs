using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    [System.Serializable]
    public class ALSOCreatureBoneObjInfo : _AALSOBaseCreatureChildObjInfo
    {
        public string boneObjName;

        public ALSOCreatureBoneObjInfo()
        {
            boneObjName = "";
        }

        /** 获得对应的子物体 */
        public override Transform getCreatureChildObj(_AALBasicCreatureControl _creature)
        {
            Transform transform = _creature.getBoneMgr().getBone(boneObjName);

            if (null == transform)
                Debug.LogError("Can not find bone: " + boneObjName);

            return transform;
        }

        public override string ToString()
        {
            return " Bone: " + boneObjName;
        }
    }
}
#endif
