
using GOE;
using UnityEngine;

/// <summary>
/// 用于文本中由超链接的点击跳转网页需求的mono
/// </summary>
public class TextExClickBaseOpenUrl:_ATextExClickBase
{
    
    /// <summary>
    /// 在设置Text之前调用，设置下划线距离文本的距离
    /// </summary>
    /// <param name="_color"></param>
    public void setUnderLineDistance(float _distance)
    {
        _m_underLineDistance = _distance;
    }
    
    /// <summary>
    /// 在设置Text之前调用，设置链接颜色
    /// </summary>
    /// <param name="_color"></param>
    public void setUrlColor(Color _color)
    {
        urlColor =_color;
    }
    
    /// <summary>
    /// 在设置Text之前调用，设置是否显示链接下划线
    /// </summary>
    /// <param name="_isShowUnderLine"></param>
    public void setIsShowUnderLine(bool _isShowUnderLine)
    {
        isShowUnderLine = _isShowUnderLine; 
    }

    protected override void _onHrefClick(string _infoName)
    {
#if NP_GAME
        GCommon.openURLByBrowser(_infoName);
#endif
    }
}