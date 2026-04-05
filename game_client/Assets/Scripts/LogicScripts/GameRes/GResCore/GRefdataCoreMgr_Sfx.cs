using System.Collections.Generic;

namespace GOE
{
    //皮肤相关
    public partial class GRefdataCoreMgr
    {
        private void _initSfxRef()
        {
            //遍历所有3d数据检索特效数据
            NPSfx3DRefObj sfx3DRef = null;
            for (int i = 0; i < sfx3dList.refList.Count; i++)
            {
                sfx3DRef = sfx3dList.refList[i];
                if (null == sfx3DRef)
                    continue;

                NPSfxRefObj sfxRef = sfxMap.getRef(sfx3DRef.id);
                if (null == sfxRef)
                {
                    UnityEngine.Debug.LogError($"can not find sfx: {sfx3DRef.id}");
                    continue;
                }

#if UNITY_EDITOR
                if(null != sfxRef.sfx3DRef)
                {
                    UnityEngine.Debug.LogError($"multiple sfx 3d ref for: {sfx3DRef.id}");
                }
#endif

                sfxRef.init3DRef(sfx3DRef);
            }
        }
    }
}