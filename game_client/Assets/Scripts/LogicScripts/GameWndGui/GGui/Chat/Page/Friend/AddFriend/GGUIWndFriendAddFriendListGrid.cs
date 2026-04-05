using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;


//添加编辑界面好友列表grid
public class GGUIWndFriendAddFriendListGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoFriendAddFriendListGridItem, GGUIMonoFriendAddFriendListGrid, GGUIWndFriendAddFriendListGridItem>
{
    
    //数据列表
    private List<NPCommonSimplePlayerInfo> _m_infoList;
    private List<long> _m_applyList;//已申请列表
    
    public GGUIWndFriendAddFriendListGrid(GGUIMonoFriendAddFriendListGrid _wnd) : base(_wnd)
    {
        _m_infoList = new List<NPCommonSimplePlayerInfo>();
        _m_applyList = new List<long>();
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
        _m_applyList.Clear();
    }

    protected override void _onWndInitDone()
    {
        
    }

    protected override GGUIWndFriendAddFriendListGridItem _createItemWnd(GGUIMonoFriendAddFriendListGridItem _itemMono)
    {
        GGUIWndFriendAddFriendListGridItem itemWnd = new GGUIWndFriendAddFriendListGridItem(_itemMono);
        itemWnd.onSendApply += _onSendApply;
        return itemWnd;
    }

    private void _onSendApply(long _cid)
    {
        if(_m_applyList.Contains(_cid))
            return;
        _m_applyList.Add(_cid);
        forceRefreshAllItem();
    }

    protected override void _onRefreshItemWnd(GGUIWndFriendAddFriendListGridItem _itemMono, int _itemIdx)
    {
        if (null == _m_infoList || _itemIdx >= _m_infoList.Count)
            return;

        //获取数据对象
        NPCommonSimplePlayerInfo showData = _m_infoList[_itemIdx];
        if (null == showData)
            return;

        //刷新物品UI
        _itemMono.setInfo(showData);
        _itemMono.setApplyed(_m_applyList.Contains(showData.cid));
    }

    /// <summary>
    /// 显示列表
    /// </summary>
    /// <param name="infoList"></param>
    public void showItem(NPCommonSimplePlayerInfo playerInfo)
    {
        if (null == playerInfo)
        {
            showItemList(null);
        }
        else
        {
            showItemList(new List<NPCommonSimplePlayerInfo>() {playerInfo});
        }
    }
    
    /// <summary>
    /// 显示列表
    /// </summary>
    /// <param name="infoList"></param>
    public void showItemList(List<NPCommonSimplePlayerInfo> infoList)
    {
        _m_infoList.Clear();
        if (null != infoList)
        {
            _m_infoList.AddRange(infoList);
        }
        setItemCount(_m_infoList.Count);
        ALUGUICommon.setGameObjEnable(wnd.emptyShow, _m_infoList.Count == 0);
        moveToTop();
    }
}
