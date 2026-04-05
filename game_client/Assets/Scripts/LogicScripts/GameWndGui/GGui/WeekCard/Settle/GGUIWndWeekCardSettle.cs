using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using ALPackage;
using Common.WeekCardObj;
using CommonEnum;
using GOE;
using NPCommon;
using NPEnum;
using UnityEngine.UI;


/// <summary>
/// 周卡结算信息
/// </summary>
///
public class GGUIWndWeekCardSettle: _ANPGGUIBasicWnd<GGUIMonoWeekCardSettle>
{
    private static GGUIWndWeekCardSettle _g_instance = new GGUIWndWeekCardSettle();
    public static GGUIWndWeekCardSettle instance
    {
        get
        {
            if(null == _g_instance)
                _g_instance = new  GGUIWndWeekCardSettle();
            return _g_instance;
        }
    }
    
    private WeekCard_SettleInfo _m_info;
    private NPGGUIWndCommonItemContainer _m_itemContainer;
    private NPGGUIWndCommonShowCase _m_assignShowcase;
    private long _m_offLineTimeMs;
    public event Action onCloseWnd;


    public GGUIWndWeekCardSettle() : base(EALUIWndLayer.ADDITION)
    {
    }

    protected override string _monoAssetPath { get => GGUIMonoWeekCardSettle.assetPath; }
    protected override string _monoObjName { get => GGUIMonoWeekCardSettle.objName; }
    protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

    protected override void _onShowWnd()
    {
    }

    protected override void _onHideWnd()
    {
        _m_assignShowcase?.hideWnd();
    }

    protected override void _onReset()
    {
        
    }

    protected override void _onDiscard()
    {
        _m_itemContainer?.discard();
        _m_itemContainer = null;
            
        _m_assignShowcase?.discard();
        _m_assignShowcase = null;

        onCloseWnd = null;
    }

    protected override void _onWndInitDone()
    {
        if(null == wnd)
            return;
        ALUGUICommon.combineBtnClick(wnd.btnClose, _clickClose);
        if (null != wnd.itemContainer)
        {
            _m_itemContainer = new NPGGUIWndCommonItemContainer(wnd.itemContainer);
        }

        if (null != wnd.assignShowcase)
        {
            _m_assignShowcase = new NPGGUIWndCommonShowCase(wnd.assignShowcase);
        }
    }

    private void _clickClose(GameObject obj)
    {
        onCloseWnd?.Invoke();
    }

    public void setInfo(WeekCard_SettleInfo _info, long _offLineTimeMs)
    {
        _m_info = _info;
        _m_offLineTimeMs = _offLineTimeMs;
        _refreshWnd();
    }

    private void _refreshShowCaseWnd()
    {
        NPGGoIndex goIndex = NPPlayer.instance.weekCardComp.getTDShowIndex();
            
        _m_assignShowcase?.showWnd(new ShowCaseCommonResUnitInfoObj(goIndex));
    }


    /// <summary>
    /// 刷新显示
    /// </summary>
    private void _refreshWnd()
    {
        if(null == wnd)
            return;
        List<CommonItemData> itemDataList = new List<CommonItemData>();
        foreach (NPCommon_ItemInfo itemInfo in _m_info.getItemList())
        {
            if(itemInfo.getItemType() == (int)ENPItemType.LAZY_CD || itemInfo.getItemType() == (int)ENPItemType.FIXED_CD)
                continue;
            itemDataList.Add(new CommonItemData(itemInfo));
        }
        _m_itemContainer?.showWnd();
        _m_itemContainer?.showItemList(itemDataList);

        List<_IWeekCardSettleShowInfo> _infoList = new List<_IWeekCardSettleShowInfo>();
        foreach (WeekCard_SettleDetailInfo detailInfo in _m_info.getDetailList())
        {
            _IWeekCardSettleShowInfo info = _getShowInfo(detailInfo);
            
            if(null == info)
                continue;
            _infoList.Add(info);
        }
        
        _infoList.Sort(_sortSettleInfo);

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < _infoList.Count; i++)
        {
            _IWeekCardSettleShowInfo info = _infoList[i];
            sb.Append(info.getShowContent());
            if (i != _infoList.Count - 1)
                sb.Append("\n");
        }
        
        ALUGUICommon.setGameObjEnable(wnd.noSettleInfoShow, _infoList.Count == 0);
        ALUGUICommon.setGameObjEnable(wnd.noSettleInfoHide, _infoList.Count > 0);

        ALUGUICommon.setLabelTxt(wnd.txtSettleInfo, sb.ToString());
        // ALUGUICommon.setLabelTxt(wnd.txtTime, TextTranslate.instance.getLanguage(TransKeyConst.levy_silverOfflineTime_num, TimeUtil.millisecondsToTime_Two_NoSec(_m_offLineTimeMs)));
        ALUGUICommon.setGameObjEnable(wnd.weekCardExpiredShow, !NPPlayer.instance.weekCardComp.hasWeekCard());
        _refreshShowCaseWnd();
    }

    /// <summary>
    /// 对信息排序
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private int _sortSettleInfo(_IWeekCardSettleShowInfo x, _IWeekCardSettleShowInfo y)
    {
        int _xIdx = wnd.settleTypeSort.IndexOf(x.getSettleType());
        int _yIdx = wnd.settleTypeSort.IndexOf(x.getSettleType());
        if (_xIdx < _yIdx)
            return -1;
        if (_xIdx > _yIdx)
            return 1;
        return 0;
    }

    private _IWeekCardSettleShowInfo _getShowInfo(WeekCard_SettleDetailInfo _detailInfo)
    {
        switch (_detailInfo.getType())
        {
            case EWeekCardSettleType.TRAVEL:
                return new WeekCardSettleShowInfo_TRAVEL(_detailInfo);
            case EWeekCardSettleType.ANECDOTE:
                return new WeekCardSettleShowInfo_ANECDOTE(_detailInfo);
            case EWeekCardSettleType.CHILD_TRAIN:
                return new WeekCardSettleShowInfo_CHILD_TRAIN(_detailInfo);
            case EWeekCardSettleType.LEVY:
                return new WeekCardSettleShowInfo_LEVY(_detailInfo);
            case EWeekCardSettleType.COLLEGE_STUDY:
                return new WeekCardSettleShowInfo_COLLEGE_STUDY(_detailInfo);
            case EWeekCardSettleType.CONSORT_RND_CALL:
                return new WeekCardSettleShowInfo_CONSORT_RND_CALL(_detailInfo);
            
            default:
                return null;
        }
    }
}
