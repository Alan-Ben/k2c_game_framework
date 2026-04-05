using ALPackage;
using GC2GS.p041_MarsExploreOp;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsExplore : _ANPGGUIBasicWnd<GGUIMonoMarsExplore>
    {
        private static GGUIWndMarsExplore _g_instance;
        [NotNull] public static GGUIWndMarsExplore instance { get { return _g_instance ??= new GGUIWndMarsExplore(); } }
        

        private GGUISubWndMarsExploreTeamMiniInfo _m_subWndTeamMiniInfo;
        private GGUIWndCommonLazyCDCountResume _m_exploreCDWnd;


        protected override string _monoAssetPath { get { return GGUIMonoMarsExplore.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsExplore.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        public GGUIWndMarsExplore() 
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override void _onShowWnd()
        {
            _m_subWndTeamMiniInfo?.showWnd();
            _m_exploreCDWnd?.showWnd();
            
            refreshWnd();

            NPPlayer.instance.marsComp.exploreSubComponent.onExploreDataChg += refreshLevelProcess;
            
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_MARS_EXPLORE, _simulateClickExplore);
        }
        protected override void _onHideWnd()
        {
            NPPlayer.instance.marsComp.exploreSubComponent.onExploreDataChg -= refreshLevelProcess;
            
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_MARS_EXPLORE, _simulateClickExplore);
            
            _m_subWndTeamMiniInfo?.hideWnd();
            _m_exploreCDWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_subWndTeamMiniInfo?.resetWnd();
            _m_exploreCDWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_subWndTeamMiniInfo?.discard();
            _m_subWndTeamMiniInfo = null;
            _m_exploreCDWnd?.discard();
            _m_exploreCDWnd = null;
            
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnLevelDetail, _onClickLevelDetail);
            ALUGUICommon.uncombineBtnClick(wnd.btnExplore, _onClickExplore);
            ALUGUICommon.uncombineBtnClick(wnd.btnRecord, _onClickRecord);
            ALUGUICommon.uncombineBtnClick(wnd.btnMineShare, _onClickMineShare);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoTeamMiniInfo != null)
                _m_subWndTeamMiniInfo = new GGUISubWndMarsExploreTeamMiniInfo(wnd.monoTeamMiniInfo);
            if (wnd.monoExploreLazyCD != null)
                _m_exploreCDWnd = new GGUIWndCommonLazyCDCountResume(wnd.monoExploreLazyCD);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnLevelDetail, _onClickLevelDetail);
            ALUGUICommon.combineBtnClick(wnd.btnExplore, _onClickExplore);
            ALUGUICommon.combineBtnClick(wnd.btnRecord, _onClickRecord);
            ALUGUICommon.combineBtnClick(wnd.btnMineShare, _onClickMineShare);
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            refreshLevelProcess();
            _m_subWndTeamMiniInfo?.refreshWnd();
            _m_exploreCDWnd?.setInfo(GRefdataCoreMgr.instance.npGeneral.mars_explore_cd);
        }
        public void refreshLevelProcess()
        {
            MarsExploreSubComponent exploreComp = NPPlayer.instance.marsComp.exploreSubComponent;
            MarsExploreLvlRefObj curLevelRef = exploreComp.levelRef;
            MarsExploreLvlRefObj nextLevelRef = exploreComp.nextLevelRef;

            if (curLevelRef != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtExploreLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, curLevelRef.explore_level));
                long curExploreNum = exploreComp.exploreNumTotal;
                long needExploreNum = curLevelRef.upgrade_need_explore_num;
                ALUGUICommon.setLabelTxt(wnd.txtLevelUpProcess, TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreLevelUpNeed_num, needExploreNum - curExploreNum));
                
                if (wnd.sldLevelUpProcess != null)
                {
                    wnd.sldLevelUpProcess.minValue = 0;
                    wnd.sldLevelUpProcess.maxValue = needExploreNum;
                    wnd.sldLevelUpProcess.value = curExploreNum;
                }
            }

            wnd.setLevelMax(nextLevelRef == null);
        }


        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_EXPLORE);
        }
        private void _onClickLevelDetail(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsExploreLevelDetail.instance, GGUIWndMarsExploreLevelDetail.instance.showWnd, UINodeTagConst.C_MARS_EXPLORE_LEVEL_DETAIL);
        }
        private void _simulateClickExplore(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;
            
            long posId = (long)_objects[0];
            int eventCount = NPPlayer.instance.marsComp.exploreSubComponent.eventCount;
            int maxEventCount = NPPlayer.instance.marsComp.exploreSubComponent.levelRef.explore_event_exist_limit;
            if (eventCount >= maxEventCount)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.mars_exploreEventMaxTip_none);
                return;
            }

            if (!GCommon.isItemEnough(ENPItemType.LAZY_CD, GRefdataCoreMgr.instance.npGeneral.mars_explore_cd, 1, true))
                return;

            if (posId > 0)
            {
                NPGSClientListener.sendMsgByLog(new GC2GS_041_001_ReqBuildExploreEventByPos(posId));
                return;
            }
            
            NPGSClientListener.sendMsgByLog(new GC2GS_041_002_ReqBuildExploreEvent());
        }
        private void _onClickExplore(GameObject _go)
        {
            int eventCount = NPPlayer.instance.marsComp.exploreSubComponent.eventCount;
            int maxEventCount = NPPlayer.instance.marsComp.exploreSubComponent.levelRef.explore_event_exist_limit;
            if (eventCount >= maxEventCount)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.mars_exploreEventMaxTip_none);
                return;
            }

            if (!GCommon.isItemEnough(ENPItemType.LAZY_CD, GRefdataCoreMgr.instance.npGeneral.mars_explore_cd, 1, true))
                return;
            
            NPGSClientListener.sendMsgByLog(new GC2GS_041_002_ReqBuildExploreEvent());
        }
        private void _onClickRecord(GameObject _go)
        {
            GGUIWndMarsExplorePvPLog.instance.showWnd();
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsExplorePvPLog.instance, GGUIWndMarsExplorePvPLog.instance.showWnd, EUIQueueStageType.MAIN, UINodeTagConst.C_MARS_EXPLORE_PVP_LOG, false, false);
        }
        private void _onClickMineShare(GameObject _go)
        {
            GGUIWndMarsExploreMineShare.instance.showWnd();
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsExploreMineShare.instance, GGUIWndMarsExploreMineShare.instance.showWnd, EUIQueueStageType.MAIN, UINodeTagConst.C_MARS_EXPLORE_MINE_SHARE, false, false);
        }
    }
}
