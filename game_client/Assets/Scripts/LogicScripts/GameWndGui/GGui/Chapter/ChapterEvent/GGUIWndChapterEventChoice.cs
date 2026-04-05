using System;
using ALPackage;
using GOE;
using NPEnum;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class GGUIWndChapterEventChoice : _ATALBasicUIWnd<GGUIMonoChapterEventChoice>
{
    private static GGUIWndChapterEventChoice _g_instance = new GGUIWndChapterEventChoice();

    public static GGUIWndChapterEventChoice instance
    {
        get
        {
            if (null == _g_instance)
                _g_instance = new GGUIWndChapterEventChoice();
            return _g_instance;
        }
    }
    private GGUIWndChapterEventChoiceItemContainer _m_choiceContainer;
    private ChapterEventChoiceRefObj _m_refObj;
    private Action<int> _m_onSelectChoice;
    
    public GGUIWndChapterEventChoice() : base(EALUIWndLayer.ADDITION)
    {
    }

    protected override string _monoAssetPath { get => GGUIMonoChapterEventChoice.assetPath; }
    protected override string _monoObjName { get => GGUIMonoChapterEventChoice.objName; }
    protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

    protected override void _onShowWnd()
    {
        _m_choiceContainer?.showWnd();
        
        refreshWnd();
    }
    protected override void _onHideWnd()
    {
        _m_choiceContainer?.hideWnd();
    }
    protected override void _onReset()
    {
        _m_choiceContainer?.resetWnd();
    }
    protected override void _onDiscard()
    {
        _m_choiceContainer?.discard();
        _m_choiceContainer = null;

    }
    protected override void _onWndInitDone()
    {
        if (wnd == null)
            return;
        
        if (wnd.monoChoiceContainer != null)
            _m_choiceContainer = new GGUIWndChapterEventChoiceItemContainer(wnd.monoChoiceContainer, _onItemSelect);
    }


    public void refreshWnd(ChapterEventChoiceRefObj _refObj, Action<int> _selectChoice)
    {
        _m_refObj = _refObj;
        _m_onSelectChoice = _selectChoice;
        refreshWnd();
    }
    
    public void refreshWnd()
    {
        if (wnd == null || !_m_bIsShow || _m_refObj == null)
            return;

        ALUGUICommon.setLabelTxt(wnd.txtTitle, TextTranslate.instance.getLanguage(_m_refObj.event_title));
        ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(_m_refObj.event_desc));
        _m_choiceContainer?.refreshWnd(_m_refObj.option_ref_list);
    }
    
    
    private void _onItemSelect(int _index)
    {
        if (_m_refObj == null)
            return;
        
        _m_onSelectChoice?.Invoke(_index);
    }
}