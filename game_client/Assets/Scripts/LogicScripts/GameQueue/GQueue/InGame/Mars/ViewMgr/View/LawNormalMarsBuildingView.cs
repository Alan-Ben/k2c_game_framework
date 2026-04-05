using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 律令所正常状态视图
    /// </summary>
    public class LawNormalMarsBuildingView : CommonNormalMarsBuildingView
    {
        public LawNormalMarsBuildingView([NotNull] MarsBuildingInfo _buildingInfo) 
            : base(_buildingInfo)
        {
        }


        protected override void _triggerClickInternal()
        {
            // 火星智能控制主界面
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndMarsIntelligentControl.instance, UINodeTagConst.C_MARS_INTELLIGENT_CONTROL, null,
                () =>
                {
                    GGUIWndMarsIntelligentControl.instance.showWnd();
                },
                0);
        }
    }
}
