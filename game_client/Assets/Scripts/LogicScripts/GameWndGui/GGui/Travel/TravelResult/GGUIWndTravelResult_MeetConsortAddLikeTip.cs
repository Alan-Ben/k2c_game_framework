using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndTravelResult_MeetConsortAddLikeTip : _ANPGGUIBasicWnd<GGUIMonoTravelResult_MeetConsortAddLikeTip>
    {
        private static GGUIWndTravelResult_MeetConsortAddLikeTip _g_instance = new GGUIWndTravelResult_MeetConsortAddLikeTip();

        public static GGUIWndTravelResult_MeetConsortAddLikeTip instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new  GGUIWndTravelResult_MeetConsortAddLikeTip();
                return _g_instance;
            }
        }

        public GGUIWndTravelResult_MeetConsortAddLikeTip() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoTravelResult_MeetConsortAddLikeTip.assetPath; }
        protected override string _monoObjName { get => GGUIMonoTravelResult_MeetConsortAddLikeTip.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        public override bool needDiscardOnSwitch { get => true; }

        protected override void _onShowWnd()
        {
            CommonTaskController.CommonActionAddMonoTask(_closeWnd, wnd.closeDelayTime);
        }

        protected override void _onHideWnd()
        {
            
        }

        protected override void _onReset()
        {
            
        }

        protected override void _onDiscard()
        {
            if(wnd != null)
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }

        private void _closeWnd()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TRAVEL_RESULT_MEET_CONSORT_ADD_LIKE_TIP);
        }

        private void _onCloseBtnClick(GameObject _go)
        {
            _closeWnd();
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        public static void showMeetConsortAddLikeTipWnd(Action _onClose)
        {
            QueueMgr.instance.AddNode(new BaseOnAddContainerSceneUIWndQueueNode(EUIQueueStageType.MAIN, UINodeTagConst.C_TRAVEL_RESULT_MEET_CONSORT_ADD_LIKE_TIP, false
                , false, false, null, GGUIWndTravelResult_MeetConsortAddLikeTip.instance, true, false,
                null, null, null, () =>
                {
                    _onClose?.Invoke();
                }));
        }
    }
}