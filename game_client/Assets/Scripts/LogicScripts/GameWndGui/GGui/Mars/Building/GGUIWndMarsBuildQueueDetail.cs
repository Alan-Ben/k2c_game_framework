
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsBuildQueueDetail : _ANPGGUIBasicWnd<GGUIMonoMarsBuildQueueDetail>
    {
        [NotNull] public static GGUIWndMarsBuildQueueDetail instance { get { return _g_instance ??= new GGUIWndMarsBuildQueueDetail(); } }
        private static GGUIWndMarsBuildQueueDetail _g_instance;


        private GGUISubWndMarsBuildQueueDetailContainer _m_detailContainer;
        private ALCommonEnableTaskController _m_tickTask;


        public GGUIWndMarsBuildQueueDetail()
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoMarsBuildQueueDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsBuildQueueDetail.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_detailContainer?.showWnd();

            refreshWnd();

            _m_tickTask = ALCommonEnableTickActionMonoTask.addMonoTask(_tick);

            NPPlayer.instance.marsComp.buildingSubComponent.onQueueListChg += refreshWnd;
        }
        protected override void _onHideWnd()
        {
            NPPlayer.instance.marsComp.buildingSubComponent.onQueueListChg -= refreshWnd;
            
            _m_tickTask.setDisable();

            _m_detailContainer?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_detailContainer?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_detailContainer?.discard();
            _m_detailContainer = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoContainer != null)
                _m_detailContainer = new GGUISubWndMarsBuildQueueDetailContainer(wnd.monoContainer);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }


        public void refreshWnd()
        {
            if (wnd == null || !isShow)
                return;

            _m_detailContainer?.refreshWnd();
        }


        private void _tick()
        {
            _m_detailContainer?.tickRefresh();
            if (NPPlayer.instance.marsComp.buildingSubComponent.hasLeisureQueue)
                _onBtnCloseClick(null);
        }
        private void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_BUILD_QUEUE_DETAIL);
        }
    }
}