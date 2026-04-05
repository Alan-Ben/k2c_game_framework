using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

//好友列表分组编辑的分组item 容器
public class GGUIWndFriendGruopEditorGroupItemContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoFriendGruopEditorGroupItem,GGUIMonoFriendGruopEditorGroupItemContainer,GGUIWndFriendGruopEditorGroupItem>
{
    
    public List<GGUIWndFriendGruopEditorGroupItem> _m_lItemGroupList;//子控件列表
    public Action<PlayerFriendGroup> clickDelegate;
    
    public GGUIWndFriendGruopEditorGroupItemContainer(GGUIMonoFriendGruopEditorGroupItemContainer _containerMono) : base(_containerMono)
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
        if(_m_lItemGroupList != null)
            _m_lItemGroupList.Clear();
    }

    protected override void _onDiscard()
    {
        if(_m_lItemGroupList != null)
            _m_lItemGroupList.Clear();
        _m_lItemGroupList = null;
    }

    protected override void _onWndInitDone()
    {
        if (null == wnd)
            return;
        _m_lItemGroupList = new List<GGUIWndFriendGruopEditorGroupItem>();
        ALUGUICommon.combineBtnClick(wnd.btnClose, _clickClose);
    }

    protected override GGUIWndFriendGruopEditorGroupItem _createItemWnd(GGUIMonoFriendGruopEditorGroupItem _itemMono)
    {
        GGUIWndFriendGruopEditorGroupItem itemWnd = new GGUIWndFriendGruopEditorGroupItem(_itemMono);
        itemWnd.clickDelegate += _onClickItem;
        return itemWnd;
    }

    private void _onClickItem(PlayerFriendGroup _friendGroup)
    {
        clickDelegate?.Invoke(_friendGroup);
        hideWnd();
    }

    private void _clickClose(GameObject obj)
    {
        hideWnd();
    }

    /// <summary>
    /// 显示item列表
    /// </summary>
    /// <param name="_itemDataList"></param>
    public void showItemList(List<PlayerFriendGroup> _itemDataList)
    {
        if (_itemDataList == null)
            return;

        PlayerFriendGroup tempData = null;
        GGUIWndFriendGruopEditorGroupItem tempItemWnd = null;
        int count = 0;
        for (int i = 0; i < _itemDataList.Count; ++i)
        {
            tempData = _itemDataList[i];
            if (tempData == null)
                continue;
            if (count >= _m_lItemGroupList.Count)
            {
                tempItemWnd = addItemWnd();
                if (tempItemWnd == null)
                    continue;
                //放入数据队列
                _m_lItemGroupList.Add(tempItemWnd);
            }
            else
            {
                tempItemWnd = _m_lItemGroupList[count];
            }

            tempItemWnd.setInfo(tempData);
            count++;
        }

        for (int i = _m_lItemGroupList.Count; i > count; i--)
        {
            removeItemWnd(_m_lItemGroupList[i - 1]);
            _m_lItemGroupList.RemoveAt(i - 1);
        }

        ALUGUICommon.setGameObjEnable(wnd.emptyShow, _itemDataList.Count == 0);
        _refreshContentLayout();
    }

    /// <summary>
    /// 刷新容器布局
    /// </summary>
    public void _refreshContentLayout()
    {
        ALCommonActionMonoTask.addNextFrameTask(() =>
        {
            if (wnd == null || wnd.itemContainer == null)
                return;

            LayoutRebuilder.ForceRebuildLayoutImmediate(wnd.itemContainer.GetComponent<RectTransform>());
            moveToLeft();
        });
    }
}