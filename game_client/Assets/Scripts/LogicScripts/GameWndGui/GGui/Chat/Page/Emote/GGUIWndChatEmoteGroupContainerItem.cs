using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 聊天表情组item
/// </summary>
public class GGUIWndChatEmoteGroupContainerItem : _ATNPGGUIWndSingleChoiceItem<GGUIMonoChatEmoteGroupContainerItem,GGUIWndChatEmoteGroupContainerItem>
{
    private GChatEmoteGroupRefObj _m_groupRef;
    private NPGGuiWndTexture _m_iconWnd;

    public GGUIWndChatEmoteGroupContainerItem(GGUIMonoChatEmoteGroupContainerItem _wnd) : base(_wnd)
    {
        initWnd();
    }

    public GChatEmoteGroupRefObj groupRef { get => _m_groupRef; }

    protected override void _onShowWndEx()
    {
        
    }

    protected override void _onHideWndEx()
    {
        
    }

    protected override void _onResetEx()
    {
        _m_iconWnd?.discardTexture();
    }

    protected override void _onDiscardEx()
    {
        _m_iconWnd?.discard();
        _m_iconWnd = null;
    }

    protected override void _onWndInitDoneEx()
    {
        if(null == wnd)
            return;
        if (null != wnd.icon)
        {
            _m_iconWnd = new NPGGuiWndTexture(wnd.icon);
        }
    }

    public void setInfo(GChatEmoteGroupRefObj _groupRef)
    {
        _m_groupRef = _groupRef;
        _refreshWnd();
    }

    private void _refreshWnd()
    {
        if(null == wnd)
            return;

        ALUGUICommon.setLabelTxt(wnd.txtName, GCommon.getItemName(ENPItemType.CHAT_EMOTE_GROUP, _m_groupRef.id));
        
        _m_iconWnd?.showWnd();
        _m_iconWnd?.setTexture(GCommon.getItemTexIcon(ENPItemType.CHAT_EMOTE_GROUP, _m_groupRef.id));

        EGameCommonUnlockType stat = GCommon.getItemCount(ENPItemType.CHAT_EMOTE_GROUP, _m_groupRef.id) > 0 ? EGameCommonUnlockType.UNLOCK : EGameCommonUnlockType.LOCK;
        NPCommonEnumStatInfo<EGameCommonUnlockType>.setStat(wnd.statInfos, stat);

    }
}