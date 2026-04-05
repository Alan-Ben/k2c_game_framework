
using System;
using GOE;
using UnityEngine;

public class LightEnvChangeMono : MonoBehaviour
{
    private long _m_serialize = 0;
    public GLightGoIndex lightGoIndex;
    
    private void OnEnable()
    {
#if NP_GAME
        _m_serialize = LightsMgr.instance.openAdditionLight(lightGoIndex, null);
        Debug.LogError_EditorOnly($"LightEnvChangeMono 这个脚本理论上只能临时测试用，正式环境不能挂，资源名字{this.name}  策划注意删除！！！！！！！");
#endif
    }

    private void OnDisable()
    {
#if NP_GAME
        if(_m_serialize != 0)
            LightsMgr.instance.closeAdditionLight(_m_serialize);
        _m_serialize = 0;
#endif
    }
}
