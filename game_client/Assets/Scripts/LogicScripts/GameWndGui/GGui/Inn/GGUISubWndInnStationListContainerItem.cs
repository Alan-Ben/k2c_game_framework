using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndInnStationListContainerItem : _ATALBasicUISubWnd<GGUIMonoInnStationListContainerItem>
    {
        private InnStationInfo _m_stationInfo;
        private NPGGuiWndTexture _m_stationIcon;
        private NPGGuiWndTexture _m_stationIcon2;
        private InnViewMgr _m_viewMgr;
        private int _m_itemIdx = -1;
        private Action<GGUISubWndInnStationListContainerItem> _m_aOnSelectItem;

        /// <summary>
        /// 选中item时的回调
        /// </summary>
        public Action<GGUISubWndInnStationListContainerItem> onSelectItem { get { return _m_aOnSelectItem; } set { _m_aOnSelectItem = value; } }
        /// <summary>
        /// 设施信息
        /// </summary>
        public InnStationInfo stationInfo { get { return _m_stationInfo; } }


        public GGUISubWndInnStationListContainerItem(GGUIMonoInnStationListContainerItem _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            _m_stationIcon?.showWnd();
            _m_stationIcon2?.showWnd();
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_INN_STATION_ENTER_BY_INDEX, _onSimulateClickEnter);
        
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_stationIcon?.hideWnd();
            _m_stationIcon2?.hideWnd();
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_INN_STATION_ENTER_BY_INDEX, _onSimulateClickEnter);
        }
        protected override void _onReset()
        {
            _m_stationIcon?.discardTexture();
            _m_stationIcon2?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_stationIcon?.discard();
            _m_stationIcon = null;
            _m_stationIcon2?.discard();
            _m_stationIcon2 = null;
            
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnEnter, _onBtnEnterClicked);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_stationIcon = new NPGGuiWndTexture(wnd.imgIcon);
            if (wnd.imgIcon2 != null)
                _m_stationIcon2 = new NPGGuiWndTexture(wnd.imgIcon2);
            
            ALUGUICommon.combineBtnClick(wnd.btnEnter, _onBtnEnterClicked);
        }


        public void refreshWnd(InnStationInfo _stationInfo, InnViewMgr _viewMgr, int _index)
        {
            _m_stationInfo = _stationInfo;
            _m_viewMgr = _viewMgr;
            _m_itemIdx = _index;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_stationInfo == null || _m_viewMgr == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtLevelAndName, TextTranslate.instance.getLanguage(TransKeyConst.inn_stationLevelAndName_level_name, _m_stationInfo.level, _m_stationInfo.nameTranslated));
            ALUGUICommon.setLabelTxt(wnd.txtName, _m_stationInfo.nameTranslated);
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_stationInfo.level));
            ALUGUICommon.setLabelTxt(wnd.txtTip, _m_stationInfo.getCurrentTipTranslated());
            _m_stationIcon?.setTexture(_m_stationInfo.refObj.icon);
            _m_stationIcon2?.setTexture(_m_stationInfo.refObj.icon);
            wnd.setLevelMax(_m_stationInfo.isLevelMax);
            long curHadSettleGuestsCount = _m_viewMgr.getHadSettleGuestsCount();
            long needReceiveGuestNum = _m_stationInfo.refObj.need_receive_guest_num;
            InnStationInfo requireStationInfo = NPPlayer.instance.innComp.getRequireStationInfo(_m_stationInfo);
            bool isNextBuilding = requireStationInfo is { isBuilt: true } or null;
            bool canUnlock = curHadSettleGuestsCount >= needReceiveGuestNum && isNextBuilding && !_m_stationInfo.isBuilt;
            bool isPendingUnlock = curHadSettleGuestsCount < needReceiveGuestNum && isNextBuilding && !_m_stationInfo.isBuilt;
            wnd.setUnlock(_m_stationInfo.isBuilt, canUnlock, _m_stationInfo.isUpgradable(), isPendingUnlock);
        }

        /// <summary>
        /// 设置选中状态
        /// </summary>
        /// <param name="_isSelect"></param>
        public void setSelect(bool _isSelect)
        {
            if (wnd == null)
                return;
            ALUGUICommon.setGameObjEnable(wnd.goSelectShowList, _isSelect);
            ALUGUICommon.setGameObjEnable(wnd.goSelectHideList, !_isSelect);
        }

        private void _onBtnEnterClicked(GameObject _obj)
        {
            // if (_m_stationInfo == null)
            //     return;
            //
            // if (_m_stationInfo.isBuilt)
            // {
            //     GGUIWndInnStationLevelUp.instance.refreshWnd(_m_stationInfo);
            //     QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndInnStationLevelUp.instance, GGUIWndInnStationLevelUp.instance.showWnd, UINodeTagConst.C_INN_STATION_LEVEL_UP);
            // }
            // else
            // {
            //     GGUIWndInnStationBuild.instance.refreshWnd(_m_stationInfo, _m_viewMgr);
            //     QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndInnStationBuild.instance, GGUIWndInnStationBuild.instance.showWnd, UINodeTagConst.C_INN_STATION_BUILD);
            // }

            _m_aOnSelectItem?.Invoke(this);
        }
        
        private void _onSimulateClickEnter(params object[] _objects)
        {
            if(_objects == null || _objects.Length == 0 || _objects[0] is not long targetIndex) 
                return;

            if (targetIndex == _m_itemIdx)
                _onBtnEnterClicked(null);
        }
    }
}