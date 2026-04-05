using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndTreasureHuntCompositeCatalogItem : _ANPGGUIBasicGridItemWnd<GGUIMonoTreasureHuntCompositeCatalogItem>
    {
        private TreasureHuntCompositeCatalogInfo _m_iCompositeCatalogInfo;//组合图鉴数据
        private List<_ITreasureHuntOreInfo> _m_lOreInfoList;
        
        private GGUISubWndQualityShowGo _m_wQualityShowGo;
        private GGUIWndTreasureHuntOreItemContainer _m_wOreItemContainer;
        private NPGGUIWndCommonRedTip _m_wRedTip;
        
        public GGUIWndTreasureHuntCompositeCatalogItem(GGUIMonoTreasureHuntCompositeCatalogItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        public event Action<GGUIWndTreasureHuntCompositeCatalogItem> onItemClick; 
        
        public TreasureHuntCompositeCatalogInfo compositeCatalogInfo { get { return _m_iCompositeCatalogInfo; } }
        public List<_ITreasureHuntOreInfo> oreInfoList { get { return _m_lOreInfoList; } }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.qualityShowGoMono != null)
                _m_wQualityShowGo = new GGUISubWndQualityShowGo(wnd.qualityShowGoMono);

            if (wnd.monoOreItemContainer != null)
                _m_wOreItemContainer = new GGUIWndTreasureHuntOreItemContainer(wnd.monoOreItemContainer);
            
            if(wnd.monoRedTip != null)
                _m_wRedTip = new NPGGUIWndCommonRedTip(wnd.monoRedTip);
            
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onItemClick);
        }

        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onItemClick);
            }

            onItemClick = null;
            
            _m_wQualityShowGo?.discard();
            _m_wQualityShowGo = null;

            _m_wOreItemContainer?.discard();
            _m_wOreItemContainer = null;

            _m_wRedTip?.discard();
            _m_wRedTip = null;

            _m_lOreInfoList?.Clear();
            _m_lOreInfoList = null;
        }

        protected override void _onShowWnd()
        {
            _m_wRedTip?.showWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wQualityShowGo?.hideWnd();
            _m_wOreItemContainer?.hideWnd();
            _m_wRedTip?.hideWnd();
            
            _m_lOreInfoList?.Clear();
        }

        protected override void _onReset()
        {
            _m_wQualityShowGo?.resetWnd();
            _m_wOreItemContainer?.resetWnd();
            _m_wRedTip?.resetWnd();
            
            _m_lOreInfoList?.Clear();
        }

        protected override void _resetGridItem()
        {
            _m_wQualityShowGo?.resetWnd();
            _m_wOreItemContainer?.resetWnd();
            _m_wRedTip?.resetWnd();
            
            _m_lOreInfoList?.Clear();
        }

        public void setData(TreasureHuntCompositeCatalogInfo _compositeCatalogInfo)
        {
            _m_iCompositeCatalogInfo = _compositeCatalogInfo;
            
            if (_m_lOreInfoList == null)
                _m_lOreInfoList = new List<_ITreasureHuntOreInfo>();
            _m_lOreInfoList.Clear();
            if (_m_iCompositeCatalogInfo != null && _m_iCompositeCatalogInfo.compositeCatalogRefObj != null && 
                _m_iCompositeCatalogInfo.compositeCatalogRefObj.ore_list != null)
            {
                foreach (var oreId in _m_iCompositeCatalogInfo.compositeCatalogRefObj.ore_list)
                {
                    _m_lOreInfoList.Add(TreasureHuntUtil.getOreInfo(oreId));
                }
            }

            _refreshWnd();
        }

        public void setShowRedTip(bool _show)
        {
            if (_m_wRedTip != null)
            {
                _m_wRedTip.showWnd();
                _m_wRedTip?.showRedTipNum(_show ? 1 : 0);
            }
        }

        private void _refreshWnd()
        {
            if(wnd == null || _m_iCompositeCatalogInfo == null || _m_iCompositeCatalogInfo.compositeCatalogRefObj == null)
                return;

            // 设置组合名称
            ALUGUICommon.setLabelTxt(wnd.txtCompositeName, TextTranslate.instance.getLanguage(_m_iCompositeCatalogInfo.compositeCatalogRefObj.name));

            // 设置品质显示
            NPQualityExtRefObj qualityExtRefObj = GRefdataCoreMgr.instance.qualityExtRefCore.getRef((long) _m_iCompositeCatalogInfo.compositeCatalogRefObj.quality);
            if (qualityExtRefObj != null && _m_wQualityShowGo != null)
            {
                _m_wQualityShowGo.showWnd();
                _m_wQualityShowGo.setData(qualityExtRefObj);
            }
            else
            {
                _m_wQualityShowGo?.hideWnd();
            }

            // 设置矿石列表
            if (_m_wOreItemContainer != null)
            {
                _m_wOreItemContainer.showWnd();
                _m_wOreItemContainer.setData(_m_lOreInfoList);
            }

            NPCommonEnumStatInfo<ETreasureHuntCompositeCatalogState>.setStat(wnd.compositeCatalogStateShowList, _m_iCompositeCatalogInfo.state);
        }

        /// <summary>
        /// 
        /// </summary>
        private void _onItemClick(GameObject _go)
        {
            onItemClick?.Invoke(this);
        }
    }
}