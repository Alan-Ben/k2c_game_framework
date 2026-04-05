using System;
using ALPackage;
using GC2GS.p007_CommOp;
using GC2GS.p013_HeroOp;
using GS2GC.p007_CommOp;
using GS2GC.p013_HeroOp;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 商店评价主界面
    /// </summary>
    public class GGUIWndStoreReviewsMain : _ANPGGUIBasicWnd<GGUIMonoStoreReviewsMain>
    {
        private static GGUIWndStoreReviewsMain _g_instance = new GGUIWndStoreReviewsMain();

        public static GGUIWndStoreReviewsMain instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndStoreReviewsMain();

                return _g_instance;
            }
        }

        private Action _m_doneAction;
        
        public GGUIWndStoreReviewsMain() : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoStoreReviewsMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoStoreReviewsMain.objName; } }
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
            //如果未完成商店评价，记录关闭时间，进入cd
            if(!GameSetting.instance.getIsFinishStoreReviews())
                GameSetting.instance.setStoreReviewsCdStartTimeMs(FpsAndPingMgr.instance.serverTimeTag);

            if (null != _m_doneAction)
                _m_doneAction();
            _m_doneAction = null;
            
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnPraise, _onClickPraise);
            ALUGUICommon.uncombineBtnClick(wnd.btnRoast, _onClickRoast);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnPraise, _onClickPraise);
            ALUGUICommon.combineBtnClick(wnd.btnRoast, _onClickRoast);
        }

        public void setInfo(Action _doneAction)
        {
            _m_doneAction = _doneAction;
        }

        #region 点击事件

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_btn"></param>
        private void _onClickClose(GameObject _btn)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_STORE_REVIEWS_MAIN);
        }

        /// <summary>
        /// 点击赞扬按钮
        /// </summary>
        /// <param name="_btn"></param>
        private void _onClickPraise(GameObject _btn)
        {
            //发送埋点-商店好评界面点击赞扬
            GCommon.sendStepReport(TraceConst.STORE_REVIEWS_CLICK_PRAISE);
            //发送奖励
            NPGSClientListener.sendRequestByLog(new GC2GS_007_034_ReqStoreReviewsGetReward(), new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_007_034_RetStoreReviewsGetReward>(null));
            //设置已完成商店评价
            GameSetting.instance.setIsFinishStoreReviews(true);
            NPPlayer.instance.playerInfoComp.clientDataRemarkInfo.setIsFinishStoreReviews(true);
            //关闭界面
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_STORE_REVIEWS_MAIN);
            //打开商店评价界面
            if (SDKMgr.instance.isUseSDK)
            {
#if UNITY_IOS
                SDKMgr.instance.sys_appReview();
#elif UNITY_ANDROID      
                //谷歌渠道打开谷歌商店的评价界面
                if (MainCameraMono.selfInstance.platType == EWCGPlatType.MJ_GOOGLE_RU || MainCameraMono.selfInstance.platType == EWCGPlatType.MJ_GOOGLE_US)
                    SDKMgr.instance.google_appReview();
#endif
            }
        }

        /// <summary>
        /// 点击吐槽按钮
        /// </summary>
        /// <param name="_btn"></param>
        private void _onClickRoast(GameObject _btn)
        {
            //发送埋点-商店好评界面点击吐槽
            GCommon.sendStepReport(TraceConst.STORE_REVIEWS_CLICK_ROAST);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndStoreReviewsRoast.instance, GGUIWndStoreReviewsRoast.instance.showWnd, UINodeTagConst.C_STORE_REVIEWS_ROAST);
        }

#endregion
    }
}
