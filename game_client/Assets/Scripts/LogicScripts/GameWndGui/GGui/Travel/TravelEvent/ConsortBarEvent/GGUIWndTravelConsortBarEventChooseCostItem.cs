using System;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIWndTravelConsortBarEventChooseCostItem : _ANPGGUIBasicSubWnd<GGUIMonoTravelConsortBarEventChooseCostItem>
    {
        private TravelEventConsortBarCostRefObj _m_rConsortBarCostRefObj;//妃子酒馆事件消耗数据
        private ETravelConsortUnlockStat _m_eConsortUnlockStat;//妃子解锁状态

        private NPGGuiWndTexture _m_wOptionIcon;//选项图标
        private NPGGUIWndCommonItem _m_wCostItem;//消耗道具
        
        public GGUIWndTravelConsortBarEventChooseCostItem(GGUIMonoTravelConsortBarEventChooseCostItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        public TravelEventConsortBarCostRefObj consortBarCostRefObj => _m_rConsortBarCostRefObj;
        public ETravelConsortUnlockStat consortUnlockStat => _m_eConsortUnlockStat;

        public event Action<GGUIWndTravelConsortBarEventChooseCostItem> onSelectItem; 
        
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if(wnd.optionIcon != null)
                _m_wOptionIcon = new NPGGuiWndTexture(wnd.optionIcon);

            if (wnd.monoCostItem != null)
                _m_wCostItem = new NPGGUIWndCommonItem(wnd.monoCostItem);
            
            ALUGUICommon.combineBtnClick(wnd.btnSelect, _onSelectItemBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnSelect, _onSelectItemBtnClick);
            }
            
            onSelectItem = null;
            
            _m_wOptionIcon?.discard();
            _m_wOptionIcon = null;
            
            _m_wCostItem?.discard();
            _m_wCostItem = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wOptionIcon?.hideWnd();
            _m_wCostItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wOptionIcon?.discardTexture();
            _m_wCostItem?.resetWnd();
        }

        public void setData(TravelEventConsortBarCostRefObj _consortBarCostRefObj, ETravelConsortUnlockStat _stat)
        {
            _m_rConsortBarCostRefObj = _consortBarCostRefObj;
            if (_m_rConsortBarCostRefObj == null && wnd != null)
                _m_rConsortBarCostRefObj = GRefdataCoreMgr.instance.travelEventConsortBarCostRefCore.getRef(wnd.optionRefId);
            
            _m_eConsortUnlockStat = _stat;
            
            _refreshWnd();
        }
        
        private void _refreshWnd()
        {
            if(_m_rConsortBarCostRefObj == null || wnd == null)
                return;

            if (_m_wOptionIcon != null)
            {
                _m_wOptionIcon.showWnd();
                _m_wOptionIcon.setTexture(_m_rConsortBarCostRefObj.icon);
            }
            ALUGUICommon.setLabelTxt(wnd.txtOptionName, TextTranslate.instance.getLanguage(_m_rConsortBarCostRefObj.name));
            
            NPCommonEnumStatInfo<ETravelConsortUnlockStat>.setStat(wnd.statInfos, _m_eConsortUnlockStat);
         
            string addLikeKey = string.IsNullOrEmpty(wnd.txtAddLikeKey) ? TransKeyConst.common_add_num : wnd.txtAddLikeKey;
            ALUGUICommon.setLabelTxt(wnd.txtAddLike, TextTranslate.instance.getLanguage(addLikeKey, _m_rConsortBarCostRefObj.add_like));
            
            string addIntimacyKey = string.IsNullOrEmpty(wnd.txtAddIntimacyKey) ? TransKeyConst.common_add_num : wnd.txtAddIntimacyKey;
            ALUGUICommon.setLabelTxt(wnd.txtAddIntimacy, TextTranslate.instance.getLanguage(addIntimacyKey, _m_rConsortBarCostRefObj.add_intimacy));

            // 没有配置消耗道具时
            if (_m_rConsortBarCostRefObj.cost == null || !_m_rConsortBarCostRefObj.cost.IsValid)
            {
                _m_wCostItem?.hideWnd();
                
                ALUGUICommon.setGameObjEnable(wnd.hasCostShowGoList, false);
                ALUGUICommon.setGameObjEnable(wnd.freeShowGoList, true);
            }
            else
            {
                if (_m_wCostItem != null)
                {
                    _m_wCostItem.showWnd();
                    _m_wCostItem.setItem(_m_rConsortBarCostRefObj.cost);
                }
                
                ALUGUICommon.setGameObjEnable(wnd.hasCostShowGoList, true);
                ALUGUICommon.setGameObjEnable(wnd.freeShowGoList, false);
            }
        }

        private void _onSelectItemBtnClick(GameObject _go)
        {
            if(_m_rConsortBarCostRefObj == null)
                return;
            
            // 若有消耗且消耗道具不足时
            if(_m_rConsortBarCostRefObj.cost != null && _m_rConsortBarCostRefObj.cost.IsValid && !GCommon.isItemEnough(_m_rConsortBarCostRefObj.cost, true))
                return;
            
            onSelectItem?.Invoke(this);
        }
    }
}