using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;


//分组编辑界面好友列表grid
public class GGUIWndFriendGruopEditorFriendListGrid : _ANPGGUIBasicGridSubWnd<GGUIMonoFriendGruopEditorFriendListGridItem, GGUIMonoFriendGruopEditorFriendListGrid, GGUIWndFriendGruopEditorFriendListGridItem>
{
    //数据列表
    private List<PlayerFriendItemData> _m_infoList;
    private List<PlayerFriendItemData> _m_selectedList;
    public event Action<int> onSelectedItemChg;//选中变化

    public GGUIWndFriendGruopEditorFriendListGrid(GGUIMonoFriendGruopEditorFriendListGrid _wnd) : base(_wnd)
    {
        _m_infoList = new List<PlayerFriendItemData>();
        _m_selectedList = new List<PlayerFriendItemData>();
        initWnd();
    }

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
        _m_infoList.Clear();
        _m_selectedList.Clear();
    }

    protected override void _onWndInitDone()
    {
    }

    protected override GGUIWndFriendGruopEditorFriendListGridItem _createItemWnd(GGUIMonoFriendGruopEditorFriendListGridItem _itemMono)
    {
        // 创建对象
        GGUIWndFriendGruopEditorFriendListGridItem gridItem = new GGUIWndFriendGruopEditorFriendListGridItem(_itemMono);
        gridItem.clickCallBack += _onSelectedItem;
        return gridItem;
    }

    /// <summary>
    /// 点击item的选中按钮的时候
    /// </summary>
    /// <param name="_itemWnd"></param>
    private void _onSelectedItem(GGUIWndFriendGruopEditorFriendListGridItem _itemWnd)
    {
        if (_itemWnd.isSelected)
        {
            if (!_m_selectedList.Contains(_itemWnd.itemData))
            {
                //从配置获取
                int maxSelectedFriendCount = 10;
                if (maxSelectedFriendCount <= _m_selectedList.Count)
                {
                    //已达最大数量
                    _itemWnd?.setSelected(false);
                    return;
                }
                _m_selectedList.Add(_itemWnd.itemData);
            }
        }
        else
        {
            if (_m_selectedList.Contains(_itemWnd.itemData))
            {
                _m_selectedList.Remove(_itemWnd.itemData);
            }
        }
        onSelectedItemChg?.Invoke(_m_selectedList.Count);
    }

    protected override void _refreshItemwnd(GGUIWndFriendGruopEditorFriendListGridItem _itemMono, int _itemIdx)
    {
        if (null == _m_infoList || _itemIdx >= _m_infoList.Count)
            return;

        //获取数据对象
        PlayerFriendItemData showData = _m_infoList[_itemIdx];
        if (null == showData)
            return;

        //刷新物品UI
        _itemMono.setInfo(showData);
        _itemMono.setSelected(_m_selectedList.Contains(showData));
    }
    
    
    /// <summary>
    /// 显示列表
    /// </summary>
    /// <param name="infoList"></param>
    public void showItemList(List<PlayerFriendItemData> infoList)
    {
        if (null == infoList)
            return;
        _m_infoList.Clear();
        _m_infoList.AddRange(infoList);
        _m_selectedList?.Clear();
        _refreshWnd();
        onSelectedItemChg?.Invoke(_m_selectedList.Count);
    }

    private void _refreshWnd()
    {
        if (null == _m_infoList)
            return;
        setItemCount(_m_infoList.Count);
        ALUGUICommon.setGameObjEnable(wnd.emptyShow, _m_infoList.Count == 0);
    }

    /// <summary>
    /// 当前选中的列表
    /// </summary>
    /// <returns></returns>
    public List<PlayerFriendItemData> getSelectedItemList()
    {
        return _m_selectedList;
    }
}
