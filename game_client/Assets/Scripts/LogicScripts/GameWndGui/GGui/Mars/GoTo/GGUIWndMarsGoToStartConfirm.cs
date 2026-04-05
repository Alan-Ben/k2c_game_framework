using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星确认开始前往界面
    /// </summary>
    public class GGUIWndMarsGoToStartConfirm : _ANPGGUIBasicResBarWnd<GGUIMonoMarsGoToStartConfirm>
    {
        private static GGUIWndMarsGoToStartConfirm _g_instance;
        public static GGUIWndMarsGoToStartConfirm instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndMarsGoToStartConfirm();
                return _g_instance;
            }
        }


        public GGUIWndMarsGoToStartConfirm() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoMarsGoToStartConfirm.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsGoToStartConfirm.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override bool isShowAniPlayOnlyOne { get { return true; } }
        
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_GO_TO_START_CONFIRM, _onSimulateClickConfirmBtn);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_GO_TO_START_CONFIRM, _onSimulateClickConfirmBtn);
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnConfirm, _onClickConfirmBtn);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _onClickConfirmBtn);
        }

        /// <summary>
        /// 点击确认按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickConfirmBtn(GameObject _go)
        {
            //发送埋点-点击发射火箭开始前往火星
            GCommon.sendStepReport(TraceConst.CLICK_START_GO_TO_MARS);

            // 先屏蔽输入，等待请求回来
            int maskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            //请求前往火星
            NPPlayer.instance.marsComp.goToSubComponent.reqStartToGoMars((_isSuc) =>
            {
                // 请求结束，关闭输入屏蔽
                MainCameraMono.selfInstance.closeAllInputMask(maskSerialize);
                if (_isSuc && wnd != null)
                {
                    // 第一阶段到达，触发对话
                    MarsGoRouteRefObj curRoute = GRefdataCoreMgr.instance.getMarsGoRouteRefByStageId(1);
                    // 打开对话
                    if (curRoute != null && curRoute.arrive_dialogue_id > 0)
                        GCommon.enterDialogueNode(curRoute.arrive_dialogue_id, () =>
                        {
                            // 关闭窗口
                            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_GO_TO_START_CONFIRM);
                            // 打开前往火星界面
                            QueueMgr.instance.AddNode(new GNodeMarsGoTo());
                        });
                }
            });
        }

        /// <summary>
        /// 模拟点击确认按钮
        /// </summary>
        private void _onSimulateClickConfirmBtn()
        {
            _onClickConfirmBtn(null);
        }
    }
}