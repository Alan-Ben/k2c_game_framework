using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using JetBrains.Annotations;
using UnityEngine;


//分享icon的grid
public class GGUIWndShareIconItemGrid  : _ANPGGUIBasicGridSubWnd<GGUIMonoShareIconItem, GGUIMonoShareIconItemGrid, GGUIWndShareIconItem>
{
    [NotNull]private List<_IShareIconSHow> _m_infoList;
    private _IShareIconSHow _m_curSelected;

    public event Action<_IShareIconSHow> selectedItem;

    public GGUIWndShareIconItemGrid(GGUIMonoShareIconItemGrid _wnd) : base(_wnd)
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
        _m_infoList.Clear();
        _m_infoList = null;

        _m_curSelected = null;
    }

    protected override void _onWndInitDone()
    {
        _m_infoList = new List<_IShareIconSHow>();
    }

    protected override GGUIWndShareIconItem _createItemWnd(GGUIMonoShareIconItem _itemMono)
    {
        GGUIWndShareIconItem itemWnd = new GGUIWndShareIconItem(_itemMono);
        itemWnd.clickItem += _clickItem;
        return itemWnd;
    }

    private void _clickItem(_IShareIconSHow obj)
    {
        if (_m_curSelected == obj)
            return;

        _m_curSelected = obj;
        
        selectedItem?.Invoke(_m_curSelected);
        forceRefreshAllItem();
    }

    protected override void _refreshItemwnd(GGUIWndShareIconItem _itemMono, int _itemIdx)
    {
        if (_itemIdx >= _m_infoList.Count)
            return;

        //获取数据对象
        _IShareIconSHow showData = _m_infoList[_itemIdx];
        if (null == showData)
            return;

        //刷新物品UI
        _itemMono.setInfo(showData);
        _itemMono.setSelected(_m_curSelected == showData);
    }

    /// <summary>
    /// 显示列表
    /// </summary>
    /// <param name="_list"></param>
    public void showItemList(List<_IShareIconSHow> _list)
    {
        _m_infoList.Clear();
        _m_infoList.AddRange(_list);
        //默认选中第一个
        if (_m_infoList.Count > 0)
        {
            _clickItem(_m_infoList[0]);
        }
        setItemCount(_m_infoList.Count);
    }
}
