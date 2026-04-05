using System;
using ALPackage;
using System.Collections;
using System.Collections.Generic;
using GOE;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;


//聊天表情组container
public class GGUIWndChatEmoteGroupContainer  : _ATNPGGUIWndSingleChoiceContainer <GGUIMonoChatEmoteGroupContainerItem, GGUIMonoChatEmoteGroupContainer, GGUIWndChatEmoteGroupContainerItem>
{
    private List<GGUIWndChatEmoteGroupContainerItem> _m_lItemGroupList;
    
    public GGUIWndChatEmoteGroupContainer(GGUIMonoChatEmoteGroupContainer _containerMono) : base(_containerMono)
    {
        initWnd();
    }

    protected override void _onShowWndEx()
    {
        _refreshWnd();
        NPPlayer.instance.chatComp.emoteGroupChg += _onEmoteGroupChg;
    }

    protected override void _onHideWndEx()
    {
        NPPlayer.instance.chatComp.emoteGroupChg -= _onEmoteGroupChg;
    }

    protected override void _onResetEx()
    {
        _m_lItemGroupList?.Clear();
    }

    protected override void _onDiscardEx()
    {
        _m_lItemGroupList?.Clear();
        _m_lItemGroupList = null;
    }

    protected override void _onWndInitDoneEx()
    {
        _m_lItemGroupList = new List<GGUIWndChatEmoteGroupContainerItem>();
    }

    protected override GGUIWndChatEmoteGroupContainerItem _createItemWnd(GGUIMonoChatEmoteGroupContainerItem _itemMono)
    {
        GGUIWndChatEmoteGroupContainerItem itemWnd = new GGUIWndChatEmoteGroupContainerItem(_itemMono);
        return itemWnd;
    }


    private void _onEmoteGroupChg()
    {
        _refreshWnd();
    }

    private void _refreshWnd()
    {
        GGUIWndChatEmoteGroupContainerItem tempItemWnd = null;
        int count = 0;
        List<GChatEmoteGroupRefObj> emoteGroupList = GRefdataCoreMgr.instance.chatEmoteGroupRefCore.makeNewAllRefList();
        emoteGroupList.Sort(_sortEmoteGroup);
        GChatEmoteGroupRefObj emoteGroupRefObj = null;
        for (int i = 0; i < emoteGroupList.Count; i++)
        {
            emoteGroupRefObj = emoteGroupList[i];
            if (null == emoteGroupRefObj || !emoteGroupRefObj.show_cond.IsEnable(null))
                return;

            if (count >= _m_lItemGroupList.Count)
            {
                tempItemWnd = addItemWnd();
                if (tempItemWnd == null)
                    return;
                //放入数据队列
                _m_lItemGroupList.Add(tempItemWnd);
            }
            else
            {
                tempItemWnd = _m_lItemGroupList[count];
            }

            tempItemWnd.setInfo(emoteGroupRefObj);
            count++;
        }

        for (int i = _m_lItemGroupList.Count; i > count; i--)
        {
            removeItemWnd(_m_lItemGroupList[i - 1]);
            _m_lItemGroupList.RemoveAt(i - 1);
        }

        _refreshContentLayout();
    }

    /// <summary>
    /// 对表情包的排序
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private int _sortEmoteGroup(GChatEmoteGroupRefObj x, GChatEmoteGroupRefObj y)
    {
        //解锁优先
        if (GCommon.getItemCount(ENPItemType.CHAT_EMOTE_GROUP, x.id) > 0
            && GCommon.getItemCount(ENPItemType.CHAT_EMOTE_GROUP, y.id) == 0)
            return -1;
        if (GCommon.getItemCount(ENPItemType.CHAT_EMOTE_GROUP, x.id) == 0
            && GCommon.getItemCount(ENPItemType.CHAT_EMOTE_GROUP, y.id) > 0)
            return 1;
        //排序id小的优先
        if (x.sort_id < y.sort_id)
            return -1;
        if (x.sort_id > y.sort_id)
            return 1;
        //id小的优先
        if (x.id < y.id)
            return -1;
        if (x.id > y.id)
            return 1;
        return 0;
    }

    /// <summary>
    /// 刷新容器布局
    /// </summary>
    private void _refreshContentLayout()
    {
        ALCommonActionMonoTask.addNextFrameTask(() =>
        {
            if (wnd == null || wnd.itemContainer == null)
                return;

            LayoutRebuilder.ForceRebuildLayoutImmediate(wnd.itemContainer.GetComponent<RectTransform>());
        });
    }

    /// <summary>
    /// 设置选中
    /// </summary>
    /// <param name="_curSelectedGroup"></param>
    public void setSelectInfo(GChatEmoteGroupRefObj _curSelectedGroup)
    {
        _refreshWnd();
        GGUIWndChatEmoteGroupContainerItem selectedWnd = null;
        if (null == _curSelectedGroup)
        {
            foreach (GGUIWndChatEmoteGroupContainerItem itemWnd in _m_lItemGroupList)
            {
                if (itemWnd.groupRef == _curSelectedGroup)
                {
                    selectedWnd = itemWnd;
                    break;
                }
            }
        }

        //没有选中就默认选中第一个
        if (null == selectedWnd)
            selectedWnd = _m_lItemGroupList.Count > 0 ? _m_lItemGroupList[0] : null;
        if(null != selectedWnd)        
            setSelectItem(selectedWnd);
    }
}
