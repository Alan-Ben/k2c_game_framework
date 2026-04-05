using System;
using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUISubWndBusinessBuildingOperatingHeroSelectGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoBusinessBuildingOperatingHeroSelectGridItem, GGUIMonoBusinessBuildingOperatingHeroSelectGrid, GGUISubWndBusinessBuildingOperatingHeroSelectGridItem>
    {
        private readonly Action<HeroInfo> _m_onHeroSelect;
        
        private BusinessBuildingRefObj _m_buildingRef;
        private List<HeroInfo> _m_curSelectHeroList;
        private List<HeroGridData> _m_heroGridDataList;
        
        private GGUISubWndCommonGridTextBarController _m_availableTextBar;
        private GGUISubWndCommonGridTextBarController _m_notGainTextBar;
        
        
        
        public GGUISubWndBusinessBuildingOperatingHeroSelectGrid(GGUIMonoBusinessBuildingOperatingHeroSelectGrid _wnd, Action<HeroInfo> _onHeroSelect) : base(_wnd)
        {
            _m_onHeroSelect = _onHeroSelect;

            initWnd();
        }
        
        
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_BUSINESS_BUILDING_HERO_CARD, _simulateClickBusinessBuildingHeroCard);
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_BUSINESS_BUILDING_HERO_CARD, _simulateClickBusinessBuildingHeroCard);
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
        }
        protected override void _onWndInitDone()
        {
            refreshWnd();
        }
        protected override GGUISubWndBusinessBuildingOperatingHeroSelectGridItem _createItemWnd(GGUIMonoBusinessBuildingOperatingHeroSelectGridItem _itemMono)
        {
            return new GGUISubWndBusinessBuildingOperatingHeroSelectGridItem(_itemMono, _onItemClick);
        }
        protected override void _onRefreshItemWnd(GGUISubWndBusinessBuildingOperatingHeroSelectGridItem _itemMono, int _itemIdx)
        {
            if (_m_heroGridDataList == null || _itemMono == null)
                return;
            
            if (_itemIdx < 0 || _itemIdx >= _m_heroGridDataList.Count)
                return;

            HeroGridData gridData = _m_heroGridDataList[_itemIdx];
            _itemMono.refreshWnd(gridData, (_m_curSelectHeroList?.IndexOf(gridData.heroInfo) ?? 0) + 1, _m_buildingRef);
        }
        

        public void refreshWnd(BusinessBuildingRefObj _buildingRef, List<HeroInfo> _curSelectHeroList)
        {
            _m_buildingRef = _buildingRef;
            _m_curSelectHeroList = _curSelectHeroList;
            
            _m_heroGridDataList = new List<HeroGridData>();
            List<HeroRefObj> heroRefList = GRefdataCoreMgr.instance.getAllBuildingAvailableHeroRef(_m_buildingRef.attr_type);
            foreach (HeroRefObj heroRef in heroRefList)
                _m_heroGridDataList.Add(new HeroGridData(heroRef));
            _m_heroGridDataList.Sort((_a, _b) =>
            {
                if (_a.isGain && !_b.isGain)
                    return -1;
                if (!_a.isGain && _b.isGain)
                    return 1;

                if (_a.heroInfo != null && _b.heroInfo != null)
                {
                    long addPropA = _a.heroInfo.getBusinessSkillAddPropValue(_buildingRef, EBonusPropertyType.BUILDING_PROFIT_ADD_PER);
                    long addPropB = _b.heroInfo.getBusinessSkillAddPropValue(_buildingRef, EBonusPropertyType.BUILDING_PROFIT_ADD_PER);
                    if (addPropA != addPropB)
                        return addPropB.CompareTo(addPropA);

                    long powerA = _a.heroInfo.power;
                    long powerB = _b.heroInfo.power;
                    if (powerA != powerB)
                        return powerB.CompareTo(powerA);
                }

                return _a.heroRef.id.CompareTo(_b.heroRef.id);
            });

            int notGainBarInsertIndex = 0;
            for (; notGainBarInsertIndex < _m_heroGridDataList.Count; notGainBarInsertIndex++)
            {
                HeroGridData gridData = _m_heroGridDataList[notGainBarInsertIndex];
                if (!gridData.isGain)
                    break;
            }
            
            if (notGainBarInsertIndex == 0)
                _resetAvailableTextBar();
            else
                _setAvailableTextBar(0);
                
            if (notGainBarInsertIndex == _m_heroGridDataList.Count)
                _resetNotGainTextBar();
            else
                _setNotGainTextBar(notGainBarInsertIndex);
            
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || _m_heroGridDataList == null)
                return;

            setItemCount(_m_heroGridDataList.Count);
        }


        private void _onItemClick(HeroInfo _heroInfo)
        {
            _m_onHeroSelect?.Invoke(_heroInfo);
        }
        private void _setAvailableTextBar(int _insertIndex)
        {
            if (wnd == null)
                return;

            if (wnd.monoAvailableTextBar == null || _m_buildingRef == null)
                return;
            
            _resetAvailableTextBar();
            
            ALUGUICommon.setGameObjEnable(wnd.monoAvailableTextBar, true);
            _m_availableTextBar = new GGUISubWndCommonGridTextBarController(_insertIndex, wnd.monoAvailableTextBar);
            _m_availableTextBar.setBarText(TextTranslate.instance.getLanguage(TransKeyConst.building_businessBuildingAvailableHeroBarTitle_name, TextTranslate.instance.getLanguage(_m_buildingRef.name)));
            addBar(_m_availableTextBar);
        }
        private void _setNotGainTextBar(int _insertIndex)
        {
            if (wnd == null)
                return;

            if (wnd.monoNotGainTextBar == null || _m_buildingRef == null)
                return;
            
            _resetNotGainTextBar();
            
            ALUGUICommon.setGameObjEnable(wnd.monoNotGainTextBar, true);
            _m_notGainTextBar = new GGUISubWndCommonGridTextBarController(_insertIndex, wnd.monoNotGainTextBar);
            _m_notGainTextBar.setBarText(TextTranslate.instance.getLanguage(TransKeyConst.building_businessBuildingNotGainHeroBarTitle_name, TextTranslate.instance.getLanguage(_m_buildingRef.name)));
            addBar(_m_notGainTextBar);
        }
        private void _resetAvailableTextBar()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.setGameObjEnable(wnd.monoAvailableTextBar, false);
            if (_m_availableTextBar == null)
                return;
            
            removeBar(_m_availableTextBar);
            _m_availableTextBar = null;
        }
        private void _resetNotGainTextBar()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.setGameObjEnable(wnd.monoNotGainTextBar, false);
            if (_m_notGainTextBar == null)
                return;
            
            removeBar(_m_notGainTextBar);
            _m_notGainTextBar = null;
        }
        private void _simulateClickBusinessBuildingHeroCard(object[] _params)
        {
            if (_params is not { Length: > 0 } || _params[0] is not long itemIndex)
                return;

            HeroGridData heroData = _m_heroGridDataList.SafeGet((int)itemIndex);
            if (heroData.isGain)
                _onItemClick(heroData.heroInfo);
        }


        public readonly struct HeroGridData
        {
            [NotNull] private readonly HeroRefObj _m_heroRef;
            private readonly HeroInfo _m_heroInfo;


            public HeroGridData([NotNull] HeroRefObj _heroRef)
            {
                _m_heroRef = _heroRef;
                _m_heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_heroRef.id);
            }
            
            
            public bool isGain { get { return _m_heroInfo != null; } }
            [NotNull] public HeroRefObj heroRef { get { return _m_heroRef; } }
            public HeroInfo heroInfo { get { return _m_heroInfo; } }
        }
    }
}