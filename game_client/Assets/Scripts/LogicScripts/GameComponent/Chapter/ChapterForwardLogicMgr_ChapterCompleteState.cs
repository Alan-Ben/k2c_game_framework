using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public partial class ChapterForwardLogicMgr
    {
        // Idle的状态
        private class ChapterCompleteState : _AChapterBaseState
        {
            public ChapterCompleteState(ChapterForwardLogicMgr _chapterForwardLogicMgr) : base(_chapterForwardLogicMgr){}

            public override EChapterForwardType state { get { return EChapterForwardType.CHAPTER_COMPLETE; } }

            protected override void _onEnter()
            {
                if (QueueMgr.instance.findLastNode(typeof(GNodeChapterMap)) is GNodeChapterMap _chapterNode)
                {
                    _chapterNode.setNeedShowNewChapterUnlock();
                    QueueMgr.instance.QuitUntilCanStop((_node) => _node == _chapterNode);
                }
                else
                {
                    QueueMgr.instance.AddNode(new GNodeChapterMap(true));
                }
                
                _changeState(EChapterForwardType.IDLE);
            }

            protected override void _onExit()
            {
                
            }

            protected override void _onTick(float _deltaTime)
            {
                
            }

            public override bool canEnterState(_ATALStateBase<EChapterForwardType> _newState)
            {
                return _newState.state is EChapterForwardType.IDLE ||
                       _newState.state is EChapterForwardType.ONCE_FORWARD;
            }

            public override void resetData()
            {
                
            }
        }
    }
}