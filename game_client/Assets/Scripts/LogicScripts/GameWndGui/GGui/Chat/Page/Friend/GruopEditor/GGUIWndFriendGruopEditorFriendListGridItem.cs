using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;


/// <summary>
/// 分组编辑界面好友列表item
/// </summary>
public class GGUIWndFriendGruopEditorFriendListGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoFriendGruopEditorFriendListGridItem>
{

    //好友数据
    private PlayerFriendItemData _m_itemData;

    //头像
    private NPGGUIWndPlayerIcon _m_playerIconWnd;
    private NPGGUIWndCommonToggleEx _m_selectedTog;
    private bool _m_isSelected = false;
    public event Action<GGUIWndFriendGruopEditorFriendListGridItem> clickCallBack ;

    public GGUIWndFriendGruopEditorFriendListGridItem(GGUIMonoFriendGruopEditorFriendListGridItem _wnd) : base(_wnd)
    {
    }

    public PlayerFriendItemData itemData { get => _m_itemData; }
    public bool isSelected { get => _m_isSelected; }

    protected override void _onShowWnd()
    {
        _refreshWnd();
    }

    protected override void _onHideWnd()
    {
        
    }

    protected override void _onReset()
    {
        
    }

    protected override void _onDiscard()
    {
        if (null != _m_playerIconWnd)
            _m_playerIconWnd.discard();
        _m_playerIconWnd = null;
        
        _m_selectedTog?.discard();
        _m_selectedTog = null;
    }

    protected override void _onWndInitDone()
    {
        if (null == wnd)
            return;
        if (null != wnd.playerIcon)
            _m_playerIconWnd = new NPGGUIWndPlayerIcon(wnd.playerIcon);

        if (null != wnd.selectedTog)
        {
            _m_selectedTog = new NPGGUIWndCommonToggleEx(wnd.selectedTog);
            _m_selectedTog.clickDelegate += _clickDelegate;
        }
    }

    private void _clickDelegate(NPGGUIWndCommonToggleEx obj)
    {
        setSelected(!_m_isSelected);
        clickCallBack?.Invoke(this);
    }

    protected override void _resetGridItem()
    {
        
    }
    
    //设置数据
    public void setInfo(PlayerFriendItemData _item)
    {
        if (null == _item)
            return;

        _m_itemData = _item;
        _refreshWnd();
    }

    /// <summary>
    /// 设置选中状态
    /// </summary>
    /// <param name="_isSelected"></param>
    public void setSelected(bool _isSelected)
    {
        _m_isSelected = _isSelected;
        _refreshSelected();
    }

    /// <summary>
    /// 刷新选中显示
    /// </summary>
    private void _refreshSelected()
    {
        _m_selectedTog?.setSelected(_m_isSelected);
    }

    //刷新
    private void _refreshWnd()
    {
        if (null == wnd || null == _m_itemData)
            return;

        if (null != _m_playerIconWnd)
        {
            _m_playerIconWnd.setPlayerInfo(_m_itemData.playerInfo);
        }

        _refreshSelected();
    }
}
