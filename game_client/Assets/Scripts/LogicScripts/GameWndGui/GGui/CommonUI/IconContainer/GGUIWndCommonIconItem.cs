using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 带解锁状态的icon的item
/// </summary>
public class GGUIWndCommonIconItem : _ATNPGGUIWndSingleChoiceItem<GGUIMonoCommonIconItem,GGUIWndCommonIconItem>
{
    private _ICommonIconShowData _m_showData;
    private NPGGuiWndTexture _m_texIcon;

    public GGUIWndCommonIconItem(GGUIMonoCommonIconItem _wnd) : base(_wnd)
    {
    }

    public _ICommonIconShowData showData { get => _m_showData; }

    protected override void _onShowWndEx()
    {
        
    }

    protected override void _onHideWndEx()
    {
    }

    protected override void _onResetEx()
    {
        _m_texIcon?.discardTexture();
    }

    protected override void _onDiscardEx()
    {
        _m_texIcon?.discard();
        _m_texIcon = null;
    }

    protected override void _onWndInitDoneEx()
    {
        if (null == wnd)
            return;
        if (null != wnd.texIcon)
        {
            _m_texIcon = new NPGGuiWndTexture(wnd.texIcon);
        }
    }

    public void setInfo(_ICommonIconShowData _showData)
    {
        _m_showData = _showData;
        _refreshWnd();
    }

    private void _refreshWnd()
    {
        if (null == wnd)
            return;
        
        _m_texIcon?.showWnd();
        _m_texIcon?.setTexture(_m_showData.getIcon());

        ALUGUICommon.setLabelTxt(wnd.txtContent, _m_showData.getContent());
        
        NPCommonEnumStatInfo<EGameCommonUnlockType>.setStat(wnd.statInfos, _m_showData.getUnlockType());
    }
}
