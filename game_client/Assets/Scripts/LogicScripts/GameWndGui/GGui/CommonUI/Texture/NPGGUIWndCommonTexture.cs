using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

public class NPGGUIWndCommonTexture:_ANPGGUIBasicSubWnd<NPGGUIMonoCommonTexture>
{
    private NPGGuiWndTexture _m_textureWnd;

    public NPGGUIWndCommonTexture(NPGGUIMonoCommonTexture _wnd) : base(_wnd)
    {
        initWnd();
    }

    protected override void _onShowWnd()
    {
        
    }

    protected override void _onHideWnd()
    {
        
    }

    protected override void _onReset()
    {
        _m_textureWnd?.discardTexture();
    }

    protected override void _onDiscard()
    {
        _m_textureWnd?.discard();
        _m_textureWnd = null;
    }

    protected override void _onWndInitDone()
    {
        if (null == wnd)
            return;
        if (null != wnd.texture)
        {
            _m_textureWnd = new NPGGuiWndTexture(wnd.texture);
        }
    }

    /// <summary>
    /// 设置进度显示
    /// </summary>
    /// <param name="_scale"></param>
    public void setTexture(NPGTextureIndex _index)
    {
        if (null == _m_textureWnd)
            return;
        
        _m_textureWnd.setTexture(_index);
        _m_textureWnd.showWnd();
    }
}