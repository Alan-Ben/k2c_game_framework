using System;
using ALPackage;
using System.Collections;
using System.Collections.Generic;using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 周卡--选择骑士容器
/// </summary>

public class GGUIWndWeekCardSelectedHeroGrid :_ATNPGGUIWndShowAnimGrid<GGUIMonoWeekCardSelectedHeroGridItem,GGUIMonoWeekCardSelectedHeroGrid, GGUIWndWeekCardSelectedHeroGridItem>
{
    private List<HeroInfo> _m_allHeroInfo;
    private List<long> _m_selectedHeroList;
    private int _m_maxLimit;//限制可选数量
    public event Action<int,int> onSelectedCountChg;

    public GGUIWndWeekCardSelectedHeroGrid(GGUIMonoWeekCardSelectedHeroGrid _wnd) : base(_wnd)
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
        onSelectedCountChg = null;
    }

    protected override void _onWndInitDone()
    {
        _m_allHeroInfo = new List<HeroInfo>();
        _m_selectedHeroList = new List<long>();
    }

    protected override GGUIWndWeekCardSelectedHeroGridItem _createItemWnd(GGUIMonoWeekCardSelectedHeroGridItem _itemMono)
    {
        GGUIWndWeekCardSelectedHeroGridItem itemWnd = new GGUIWndWeekCardSelectedHeroGridItem(_itemMono);
        itemWnd.onClickSelected += _onClickSelected;
        return itemWnd;
    }

    protected override void _onRefreshItemWnd(GGUIWndWeekCardSelectedHeroGridItem _itemMono, int _itemIdx)
    {
        if (_itemIdx < 0 || _itemIdx >= _m_allHeroInfo.Count)
            return;
        HeroInfo info = _m_allHeroInfo[_itemIdx];
        _itemMono.setInfo(info);
        _itemMono.setSelected(_m_selectedHeroList.Contains(info.id));
    }

    /// <summary>
    /// 设置显示信息
    /// </summary>
    /// <param name="_allHeroInfo"></param>
    /// <param name="_defaultSelectedHeroList"></param>
    /// <param name="_maxLimit"></param>
    public void setInfo(List<HeroInfo> _allHeroInfo, List<long> _defaultSelectedHeroList, int _maxLimit)
    {
        _m_allHeroInfo.Clear();
        _m_allHeroInfo.AddRange(_allHeroInfo);
        _m_selectedHeroList.Clear();
        _m_selectedHeroList.AddRange(_defaultSelectedHeroList);
        _m_maxLimit = _maxLimit;
        setItemCount(_m_allHeroInfo.Count);
        onSelectedCountChg?.Invoke(_m_selectedHeroList.Count,_m_maxLimit);
    }

    private void _onClickSelected(GGUIWndWeekCardSelectedHeroGridItem itemWnd)
    {
        if(null == itemWnd || null == itemWnd.info)
            return;
        if (_m_selectedHeroList.Contains(itemWnd.info.id))
        {
            _m_selectedHeroList.Remove(itemWnd.info.id);
            itemWnd.setSelected(false);
        }
        else
        {
            if (_m_selectedHeroList.Count >= _m_maxLimit)
            {
                //选中数量已经达到上限
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.week_card_selected_hero_max));
                return;
            }

            _m_selectedHeroList.Add(itemWnd.info.id);
            itemWnd.setSelected(true);
        }
        onSelectedCountChg?.Invoke(_m_selectedHeroList.Count,_m_maxLimit);
    }

    /// <summary>
    /// 获取选中的列表
    /// </summary>
    /// <param name="_list"></param>
    public void getSelectedList(List<long> _list)
    {
        if(null == _list)
            return;
        _list.AddRange(_m_selectedHeroList);
    }
}
