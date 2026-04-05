using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;

/// <summary>
/// 规则列表弹窗
/// </summary>
///
public class NPGGUIWndRuleList : _ATALBasicUIWnd<NPGGUIMonoRuleList>
{
    private static NPGGUIWndRuleList _m_instance;

    public static NPGGUIWndRuleList instance
    {
        get
        {
            if(null == _m_instance)
                _m_instance = new NPGGUIWndRuleList();
            return _m_instance;
        }
    }
    private NPGGUIWndRuleListItemGrid _m_itemGrid;

    public NPGGUIWndRuleList() : base(EALUIWndLayer.ADDITION)
    {
    }

    protected override string _monoAssetPath { get => NPGGUIMonoRuleList.assetPath; }
    protected override string _monoObjName { get => NPGGUIMonoRuleList.objName; }
    protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
    

    protected override void _onShowWnd()
    {
        _refreshWnd();
    }

    private void _refreshWnd()
    {
        _m_itemGrid?.showItemList(GRefdataCoreMgr.instance.ruleList.refList);
        _m_itemGrid?.showWnd();
    }

    protected override void _onHideWnd()
    {
        
    }

    protected override void _onReset()
    {
        
    }

    protected override void _onDiscard()
    {
        _m_itemGrid?.discard();
        _m_itemGrid = null;
    }

    protected override void _onWndInitDone()
    {
        if (null == wnd)
            return;
        ALUGUICommon.combineBtnClick(wnd.closeBtn, _clickClose);

        if (null != wnd.itemGrid)
        {
            _m_itemGrid = new NPGGUIWndRuleListItemGrid(wnd.itemGrid);
        }
    }

    private void _clickClose(GameObject _obj)
    {
        QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ADD_RULE_LIST);
    }
}
