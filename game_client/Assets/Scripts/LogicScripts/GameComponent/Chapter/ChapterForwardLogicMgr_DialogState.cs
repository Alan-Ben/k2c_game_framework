using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public partial class ChapterForwardLogicMgr
    {
        // boss战等待连线状态
        private class DialogState : _AChapterBaseState
        {
            private long _m_serialId = 0;
            private long _m_dialogId = 0;
            private long _m_pointId = 0;
            private bool _m_isNeedTip = false;
            private NPGTextureIndex _m_dialogTextureIndex = null;
            private long _m_tipResId = 0;
            private string _m_name;
            private Action _m_dialogEndCallback;//对话结束回调，如果没有就执行回idle状态
            
            public DialogState(ChapterForwardLogicMgr _chapterForwardLogicMgr) : base(_chapterForwardLogicMgr){}
            public override EChapterForwardType state { get { return EChapterForwardType.DIALOG; } }

            public void setDialogId(long _dialogId, long _pointId, bool _isNeedTip, long _tipResId = 0, NPGTextureIndex _textureIndex = null, string _name = null)
            {
                _m_dialogId = _dialogId;
                _m_pointId = _pointId;
                _m_isNeedTip = _isNeedTip;
                _m_dialogTextureIndex = _textureIndex;
                _m_name = _name;
                _m_tipResId = _tipResId;
            }

            public void setNextStateAction(Action _nextStateAction)
            {
                _m_dialogEndCallback = _nextStateAction;
            }
            
            protected override void _onEnter()
            {
                _m_serialId++;
                long serialId = _m_serialId;
                
                if (_m_dialogId == 0)
                {
                    _changeState(EChapterForwardType.IDLE);
                    return;
                }

                //先看是否需要提示
                if (_m_isNeedTip && null != _m_dialogTextureIndex)
                {
                    _m_forwardLogicMgr.showDialogTipEffect();
                    _m_forwardLogicMgr.showDialogSliderTip(_m_pointId, () =>
                    {
                        _enterDialogueNode(serialId);
                    });
                }
                else
                {
                    _enterDialogueNode(serialId);
                }
                
                _m_forwardLogicMgr._dealPlayForwardVideoAniState(EChapterVideoForwardState.NORMAL);
            }

            protected override void _onExit()
            {
                _m_serialId++;
                _m_dialogId = 0;
                _m_pointId = 0;
                _m_isNeedTip = false;
                _m_dialogTextureIndex = null;
                _m_name = null;
            }
            
            protected override void _onTick(float _deltaTime)
            {

            }

            public override bool canEnterState(_ATALStateBase<EChapterForwardType> _newState)
            {
                return _newState.state is EChapterForwardType.IDLE
                       || _newState.state is EChapterForwardType.FIGHT_BOSS_PER
                       || _newState.state is EChapterForwardType.CHAPTER_COMPLETE
                       || _newState.state is EChapterForwardType.FIGHT_BOSS
                       || _newState.state is EChapterForwardType.NODE_MOVE_BOSS 
                       || _newState.state is EChapterForwardType.NODE_MOVE_NORMAL;
            }

            public override void resetData()
            {
                _m_dialogId = 0;
                _m_pointId = 0;
                _m_isNeedTip = false;
                _m_dialogTextureIndex = null;
                _m_name = null;
                _m_dialogEndCallback = null;
            }
            
            private void _enterDialogueNode(long _serialId)
            {
                GCommon.enterDialogueNode(_m_dialogId, () =>
                {
                    if(_serialId != _m_serialId)
                        return;

                    //对话结束有回调直接执行回调
                    if (_m_dialogEndCallback != null)
                    {
                        _m_dialogEndCallback();
                        _m_dialogEndCallback = null;
                    }
                    else if (NPPlayer.instance.chapterComp.curIsBossPoint())
                    {
                        if(_serialId != _m_serialId)
                            return;
                        ALCommonTaskController.CommonActionAddNextFrameTask(() =>
                        {
                            if(_serialId != _m_serialId)
                                return;
                            _changeState(EChapterForwardType.FIGHT_BOSS_PER);
                        });
                    }
                    else
                    {
                        _changeState(EChapterForwardType.IDLE);
                    }
                });
            }
        }
    }
}