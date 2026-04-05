using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIPrefabSubWndAdultEngageRequestSendGuild : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoAdultEngageRequestSendGuild>
    {
        private GGUISubWndAdultEngageRequestSendGuildContainer _m_recommendContainer;
        private GGUISubWndChildInfo _m_adultInfoWnd;
        
        private AdultInfo _m_myAdultInfo;
        private int _m_refreshSerialize;
        
        
        public GGUIPrefabSubWndAdultEngageRequestSendGuild(Transform _parent) 
            : base(_parent)
        {
        }
        

        protected override string _monoAssetPath { get { return GGUIMonoAdultEngageRequestSendGuild.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoAdultEngageRequestSendGuild.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        { 
            _m_recommendContainer?.showWnd();
            _m_adultInfoWnd?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        { 
            _m_recommendContainer?.hideWnd();
            _m_adultInfoWnd?.hideWnd();
        }
        protected override void _onReset()
        { 
            _m_recommendContainer?.resetWnd();
            _m_adultInfoWnd?.resetWnd();
        }
        protected override void _onDiscard()
        { 
            _m_recommendContainer?.discard();
            _m_recommendContainer = null;
            _m_adultInfoWnd?.discard();
            _m_adultInfoWnd = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnRandom, _onRandomBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoRecommendContainer != null)
                _m_recommendContainer = new GGUISubWndAdultEngageRequestSendGuildContainer(wnd.monoRecommendContainer);
            if (wnd.monoAdultInfo != null)
                _m_adultInfoWnd = new GGUISubWndChildInfo(wnd.monoAdultInfo);
            
            ALUGUICommon.combineBtnClick(wnd.btnRandom, _onRandomBtnClick);
        }


        public void refreshWnd(AdultInfo _adultInfo)
        {
            _m_myAdultInfo = _adultInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            _m_refreshSerialize = ALSerializeOpMgr.next();
            if (wnd == null || !_m_bIsShow || _m_myAdultInfo == null || _m_recommendContainer == null)
                return;
            
            // wnd.setLoadingShow(true);
            // int serialize = _m_infoSerialize;
            // NPGSClientListener.sendRequestByLog(GSWriter_014_ChildOp.make_012_ReqGetRecommendPlayerList(_m_myAdultInfo.id),
            //     new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_014_012_RetGetRecommendPlayerList>(
            //         (_isSuc, _msg) =>
            //         {
            //             if (serialize != _m_infoSerialize)
            //                 return;
            //             
            //             wnd.setLoadingShow(false);
            //             List<Adult_PoolBaseInfo> serverList = _msg.getMatchList();
            //             List<PoolSimpleAdultInfo> recommendList = serverList.ConvertAll(_serverInfo => new PoolSimpleAdultInfo(_serverInfo.getApplyAdultId(), _serverInfo.getApplyCid()));
            //             _m_recommendContainer?.refreshWnd(recommendList);
            //         }));
            //
            // _m_adultInfoWnd?.refreshWnd(_m_myAdultInfo);
        }
        
        
        private void _onRandomBtnClick(GameObject _obj)
        {
        }
    }
}