using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsBuildingTimeBuffExplain : _ATALBasicUIWnd<GGUIMonoMarsBuildingTimeBuffExplain>
    {
        [NotNull] public static GGUIWndMarsBuildingTimeBuffExplain instance { get { return _g_instance ??= new GGUIWndMarsBuildingTimeBuffExplain(); } }
        private static GGUIWndMarsBuildingTimeBuffExplain _g_instance;


        public GGUIWndMarsBuildingTimeBuffExplain() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        

        protected override string _monoAssetPath { get { return GGUIMonoMarsBuildingTimeBuffExplain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsBuildingTimeBuffExplain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            refreshWnd();
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

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }


        public void refreshWnd() 
        {
            if (wnd == null || !_m_bIsShow)
                return;
        }
        

        private void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Mars.C_MARS_BUILDING_TIME_BUFF_EXPLAIN);
        }
    }
}