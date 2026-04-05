using System;
using System.Collections.Generic;
using GOE;


//表情grid
public class GGUIWndChatEmoteGrid : _ANPGGUIBasicGridSubWnd<GGUIMonoChatEmoteGridItem, GGUIMonoChatEmoteGrid, GGUIWndChatEmoteGridItem>
{
    
    //数据列表
    private List<GChatEmoteItemRefObj> _m_infoList;

    public event Action<GChatEmoteItemRefObj> onClickEmoteItem;
    
    public GGUIWndChatEmoteGrid(GGUIMonoChatEmoteGrid _wnd) : base(_wnd)
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
    }

    protected override void _onWndInitDone()
    {
        _m_infoList = new List<GChatEmoteItemRefObj>();
    }

    protected override GGUIWndChatEmoteGridItem _createItemWnd(GGUIMonoChatEmoteGridItem _itemMono)
    {
        GGUIWndChatEmoteGridItem itemWnd = new GGUIWndChatEmoteGridItem(_itemMono);
        itemWnd.onClickAction += _onClickItemAction;
        return itemWnd;
    }

    protected override void _refreshItemwnd(GGUIWndChatEmoteGridItem _itemMono, int _itemIdx)
    {
        if (null == _m_infoList || _itemIdx >= _m_infoList.Count)
            return;

        //获取数据对象
        GChatEmoteItemRefObj showData = _m_infoList[_itemIdx];
        if (null == showData)
            return;

        //刷新物品UI
        _itemMono.setInfo(showData);
    }
    

    private void _onClickItemAction(GGUIWndChatEmoteGridItem _itemWnd)
    {
        if(null == _itemWnd)
            return;
        if(null == _itemWnd.emoteRef)
            return;
        
        onClickEmoteItem?.Invoke(_itemWnd.emoteRef);
    }
    
    /// <summary>
    /// 设置显示信息
    /// </summary>
    /// <param name="_infoList"></param>
    public void setInfo(List<GChatEmoteItemRefObj> _infoList)
    {
        if(null == _infoList)
            return;
        _m_infoList.Clear();
        _m_infoList.AddRange(_infoList);
        
        setItemCount(_m_infoList.Count);
    }
}
