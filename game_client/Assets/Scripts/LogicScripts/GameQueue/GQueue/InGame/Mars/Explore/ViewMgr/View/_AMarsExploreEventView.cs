using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public abstract class _AMarsExploreEventView : _AALBasicLoadObj
    {
        public _AMarsExploreEventInfo eventInfo { get; }

        protected _AMarsExploreEventView([NotNull] _AMarsExploreEventInfo _eventInfo)
        {
            eventInfo = _eventInfo;
        }

        public abstract Vector3 position { get; }
        public virtual void tick() { }
        public virtual void triggerClick() { }
        public abstract void updateView(bool _newEventStacked);
        public abstract void setNeedShowLoadedEffect();
    }

    public abstract class _AMarsExploreEventView<TEventInfo, TMono> : _AMarsExploreEventView
        where TEventInfo : _AMarsExploreEventInfo
        where TMono : GTDMonoMarsExploreEventBase
    {
        [NotNull] private readonly MarsExploreViewMgr _m_viewMgr;
        [NotNull] private readonly TEventInfo _m_eventInfo;
        private GGUICommonFollowTarget _m_followTarget;
        private TMono _m_mono;
        private int _m_serialize;
        
        protected bool _m_needShowLoadedEffect;


        protected _AMarsExploreEventView([NotNull] MarsExploreViewMgr _viewMgr, [NotNull] TEventInfo _eventInfo)
            : base(_eventInfo)
        {
            _m_viewMgr = _viewMgr;
            _m_eventInfo = _eventInfo;
        }


        [NotNull] public MarsExploreViewMgr viewMgr { get { return _m_viewMgr; } }
        [NotNull] public TEventInfo specificEventInfo { get { return _m_eventInfo; } }
        public TMono mono { get { return _m_mono; } }
        public GGUICommonFollowTarget followTarget { get { return _m_followTarget; } }
        public override Vector3 position
        {
            get
            {
                if (_m_mono == null)
                    return Vector3.zero;
                return _m_mono.transform.position;
            }
        }
        
        
        public override void setNeedShowLoadedEffect()
        {
            _m_needShowLoadedEffect = true;
        }


        protected override void _loadOp()
        {
            long posId = _m_eventInfo.posId;
            GTDMonoMarsExploreEventPos eventPos = MainAdditionMarsExploreTDScene.instance.getEventPos(posId);
            if (eventPos == null)
            {
                _setLoadDone();
                return;
            }

            Vector3 position = eventPos.transform.position;
            NPGGoIndex goIndex = _m_eventInfo.refObj.scene_go_index;
            MainAdditionMarsExploreTDScene.instance.createUnit<TMono>(goIndex, position, _mono =>
            {
                _m_mono = _mono;
                if (_m_mono != null)
                    _onInitDone();
                _setLoadDone();
            });
        }
        protected override void _discard()
        {
            _onDiscard();

            if (_m_mono != null)
            {
                NPGGoIndex goIndex = _m_eventInfo.refObj.scene_go_index;
                MainAdditionMarsExploreTDScene.instance.discardUnit(goIndex, _m_mono);
                _m_mono = null;
            }
        }


        protected virtual void _onInitDone()
        {
            if (_m_mono == null)
                return;

            if (_m_mono.followTarget != null)
            {
                _m_followTarget = new GGUICommonFollowTarget(_m_mono.followTarget, Vector3.zero);
                GGUIWndMarsExploreHUDRoot.instance.regInstance(_m_followTarget);
            }

            if (_m_mono.monoClick != null)
                _m_mono.monoClick.onClick += triggerClick;
            
            if (_m_needShowLoadedEffect)
            {
                _m_needShowLoadedEffect = false;
                MainAdditionMarsExploreTDScene.instance.showEventLoadedEffect(position);
            }
        }
        protected virtual void _onDiscard()
        {
            _m_serialize = ALSerializeOpMgr.next();
            
            if (_m_followTarget != null)
            {
                GGUIWndMarsExploreHUDRoot.instance.removeInstance(_m_followTarget);
                _m_followTarget.discard();
                _m_followTarget = null;
            }

            if (_m_mono == null)
                return;
            
            if (_m_mono.monoClick != null)
                _m_mono.monoClick.onClick -= triggerClick;    
        }
    }
}
