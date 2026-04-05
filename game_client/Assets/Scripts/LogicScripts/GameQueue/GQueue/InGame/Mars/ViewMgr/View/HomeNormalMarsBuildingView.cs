using JetBrains.Annotations;

namespace GOE
{
    public class HomeNormalMarsBuildingView : CommonNormalMarsBuildingView
    {
        private GGUIWndMarsHomeCollectBtnFollowerController _m_collectBtnWnd;

        public HomeNormalMarsBuildingView([NotNull] MarsBuildingInfo _buildingInfo) 
            : base(_buildingInfo)
        {
        }


        protected override void _triggerClickInternal()
        {
            GGUIWndMarsBuildingHomeInfo.instance.refreshWnd(this);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsBuildingHomeInfo.instance, GGUIWndMarsBuildingHomeInfo.instance.showWnd,
                EUIQueueStageType.MAIN ,UINodeTagConst_Mars.C_MARS_BUILDING_HOME_INFO, false, false);
        }


        protected override void _onInitDone()
        {
            base._onInitDone();

            if (followTarget != null)
            {
                _m_collectBtnWnd = new GGUIWndMarsHomeCollectBtnFollowerController();

                GGUIWndMarsHud.instance.addController(followTarget, _m_collectBtnWnd);
            }
        }
        protected override void _onDiscard()
        {
            base._onDiscard();

            _m_collectBtnWnd?.discard();
            _m_collectBtnWnd = null;
        }


        public override void tick()
        {
            base.tick();
            _m_collectBtnWnd?.tick();
        }
    }
}