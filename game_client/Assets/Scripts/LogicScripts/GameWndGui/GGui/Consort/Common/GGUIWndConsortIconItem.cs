using System;
using ALPackage;
using GOE;
using NPEnum;
using UnityEngine;

/// <summary>
/// 情人item
/// </summary>
public class GGUIWndConsortIconItem:_ATNPGGUIWndSingleChoiceItem<GGUIMonoConsortIconItem,GGUIWndConsortIconItem>
{
    private NPGGuiWndTexture _m_texConsortIcon;//情人图标
    private GGuiWndSprite _m_wConsortIconBg;//情人图标背景
    private _IConsortShowInfo _m_consortShowData;//情人配置信息
    private int _m_itemIdx;

    public GGUIWndConsortIconItem(GGUIMonoConsortIconItem _wnd) : base(_wnd)
    {
        initWnd();
    }

    /// <summary>
    /// 妃子展示信息
    /// </summary>
    public _IConsortShowInfo consortShowInfo { get => _m_consortShowData; }

    /// <summary>
    /// 情人的配置
    /// </summary>
    public long consortId { get => _m_consortShowData?.consortId ?? 0; }
    public int itemIdx { get => _m_itemIdx; }

    protected override void _onShowWndEx()
    {
    }

    protected override void _onHideWndEx()
    {
        _m_texConsortIcon?.hideWnd();
        _m_wConsortIconBg?.hideWnd();
    }

    protected override void _onResetEx()
    {
        _m_texConsortIcon?.discardTexture();
        _m_wConsortIconBg?.discardTexture();

        setScale(1f);
    }

    protected override void _onDiscardEx()
    {
        setScale(1f);
        
        _m_texConsortIcon?.discard();
        _m_texConsortIcon = null;
        
        _m_wConsortIconBg?.discard();
        _m_wConsortIconBg = null;
    }

    protected override void _onWndInitDoneEx()
    {
        if (null == wnd)
            return;
        if (null != wnd.texConsortIcon)
        {
            _m_texConsortIcon = new NPGGuiWndTexture(wnd.texConsortIcon);
        }

        if (wnd.imgConsortIconBg != null)
            _m_wConsortIconBg = new GGuiWndSprite(wnd.imgConsortIconBg);
    }

    /// <summary>
    /// 显示情人item
    /// </summary>
    /// <param name="_consortRef"></param>
    public void setInfo(_IConsortShowInfo _consortRef, int _itemIdx)
    {
        _m_itemIdx = _itemIdx;
        _m_consortShowData = _consortRef;
        _refreshWnd();
    }

    /// <summary>
    /// 刷新显示信息
    /// </summary>
    private void _refreshWnd()
    {
        if(null == wnd || null == _m_consortShowData)
            return;
        ALUGUICommon.setLabelTxt(wnd.txtName, _m_consortShowData.consortTransName);
        _m_texConsortIcon?.showWnd();
        _m_texConsortIcon?.setTexture(_m_consortShowData.consortSkinShowInfo?.consortHeadIcon);

        if (_m_wConsortIconBg != null)
        {
            _m_wConsortIconBg.showWnd();
            _m_wConsortIconBg.setTexture(GCommon.getQualityExtRefObj(ENPItemType.CONSORT, _m_consortShowData.consortId)?.consort_head_bg);
        }
    }

    /// <summary>
    /// 设置窗口缩放值
    /// </summary>
    /// <param name="_wndScale"></param>
    public void setScale(float _wndScale)
    {
        if (null == wnd)
            return;
        if(null != wnd.scaleGo)
            wnd.scaleGo.localScale = new Vector3(_wndScale, _wndScale, _wndScale);
    }
}