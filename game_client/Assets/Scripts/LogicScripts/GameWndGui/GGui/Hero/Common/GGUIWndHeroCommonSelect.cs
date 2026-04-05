using System;
using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using GOE;
using NPEnum;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class GGUIWndHeroCommonSelect : _ANPGGUIBasicSubWnd<GGUIMonoHeroCommonSelect>
{
    private readonly Action<_IHeroCardShow> _m_onHeroSelect;
    
    private GGUIWndHeroCommonSelectItemGrid _m_wHeroSelectGrid;//大臣选择Grid
    
    private GGUIWndHeroMainFilter _m_wHeroListFilter;    //筛选子窗口
    
    private ESpecAttrType _m_filterType = ESpecAttrType.NONE;
    
    //伙伴数据列表
    private List<_IHeroCardShow> _m_heroShowList;
    //伙伴筛选后数据列表
    private List<_IHeroCardShow> _m_heroFilterShowList;
    // 当前选中的大臣列表
    private List<_IHeroCardShow> _m_curSelectHeroList;
    
    public GGUIWndHeroCommonSelect(GGUIMonoHeroCommonSelect _mono, Action<_IHeroCardShow> _onHeroSelect) : base(_mono)
    {
        _m_onHeroSelect = _onHeroSelect;
        initWnd();
    }

    protected override void _onShowWnd()
    {
        _m_wHeroSelectGrid?.showWnd();
        if (_m_wHeroListFilter != null)
        {
            _m_wHeroListFilter.showWnd();
            _m_wHeroListFilter.initState(ESpecAttrType.NONE);
        }
        _refreshWnd();
    }

    protected override void _onHideWnd()
    {
        _m_wHeroSelectGrid?.hideWnd();
        _m_wHeroListFilter?.hideWnd();
    }

    protected override void _onReset()
    {
        _m_wHeroSelectGrid?.resetWnd();
        
        _m_wHeroListFilter?.resetWnd();
    }

    protected override void _onDiscard()
    {
        _m_wHeroSelectGrid?.discard();
        _m_wHeroSelectGrid = null;
        
        _m_wHeroListFilter?.discard();
        _m_wHeroListFilter = null;
    }

    protected override void _onWndInitDone()
    {
        if(null == wnd)
            return;
        if (wnd.monoHeroSelectGrid != null)
            _m_wHeroSelectGrid = new GGUIWndHeroCommonSelectItemGrid(wnd.monoHeroSelectGrid, _onHeroSelect);

        if (wnd.monoHeroListFilter != null)
        {
            _m_wHeroListFilter = new GGUIWndHeroMainFilter(wnd.monoHeroListFilter);
            _m_wHeroListFilter.initState(_m_filterType);
            _m_wHeroListFilter.onFilterChanged += _onFilterChg;
        }
    }

    public void refreshWnd(List<_IHeroCardShow> _heroInfos, List<_IHeroCardShow> _selectedHeroInfos, ESpecAttrType _filterType)
    {
        _m_heroShowList = _heroInfos;
        _m_curSelectHeroList = _selectedHeroInfos;
        _m_filterType = _filterType;
        _refreshWnd();
    }

    /// <summary>
    /// 刷新界面
    /// </summary>
    private void _refreshWnd()
    {
        if(null == wnd)
            return;
        _dealFilterHeroList();
        _refreshGrid();
    }
    //刷新列表
    private void _refreshGrid()
    {
        if (wnd == null)
            return;

        if (_m_wHeroSelectGrid != null)
        {
            _m_wHeroSelectGrid.showWnd();
            _m_wHeroSelectGrid.refreshWnd(_m_heroFilterShowList, _m_curSelectHeroList);
        }
    }
    /// <summary>
    /// 处理筛选列表
    /// </summary>
    private void _dealFilterHeroList()
    {
        if (_m_heroShowList == null)
            return;

        if (_m_heroFilterShowList == null)
            _m_heroFilterShowList = new List<_IHeroCardShow>();
        _m_heroFilterShowList.Clear();

        //全部
        if(_m_filterType == ESpecAttrType.NONE)
            _m_heroFilterShowList.AddRange(_m_heroShowList);
        else
        {
            //指定相性
            for (int i = 0; i < _m_heroShowList.Count; i++)
            {
                if (_m_heroShowList[i] != null && 
                    _m_heroShowList[i].heroRefObj != null &&
                    _m_heroShowList[i].heroRefObj.spec_attr_type == _m_filterType)
                    _m_heroFilterShowList.Add(_m_heroShowList[i]);
            }
        }
    }
    
    private void _onHeroSelect(_IHeroCardShow _heroInfo)
    {
        _m_onHeroSelect?.Invoke(_heroInfo);
    }
    
    //筛选变更
    private void _onFilterChg(ESpecAttrType _type)
    {
        //保存选择
        _m_filterType = _type;

        _refreshWnd();
    }
}