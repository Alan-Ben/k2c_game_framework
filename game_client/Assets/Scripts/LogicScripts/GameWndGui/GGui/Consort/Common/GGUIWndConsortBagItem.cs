using System;
using ALPackage;
using GOE;
using NPEnum;
using UnityEngine;

/// <summary>
/// 情人物品使用item
/// </summary>
public class GGUIWndConsortBagItem:_ATNPGGUIWndSingleChoiceItem<GGUIMonoConsortBagItem,GGUIWndConsortBagItem>
{
    private NPGGUIWndCommonItem _m_item;//物品展示
    private NPGGuiWndTexture _m_gainIcon;
    private NPGGUIWndCommonRedTip _m_wRedTip;
    
    private BagItem _m_bagItem;//物品信息
    private BagItemRefObj _m_bagItemRefObj;//物品配置
    
    private int _m_index;//item下标
    private Func<BagItemRefObj, bool> _m_checkRedTipFunc;//红点检查函数

    public GGUIWndConsortBagItem(GGUIMonoConsortBagItem _wnd) : base(_wnd)
    {
        initWnd();
    }

    /// <summary>
    /// 情人的配置
    /// </summary>
    public BagItem bagItem { get => _m_bagItem; }
    
    public BagItemRefObj bagItemRefObj { get => _m_bagItemRefObj; }

    public int index { get => _m_index; }

    protected override void _onShowWndEx()
    {
    }

    protected override void _onHideWndEx()
    {
        _m_wRedTip?.hideWnd();
    }

    protected override void _onResetEx()
    {
        _m_gainIcon?.discardTexture();
        _m_wRedTip?.resetWnd();
    }

    protected override void _onDiscardEx()
    {
        _m_item?.discard();
        _m_item = null;
        
        _m_gainIcon?.discard();
        _m_gainIcon = null;
        
        _m_wRedTip?.discard();
        _m_wRedTip = null;

        _m_checkRedTipFunc = null;
    }

    protected override void _onWndInitDoneEx()
    {
        if (null == wnd)
            return;
        if (null != wnd.item)
        {
            _m_item = new NPGGUIWndCommonItem(wnd.item);
        }

        if (null != wnd.texGainIcon)
        {
            _m_gainIcon = new NPGGuiWndTexture(wnd.texGainIcon);
        }
        
        if(wnd.monoRedTip != null)
            _m_wRedTip = new NPGGUIWndCommonRedTip(wnd.monoRedTip);
    }

    /// <summary>
    /// 显示item
    /// </summary>
    /// <param name="_heroRef"></param>
    public void setInfo(BagItem _heroRef, int _index, Func<BagItemRefObj, bool> _checkRedTipFunc = null)
    {
        _m_bagItem = _heroRef;
        _m_bagItemRefObj = _m_bagItem?.itemRefObj;
        
        _m_index = _index;
        _m_checkRedTipFunc = _checkRedTipFunc;
        _refreshWnd();
    }
    
    public void setInfo(BagItemRefObj _bagItemRefObj, int _index, Func<BagItemRefObj, bool> _checkRedTipFunc = null)
    {
        _m_bagItemRefObj = _bagItemRefObj;
        if(_m_bagItemRefObj != null)
            _m_bagItem = NPPlayer.instance.bagComp.getItem(_m_bagItemRefObj.id);
        
        _m_index = _index;
        _m_checkRedTipFunc = _checkRedTipFunc;
        _refreshWnd();
    }

    /// <summary>
    /// 外部调用刷新显示
    /// </summary>
    public void refreshWnd()
    {
        _refreshWnd();
    }

    /// <summary>
    /// 刷新显示信息
    /// </summary>
    private void _refreshWnd()
    {
        if(null == wnd)
            return;
        
        _m_item?.showWnd();
        if (_m_bagItem != null)
        {
            _m_item?.setItem(new NPCommonCostItem(_m_bagItem.itemType, _m_bagItem.itemId, _m_bagItem.count));
        }
        else
        {
            _m_item?.setItem(new NPCommonCostItem(ENPItemType.BAG_ITEM, _m_bagItemRefObj?.id ?? 0, 0));
        }

        BagItemConsortRefObj bagItemConsortRefObj = GRefdataCoreMgr.instance.bagItemConsortCore.getRef(_m_bagItemRefObj?.id ?? 0);
        if (null != bagItemConsortRefObj)
        {
            if (null != _m_gainIcon)
            {
                _m_gainIcon.showWnd();
                _m_gainIcon.setTexture(bagItemConsortRefObj.gain_item_icon);
            }

            ALUGUICommon.setLabelTxt(wnd.txtGainCount, TextTranslate.instance.getLanguage(bagItemConsortRefObj.consort_detail_give_desc, bagItemConsortRefObj.consort_detail_give_desc_args));
        }

        if (_m_bagItem == null || _m_bagItem.count == 0)//0置灰
        {
            GGameCommonInfo.grayImage(wnd.goEmptyGray);
            ALUGUICommon.setGameObjEnable(wnd.goEmptyShow,true);
        }
        else
        {
            GGameCommonInfo.disgrayImage(wnd.goEmptyGray);
            ALUGUICommon.setGameObjEnable(wnd.goEmptyShow,false);
        }

        refreshRedTipShow();
    }

    public void refreshRedTipShow()
    {
        if(_m_wRedTip == null || _m_checkRedTipFunc == null)
           return;
        
        bool needShowRedTip = _m_checkRedTipFunc.Invoke(_m_bagItemRefObj);
        if (needShowRedTip)
        {
            _m_wRedTip.showWnd();
            _m_wRedTip.showRedTipNum(1);
        }
        else
        {
            _m_wRedTip.hideWnd();
        }
    }
    
    public void showItemDetail(GameObject _go)
    {
        if(_m_item == null)
            return;
        
        _m_item.showWnd();
        _m_item.showItemDetail(_go);
    }
}