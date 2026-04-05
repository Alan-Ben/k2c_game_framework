using System.Collections.Generic;
using ALPackage;
using Common.DinnerEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 宴会举办弹窗
    /// </summary>
    public class GGUIWndDinnerCreate : _ATALBasicUIWnd<GGUIMonoDinnerCreate>
    {
        private static GGUIWndDinnerCreate _g_instance = new GGUIWndDinnerCreate();

        public static GGUIWndDinnerCreate instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndDinnerCreate();
                return _g_instance;
            }
        }

        private GGUIWndDinnerCreateItemContainer _m_dinnerTypeContainer;//举办方式列表
        private GGUIWndDinnerCreateItemContainer _m_permitDinnerContainer;//举办方式列表

        public GGUIWndDinnerCreate() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoDinnerCreate.assetPath; }
        protected override string _monoObjName { get => GGUIMonoDinnerCreate.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

	    protected override void _onShowWnd()
	    {
	        WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_HOLD_DINNER_INDEX, _onSimulateClickHoldDinner);
	        WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_HOLD_CONSORT_DINNER_INDEX, _onSimulateClickHoldConsortDinner);
	        refreshWnd();
	    }

	    protected override void _onHideWnd()
	    {
	        WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_HOLD_DINNER_INDEX, _onSimulateClickHoldDinner);
	        WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_HOLD_CONSORT_DINNER_INDEX, _onSimulateClickHoldConsortDinner);
	    }

        protected override void _onReset()
        {
        
        }

        protected override void _onDiscard()
        {

            _m_dinnerTypeContainer?.discard();
            _m_dinnerTypeContainer = null;
            
            _m_permitDinnerContainer?.discard();
            _m_permitDinnerContainer = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
   
            if (null != wnd.dinnerTypeContainer)
            {
                _m_dinnerTypeContainer = new GGUIWndDinnerCreateItemContainer(wnd.dinnerTypeContainer);
            }
            
            if(null != wnd.dinnerPermitContainer)
                _m_permitDinnerContainer = new GGUIWndDinnerCreateItemContainer(wnd.dinnerPermitContainer);
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        public void refreshWnd()
        {
            if(null == wnd)
                return;
            _m_dinnerTypeContainer?.showWnd();
            List<DinnerCreateItemShowInfo> itemDataList = new List<DinnerCreateItemShowInfo>();
            List<DinnerCreateItemShowInfo> permitDataList = new List<DinnerCreateItemShowInfo>();
            // //添加bar
            // if(NPPlayer.instance.dinnerComp.permitList.Count > 0)
            //     itemDataList.Add(new DinnerCreateItemShowInfo(null, null, EDinnerCreateItemType.Bar_Celebration));
            foreach (DinnerPermit permit in NPPlayer.instance.dinnerComp.permitList)
            {
                DinnerPermitRefObj permitRef = GRefdataCoreMgr.instance.dinnerPermitRefCore.getRef((long)permit.type);
                GDinnerTypeRefObj dinnerTypeRef = GRefdataCoreMgr.instance.dinnerTypeRefCore.getRef(permitRef.dinner_id);
                DinnerCreateItemShowInfo info = new DinnerCreateItemShowInfo( permit, dinnerTypeRef);
                permitDataList.Add(info);
            }
            // //添加bar
            // itemDataList.Add(new DinnerCreateItemShowInfo(null, null, EDinnerCreateItemType.Bar_Party));
            foreach (GDinnerTypeRefObj dinnerType in GRefdataCoreMgr.instance.dinnerTypeRefCore.refList)
            {
                if(dinnerType.is_permit_open)
                    continue;
                DinnerCreateItemShowInfo info = new DinnerCreateItemShowInfo(null, dinnerType);
                itemDataList.Add(info);
            }
            _m_dinnerTypeContainer?.setInfo(itemDataList);
            
            permitDataList.Sort((_info, _showInfo) =>
            {
                if (_info == null && _showInfo == null) return 0;
                if (_info == null) return 1;
                if (_showInfo == null) return -1;
                if (_info.permit == null && _showInfo.permit == null) return 0;
                if (_info.permit == null) return 1;
                if (_showInfo.permit == null) return -1;
                return _info.permit.expiredTs.CompareTo(_showInfo.permit.expiredTs);
            });
            _m_permitDinnerContainer?.setInfo(permitDataList);
        }

        /// <summary>
        /// 点击关闭
        /// </summary>
        /// <param name="obj"></param>
        private void _onClickClose(GameObject obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_DINNER_CREATE);
        }
        
        //根据下标模拟点击举办宴会
        private void _onSimulateClickHoldDinner(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            long index = (long)_objects[0];
            _m_dinnerTypeContainer?.createDinnerByIndex((int)index);
        }
        
        //根据下标模拟点击举办情人宴会
        private void _onSimulateClickHoldConsortDinner(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            long index = (long)_objects[0];
            _m_permitDinnerContainer?.createDinnerByIndex((int)index);
        }
    }
}