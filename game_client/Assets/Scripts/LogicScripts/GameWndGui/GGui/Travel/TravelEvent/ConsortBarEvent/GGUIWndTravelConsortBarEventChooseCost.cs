using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndTravelConsortBarEventChooseCost : _ANPGGUIBasicSubWnd<GGUIMonoTravelConsortBarEventChooseCost>
    {
        private TravelConsortBarEventInfo _m_iConsortBarEventInfo;//妃子酒馆事件信息
        private _IConsortShowInfo _m_selectedConsortInfo;//选中的妃子数据
        private ETravelConsortUnlockStat _m_selectConsortUnlockStat;//选中妃子的解锁状态
        
        private NPGGUIWndCommonShowCase _m_wConsortShowCase;//妃子形象
        [NotNull] private List<GGUIWndTravelConsortBarEventChooseCostItem> _m_lChooseCostItemList = new List<GGUIWndTravelConsortBarEventChooseCostItem>();//消耗道具列表
        
        public GGUIWndTravelConsortBarEventChooseCost(GGUIMonoTravelConsortBarEventChooseCost _wnd) : base(_wnd)
        {
            initWnd();
        }

        public event Action<TravelEventConsortBarCostRefObj> onChooseCost;//消耗道具被选择时
        public event Action onCloseWndBtnClick;//关闭按钮被点击时
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.consortTdShow != null)
                _m_wConsortShowCase = new NPGGUIWndCommonShowCase(wnd.consortTdShow);

            if (wnd.monoChooseCostList != null)
            {
                foreach (var costItemMono in wnd.monoChooseCostList)
                {
                    if(costItemMono == null)
                        continue;

                    GGUIWndTravelConsortBarEventChooseCostItem costItemWnd = new GGUIWndTravelConsortBarEventChooseCostItem(costItemMono);
                    costItemWnd.onSelectItem += _onCostItemSelect;
                    _m_lChooseCostItemList.Add(costItemWnd);
                }
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseWndBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseWndBtnClick);
            }
            
            onChooseCost = null;
            onCloseWndBtnClick = null;
         
            _m_wConsortShowCase?.discard();
            _m_wConsortShowCase = null;
            
            foreach (var costItemWnd in _m_lChooseCostItemList)
            {
                if(costItemWnd == null)
                    continue;
                
                costItemWnd.onSelectItem -= _onCostItemSelect;
            }
            _m_lChooseCostItemList.Clear();
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wConsortShowCase?.hideWnd();
            
            foreach (var costItemWnd in _m_lChooseCostItemList)
            {
                costItemWnd?.hideWnd();
            }
        }

        protected override void _onReset()
        {
            _m_wConsortShowCase?.resetWnd();
            
            foreach (var costItemWnd in _m_lChooseCostItemList)
            {
                costItemWnd?.resetWnd();
            }
        }

        public void setData(TravelConsortBarEventInfo _eventInfo, _IConsortShowInfo _selectedConsortInfo, ETravelConsortUnlockStat _selectConsortUnlockStat)
        {
            _m_iConsortBarEventInfo = _eventInfo;
            _m_selectedConsortInfo = _selectedConsortInfo;
            _m_selectConsortUnlockStat = _selectConsortUnlockStat;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || _m_iConsortBarEventInfo == null)
                return;

            foreach (var costItemWnd in _m_lChooseCostItemList)
            {
                if(costItemWnd == null)
                    continue;
                
                costItemWnd.showWnd();
                costItemWnd.setData(null, _m_selectConsortUnlockStat);
            }
            
            if (_m_selectedConsortInfo != null)
            {
                if (_m_wConsortShowCase != null)
                {
                    _AShowCaseUnitInfoObj[] showCaseUnitInfoObjList = new _AShowCaseUnitInfoObj[4];
                    showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(_m_selectedConsortInfo.consortSkinShowInfo?.tdShow), 0);
                    showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(_m_selectedConsortInfo.consortSkinShowInfo?.tdBgIndex), 3);
                
                    _m_wConsortShowCase.showWnd(showCaseUnitInfoObjList);
                }

                TravelConsortInfo travelConsortInfo = NPPlayer.instance.travelComp.getTravelConsortInfo(_m_selectedConsortInfo.consortId);
                TravelConsortRefObj travelConsortRefObj = travelConsortInfo?.travelConsortRefObj ??
                                                          GRefdataCoreMgr.instance.travelConsortRefCore.getRef(_m_selectedConsortInfo.consortId);

                // 设置好感度相关文本
                if (string.IsNullOrEmpty(wnd.txtLikeConsortNameKey))
                {
                    ALUGUICommon.setLabelTxt(wnd.txtLikeConsortName, _m_selectedConsortInfo.consortTransName);
                }
                else
                {
                    ALUGUICommon.setLabelTxt(wnd.txtLikeConsortName, TextTranslate.instance.getLanguage(wnd.txtLikeConsortNameKey, _m_selectedConsortInfo.consortTransName));
                }
                if (string.IsNullOrEmpty(wnd.txtLikeValueKey))
                {
                    ALUGUICommon.setLabelTxt(wnd.txtLikeValue, TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, travelConsortInfo?.like ?? 0, travelConsortRefObj?.marry_need_like ?? 0));
                }
                else
                {
                    ALUGUICommon.setLabelTxt(wnd.txtLikeValue, TextTranslate.instance.getLanguage(wnd.txtLikeValueKey, travelConsortInfo?.like ?? 0, travelConsortRefObj?.marry_need_like ?? 0));
                }

                // 设置亲密度相关文本
                if (string.IsNullOrEmpty(wnd.txtIntimacyConsortNameKey))
                {
                    ALUGUICommon.setLabelTxt(wnd.txtIntimacyConsortName, _m_selectedConsortInfo.consortTransName);
                }
                else
                {
                    ALUGUICommon.setLabelTxt(wnd.txtIntimacyConsortName, TextTranslate.instance.getLanguage(wnd.txtIntimacyConsortNameKey, _m_selectedConsortInfo.consortTransName));
                }
                if (string.IsNullOrEmpty(wnd.txtIntimacyValueKey))
                {
                    ALUGUICommon.setLabelTxt(wnd.txtIntimacyValue, _m_selectedConsortInfo.intimacy);
                }
                else
                {
                    ALUGUICommon.setLabelTxt(wnd.txtIntimacyValue, TextTranslate.instance.getLanguage(wnd.txtIntimacyValueKey, _m_selectedConsortInfo.intimacy));
                }
            }
            
            NPCommonEnumStatInfo<ETravelConsortUnlockStat>.setStat(wnd.statInfos, _m_selectConsortUnlockStat);
        }
        
        /// <summary>
        /// 消耗道具被选择时
        /// </summary>
        /// <param name="_itemWnd"></param>
        private void _onCostItemSelect(GGUIWndTravelConsortBarEventChooseCostItem _itemWnd)
        {
            if(_itemWnd == null)
                return;
            
            onChooseCost?.Invoke(_itemWnd.consortBarCostRefObj);
        }
        
        private void _onCloseWndBtnClick(GameObject _go)
        {
            onCloseWndBtnClick?.Invoke();
        }
    }
}