using System;
using ALPackage;
using GOE;
using UnityEngine;

public class GGUIWndAchieveConsortIconItem:_ANPGGUIBasicSubWnd<GGUIMonoAchieveConsortIconItem>
{
    private _IConsortShowInfo _m_showData;

    public event Action<GGUIWndAchieveConsortIconItem> clickAction;

    private NPGGuiWndTexture _m_wConsortHeadIcon;//妃子头像
    
    public GGUIWndAchieveConsortIconItem(GGUIMonoAchieveConsortIconItem _wnd) : base(_wnd)
    {
        initWnd();
    }

    public _IConsortShowInfo consortCardShow
    {
        get { return _m_showData; }
    }

    protected override void _onShowWnd()
    {
        
    }

    protected override void _onHideWnd()
    {
        
        _m_wConsortHeadIcon?.hideWnd();
    }

    protected override void _onReset()
    {
            
        _m_wConsortHeadIcon?.discardTexture();
    }

    protected override void _onDiscard()
    {
        clickAction = null;
        _m_wConsortHeadIcon?.discard();
        _m_wConsortHeadIcon = null;
    }

    protected override void _onWndInitDone()
    {
        if (null == wnd)
            return;
        ALUGUICommon.combineBtnClick(wnd.btnClick, _onClick);
        if (wnd.texIcon != null)
            _m_wConsortHeadIcon = new NPGGuiWndTexture(wnd.texIcon);
    }

    private void _onClick(GameObject obj)
    {
        clickAction?.Invoke(this);
    }

    public void setInfo(_IConsortShowInfo _showData)
    {
        _m_showData = _showData;
        _refreshWnd();
    }

    private void _refreshWnd()
    {
        if (null == wnd || _m_showData == null)
            return;

        if (_m_wConsortHeadIcon != null)
        {
            _m_wConsortHeadIcon.showWnd();
            _m_wConsortHeadIcon.setTexture(_m_showData?.consortSkinShowInfo?.consortHeadIcon);
        }

        GGottenConsortInfo gottenConsortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_showData.consortId);
        EGameCommonUnlockType stat = (null == gottenConsortInfo) ? EGameCommonUnlockType.LOCK : EGameCommonUnlockType.UNLOCK;

        NPCommonEnumStatInfo<EGameCommonUnlockType>.setStat(wnd.statInfos, stat);
    }
}