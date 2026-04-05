using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 杰出者大厅主界面
    /// </summary>
    public class GGUIWndGraveMain : _ATALBasicUIWnd<GGUIMonoGraveMain>
    {
        private static GGUIWndGraveMain _g_instance = new GGUIWndGraveMain();
    
        public static GGUIWndGraveMain instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGraveMain();
                return _g_instance;
            }
        }
        
        // <AutoGen:WndDeclaration>
        
        // </AutoGen:WndDeclaration>
        
        public GGUIWndGraveMain() : base(EALUIWndLayer.NORMAL)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoGraveMain.assetPath; }
        protected override string _monoObjName { get => GGUIMonoGraveMain.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
    
        protected override void _onShowWnd()
        {
            _refreshWnd();
            WinMsg.RegisterMsgAct(WinMsgType.ON_GRAVE_NEW_CHANGE, _refreshNewCommoner);
            // <AutoGen:_onShowWnd>
            
            // </AutoGen:_onShowWnd>
        }
    
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GRAVE_NEW_CHANGE, _refreshNewCommoner);
            // <AutoGen:_onHideWnd>
            
            // </AutoGen:_onHideWnd>
        }
    
        protected override void _onReset()
        {
            // <AutoGen:_onReset>
            
            // </AutoGen:_onReset>
        }
    
        protected override void _onDiscard()
        {
            // <AutoGen:_onDiscard>
            ALUGUICommon.uncombineBtnClick(wnd.btnCelebrate, _onClickbtnCelebrate);
            ALUGUICommon.uncombineBtnClick(wnd.btnNewCommer, _onClickbtnNewCommer);
            ALUGUICommon.uncombineBtnClick(wnd.btnBless, _onClickbtnBless);
            // </AutoGen:_onDiscard>
            ALUGUICommon.uncombineBtnClick(wnd.btnNextGraveMain, _onClickbtnNextGraveMain);
            ALUGUICommon.uncombineBtnClick(wnd.btnPreGraveMain, _onClickbtnPreGraveMain);
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            // <AutoGen:_onWndInitDone>
            ALUGUICommon.combineBtnClick(wnd.btnCelebrate, _onClickbtnCelebrate);
            ALUGUICommon.combineBtnClick(wnd.btnNewCommer, _onClickbtnNewCommer);
            ALUGUICommon.combineBtnClick(wnd.btnBless, _onClickbtnBless);
            // </AutoGen:_onWndInitDone>
            ALUGUICommon.combineBtnClick(wnd.btnNextGraveMain, _onClickbtnNextGraveMain);
            ALUGUICommon.combineBtnClick(wnd.btnPreGraveMain, _onClickbtnPreGraveMain);
        }

        public void refreshWnd()
        {
            _refreshWnd();
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
    
            _refreshNewCommoner();
            
            ALUGUICommon.setGameObjEnable(wnd.hasNextGraveMainShowGos, MainAdditionGraveMainTDScene.instance.hasNextGraveMain());

            ALUGUICommon.setGameObjEnable(wnd.hasPreGraveMaineGos, MainAdditionGraveMainTDScene.instance.hasPreGraveMain());
            // <AutoGen:_refreshWnd>


            // </AutoGen:_refreshWnd>
        }
        

        private void _refreshNewCommoner()
        {
            bool hasNewProminent = NPPlayer.instance.graveComp.hasGraveNewReward;
            NPPlayerFixedCDInfo cdInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(GRefdataCoreMgr.instance.npGeneral.grave_celebrate_fixed_cd_id);

            bool canCelebrate = NPPlayer.instance.graveComp.canCelebrate && cdInfo != null && cdInfo.getCount() > 0;
            // <UserCode name="canCelebrateShowGos">
            ALUGUICommon.setGameObjEnable(wnd.canCelebrateShowGos, canCelebrate);
            // </UserCode>
            // <UserCode name="canCelebrateHideGos">
            ALUGUICommon.setGameObjEnable(wnd.canCelebrateHideGos, !canCelebrate);
            // </UserCode>

            // <UserCode name="hasNewCommerShowGos">
            ALUGUICommon.setGameObjEnable(wnd.hasNewCommerShowGos, hasNewProminent);
            // </UserCode>
            // <UserCode name="hasNewCommerHideGos">
            ALUGUICommon.setGameObjEnable(wnd.hasNewCommerHideGos, !hasNewProminent);
            // </UserCode>
        }
        
        // <AutoGen:Method>
        // 庆祝按钮点击事件
        private void _onClickbtnCelebrate(GameObject go)
        {
            // <UserCode name="btnCelebrate">
            if (!NPPlayer.instance.graveComp.canCelebrate)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.grave_congratulate_no_player_achieve_tip));
                return;
            }
            NPPlayerFixedCDInfo cdInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(GRefdataCoreMgr.instance.npGeneral.grave_celebrate_fixed_cd_id);

            if (cdInfo == null || cdInfo.getCount() <= 0)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.grave_congratulate_in_cd_tip));
                return;
            }
            NPPlayer.instance.graveComp.reqGraveCelebrate((_cid, _buffId, _items) =>
            {
                ALProcess process = ALProcess.CreateProcess();
                if (_items != null)
                {
                    List<NPCommonCostItem> gainItemList = new List<NPCommonCostItem>();
                    foreach (var item in _items)
                    {
                        gainItemList.Add(new NPCommonCostItem(item));
                    }
                    process.addDelegateProcess((_done) =>
                    {
                        NPNoticeDealer_GraveCongrats congratsNotice = new NPNoticeDealer_GraveCongrats(false, _cid, gainItemList, _done);
                        NPUINoticeMgr.instance.addDealer(congratsNotice);
                    });
                    process.addDelegateProcess((_done) =>
                    {
                        GCommon.dealGainItem(_items, TransKeyConst.common_getreward_tip, _done);
                    });
                }
                process
                    .addDelegateProcess((_done) =>
                    {
                        NPNoticeDealer_GraveBlessGet buffNotice = new NPNoticeDealer_GraveBlessGet(_buffId, _done);
                        NPUINoticeMgr.instance.addDealer(buffNotice);
                    })
                    .addProcess(() =>
                    {
                        _refreshNewCommoner();
                    })
                    .deal();
            });
            // </UserCode>
        }
        // 新晋者按钮点击事件
        private void _onClickbtnNewCommer(GameObject go)
        {
            // <UserCode name="btnNewCommer">
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGraveNewProminent.instance, GGUIWndGraveNewProminent.instance.showWnd, UINodeTagConst.C_GRAVE_NEW_PROMINENT);
            // </UserCode>
        }
        // 祝福效果按钮点击事件
        private void _onClickbtnBless(GameObject go)
        {
            // <UserCode name="btnBless">
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGraveBlessDetail.instance, GGUIWndGraveBlessDetail.instance.showWnd, UINodeTagConst.C_GRAVE_BLESS_DETAIL);
            // </UserCode>
        }
        // </AutoGen:Method>
        
        private void _onClickbtnNextGraveMain(GameObject go)
        { 
            MainAdditionGraveMainTDScene.instance.changeNextGraveMain();
            _refreshWnd();
        }
        private void _onClickbtnPreGraveMain(GameObject go)
        { 
            MainAdditionGraveMainTDScene.instance.changePreGraveMain();
            _refreshWnd();
        }
    }
}