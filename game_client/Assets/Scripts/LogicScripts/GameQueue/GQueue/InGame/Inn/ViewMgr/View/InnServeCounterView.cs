using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class InnServeCounterView : _AALBasicLoadObj
    {
        [NotNull] private readonly InnViewMgr _m_viewMgr;
        private GGUICommonFollowTarget _m_followTarget;
        private GTDMonoInnServeCounter _m_mono;
        private int _m_serialize;

        
        public InnServeCounterView([NotNull] InnViewMgr _viewMgr)
        {
            _m_viewMgr = _viewMgr;
        }

        
        public GTDMonoInnServeCounter mono { get { return _m_mono; } }


        protected override void _loadOp()
        {
            _m_mono = MainAdditionInnTDScene.instance.getServeCounter();
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

        
        public void startServing(InnGuestInfo _guestInfo)
        {
            if (_guestInfo == null)
                return;

            ServingShow servingShow = new ServingShow(this, _guestInfo);
            servingShow.play();
        }


        private void _onInitDone()
        {
            if (_m_mono == null)
                return;
            
            if (_m_mono.followTarget != null)
            {
                _m_followTarget = new GGUICommonFollowTarget(_m_mono.followTarget, Vector3.zero);
                GGUIWndInnHud.instance.regInstance(_m_followTarget);
            }
        }
        private void _onDiscard()
        {
            _m_serialize = ALSerializeOpMgr.next();
            
            GGUIWndInnHud.instance.removeInstance(_m_followTarget);
            _m_followTarget?.discard();
        }


        private class ServingShow
        {
            [NotNull] private readonly InnServeCounterView _m_view;
            [NotNull] private readonly InnGuestInfo _m_guestInfo;
            private readonly int _m_serialize;
            
            
            public ServingShow([NotNull] InnServeCounterView _view, [NotNull] InnGuestInfo _guestInfo)
            {
                _m_view = _view;
                _m_serialize = _view._m_serialize;
                _m_guestInfo = _guestInfo;
            }
            
            
            public void play()
            {
                if (_m_view._m_mono == null)
                    return;
                
                _m_view._m_followTarget?.addController(new GGUIWndInnServeShowFollowerController(_m_guestInfo));
                ALCommonActionMonoTask.addMonoTask(() =>
                {
                    if (_m_view._m_serialize  != _m_serialize)
                        return;
                    
                    if (_m_view._m_mono.serveAnimation != null)
                        _m_view._m_mono.serveAnimation.ForcePlay(_m_view._m_mono.serveAnimationName);
                }, _m_view._m_mono.servingAnimationDelay);
                ALCommonActionMonoTask.addMonoTask(() =>
                {
                    if (_m_view._m_serialize != _m_serialize)
                        return;
                    
                    _m_view._m_viewMgr.guestDataMgr.setGuestSettled(_m_guestInfo.guestInstanceId);
                    _m_view._m_viewMgr.refreshGuestNum();
                    _m_view._m_viewMgr.refreshHadSettledGuestNum();
                    _m_view._m_viewMgr.cashRegister.popRewardGainTip(_m_guestInfo.calculatePopularityGain(), _m_guestInfo.calculateFinesseGain(), _m_guestInfo.calculateAffectionGain());
                    _m_view._m_viewMgr.cashRegister.playRewardAddEffect();
                }, _m_view._m_mono.servingCompleteDelay);
            }
        }
    }
}