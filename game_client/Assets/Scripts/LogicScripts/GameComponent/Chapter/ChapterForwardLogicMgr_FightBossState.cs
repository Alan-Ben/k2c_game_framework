using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public partial class ChapterForwardLogicMgr
    {
        // 单次前进的状态
        private class FightBossState : _AChapterBaseState
        {
            private long _m_serialId = 0;
            //经历时间
            private float _m_dealTime = 0;
            
            public FightBossState(ChapterForwardLogicMgr _chapterForwardLogicMgr) : base(_chapterForwardLogicMgr){}

            public override EChapterForwardType state { get { return EChapterForwardType.FIGHT_BOSS; } }

            protected override void _onEnter()
            {
                _m_serialId++;
                long serialId = _m_serialId;

                //切换boss战状态
                _m_forwardLogicMgr._dealPlayForwardVideoAniState(EChapterVideoForwardState.BOSS);
                
                long oldChapterId = NPPlayer.instance.chapterComp.curChapterId;
                _m_forwardLogicMgr._dealChapterBossEffect(NPPlayer.instance.chapterComp.chapterRefObj, () =>
                {
                    if(serialId != _m_serialId)
                        return;

                    if(oldChapterId < NPPlayer.instance.chapterComp.curChapterId)
                    {
                        _enterChapterCompleteState(oldChapterId);
                    }
                    else
                    {
                        _changeState(EChapterForwardType.IDLE);
                    }
                });
            }

            protected override void _onExit()
            {
                _m_serialId++;
                _m_dealTime = 0;
            }
            
            protected override void _onTick(float _deltaTime)
            {
                _m_dealTime += _deltaTime;
                
                // //如果自动前进，要帮他自动选buff，自动关闭弹窗
                // if (_m_forwardLogicMgr.isInAutoForward && _m_dealTime > 0.5f)
                // {
                //     if(GGUIWndChapterBossMain.instance.isShow)
                //         GGUIWndChapterBossMain.instance.aiControlNext();
                //     _m_dealTime = 0;
                // }
            }

            public override bool canEnterState(_ATALStateBase<EChapterForwardType> _newState)
            {
                return _newState.state is EChapterForwardType.IDLE ||
                       _newState.state is EChapterForwardType.DIALOG ||
                       _newState.state is EChapterForwardType.CHAPTER_COMPLETE;
            }

            public override void resetData()
            {
                _m_dealTime = 0;
                _m_serialId = 0;
            }
            
            private void _enterChapterCompleteState(long _oldChapterId)
            {
                //进入章节完成状态
                _m_forwardLogicMgr._m_stateMachine?.changeState<ChapterCompleteState>(
                    (_state) =>
                    {

                    });
            }
        }
    }
}