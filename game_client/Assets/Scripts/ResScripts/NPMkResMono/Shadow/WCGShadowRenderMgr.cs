using System;
using System.Collections.Generic;
using UnityEngine;

/*****************
 * 阴影管理对象
 **/
public class WCGShadowRenderMgr
{
    private static WCGShadowRenderMgr _g_instance = new WCGShadowRenderMgr();
    public static WCGShadowRenderMgr instance
    {
        get
        {
            if (null == _g_instance)
                _g_instance = new WCGShadowRenderMgr();
            return _g_instance;
        }
    }

    //阴影图片
    private RenderTexture _m_rtShadowRenderTexture;
    //明暗
    private float _m_fShadowAlpha = 0.5f;

    protected WCGShadowRenderMgr()
    {
        _m_rtShadowRenderTexture = null;
    }

    public RenderTexture shadowRenderTexture { get { return _m_rtShadowRenderTexture; } }
    public float shadowAlpha { get { return _m_fShadowAlpha; } }

    //设置初阴影渲染图
    public void initRenderTexture(RenderTexture _renderTexture)
    {
        _m_rtShadowRenderTexture = _renderTexture;
    }

    public void setShadowAlpha(float _alpha)
    {
        _m_fShadowAlpha = _alpha;
    }
}
