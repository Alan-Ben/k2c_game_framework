using System.Collections.Generic;
using ALPackage;
using GC2GS.p014_ChildOp;
using GS2GC.p014_ChildOp;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndAdultEngageRequestReceive : _ATALBasicUIWnd<GGUIMonoAdultEngageRequestReceive>
    {
        [NotNull] public static GGUIWndAdultEngageRequestReceive instance { get { return _g_instance ??= new GGUIWndAdultEngageRequestReceive(); } }
        private static GGUIWndAdultEngageRequestReceive _g_instance;

        [ItemNotNull, NotNull] private readonly List<AdultEngageRequestInfo> _m_requestList;

        private GGUISubWndAdultEngageRequestReceiveGrid _m_requestGrid;
        private NPGGUIWndCommonToggleEx _m_toggleRefuseAll;
        private ALCommonEnableTaskController _m_timeTask;
        

        public GGUIWndAdultEngageRequestReceive() 
            : base(EALUIWndLayer.ADDITION)
        {
            _m_requestList = new List<AdultEngageRequestInfo>();
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoAdultEngageRequestReceive.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoAdultEngageRequestReceive.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_requestGrid?.showWnd();
            _m_toggleRefuseAll?.showWnd();

            refreshWnd();

            NPPlayer.instance.childComp.onEngageRequestReceiveChg += refreshWnd;
            _m_timeTask = ALCommonEnableDurationActionMonoTask.addMonoTask(_timeTask, 1f);
            NPPlayer.instance.childComp.readAllEngageRequest();
        }
        protected override void _onHideWnd()
        {
            NPPlayer.instance.childComp.onEngageRequestReceiveChg -= refreshWnd;
            
            _m_requestGrid?.hideWnd();
            _m_toggleRefuseAll?.hideWnd();
            _m_timeTask.setDisable();
        }
        protected override void _onReset()
        {
            _m_requestGrid?.resetWnd();
            _m_toggleRefuseAll?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_requestGrid?.discard();
            _m_requestGrid = null;
            if (_m_toggleRefuseAll != null)
            {
                _m_toggleRefuseAll.clickDelegate -= _onToggleRefuseAllClick;
                _m_toggleRefuseAll.discard();
                _m_toggleRefuseAll = null;
            }

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnOneKeyRefuse, _onOneKeyRefuseBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoRequestGrid != null)
                _m_requestGrid = new GGUISubWndAdultEngageRequestReceiveGrid(wnd.monoRequestGrid);
            if (wnd.monoToggleRefuseAll != null)
            {
                _m_toggleRefuseAll = new NPGGUIWndCommonToggleEx(wnd.monoToggleRefuseAll);
                _m_toggleRefuseAll.clickDelegate += _onToggleRefuseAllClick;
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnOneKeyRefuse, _onOneKeyRefuseBtnClick);
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            NPPlayer.instance.childComp.getEngageRequestInfoListNonAlloc(_m_requestList);
            _m_requestGrid?.refreshWnd(_m_requestList);
            _m_toggleRefuseAll?.setSelected(NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.IS_REFUSE_MARRY_REQUEST) == 1);
        }
        

        private void _onCloseBtnClick(GameObject _obj)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }
        private void _onOneKeyRefuseBtnClick(GameObject _obj)
        {
            int serialize = MainCameraMono.selfInstance.openAllInputMask();
            NPGSClientListener.sendRequestByLog(GSWriter_014_ChildOp.make_010_ReqAkeyRefuseToMeApply(),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_014_010_RetAkeyRefuseToMeApply>(
                    (_isSuc, _msg) =>
                    {
                        MainCameraMono.selfInstance.closeAllInputMask(serialize);
                        if (_isSuc)
                            QueueMgr.instance.DoUIRollBackByEsc();
                    }));
        }
        private void _onToggleRefuseAllClick(NPGGUIWndCommonToggleEx _toggle)
        {
            if (_toggle == null)
                return;
            
            NPGSClientListener.sendRequestByLog(new GC2GS_014_020_ReqSetRefuseMarry(!_toggle.isOn), 
                new CommonErrCodeRequestCallbackDispatherTriggerDealer(() =>
                {
                    _m_toggleRefuseAll?.setSelected(NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.IS_REFUSE_MARRY_REQUEST) == 1);
                }));
        }
        private void _timeTask()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            _m_requestGrid?.refreshAllItem((_item, _index) =>
            {
                if (_item == null)
                    return;
                
                _item.refreshTime();
                AdultEngageRequestInfo requestData = _m_requestList.SafeGet(_index);
                if (requestData is { isEnable: false })
                    refreshWnd();
            });
        }
    }
}