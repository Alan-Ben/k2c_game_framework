// using System.Collections.Generic;

using System;
using ALPackage;
using Common.NpChatObj;
using GOE;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 妃子分享详情界面
/// </summary>
public class GGUIWndShareConsortDetail : _ANPGGUIBasicWnd<GGUIMonoShareConsortDetail>
{
    private static GGUIWndShareConsortDetail _g_instance;

    public static GGUIWndShareConsortDetail instance
    {
        get
        {
            if (_g_instance == null)
                _g_instance = new GGUIWndShareConsortDetail();
            return _g_instance;
        }
    }
    
    private NPCommon_ChatContent_ConsortShare _m_showData;
    private GConsortRefObj _m_rConsortRefObj;

    private GGUISubWndQualityShowGo _m_wQualityShowGo;
    private NPGGUIWndCommonShowCase _m_wConsortShowCase;
    private GGUISubWndConsortFetterInfo _m_wFetterInfo; 
    
    public GGUIWndShareConsortDetail() : base(EALUIWndLayer.NORMAL)
    {
    }

    protected override string _monoAssetPath { get => GGUIMonoShareConsortDetail.assetPath; }
    protected override string _monoObjName { get => GGUIMonoShareConsortDetail.objName; }
    protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

    protected override void _onShowWnd()
    {
    }

    protected override void _onHideWnd()
    {
        _m_wQualityShowGo?.hideWnd();
        _m_wConsortShowCase?.hideWnd();
        _m_wFetterInfo?.hideWnd();
    }

    protected override void _onReset()
    {
        _m_wQualityShowGo?.resetWnd();
        _m_wConsortShowCase?.resetWnd();
        _m_wFetterInfo?.resetWnd();
    }

