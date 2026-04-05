using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

public class NPGGUIWndCommonImageProcess:_ANPGGUIBasicSubWnd<NPGGUIMonoCommonImageProcess>
{
    public NPGGUIWndCommonImageProcess(NPGGUIMonoCommonImageProcess _wnd) : base(_wnd)
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
        
    }

    protected override void _onDiscard()
    {
        
    }

    protected override void _onWndInitDone()
    {
        
    }

    /// <summary>
    /// 设置进度显示
    /// </summary>
    /// <param name="_scale"></param>
    public void setProcess(float _scale)
    {
        if (null == wnd)
            return;
        if (null == wnd.imageProcess)
            return;
        
        float realScale = wnd.minRange + (wnd.maxRange - wnd.minRange) * _scale;
        ALUGUICommon.setSliderScale(wnd.imageProcess,realScale);
    }
}