using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;
using System;


/// <summary>
/// 好友申请列表界面
/// </summary>
public class GGUIWndFriendRequestPage : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoFriendRequestPage>
{
    private GGUIWndRequestFriendsGrid _m_gridMono;//申请列表

    public GGUIWndFriendRequestPage(Transform _parent) : base(_parent)
    {
    }

    protected override string _monoAssetPath { get => GGUIMonoFriendRequestPage.assetPath; }
    protected override string _monoObjName { get => GGUIMonoFriendRequestPage.objName; }
    protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

    protected override void _onShowWnd()
    {
        _refreshWnd();
        AccountSettingMgr.instance.accountSetting.setFriendApplyShowTimeS(FpsAndPingMgr.instance.serverTimeTagS);
        
        _ARedTipNode applyRedNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_FRIEND_APPLY_ENTER);
        applyRedNode?.setCount(0);
    }

    protected override void _onHideWnd()
    {
        
    }

    protected override void _onReset()
    {
        
    }

    protected override void _onDiscard()
    {
        _m_gridMono?.discard();
        _m_gridMono = null;
    }

    protected override void _onWndInitDone()
    {
        if(null == wnd)
            return;
        if (null != wnd.gridMono)
        {
            _m_gridMono = new GGUIWndRequestFriendsGrid(wnd.gridMono);
        }
    }

    /// <summary>
    /// 刷新界面
    /// </summary>
    private void _refreshWnd()
    {
        if(null == wnd)
            return;

        if (null != _m_gridMono)
        {
            _m_gridMono.showWnd();
        }
    }
}