    protected override void _onDiscard()
    {
        _m_wQualityShowGo?.discard();
        _m_wQualityShowGo = null;
        
        _m_wConsortShowCase?.resetWnd();
        _m_wConsortShowCase = null;
        
        _m_wFetterInfo?.resetWnd();
        _m_wFetterInfo = null;
        
        if (wnd != null)
        {
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _clickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnShowUI, _clickBtnShowUI);
            ALUGUICommon.uncombineBtnClick(wnd.btnHideUI, _clickBtnHideUI);
            ALUGUICommon.uncombineBtnClick(wnd.btnConsortProfile, _onConsortProfileBtnClick);
        }
    }

    protected override void _onWndInitDone()
    {
        if(null == wnd)
            return;

        if (wnd.monoQualityShowGo != null)
            _m_wQualityShowGo = new GGUISubWndQualityShowGo(wnd.monoQualityShowGo);

        if (wnd.monoConsortShowCase != null)
            _m_wConsortShowCase = new NPGGUIWndCommonShowCase(wnd.monoConsortShowCase);

        if (wnd.monoFetterInfo != null)
            _m_wFetterInfo = new GGUISubWndConsortFetterInfo(wnd.monoFetterInfo);
        
        ALUGUICommon.combineBtnClick(wnd.btnClose, _clickClose);
        ALUGUICommon.combineBtnClick(wnd.btnShowUI, _clickBtnShowUI);
        ALUGUICommon.combineBtnClick(wnd.btnHideUI, _clickBtnHideUI);
        ALUGUICommon.combineBtnClick(wnd.btnConsortProfile, _onConsortProfileBtnClick);
    }

    private void _clickClose(GameObject obj)
    {
        QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_Main_Chat_SHARE_CONSORT_DETAIL);
    }

    #region 显隐形象

    /// <summary>
    /// 点击显示UI
    /// </summary>
    /// <param name="_obj"></param>
    private void _clickBtnHideUI(GameObject _obj)
    {
        if (null == wnd)
            return;
        _playUIAni(wnd.uiStatAni, wnd.hideUIAniName);
    }

    /// <summary>
    /// 点击隐藏UI
    /// </summary>
    /// <param name="_obj"></param>
    private void _clickBtnShowUI(GameObject _obj)
    {
        if (null == wnd)
            return;
        _playUIAni(wnd.uiStatAni, wnd.showUIAniName);
    }


    /// <summary>
    /// 播放动画
    /// </summary>
    /// <param name="_ani"></param>
    /// <param name="_aniName"></param>
    /// <param name="_dealDone"></param>
    private void _playUIAni(Animation _ani, string _aniName, Action _dealDone = null)
    {
        if (null == _ani)
        {
            _dealDone?.Invoke();
            return;
        }

        _ani.Play(_aniName, _dealDone);
    }

    #endregion

    /// <summary>
    /// 设置显示信息
    /// </summary>
    /// <param name="_showData"></param>
    public void setInfo(NPCommon_ChatContent_ConsortShare _showData)
    {
        _m_showData = _showData;
        _m_rConsortRefObj = _m_showData == null ? null : GRefdataCoreMgr.instance.consortRefCore.getRef(_m_showData.getConsortId());
        
        _refreshWnd();
    }

    private void _refreshWnd()
    {
        if(null == wnd || _m_showData == null || _m_rConsortRefObj == null)
            return;

        ALUGUICommon.setLabelTxt(wnd.txtConsortName, GCommon.getItemName(ENPItemType.CONSORT, _m_rConsortRefObj.id));

        if (_m_wQualityShowGo != null)
        {
            _m_wQualityShowGo.showWnd();
            _m_wQualityShowGo.setData(ENPItemType.CONSORT, _m_rConsortRefObj.id);
        }

        if (_m_wConsortShowCase != null)
        {
            GConsortSkinRefObj skinRefObj = GRefdataCoreMgr.instance.consortSkinRefCore.getRef(_m_showData.getSkinId());
            int maxShowCaseUnitCount = Mathf.Max(wnd.consortActorInShowCaseIndex, wnd.bgInShowCaseIndex) + 1;
            if (skinRefObj != null && maxShowCaseUnitCount > 0)
            {
                _AShowCaseUnitInfoObj[] showCaseUnitInfoObjList = new _AShowCaseUnitInfoObj[maxShowCaseUnitCount];
                
                if(wnd.consortActorInShowCaseIndex >= 0)
                    showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(skinRefObj.td_show), wnd.consortActorInShowCaseIndex);
                
                if(wnd.bgInShowCaseIndex >= 0)
                    showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(skinRefObj.td_bg_index), wnd.bgInShowCaseIndex);
                
                _m_wConsortShowCase.showWnd(showCaseUnitInfoObjList);
            }
            else
            {
                _m_wConsortShowCase.hideWnd();
            }
        }

        if (_m_wFetterInfo != null)
        {
            ConsortFetterInfo consortFetterInfo = new ConsortFetterInfo(_m_showData.getConsortFetters()?.getLvl() ?? 0, _m_rConsortRefObj.consort_fetters_skill_id);
            _m_wFetterInfo.showWnd();
            _m_wFetterInfo.setData(consortFetterInfo);
        }
        
        ALUGUICommon.setLabelTxt(wnd.txtConsortCharm, _m_showData.getCharm().ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
        ALUGUICommon.setLabelTxt(wnd.txtConsortIntimacy, _m_showData.getIntimacy().ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
    }
    
    /// <summary>
    /// 妃子简介按钮点击
    /// </summary>
    /// <param name="_go"></param>
    private void _onConsortProfileBtnClick(GameObject _go)
    {
        if(_m_rConsortRefObj == null)
            return;
        
        QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndUnlockConsortDetailProfile.instance, () =>
        {
            GGUIWndUnlockConsortDetailProfile.instance.showWnd();
            GGUIWndUnlockConsortDetailProfile.instance.setData(_m_rConsortRefObj);
        }, EUIQueueStageType.MAIN, UINodeTagConst.C_UNLOCK_CONSORT_DETAIL_PROFILE_WND, false, false);
    }
}