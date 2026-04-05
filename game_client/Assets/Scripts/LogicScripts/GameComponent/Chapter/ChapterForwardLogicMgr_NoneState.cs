using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public partial class ChapterForwardLogicMgr
    {
        // Idle的状态
        private class NoneState : _AChapterBaseState
        {
            public NoneState(ChapterForwardLogicMgr _chapterForwardLogicMgr) : base(_chapterForwardLogicMgr){}

            public override EChapterForwardType state { get { return EChapterForwardType.NONE; } }

            protected override void _onEnter()
            {
                
            }

            protected override void _onExit()
            {
                
            }

            protected override void _onTick(float _deltaTime)
            {
                
            }

            public override bool canEnterState(_ATALStateBase<EChapterForwardType> _newState)
            {
                return true;
            }

            public override void resetData()
            {
                
            }
        }
    }
}