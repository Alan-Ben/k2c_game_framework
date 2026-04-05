using ALPackage;
using GC2GS.p007_CommOp;
using GS2GC.p007_CommOp;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 商店评价吐槽界面
    /// </summary>
    public class GGUIWndStoreReviewsRoast : _ANPGGUIBasicWnd<GGUIMonoStoreReviewsRoast>
    {
        private static GGUIWndStoreReviewsRoast _g_instance = new GGUIWndStoreReviewsRoast();

        public static GGUIWndStoreReviewsRoast instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndStoreReviewsRoast();

                return _g_instance;
            }
        }

        public GGUIWndStoreReviewsRoast() : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoStoreReviewsRoast.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoStoreReviewsRoast.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnSend, _onClickSend);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnSend, _onClickSend);
        }

        #region 点击事件

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_btn"></param>
        private void _onClickClose(GameObject _btn)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_STORE_REVIEWS_ROAST);
        }

        /// <summary>
        /// 点击发送按钮
        /// </summary>
        /// <param name="_btn"></param>
        private void _onClickSend(GameObject _btn)
        {
            if (wnd == null || wnd.inputText == null)
                return;

            //输入的文本
            string inputStr = wnd.inputText.text;

            //判断文本长度
            if (!CharacterDetermineMgr.instance.isSuitableLength(inputStr, 1, wnd.limitTextLength, true))
                return;

            //设置完成商店评价
            GameSetting.instance.setIsFinishStoreReviews(true);
            NPPlayer.instance.playerInfoComp.clientDataRemarkInfo.setIsFinishStoreReviews(true);

            //发送埋点-商店好评界面吐槽点击发送
            GCommon.sendStepReport(TraceConst.STORE_REVIEWS_CLICK_SEND_ROAST);

            //发送吐槽请求
            NPGSClientListener.sendRequestByLog(new GC2GS_007_033_ReqStoreReviewsRecordRoast(inputStr), new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_007_033_RetStoreReviewsRecordRoast>(null));

            //上浮提示
            NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.stroreReviews_finishTip_none);

            //关闭界面
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_STORE_REVIEWS_ROAST);
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_STORE_REVIEWS_MAIN);
        }

        #endregion
    }
}
