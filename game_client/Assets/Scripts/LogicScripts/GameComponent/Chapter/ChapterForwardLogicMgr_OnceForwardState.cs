using System;
using ALPackage;
using CommonEnum;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public partial class ChapterForwardLogicMgr
    {
        // 单次前进的状态
        private class OnceForwardState : _AChapterBaseState
        {
            private long _m_serialId = -1;
            //经历时间
            private float _m_dealTime = 0;
            //等待时间标记
            private float _m_waitTimeTag = 0;
            
            //前进后待处理事件id
            private long _m_curEventId = 0;
            private long _m_curDialogId = 0;
            private int _m_needEnterNodeIndex = 0;

            private long _m_tipResId = 0;
            private NPGTextureIndex _m_curDialogTextureIndex = null;
            private string _m_name;

            //是否处理完
            private bool _m_isDealDone;
            
            public OnceForwardState(ChapterForwardLogicMgr _chapterForwardLogicMgr) : base(_chapterForwardLogicMgr){}

            public override EChapterForwardType state { get { return EChapterForwardType.ONCE_FORWARD; } }

            protected override void _onEnter()
            {
                _m_serialId++;
                long serialId = _m_serialId;
                _m_isDealDone = false;

                //如果是boss战了要去boss战等待状态
                if (NPPlayer.instance.chapterComp.curIsBossPoint())
                {
                    _changeState(EChapterForwardType.FIGHT_BOSS_WAIT);
                    return;
                }

                //如果金币不够直接回idle
                if (!GCommon.isItemEnough(NPPlayer.instance.chapterComp.getGoldCost(), true))
                {
                    //前进失败直接关闭自动
                    _m_forwardLogicMgr.stopAutoForwardToChapter();
                    _m_forwardLogicMgr.stopQuickForwardToChapter();
                    //失败变回idle
                    _changeState(EChapterForwardType.IDLE);
                    //发送埋点-关卡前进消耗不足
                    GCommon.sendStepReport(TraceConst.CHAPTER_FORWARD_COST_ENOUGH.setMarkParam(NPPlayer.instance.chapterComp.curChapterId));
                    return;
                }

                //根据不同情况确认不同表现时间
                _m_waitTimeTag = GRefdataCoreMgr.instance.npGeneral.chapter_normal_foward_delay_time_s;
                if (_m_forwardLogicMgr.isInQuickForward)
                    _m_waitTimeTag = GRefdataCoreMgr.instance.npGeneral.chapter_quick_forward_delay_time_s;

                NPNPCRefObj npnpcRefObj = null;
                _m_curDialogId = NPPlayer.instance.chapterComp.chapterRefObj.getAfterPointDialog(NPPlayer.instance.chapterComp.nextPointId, out _m_tipResId, out npnpcRefObj);

                if (null != npnpcRefObj)
                {
                    _m_curDialogTextureIndex = npnpcRefObj.npcIcon;
                    _m_name = npnpcRefObj.npcName;   
                }
                
                int oldNodeIndex = NPPlayer.instance.chapterComp.curNodeIndex;
                ChapterNodeStyleRefObj chapterNodeStyle = NPPlayer.instance.chapterComp.chapterRefObj.getChapterNormalNodeStyleByIndexId(oldNodeIndex);
                long sfxId = 0;
                long tdSfxId = 0;
                if (null != chapterNodeStyle)
                {
                    sfxId = chapterNodeStyle.forward_screen_sfx_id;
                    tdSfxId = chapterNodeStyle.forward_td_sfx_id;
                }
                
                //请求服务器单步前进
                NPPlayer.instance.chapterComp.reqChapterForward(_m_forwardLogicMgr.isInQuickForward, (_infoMgs) =>
                {
                    _m_isDealDone = true;
                    if(null == _infoMgs)
                        return;
                    
                    if(serialId != _m_serialId)
                        return;

                    float fade = NPPlayer.instance.chapterComp.getTotalFade();
                    
                    //快速状态或者自动状态播放快速表现
                    if(_m_forwardLogicMgr.isInQuickForward || _m_forwardLogicMgr._m_isInContinueClickForward)
                    {
                        //执行单步前进表现
                        _m_forwardLogicMgr._dealQuickForwardToChapter(fade, sfxId, tdSfxId, _infoMgs.getCoefficient(),  _infoMgs.getRewardExp(), _infoMgs.getRewardPlayerExp(), _infoMgs.getEventId(), _infoMgs.getItemList());
                        _m_forwardLogicMgr._dealPlayForwardVideoAniState(EChapterVideoForwardState.QUICK_FORWARD);
                    }
                    else
                    {
                        //执行单步前进表现
                        _m_forwardLogicMgr._dealChapterForwardEffect(fade, sfxId, tdSfxId, _infoMgs.getCoefficient(),  _infoMgs.getRewardExp(), _infoMgs.getRewardPlayerExp(), _infoMgs.getEventId(), _infoMgs.getItemList());
                        _m_forwardLogicMgr._dealPlayForwardVideoAniState(EChapterVideoForwardState.FORWARD);
                    }
                    
                    //快速模式跳过事件
                    if(!_m_forwardLogicMgr.isInQuickForward && _infoMgs.getEventId() > 0)
                    {
                        //需要处理事件
                        _m_curEventId = _infoMgs.getEventId();
                    }
                    
                    if (oldNodeIndex < NPPlayer.instance.chapterComp.curNodeIndex)
                    {
                        long oldId = NPPlayer.instance.chapterComp.chapterRefObj.getNodeIdByIndex(oldNodeIndex);
                        long newId = NPPlayer.instance.chapterComp.chapterRefObj.getNodeIdByIndex(NPPlayer.instance.chapterComp.curNodeIndex);
                        
                        //样式不一样需要重新进入
                        if(oldId != newId)
                            //需要进入的新node
                            _m_needEnterNodeIndex = NPPlayer.instance.chapterComp.curNodeIndex;
                    }
                    
                }, () =>
                {
                    _m_isDealDone = true;
                    if(serialId != _m_serialId)
                        return;
                    
                    //前进失败直接关闭自动
                    _m_forwardLogicMgr.stopAutoForwardToChapter();
                    _m_forwardLogicMgr.stopQuickForwardToChapter();
                    //失败变回idle
                    _changeState(EChapterForwardType.IDLE);
                });
            }

            protected override void _onExit()
            {
                _m_serialId++;
                _m_dealTime = 0;
                _m_waitTimeTag = 0;
                _m_curEventId = 0;
                _m_curDialogId = 0;
                _m_tipResId = 0;
                _m_needEnterNodeIndex = 0;
                _m_curDialogTextureIndex = null;
                _m_name = null;
                _m_isDealDone = false;
            }
            
            protected override void _onTick(float _deltaTime)
            {
                //累计持续时间
                _m_dealTime += _deltaTime;
                
                if (_m_dealTime >= _m_waitTimeTag && _m_isDealDone)
                {
                    _enterNewState(_m_curEventId, _m_curDialogId, _m_needEnterNodeIndex);
                    _m_isDealDone = false;
                }
            }

            public override bool canEnterState(_ATALStateBase<EChapterForwardType> _newState)
            {
                return _newState.state is EChapterForwardType.IDLE 
                       || _newState.state is EChapterForwardType.FIGHT_BOSS_PER 
                       || _newState.state is EChapterForwardType.FIGHT_BOSS_WAIT
                       || _newState.state is EChapterForwardType.DEAL_EVENT 
                       || _newState.state is EChapterForwardType.DIALOG 
                       || _newState.state is EChapterForwardType.NODE_MOVE_BOSS 
                       || _newState.state is EChapterForwardType.NODE_MOVE_NORMAL
                       ;
            }

            public override void resetData()
            {
                _m_dealTime = 0;
                _m_serialId = -1;
                _m_waitTimeTag = 0;
                _m_curEventId = 0;
                _m_curDialogId = 0;
                _m_needEnterNodeIndex = 0;
            }

            private void _enterNewState(long _eventId, long _dialogId, int _needEnterNodeIndex)
            {
                if (_m_forwardLogicMgr == null)
                    return;

                //是否跳过对话
                bool _isIgnoreDialog = AccountSettingMgr.instance.accountSetting != null && AccountSettingMgr.instance.accountSetting.isChapterQuickForwardSkipDialog && _m_forwardLogicMgr.isInQuickForward;
                
                //有事件处理事件
                if (_eventId > 0)
                {
                    _m_forwardLogicMgr._m_stateMachine?.changeState<DealEventState>((_state) =>
                    {
                        if (_state != null)
                        {
                            _state.setEventId(_eventId);
                            _state.setEndCallback(() => { _enterNewState(0, _dialogId, _needEnterNodeIndex);});
                        }
                    });
                }
                //有对话，并且没开快速处理对话
                else if (_dialogId > 0 && !_isIgnoreDialog)
                {
                    _m_forwardLogicMgr._m_stateMachine?.changeState<DialogState>((_state) =>
                    {
                        if (_state != null)
                        {
                            _state.setDialogId(_dialogId, NPPlayer.instance.chapterComp.curPointId, _m_tipResId != 0, _m_tipResId, _m_curDialogTextureIndex, _m_name);
                            _state.setNextStateAction(() => { _enterNewState(0, 0, _needEnterNodeIndex);});
                        }
                    });
                }
                //下一关是boss战要先播放一个过度动画
                else if (NPPlayer.instance.chapterComp.curIsBossPoint())
                {
                    //写死第一关直接打掉，方便引导
                    if (NPPlayer.instance.chapterComp.curChapterId == 1)
                    {
                        NPPlayer.instance.chapterComp.reqChapterFightBoss(1, (_msg) =>
                        {
                            //进入章节完成状态
                            _m_forwardLogicMgr._m_stateMachine?.changeState<ChapterCompleteState>(
                                (_state) =>
                                {

                                });

                        }, (_errCode) =>
                        {
                            //进入章节完成状态
                            _m_forwardLogicMgr._m_stateMachine?.changeState<ChapterCompleteState>(
                                (_state) =>
                                {

                                });
                        });
                    }
                    else
                    {
                        _m_forwardLogicMgr._m_stateMachine?.changeState<NodeMoveBossState>();
                    }
                }
                else if (_needEnterNodeIndex > 0)
                {
                    _m_forwardLogicMgr._m_stateMachine?.changeState<NodeMoveNormalState>((_state) =>
                    {
                        if (_state != null)
                        {
                            _state.setNodeIndex(_needEnterNodeIndex);
                        }
                    });
                }
                else
                {
                    _changeState(EChapterForwardType.IDLE);
                }
            }
        }
    }
}