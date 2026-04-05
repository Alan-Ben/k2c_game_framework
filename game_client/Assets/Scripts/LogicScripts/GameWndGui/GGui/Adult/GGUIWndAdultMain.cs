using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndAdultMain : _ANPGGUIBasicResBarWnd<GGUIMonoAdultMain>
    {
        [NotNull] public static GGUIWndAdultMain instance { get { return _g_instance ??= new GGUIWndAdultMain(); } }
        private static GGUIWndAdultMain _g_instance;
        
        
        private GGUISubWndAdultMainPageTabList _m_pageTabList;
        

        public GGUIWndAdultMain() 
            : base(EALUIWndLayer.NORMAL)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoAdultMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoAdultMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_pageTabList?.showWnd();

            refreshWnd();
            
            NPPlayer.instance.recordComp.onRecordValueChg += _onRecordValueChg;
        }
        protected override void _onHideWnd()
        {
            NPPlayer.instance.recordComp.onRecordValueChg -= _onRecordValueChg;
            
            _m_pageTabList?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_pageTabList?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_pageTabList?.discard();
            _m_pageTabList = null;
            
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnBack, _onBackBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnRank, _onRankBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnEngageRequest, _onEngageRequestBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.pageTabList != null)
                _m_pageTabList = new GGUISubWndAdultMainPageTabList(wnd.pageTabList);
            
            ALUGUICommon.combineBtnClick(wnd.btnBack, _onBackBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnRank, _onRankBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnEngageRequest, _onEngageRequestBtnClick);
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtAdultTotalNum, TextTranslate.instance.getLanguage(TransKeyConst.adult_totalAdultCount_num, NPPlayer.instance.recordComp.getValue(ENPPlayerRecordParam.ADULT_GAIN_NUM)));
            ALUGUICommon.setLabelTxt(wnd.txtAdultTotalEarnings, TextTranslate.instance.getLanguage(TransKeyConst.adult_totalAdultEarnings_num, NPPlayer.instance.childComp.totalAdultEarnings.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
        }

        
        private void _onBackBtnClick(GameObject _obj)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }
        private void _onRankBtnClick(GameObject _obj)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndAdultRank.instance, GGUIWndAdultRank.instance.showWnd);
        }
        private void _onEngageRequestBtnClick(GameObject _obj)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndAdultEngageRequestReceive.instance, GGUIWndAdultEngageRequestReceive.instance.showWnd);
        }
        private void _onRecordValueChg(ENPPlayerRecordParam _type, long _srcValue, long _value)
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            if (_type == ENPPlayerRecordParam.ADULT_GAIN_NUM)
                ALUGUICommon.setLabelTxt(wnd.txtAdultTotalNum, TextTranslate.instance.getLanguage(TransKeyConst.adult_totalAdultCount_num, NPPlayer.instance.recordComp.getValue(ENPPlayerRecordParam.ADULT_GAIN_NUM)));
        }
    }
}