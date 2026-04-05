using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

//好友列表分组编辑的分组item
public class GGUIWndFriendGruopEditorGroupItem : _ATALBasicUISubWnd<GGUIMonoFriendGruopEditorGroupItem>
{
    private PlayerFriendGroup _m_groupInfo;//分组信息
    public event Action<PlayerFriendGroup> clickDelegate;//点击回调
    
    public GGUIWndFriendGruopEditorGroupItem(GGUIMonoFriendGruopEditorGroupItem _wnd) : base(_wnd)
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
        
    }

    protected override void _onWndInitDone()
    {
        if(null == wnd)
            return;
        ALUGUICommon.combineBtnClick(wnd.btnClick, _clickBtn);
    }

    private void _clickBtn(GameObject obj)
    {
        clickDelegate?.Invoke(_m_groupInfo);
    }

    /// <summary>
    /// 设置显示信息
    /// </summary>
    /// <param name="_groupInfo"></param>
    public void setInfo(PlayerFriendGroup _groupInfo)
    {
        _m_groupInfo = _groupInfo;
        _refreshWnd();
    }

    private void _refreshWnd()
    {
        if (null == wnd)
            return;
        ALUGUICommon.setLabelTxt(wnd.groupName, _m_groupInfo.getGroupName());
    }
}