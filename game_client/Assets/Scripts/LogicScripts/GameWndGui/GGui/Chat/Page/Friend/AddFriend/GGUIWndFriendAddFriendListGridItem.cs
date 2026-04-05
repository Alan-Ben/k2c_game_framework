using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;


/// <summary>
/// 添加界面好友列表item
/// </summary>
public class GGUIWndFriendAddFriendListGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoFriendAddFriendListGridItem>
{
    private NPGGUIWndPlayerIcon _m_playerIcon;
    private NPCommonSimplePlayerInfo _m_playerInfo;
    private bool _m_isApplyed;

    public event Action<long> onSendApply;


    public GGUIWndFriendAddFriendListGridItem(GGUIMonoFriendAddFriendListGridItem _wnd) : base(_wnd)
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
        ALUGUICommon.combineBtnClick(wnd.btnAdd, _clickAdd);
        if (null != wnd.playerIcon)
        {
            _m_playerIcon = new NPGGUIWndPlayerIcon(wnd.playerIcon);
        }
    }

    protected override void _resetGridItem()
    {
        
    }
    
    /// <summary>
    /// 点击添加按钮
    /// </summary>
    /// <param name="obj"></param>
    private void _clickAdd(GameObject obj)
    {
        if (null == _m_playerInfo)
            return;
        if (_m_isApplyed)
            return;
        
        if (NPPlayer.instance.friendsComp.isFriend(_m_playerInfo.cid))
        {
            NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.friends_is_frend_tip_str, _m_playerInfo.name));
            return;
        }
        
        FriendCommon.sendAddFriendRequest(_m_playerInfo.cid, () =>
        {
            onSendApply?.Invoke(_m_playerInfo.cid);
        }, () =>
        {
            onSendApply?.Invoke(_m_playerInfo.cid);
        });
    }

    /// <summary>
    /// 设置显示信息
    /// </summary>
    /// <param name="_playerInfo"></param>
    public void setInfo(NPCommonSimplePlayerInfo _playerInfo)
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
            _m_playerIcon.showWnd();
            _m_playerIcon.setPlayerInfo(_m_playerInfo, false);
        }

        bool isOnline = _m_playerInfo.isOnline;
        ALUGUICommon.setGameObjEnable(wnd.onlineShow, isOnline);
        ALUGUICommon.setGameObjEnable(wnd.onlineHide, !isOnline);
        ALUGUICommon.setLabelTxt(wnd.offLineTxt, TextTranslate.instance.getLanguage(
            TransKeyConst.friends_offline_time_str,
            TimeUtil.getPassTimeShow(_m_playerInfo.lastOfflineMs)));
    }

    /// <summary>
    /// 设置是否已申请
    /// </summary>
    /// <param name="_isApplyed"></param>
    public void setApplyed(bool _isApplyed)
    {
        _m_isApplyed = _isApplyed;
        ALUGUICommon.setGameObjEnable(wnd.addedShowList, _isApplyed);
        if (_isApplyed)
        {
            GGameCommonInfo.grayImage(wnd.addedGrayList);
        }
        else
        {
            GGameCommonInfo.disgrayImage(wnd.addedGrayList);
        }
    }
}
