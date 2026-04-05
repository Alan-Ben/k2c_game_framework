using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

/******************
 * 地表对象需要添加本脚本
 **/
public class WCGTerrainMatMono : MonoBehaviour
{
    // Update is called once per frame
    void Start()
    {
        //获取本对象的材质
        Renderer render = GetComponent<Renderer>();
        if (null == render)
            return;

        //注册到管理对象
        for(int i = 0; i < render.sharedMaterials.Length; i++)
        {
            if (null == render.sharedMaterials[i])
                continue;

            //设置贴图
            render.sharedMaterials[i].SetTexture("_ShadowPic", WCGShadowRenderMgr.instance.shadowRenderTexture);
            //设置阴影明暗
            render.sharedMaterials[i].SetFloat("_ShadowAlpha", WCGShadowRenderMgr.instance.shadowAlpha);
        }
    }
}
