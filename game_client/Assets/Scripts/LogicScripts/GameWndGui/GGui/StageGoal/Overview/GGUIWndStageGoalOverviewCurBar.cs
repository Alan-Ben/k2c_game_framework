using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 阶段目标预览当前阶段bar
    /// </summary>
    public class GGUIWndStageGoalOverviewCurBar : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoStageGoalOverviewCurBar>
    {
        public GGUIWndStageGoalOverviewCurBar(Transform _parent) : base(_parent)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoStageGoalOverviewCurBar.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoStageGoalOverviewCurBar.objName; } }
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

        /// <summary>
        /// 重置到达新阶段动画
        /// </summary>
        public void resetStageAni()
        {
            wnd?.newStageAni?.resetAni();
        }

        /// <summary>
        /// 设置动画
        /// </summary>
        /// <param name="_normalizeTime"></param>
        public void sampleStageAni(float _normalizeTime)
        {
            wnd?.newStageAni?.sample(_normalizeTime);
        }

        /// <summary>
        /// 播放到达新阶段动画
        /// </summary>
        public void playNewStageAni(Action _onPlayDone)
        {
            if(wnd == null || wnd.newStageAni == null)
                _onPlayDone?.Invoke();
            else
                wnd.newStageAni.forcePlay(_onPlayDone);
        }
    }
}
