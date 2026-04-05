using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    public class ALCreatureBoneMgr
    {
        private _AALBasicCreatureControl m_ccParent;
        private Dictionary<string, Transform> m_dBoneDic;

        public ALCreatureBoneMgr(_AALBasicCreatureControl _parent)
        {
            m_ccParent = _parent;
            m_dBoneDic = new Dictionary<string, Transform>();
        }

        /**
         * 刷新骨骼表
         **/
        public void resetBones()
        {
            if (null == m_ccParent)
                return;

            //clear dic
            m_dBoneDic.Clear();

            int childCount = m_ccParent.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Transform childTrans = m_ccParent.transform.GetChild(i);

                SkinnedMeshRenderer skinRender = childTrans.GetComponent<SkinnedMeshRenderer>();

                if (null == skinRender)
                    continue;

                Transform bone = null;
                for(int n = 0; n < skinRender.bones.Length; n++)
                {
                    bone = skinRender.bones[n];
                    if(null == bone)
                        continue;

                    if (m_dBoneDic.ContainsKey(bone.name))
                    {
                        Debug.Log("Bone: " + bone.name + " is multiple in creature!");
                        continue;
                    }

                    m_dBoneDic.Add(bone.name, bone);
                }
            }
        }

        /**
         * 获取对应名称的骨骼
         **/
        public Transform getBone(string _name)
        {
            if (!m_dBoneDic.ContainsKey(_name))
                return null;

            return m_dBoneDic[_name];
        }

        /****************
         * 释放所有资源对象
         **/
        public void discard()
        {
            m_dBoneDic.Clear();
        }
    }
}
#endif
