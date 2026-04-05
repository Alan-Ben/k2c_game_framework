using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;


//屏蔽列表grid
public class GGUIWndFriendShieldGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoFriendShieldGridItem, GGUIMonoFriendShieldGrid, GGUIWndFriendShieldGridItem>
{
    //数据列表
    private List<PlayerShieldItem> _m_infoList;
    public GGUIWndFriendShieldGrid(GGUIMonoFriendShieldGrid _wnd) : base(_wnd)
    {
        _m_infoList = new List<PlayerShieldItem>();
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
    }

    protected override void _onWndInitDone()
    {
        
    }

    protected override GGUIWndFriendShieldGridItem _createItemWnd(GGUIMonoFriendShieldGridItem _itemMono)
    {
        GGUIWndFriendShieldGridItem itemWnd = new GGUIWndFriendShieldGridItem(_itemMono);
        return itemWnd;
    }

    protected override void _onRefreshItemWnd(GGUIWndFriendShieldGridItem _itemMono, int _itemIdx)
    {
        if (null == _m_infoList || _itemIdx >= _m_infoList.Count)
            return;

        //获取数据对象
        PlayerShieldItem showData = _m_infoList[_itemIdx];
        if (null == showData)
            return;

        //刷新物品UI
        _itemMono.setInfo(showData);
    }
    /// <summary>
    /// 显示列表
    /// </summary>
    /// <param name="infoList"></param>
    public void showItemList(List<PlayerShieldItem> infoList)
    {
        _m_infoList.Clear();
        if (null != infoList)
        {
            _m_infoList.AddRange(infoList);
        }
        setItemCount(_m_infoList.Count);
        ALUGUICommon.setGameObjEnable(wnd.emptyShow, _m_infoList.Count == 0);
    }
}
