using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using NPCommon;
using NPEnum;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class GGUIWndChapterEventReward : _ATALBasicUIWnd<GGUIMonoChapterEventReward>
{
    private static GGUIWndChapterEventReward _g_instance = new GGUIWndChapterEventReward();

    public static GGUIWndChapterEventReward instance
    {
        get
        {
            if (null == _g_instance)
                _g_instance = new GGUIWndChapterEventReward();
            return _g_instance;
        }
    }
    
    private GGUIWndCommonRewardContainer _m_itemContainer;
    private ChapterEventRewardRefObj _m_refObj;
    private List<NPCommonCostItem> _m_itemList;
    private List<NPCommon_ItemInfo> _m_itemInfoList;

    public GGUIWndChapterEventReward() : base(EALUIWndLayer.ADDITION)
    {
    }

    protected override string _monoAssetPath { get => GGUIMonoChapterEventReward.assetPath; }
    protected override string _monoObjName { get => GGUIMonoChapterEventReward.objName; }
    protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

    protected override void _onShowWnd()
    {
        _refreshWnd();
    }

    protected override void _onHideWnd()
    {
    }

    protected override void _onReset()
    {
        
    }

    protected override void _onDiscard()
    {
        // 奖励列表
        _m_itemContainer?.discard();
        _m_itemContainer = null;
        _m_itemInfoList = null;
        _m_itemList = null;
    }

    protected override void _onWndInitDone()
    {
        if(null == wnd)
            return;
        
        ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        ALUGUICommon.combineBtnClick(wnd.btnConfirm, _onBtnConfirmClick);
            
        // 奖励列表
        if (null != wnd.itemContainer)
            _m_itemContainer = new GGUIWndCommonRewardContainer(wnd.itemContainer);
    }
    
    public void refreshWnd(ChapterEventRewardRefObj _chapterEventRewardRef, List<NPCommon.NPCommon_ItemInfo> _itemInfos)
    {
        _m_refObj = _chapterEventRewardRef;

        _m_itemInfoList = _itemInfos;
        // 奖励列表
        _m_itemList = new List<NPCommonCostItem>();
        foreach (NPCommon_ItemInfo itemInfo in _itemInfos)
        {
            _m_itemList.Add(new NPCommonCostItem(itemInfo));
        }
        _refreshWnd();
    }
    /// <summary>
    /// 刷新界面
    /// </summary>
    private void _refreshWnd()
    {
        if(null == wnd)
            return;
        ALUGUICommon.setLabelTxt(wnd.txtTitle, TextTranslate.instance.getLanguage(_m_refObj.event_title));
        ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(_m_refObj.event_desc));
        
        _m_itemContainer?.showWnd();
        _m_itemContainer?.setRewardList(_m_itemList);
    }

    
    /// <summary>
    /// 点击关闭按钮
    /// </summary>
    /// <param name="_"></param>
    private void _onBtnCloseClick(GameObject _)
    {
        QueueMgr.instance.DoUIRollBackByEsc();
    }
    private void _onBtnConfirmClick(GameObject _)
    {
        //展示粒子效果
        if (wnd != null) 
            GCommon.showItemParticle(_m_itemInfoList, wnd.particleStartRectTransform);
        QueueMgr.instance.DoUIRollBackByEsc();
    }
}