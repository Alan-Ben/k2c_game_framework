using System;
using ALPackage;

namespace GOE
{
    public class MainAdditionEmptyTDScene : _AALBasicSubContainerScene_NoChild
    {
        public static MainAdditionEmptyTDScene instance
        {
            get
            {
                if (_m_instance == null)
                    _m_instance = new MainAdditionEmptyTDScene();

                return _m_instance;
            }
        }
        private static MainAdditionEmptyTDScene _m_instance;
        
        private MainAdditionEmptyTDScene() 
            : base((int)ENPSceneType.TD_SCENE)
        {
        }

        protected override void _onEnterScene()
        {
            setSceneInited();
        }

        protected override void _onSceneInited()
        {
        }

        public override bool needDiscardOnSwitch { get { return false; } }
        
        protected override void _dealQuitScene()
        {
        }

        public override void _dealShowScene(Action _delegate)
        {
            _delegate?.Invoke();
        }

        public override void _dealHideScene(Action _delegate)
        {
            _delegate?.Invoke();
        }

        public override void onSwitchHideScene()
        {
        }
    }
}