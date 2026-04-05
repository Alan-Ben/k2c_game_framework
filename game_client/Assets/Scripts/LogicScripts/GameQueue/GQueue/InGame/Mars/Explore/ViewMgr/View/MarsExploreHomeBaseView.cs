using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class MarsExploreHomeBaseView : _AALBasicLoadObj
    {
        [NotNull] private readonly MarsExploreViewMgr _m_viewMgr;
        private GGUIWndMarsExploreHomeBaseHUDNameFollowerController _m_nameWnd;
        private GGUICommonFollowTarget _m_followTarget;
        private GTDMonoMarsExploreHomeBase _m_mono;


        public MarsExploreHomeBaseView([NotNull] MarsExploreViewMgr _viewMgr)
        {
            _m_viewMgr = _viewMgr;
        }


        public GTDMonoMarsExploreHomeBase mono { get { return _m_mono; } }
        public Vector3 position
        {
            get
            {
                if (_m_mono == null)
                    return Vector3.zero;
                return _m_mono.transform.position;
            }
        }


        protected override void _loadOp()
        {
            _m_mono = MainAdditionMarsExploreTDScene.instance.getHomeBase();
            if (_m_mono == null)
            {
                _setLoadDone();
                return;
            }

            _onInitDone();
            _setLoadDone();
        }
        protected override void _discard()
        {
            _onDiscard();
        }


        private void _onInitDone()
        {
            if (_m_mono == null)
                return;

            if (_m_mono.nameTarget != null)
            {
                _m_followTarget = new GGUICommonFollowTarget(_m_mono.nameTarget, Vector3.zero);
                GGUIWndMarsExploreHUDRoot.instance.regInstance(_m_followTarget);

                _m_nameWnd = new GGUIWndMarsExploreHomeBaseHUDNameFollowerController();
                GGUIWndMarsExploreHUDRoot.instance.addController(_m_followTarget, _m_nameWnd);
            }
        }
        private void _onDiscard()
        {
            _m_nameWnd?.discard();
            _m_nameWnd = null;
            
            if (_m_followTarget != null)
            {
                GGUIWndMarsExploreHUDRoot.instance.removeInstance(_m_followTarget);
                _m_followTarget.discard();
                _m_followTarget = null;
            }
        }
    }
}
