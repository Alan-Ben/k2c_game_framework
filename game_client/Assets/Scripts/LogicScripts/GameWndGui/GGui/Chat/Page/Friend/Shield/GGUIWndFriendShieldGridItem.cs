using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 屏蔽列表item
/// </summary>
public class GGUIWndFriendShieldGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoFriendShieldGridItem>
{
    private NPGGUIWndPlayerIcon _m_playerIcon;
    private PlayerShieldItem _m_playerInfo;
    public GGUIWndFriendShieldGridItem(GGUIMonoFriendShieldGridItem _wnd) : base(_wnd)
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
        _m_playerIcon?.discard();
        _m_playerIcon = null;
    }

    protected override void _onWndInitDone()
    {
        if(null == wnd)
            return;
        ALUGUICommon.combineBtnClick(wnd.btnRemove, _clickRemove);
        if (null != wnd.playerIcon)
        {
            _m_playerIcon = new NPGGUIWndPlayerIcon(wnd.playerIcon);
        }
    }

    private void _clickRemove(GameObject obj)
    {
        FriendCommon.setUnShieldPlayer(_m_playerInfo.data.playerInfo);
    }

    protected override void _resetGridItem()
    {
        
    }

    public void setInfo(PlayerShieldItem _playerInfo)
    {
        _m_playerInfo = _playerInfo;
        _refreshWnd();
    }

    /// <summary>
    /// 刷新页面
    /// </summary>
    private void _refreshWnd()
    {
        if(null == wnd)
            return;
        if(null == _m_playerInfo)
            return;

        if (null != _m_playerIcon)
        {
            _m_playerInfo.getValue((itemData) =>
            {
                _m_playerIcon.showWnd();
                _m_playerIcon.setPlayerInfo(itemData.playerInfo, false);
            });
        }

    }
}
