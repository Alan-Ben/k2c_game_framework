using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 周卡选形象的item
/// </summary>
public class GGUIWndWeekCardAssignTDShowItem : _ATNPGGUIWndSingleChoiceItem<GGUIMonoWeekCardAssignTDShowItem,GGUIWndWeekCardAssignTDShowItem>
{
    private _ICommonIconShowData _m_showData;
    private NPGGuiWndTexture _m_texIcon;

    public GGUIWndWeekCardAssignTDShowItem(GGUIMonoWeekCardAssignTDShowItem _wnd) : base(_wnd)
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
        
    }

    /// <summary>
    /// 设置当前佩戴选择的显示
    /// </summary>
    /// <param name="_isCurSelected"></param>
    public void setIsCurSelected(bool _isCurSelected)
    {
        ALUGUICommon.setGameObjEnable(wnd.curSelectedShow, _isCurSelected);
    }
}
