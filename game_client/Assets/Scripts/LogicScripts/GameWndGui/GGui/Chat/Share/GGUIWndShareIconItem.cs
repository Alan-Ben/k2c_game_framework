using System;
using ALPackage;
using GOE;
using UnityEngine;


/// <summary>
/// 分享icon的item
/// </summary>
public class GGUIWndShareIconItem : _ANPGGUIBasicGridItemWnd<GGUIMonoShareIconItem>
{
    private _IShareIconSHow _m_showData;
    public Action<_IShareIconSHow> clickItem;
    private NPGGuiWndTexture _m_texIcon;
    private GGuiWndSprite _m_wIconBg;

    public GGUIWndShareIconItem(GGUIMonoShareIconItem _wnd) : base(_wnd)
    {
        initWnd();
    }

    public _IShareIconSHow showData { get => _m_showData; }

    protected override void _onShowWnd()
    {
        _m_texIcon?.showWnd();
        _m_wIconBg?.showWnd();
    }

    protected override void _onHideWnd()
    {
        _m_texIcon?.hideWnd();
        _m_wIconBg?.hideWnd();
    }

    protected override void _onReset()
    {
        _m_texIcon?.discardTexture();
        _m_wIconBg?.discardTexture();
    }

    protected override void _onDiscard()
    {
        _m_texIcon?.discard();
        _m_texIcon = null;
        _m_wIconBg?.discard();
        _m_wIconBg = null;
    }

    protected override void _onWndInitDone()
    {
        if (null == wnd)
            return;
        ALUGUICommon.combineBtnClick(wnd.btnSelected, _clickSelected);

        if (null != wnd.texIcon)
        {
            _m_texIcon = new NPGGuiWndTexture(wnd.texIcon);
        }

        if (null != wnd.imgIconBg)
            _m_wIconBg = new GGuiWndSprite(wnd.imgIconBg);
    }

    private void _clickSelected(GameObject obj)
    {
        clickItem?.Invoke(_m_showData);        
    }

    protected override void _resetGridItem()
    {
        
    }

    public void setInfo(_IShareIconSHow _showData)
    {
        _m_showData = _showData;
        _refreshWnd();
    }

    private void _refreshWnd()
    {
        if (wnd == null || _m_showData == null)
            return;
        
        _m_texIcon?.setTexture(_m_showData.getIcon());
        _m_wIconBg?.setTexture(_m_showData.getIconBg());

        ALUGUICommon.setGameObjEnable(wnd.goSpecialList, _m_showData.needShowSpecial());
    }

    /// <summary>
    /// 设置选中
    /// </summary>
    /// <param name="_isSelected"></param>
    public void setSelected(bool _isSelected)
    {
        ALUGUICommon.setGameObjEnable(wnd.selectedList, _isSelected);
    }
}
