using System;
using ALPackage;
using GOE;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

/// <summary>
/// 情人item
/// </summary>
public class GGUIWndConsortCardItem : _ATNPGGUIWndSingleChoiceItem<GGUIMonoConsortCardItem, GGUIWndConsortCardItem>
{
    [NotNull] private _IConsortShowInfo _m_consortCardShowData;//情人信息

    private NPGGuiWndTexture _m_wNameBg;//妃子名称背景
    private NPGGuiWndTexture _m_texConsortIcon;//情人图标
    private GGUIWndConsortCardBg _m_wConsortCardBg;//卡片品质背景
    private GGUIWndConsortQualityShow _m_wConsortQualityShow;//妃子品质展示
    
    private Action<_IConsortShowInfo> _m_clickDelegate;//点击卡牌的回调

    public GGUIWndConsortCardItem(GGUIMonoConsortCardItem _wnd,  Action<_IConsortShowInfo> _clickDelegate) : base(_wnd)
    {
        _m_clickDelegate = _clickDelegate;
        initWnd();
    }

    /// <summary>
    /// 情人的配置
    /// </summary>
    [NotNull] public _IConsortShowInfo consortCardInfo { get => _m_consortCardShowData; }

    protected override void _onShowWndEx()
    {
        _refreshWnd();
    }

    protected override void _onHideWndEx()
    {
        _m_wConsortQualityShow?.hideWnd();
    }

    protected override void _onResetEx()
    {
        _m_wNameBg?.discardTexture();
        _m_texConsortIcon?.discardTexture();
        _m_wConsortCardBg?.resetWnd();
        _m_wConsortQualityShow?.resetWnd();
    }

    protected override void _onDiscardEx()
    {
        _m_wNameBg?.discard();
        _m_wNameBg = null;
        
        _m_texConsortIcon?.discard();
        _m_texConsortIcon = null;

        _m_wConsortCardBg?.discard();
        _m_wConsortCardBg = null;
        
        _m_wConsortQualityShow?.discard();
        _m_wConsortQualityShow = null;
        
        _m_clickDelegate = null;
    }

    protected override void _onWndInitDoneEx()
    {
        if (null == wnd)
            return;

        if (wnd.imgConsortNameBg != null)
        {
            _m_wNameBg = new NPGGuiWndTexture(wnd.imgConsortNameBg);
        }
        
        if (null != wnd.texConsortIcon)
        {
            _m_texConsortIcon = new NPGGuiWndTexture(wnd.texConsortIcon);
        }
        
        if(wnd.consortCardBg != null)
        {
            _m_wConsortCardBg = new GGUIWndConsortCardBg(wnd.consortCardBg);
        }
        
        // 初始化妃子品质展示组件
        if (wnd.monoConsortQualityShow != null)
        {
            _m_wConsortQualityShow = new GGUIWndConsortQualityShow(wnd.monoConsortQualityShow);
        }
    }

    /// <summary>
    /// 显示情人item
    /// </summary>
    /// <param name="_consortCardShowInfo"></param>
    public void setInfo([NotNull] _IConsortShowInfo _consortCardShowInfo)
    {
        _m_consortCardShowData = _consortCardShowInfo;
        _refreshWnd();
    }

    /// <summary>
    /// 刷新显示信息
    /// </summary>
    private void _refreshWnd()
    {
        if(null == wnd || _m_consortCardShowData == null)
            return;

        ALUGUICommon.setLabelTxt(wnd.txtConsortName,  _m_consortCardShowData.consortTransName);
        if (_m_wNameBg != null)
        {
            _m_wNameBg.showWnd();
            _m_wNameBg.setTexture(_m_consortCardShowData.consortNameBg);
        }
        
        if (_m_texConsortIcon != null)
        {
            _m_texConsortIcon.showWnd();
            _m_texConsortIcon.setTexture(_m_consortCardShowData.consortSkinShowInfo?.consortCardImage);    
        }

        if (_m_wConsortCardBg != null)
        {
            _m_wConsortCardBg.showWnd();
            _m_wConsortCardBg.setQuality(_m_consortCardShowData.consortQuality);
        }
        
        // 显示妃子品质
        if (_m_wConsortQualityShow != null)
        {
            _m_wConsortQualityShow.showWnd();
            _m_wConsortQualityShow.setQuality(_m_consortCardShowData.consortQuality);
        }

        ALUGUICommon.setLabelTxt(wnd.txtConsortTitle, _m_consortCardShowData.consortTransTitle);
        
        if(string.IsNullOrEmpty(wnd.charmTransKey))
            ALUGUICommon.setLabelTxt(wnd.txtCharm, _m_consortCardShowData.charm);
        else 
            ALUGUICommon.setLabelTxt(wnd.txtCharm, TextTranslate.instance.getLanguage(wnd.charmTransKey, _m_consortCardShowData.charm));
        
        if(string.IsNullOrEmpty(wnd.intimacyTransKey))
            ALUGUICommon.setLabelTxt(wnd.txtIntimacy, _m_consortCardShowData.intimacy);
        else 
            ALUGUICommon.setLabelTxt(wnd.txtIntimacy, TextTranslate.instance.getLanguage(wnd.intimacyTransKey, _m_consortCardShowData.intimacy));
        
        ALUGUICommon.setLabelTxt(wnd.txtConsortSource, GCommon.getItemSource(ENPItemType.CONSORT, _m_consortCardShowData.consortId));

        EGameCommonUnlockType unlockType = _m_consortCardShowData.unlockType;
        NPCommonEnumStatInfo<EGameCommonUnlockType>.setStat(wnd.statInfos,unlockType);
        if (unlockType == EGameCommonUnlockType.LOCK)
        {
            if (wnd.needSetIconColor)
                ALUGUICommon.setUIObjColor(wnd.texConsortIcon, wnd.lockIconColor);
            
            if(wnd.needSetNameColor)
                ALUGUICommon.setUIObjColor(wnd.txtConsortName, wnd.lockNameColor);
        }
        else
        {
            if (wnd.needSetIconColor)
                ALUGUICommon.setUIObjColor(wnd.texConsortIcon, wnd.unLockIconColor);
            
            if(wnd.needSetNameColor)
                ALUGUICommon.setUIObjColor(wnd.txtConsortName, wnd.unLockNameColor);
        }
    }

    /// <summary>
    /// 强制触发点击
    /// </summary>
    public void forceClick()
    {
        _onClickBtnSelect(null);
    }

    protected override void _onClickBtnSelect(GameObject _go)
    {
        base._onClickBtnSelect(_go);
        
        _m_clickDelegate?.Invoke(_m_consortCardShowData);
    }

    /// <summary>
    /// 刷新显示
    /// </summary>
    public void refreshWnd()
    {
        _refreshWnd();
    }
}