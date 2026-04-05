using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

public class GGUIWndCommonRoundSlider : _ANPGGUIBasicSubWnd<GGUIMonoCommonRoundSlider>
{
    private float _m_minFillAmount;
    private float _m_marginFillAmount;

    public GGUIWndCommonRoundSlider(GGUIMonoCommonRoundSlider _wnd) : base(_wnd)
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
        _checkAngle();
        _refresh();
    }

    private void _refresh()
    {
        if (null == wnd || null == wnd.imageProcess)
            return;

        if (wnd.imageProcess.type != UnityEngine.UI.Image.Type.Filled || wnd.imageProcess.fillMethod != UnityEngine.UI.Image.FillMethod.Radial180 || wnd.imageProcess.fillOrigin != 0)
        {
            ALLog.Error("imageProcess imageType is not Filled or imageProcess.fillMethod is not Radial180  or imageProcess.fillOrigin is not Bottom");
            return;
        }

        _rotateImgToMinAngle(wnd.imageProcess);

        if (null != wnd.imageProcessBg)
        {
            if (wnd.imageProcessBg.type != UnityEngine.UI.Image.Type.Filled || wnd.imageProcessBg.fillMethod != UnityEngine.UI.Image.FillMethod.Radial180 || wnd.imageProcessBg.fillOrigin != 0)
            {
                ALLog.Error("imageProcessBg imageType is not Filled or imageProcessBg.fillMethod is not Radial180 or imageProcessBg.fillOrigin is not Bottom");
            }
            else
            {
                _rotateImgToMinAngle(wnd.imageProcessBg);
                wnd.imageProcessBg.fillAmount = _m_marginFillAmount + wnd.firstMargin + wnd.lastMargin;
            }
        }
    }

    /// <summary>
    /// 设置进度 0 - 1
    /// </summary>
    /// <param name="_value"></param>
    public void setValue(float _value)
    {
        if (null == wnd)
            return;

        if (_value < 0)
            _value = 0;

        if (_value > 1)
            _value = 1;

        float fillAmount = _m_marginFillAmount * _value;
        if (_value > 0)
            fillAmount += wnd.firstMargin;

        if (_value == 1)
            fillAmount += wnd.lastMargin;

        if (null != wnd.imageProcess)
            wnd.imageProcess.fillAmount = fillAmount;
    }

    private void _checkAngle()
    {
        if (null == wnd)
            return;

        float maxFillAmount = wnd.maxFillAmount;
        float minFillAmount = wnd.minFillAmount;

        if (minFillAmount < 0)
            minFillAmount = 0;

        if (maxFillAmount > 1)
            maxFillAmount = 1;

        if (maxFillAmount < minFillAmount)
        {
            ALLog.Error("error maxFillAmount isLarger than minFillAmount");
            float temp = minFillAmount;
            maxFillAmount = minFillAmount;
            minFillAmount = temp;
        }

        _m_minFillAmount = minFillAmount - wnd.firstMargin;
        if (_m_minFillAmount < 0)
            _m_minFillAmount = 0;
        _m_marginFillAmount = maxFillAmount - minFillAmount;
    }

    //把图片旋转到最小角度
    private void _rotateImgToMinAngle(Image _img)
    {
        if (null == _img || null == wnd)
            return;

        _img.rectTransform.Rotate(0, 0, -180 * _m_minFillAmount);
    }
}