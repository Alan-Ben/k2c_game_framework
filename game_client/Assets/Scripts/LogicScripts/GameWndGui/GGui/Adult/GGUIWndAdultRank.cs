using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndAdultRank : _ATALBasicUIWnd<GGUIMonoAdultRank>
    {
        [NotNull] public static GGUIWndAdultRank instance { get { return _g_instance ??= new GGUIWndAdultRank(); } }
        private static GGUIWndAdultRank _g_instance;


        private GGUISubWndAdultRankGrid _m_adultGrid;
        

        public GGUIWndAdultRank() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        

        protected override string _monoAssetPath { get { return GGUIMonoAdultRank.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoAdultRank.objName; } }
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
                _m_adultGrid = new GGUISubWndAdultRankGrid(wnd.monoAdultGrid);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;
        }
        

        private void _onCloseBtnClick(GameObject _obj)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }
    }
}