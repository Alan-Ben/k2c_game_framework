using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 增加战力事件处理窗口
    /// </summary>
    public class GGUIWndTravelAddPowerEvent : _ANPGGUIBasicWnd<GGUIMonoTravelAddPowerEvent>
    {
        private static GGUIWndTravelAddPowerEvent _g_instance;
        public static GGUIWndTravelAddPowerEvent instance { get { return _g_instance ??= new GGUIWndTravelAddPowerEvent();} }

        private TravelAddPowerEventInfo _m_eventInfo;
        private _IHeroCardShow _m_iSelectedHeroInfo;
        
        private GGUIWndHeroCommonSelectItemGrid _m_wHeroCardGrid;
        
        public GGUIWndTravelAddPowerEvent() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoTravelAddPowerEvent.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTravelAddPowerEvent.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.heroCardItemGrid != null)
            {
                _m_wHeroCardGrid = new GGUIWndHeroCommonSelectItemGrid(wnd.heroCardItemGrid, _onSelectHero);
            }

            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _onConfirmBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (_m_wHeroCardGrid != null)
            {
                _m_wHeroCardGrid.discard();
            }
            _m_wHeroCardGrid = null;

            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnConfirm, _onConfirmBtnClick);
            }
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wHeroCardGrid?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wHeroCardGrid?.resetWnd();
        }

        public void setData(TravelAddPowerEventInfo _eventInfo)
        {
            _m_eventInfo = _eventInfo;
            _m_iSelectedHeroInfo = null;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (_m_eventInfo == null)
                return;

            if (_m_wHeroCardGrid != null)
            {
                List<_IHeroCardShow> heroCardShowList = new List<_IHeroCardShow>();
                NPPlayer.instance.heroComponent.dealAllHero((_heroInfo) =>
                {
                    if(_heroInfo != null)
                        heroCardShowList.Add(_heroInfo);
                });
                heroCardShowList.Sort(HeroCommon.sortHeroByPower);
                
                _m_wHeroCardGrid.showWnd();
                _m_wHeroCardGrid.refreshWnd(heroCardShowList, null);
            }
        }

        private void _onSelectHero(_IHeroCardShow _heroCardShow)
        {
            _m_wHeroCardGrid?.removeSelectHero(_m_iSelectedHeroInfo);
            _m_iSelectedHeroInfo = null;
            if(_m_iSelectedHeroInfo != _heroCardShow)
            {
                _m_iSelectedHeroInfo = _heroCardShow;
                _m_wHeroCardGrid?.addSelectHero(_m_iSelectedHeroInfo);
            }
        }
        
        private void _onConfirmBtnClick(GameObject _go)
        {
            if (_m_iSelectedHeroInfo == null)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.travel_addPowerEventNotSelectHeroTip);
                return;
            }
            
            if(_m_eventInfo != null)
                _m_eventInfo.selectedHeroInfo = _m_iSelectedHeroInfo;
            _m_iSelectedHeroInfo = null;
            
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TRAVEL_ADD_POWER_DEAL_WND);
        }
    }
}