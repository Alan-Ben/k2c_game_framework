using System;
using ALPackage;
using GOE;
using UnityEngine.UI;


/// <summary>
/// 周卡--选择骑士item
/// </summary>
public class GGUIWndWeekCardSelectedHeroGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoWeekCardSelectedHeroGridItem>
{
    private HeroInfo _m_info;
    private NPGGUIWndCommonToggleEx _m_togSelected;
    public event Action<GGUIWndWeekCardSelectedHeroGridItem> onClickSelected;
    
    public GGUIWndWeekCardSelectedHeroGridItem(GGUIMonoWeekCardSelectedHeroGridItem _wnd) : base(_wnd)
    {
        initWnd();
    }

    public HeroInfo info { get => _m_info; }

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
        onClickSelected = null;
        
        _m_togSelected?.discard();
        _m_togSelected = null;
      
    }

    protected override void _onWndInitDone()
    {
        if(null == wnd)
            return;
        if (null != wnd.togSelected)
        {
            _m_togSelected = new NPGGUIWndCommonToggleEx(wnd.togSelected);
            _m_togSelected.clickDelegate += _clickDelegate;
        }

        if (null != wnd.heroShow)
        {
        }
    }

    protected override void _resetGridItem()
    {
        
    }

    public void setInfo(HeroInfo _info)
    {
        _m_info = _info;
        _refreshWnd();
    }

    public void setSelected(bool _isSelected)
    {
        _m_togSelected?.setSelected(_isSelected);
    }

    private void _clickDelegate(NPGGUIWndCommonToggleEx obj)
    {
        onClickSelected?.Invoke(this);
    }

    /// <summary>
    /// 刷新界面
    /// </summary>
    private void _refreshWnd()
    {
        if (null == wnd)
            return;
    }
}
