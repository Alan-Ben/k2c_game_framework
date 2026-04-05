using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 科研所状态常驻HUD按钮控制器
    /// </summary>
    public class GGUIWndMarsBuildingTechnologyStateBtnsFollowerController : _ATALGGUICommonFollowItemController<GGUIMonoMarsBuildingTechnologyStateBtns, GGUIWndMarsBuildingTechnologyStateBtnsFollower>
    {
        private readonly GResPathIndex _m_resIndex;


        public GGUIWndMarsBuildingTechnologyStateBtnsFollowerController()
        {
            _m_resIndex = new GResPathIndex(7314); // 火星拓展-科研所状态常驻HUD
        }
        public GGUIWndMarsBuildingTechnologyStateBtnsFollowerController(GResPathIndex _resIndex)
        {
            _m_resIndex = _resIndex;
        }


        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }


        protected override GGUIWndMarsBuildingTechnologyStateBtnsFollower _createItemWnd(GGUIMonoMarsBuildingTechnologyStateBtns _wndMono)
        {
            GGUIWndMarsBuildingTechnologyStateBtnsFollower wnd = new GGUIWndMarsBuildingTechnologyStateBtnsFollower(_wndMono);
            wnd.showWnd();
            return wnd;
        }
        
        public void refreshWnd()
        {
            wnd?.refreshWnd();
        }
    }

    /// <summary>
    /// 科研所状态常驻HUD按钮跟随窗口
    /// </summary>
    public class GGUIWndMarsBuildingTechnologyStateBtnsFollower : _ATALGGUIWndCommonFollowItem<GGUIMonoMarsBuildingTechnologyStateBtns>
    {
        private GGUIWndMarsUpgradingTechnologyInfo _m_upgradingTechnologyInfo;

        private List<NPGGuiWndTexture> _m_lUpgradingTechIconList;

        public GGUIWndMarsBuildingTechnologyStateBtnsFollower(GGUIMonoMarsBuildingTechnologyStateBtns _wnd) : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
            _m_upgradingTechnologyInfo?.showWnd();
            refreshWnd();
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_BUILDING_TECHNOLOGY_RESEARCH, _onSimulateClickResearch);
        }
        protected override void _onHideWnd()
        {
            _m_upgradingTechnologyInfo?.hideWnd();
            
            if (_m_lUpgradingTechIconList != null)
            {
                foreach (var iconWnd in _m_lUpgradingTechIconList)
                {
                    iconWnd?.hideWnd();
                }    
            }
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_BUILDING_TECHNOLOGY_RESEARCH, _onSimulateClickResearch);
        }
        protected override void _onReset()
        {
            _m_upgradingTechnologyInfo?.resetWnd();
            
            if (_m_lUpgradingTechIconList != null)
            {
                foreach (var iconWnd in _m_lUpgradingTechIconList)
                {
                    iconWnd?.discardTexture();
                }    
            }
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnResearch, _onBtnResearchClick);

            _m_upgradingTechnologyInfo?.discard();
            _m_upgradingTechnologyInfo = null;
            
            if (_m_lUpgradingTechIconList != null)
            {
                foreach (var iconWnd in _m_lUpgradingTechIconList)
                {
                    iconWnd?.discard();
                }    
                _m_lUpgradingTechIconList.Clear();
            }
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 构建升级中科技信息窗口
            if (wnd.monoUpgradingTechnologyInfo != null)
                _m_upgradingTechnologyInfo = new GGUIWndMarsUpgradingTechnologyInfo(wnd.monoUpgradingTechnologyInfo);

            if(_m_lUpgradingTechIconList == null)
                _m_lUpgradingTechIconList = new List<NPGGuiWndTexture>();
            if (wnd.monoIconList != null)
            {
                foreach (var monoIcon in wnd.monoIconList)
                {
                    _m_lUpgradingTechIconList.Add(new NPGGuiWndTexture(monoIcon));
                }
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnResearch, _onBtnResearchClick);
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            _m_upgradingTechnologyInfo?.refreshWnd();
            if (_m_lUpgradingTechIconList != null)
            {
                MarsTechnologyInfo nowUpgradingTechnology = NPPlayer.instance.marsComp.technologySubComponent.nowdUpgradingOrUpgradedTechnologyInfo;
                foreach (var iconWnd in _m_lUpgradingTechIconList)
                {
                    if(iconWnd == null)
                        continue;
                    
                    if (nowUpgradingTechnology == null || nowUpgradingTechnology.technologyRefObj == null)
                    {
                        iconWnd.hideWnd();
                    }
                    else
                    {
                        iconWnd.showWnd();
                        iconWnd.setTexture(nowUpgradingTechnology.technologyRefObj.icon);
                    }
                }
            }
        }


        /// <summary>
        /// 点击研究按钮，打开科技树窗口
        /// </summary>
        private void _onBtnResearchClick(GameObject _obj)
        {
            // 打开科技树窗口（无特定科技类型，显示全部）
            QueueMgr.instance.AddNode(new GNodeMarsTechnologyTree(EMarsTechnologyType.NONE));
        }

        private void _onSimulateClickResearch()
        {
            if (wnd == null) return;
            _onBtnResearchClick(wnd.btnResearch);
        }
    }
}
