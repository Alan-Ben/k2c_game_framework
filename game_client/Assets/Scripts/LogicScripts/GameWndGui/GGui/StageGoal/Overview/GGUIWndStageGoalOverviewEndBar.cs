using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 阶段目标预览结束bar
    /// </summary>
    public class GGUIWndStageGoalOverviewEndBar : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoStageGoalOverviewEndBar>
    {
        public GGUIWndStageGoalOverviewEndBar(Transform _parent) : base(_parent)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoStageGoalOverviewEndBar.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoStageGoalOverviewEndBar.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
        }
    }
}
