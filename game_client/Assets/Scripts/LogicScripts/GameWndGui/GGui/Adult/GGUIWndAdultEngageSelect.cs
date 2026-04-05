using System.Collections.Generic;
using ALPackage;
using Common.ChildEnum;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndAdultEngageSelect : _ATALBasicUIWnd<GGUIMonoAdultEngageSelect>
    {
        [NotNull] public static GGUIWndAdultEngageSelect instance { get { return _g_instance ??= new GGUIWndAdultEngageSelect(); } }
        private static GGUIWndAdultEngageSelect _g_instance;


        [ItemNotNull, NotNull] private readonly List<UnmarriedInfo> _m_unmarriedInfoList;
        
        private GGUISubWndAdultEngageSelectGrid _m_adultGrid;
        private AdultInfo _m_targetAdultInfo;
        private List<AdultInfo> _m_myAdultList;
        
        
        public GGUIWndAdultEngageSelect() 
            : base(EALUIWndLayer.ADDITION)
        {
            _m_myAdultList = new List<AdultInfo>();
            _m_unmarriedInfoList = new List<UnmarriedInfo>();
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoAdultEngageSelect.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoAdultEngageSelect.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_adultGrid?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_adultGrid?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_adultGrid?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_adultGrid?.discard();
            _m_adultGrid = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoAdultGrid != null)
                _m_adultGrid = new GGUISubWndAdultEngageSelectGrid(wnd.monoAdultGrid);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }
        
        
        public void refreshWnd(AdultInfo _targetAdultInfo, List<AdultInfo> _myAdultList)
        {
            _m_targetAdultInfo = _targetAdultInfo;
            _m_myAdultList = _myAdultList;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_targetAdultInfo == null || _m_myAdultList == null)
                return;
            
            _m_adultGrid?.refreshWnd(_m_targetAdultInfo, _m_myAdultList);
        }
        

        private void _onCloseBtnClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ADULT_ENGAGE_SELECT);
        }
    }
}