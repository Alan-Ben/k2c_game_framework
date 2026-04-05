using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerObj;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item
    /// </summary>
    public class GGUIWndStepRewardItem : _ANPGGUIBasicGridItemWnd<GGUIMonoStepRewardItem>
    {
        private GActivityStepRewardRefObj _m_itemData;
        private EStepRewardState _m_curStepRewardState = EStepRewardState.None;
        private long _m_score;		
        private EValueFormatType _m_processNumFormat;
        private bool _m_isShowGoTo;
        
        private NPGGUIWndCommonMaskItemContainer _m_wItemContainer;
        private Action _m_onClickGoTo;
        private Action<GActivityStepRewardRefObj> _m_onClickGet;
        public GGUIWndStepRewardItem(GGUIMonoStepRewardItem _wnd, Action _onClickGoTo, Action<GActivityStepRewardRefObj> _onClickGet) : base(_wnd)
        {
            _m_onClickGoTo = _onClickGoTo;
            _m_onClickGet = _onClickGet;
            initWnd();
        }

        protected override void _onShowWnd()
        {
            
        }

        protected override void _onHideWnd()
        {
            
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_wItemContainer?.discard();
            _m_wItemContainer = null;

            if (wnd == null) return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnGet, _onBtnGetClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnGoTo, _onBtnGoToClick);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if(wnd.itemContainer != null)
                _m_wItemContainer = new NPGGUIWndCommonMaskItemContainer(wnd.itemContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnGet, _onBtnGetClick);
            ALUGUICommon.combineBtnClick(wnd.btnGoTo, _onBtnGoToClick);
        }

        protected override void _resetGridItem()
        {
            
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        public void setInfo(GActivityStepRewardRefObj _data, EValueFormatType _processNumFormat, long _score, EStepRewardState _stepState, bool _isShowGoTo)
        {
            _m_itemData = _data;
            _m_curStepRewardState = _stepState;
            _m_processNumFormat = _processNumFormat;
            _m_score = _score;
            _m_isShowGoTo = _isShowGoTo;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新界面显示
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd || null == _m_itemData)
                return;
            _m_wItemContainer?.showItemList(_m_itemData.reward_item_list,_m_curStepRewardState == EStepRewardState.AlreadyGet, false);
            ALUGUICommon.setLabelTxt(wnd.textDesc, TextTranslate.instance.getLanguage(_m_itemData.name, _m_itemData.complete_count));
            string processStr = TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, GCommon.getValueFormatStr(_m_processNumFormat, _m_score),
                GCommon.getValueFormatStr(_m_processNumFormat, _m_itemData.complete_count));
            ALUGUICommon.setLabelTxt(wnd.txtProcess, processStr);
            ALUGUICommon.setLabelTxt(wnd.txtCanNotGetProcess, processStr);
            NPCommonEnumStatInfo<EStepRewardState>.setStat(wnd.statInfos, _m_curStepRewardState);    
            ALUGUICommon.setGameObjEnable(wnd.showGoToGos, _m_curStepRewardState == EStepRewardState.None && _m_isShowGoTo);
        }

        private void _onBtnGoToClick(GameObject _)
        {
            _m_onClickGoTo?.Invoke();
        }
        
        private void _onBtnGetClick(GameObject _)
        {
            _m_onClickGet?.Invoke(_m_itemData);
        }
    }
}
