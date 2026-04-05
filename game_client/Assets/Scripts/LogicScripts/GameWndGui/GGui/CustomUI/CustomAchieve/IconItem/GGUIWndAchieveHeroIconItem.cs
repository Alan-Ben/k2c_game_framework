using System;
using ALPackage;
using GOE;
using UnityEngine;

public class GGUIWndAchieveHeroIconItem:_ANPGGUIBasicSubWnd<GGUIMonoAchieveHeroIconItem>
{
    private _IHeroCardShow _m_showData;

    public event Action<GGUIWndAchieveHeroIconItem> clickAction;

    private NPGGuiWndTexture _m_wHeroHeadIcon;//骑士头像
    
    public GGUIWndAchieveHeroIconItem(GGUIMonoAchieveHeroIconItem _wnd) : base(_wnd)
    {
        initWnd();
    }

    public _IHeroCardShow heroCardShow
    {
        get { return _m_showData; }
    }

    protected override void _onShowWnd()
    {
        
    }

    protected override void _onHideWnd()
    {
        
        _m_wHeroHeadIcon?.hideWnd();
    }

    protected override void _onReset()
    {
            
        _m_wHeroHeadIcon?.discardTexture();
    }

    protected override void _onDiscard()
    {
        _m_wHeroHeadIcon?.discard();
        _m_wHeroHeadIcon = null;
        clickAction = null;
    }

    protected override void _onWndInitDone()
    {
        if (null == wnd)
            return;
        ALUGUICommon.combineBtnClick(wnd.btnClick, _onClick);
        if (wnd.texIcon != null)
            _m_wHeroHeadIcon = new NPGGuiWndTexture(wnd.texIcon);
    }

    private void _onClick(GameObject obj)
    {
        clickAction?.Invoke(this);
    }

    public void setInfo(_IHeroCardShow _showData)
    {
        _m_showData = _showData;
        _refreshWnd();
    }

    private void _refreshWnd()
    {
        if (null == wnd)
            return;

        if (_m_wHeroHeadIcon != null)
        {
            _m_wHeroHeadIcon.showWnd();
            _m_wHeroHeadIcon.setTexture(_m_showData.getIcon());
        }

        HeroInfo hero = NPPlayer.instance.heroComponent.getHeroInfo(_m_showData?.heroRefObj?.id ?? 0);
        EGameCommonUnlockType stat = (null == hero) ? EGameCommonUnlockType.LOCK : EGameCommonUnlockType.UNLOCK;

        NPCommonEnumStatInfo<EGameCommonUnlockType>.setStat(wnd.statInfos, stat);
    }
}