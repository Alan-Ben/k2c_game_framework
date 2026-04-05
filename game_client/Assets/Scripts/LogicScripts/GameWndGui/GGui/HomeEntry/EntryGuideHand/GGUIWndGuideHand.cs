
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 入口引导手指
    /// </summary>
    public class GGUIWndGuideHand : _ANPGGUIBasicWnd<GGUIMonoEntryGuideHand>
    {
        [NotNull] private readonly GResPathIndex _m_index;
        private long _m_showSerialize;
        private Transform _m_guideTarget;
        

        public GGUIWndGuideHand(int _resId, EALUIWndLayer _layer = EALUIWndLayer.TOP_NOOP) 
            : base(_layer)
        {
            _m_index = new GResPathIndex(_resId);
        }
        

        protected override string _monoAssetPath { get { return _m_index.assetPath; } }
        protected override string _monoObjName { get { return _m_index.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            refreshWnd();
            
            if (wnd == null)
                return;

            _m_showSerialize = ALSerializeOpMgr.next();
            long serialize = _m_showSerialize;

            ALCommonActionMonoTask.addMonoTask(() =>
            {
                if (serialize != _m_showSerialize || !isShow)
                    return;

                hideWnd();
            },wnd.delayHideTime);
        }
        protected override void _onHideWnd()
        {
            _m_showSerialize = ALSerializeOpMgr.next();
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


        public void refreshWnd(Transform _guideTarget)
        {
            _m_guideTarget = _guideTarget;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            GCommon.tryGetUIObjUILocalPos(wnd.transform.parent as RectTransform, _m_guideTarget as RectTransform, out Vector2 uiPos);
            ALUGUICommon.setUIPos(wnd.transform, uiPos);
        }
    }
}