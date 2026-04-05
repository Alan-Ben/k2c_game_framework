using System;
using ALPackage;
using Common.ChapterEnum;
using JetBrains.Annotations;

namespace GOE
{
    public partial class ChapterForwardLogicMgr
    {
        // 自动结算boss状态
        private class AutoBattleState : _AChapterBaseState
        {
            private long _m_serialId = -1;
            //经历时间
            private float _m_dealTime = 0;
            //目标时常
            private float _m_targetTime = 0;
            
            //是否在请求网络
            private bool _m_isInNetRequest = false;
            
            private ChapterAutoForwardRewardData _m_rewardData;
            
            public AutoBattleState(ChapterForwardLogicMgr _chapterForwardLogicMgr) : base(_chapterForwardLogicMgr){}

            public override EChapterForwardType state { get { return EChapterForwardType.AUTO_BATTLE; } }

            protected override void _onEnter()
            {
                _m_serialId++;
                _m_isInNetRequest = false;
                _m_targetTime = _getTargetTime();
                _m_rewardData = new ChapterAutoForwardRewardData(NPPlayer.instance.chapterComp.curChapterId, NPPlayer.instance.chapterComp.curPointId);

                _m_forwardLogicMgr._enterAutoState();
            }

            protected override void _onExit()
            {
                _m_serialId++;
                _m_dealTime = 0;
                _m_targetTime = 0;
                _m_isInNetRequest = false;

                _m_forwardLogicMgr._quitAutoState();

                if (_m_rewardData != null)
                {
                    _m_rewardData.setEndChapterInfo(NPPlayer.instance.chapterComp.curChapterId,
                        NPPlayer.instance.chapterComp.curPointId);
                    NPUINoticeMgr.instance.addDealer(new NoticeDealer_ChapterAutoForwardReward(_m_rewardData));
                    _m_rewardData = null;
                }
            }
            
            protected override void _onTick(float _deltaTime)
            {
                //累计持续时间
                _m_dealTime += _deltaTime;
                
                //大于间隔时间去单次前进状态
                if (_m_dealTime > _m_targetTime && !_m_isInNetRequest)
                {
                    long serialId = _m_serialId;
                    
                    _m_dealTime = 0;
                    _m_targetTime = _getTargetTime();
                    
                    _dealMain(serialId);
                }
            }

            public override bool canEnterState(_ATALStateBase<EChapterForwardType> _newState)
            {
                return _newState.state is EChapterForwardType.IDLE;
            }

            public override void resetData()
            {
                _m_serialId = -1;
                _m_dealTime = 0;
                _m_targetTime = 0;
                _m_isInNetRequest = false;
                _m_rewardData = null;
            }

            private float _getTargetTime()
            {
                if (NPPlayer.instance.chapterComp.curIsBossPoint())
                {
                    return GRefdataCoreMgr.instance.npGeneral.chapter_auto_boss_delay_time_s;
                }
                else
                {
                    return GRefdataCoreMgr.instance.npGeneral.chapter_auto_forward_delay_time_s;
                }
            }
            

            private void _dealMain(long _serialId)
            {
                ChapterRefObj chapterRefObj = NPPlayer.instance.chapterComp.chapterRefObj;
                if (null == chapterRefObj)
                {
                    _dealQuit();
                    return;
                }
                
                //未解锁不处理
                if (!GCommon.isSimpleUnlock(chapterRefObj.forward_simple_unlock_id))
                {
                    _dealQuit();
                    return;
                }

                //如果当前是boss关卡
                if (NPPlayer.instance.chapterComp.curIsBossPoint())
                {
                    _dealBoss(_serialId);
                }
                //普通关
                else
                {
                    _dealForward(_serialId);
                }
            }

            
            private void _dealForward(long _serialId)
            {
                //当前的node位置
                int oldNodeIndex = NPPlayer.instance.chapterComp.curNodeIndex;

                _m_isInNetRequest = true;
                 //请求服务器单步前进
                NPPlayer.instance.chapterComp.reqChapterForward(true, (_infoMgs) =>
                {
                    if(_serialId != _m_serialId)
                        return;
                    
                    _m_isInNetRequest = false;
                    if (null == _infoMgs)
                    {
                        //前进失败直接关闭自动
                        _dealQuit();
                        return;
                    }

                    if (_m_rewardData != null)
                    {
                        _m_rewardData.addRewardHeroExp(_infoMgs.getRewardExp());
                        _m_rewardData.addRewardPlayerExp(_infoMgs.getRewardPlayerExp());
                        _m_rewardData.addRewardItem(_infoMgs.getItemList());
                        _m_rewardData.addCostGoldNum(_infoMgs.getCostGoldNum());
                    }


                    float fade = NPPlayer.instance.chapterComp.getTotalFade();
         
                    //快速状自动状态播放快速表现
                    _m_forwardLogicMgr._dealAutoForwardToChapter(oldNodeIndex, fade, _infoMgs.getCoefficient(),  _infoMgs.getRewardExp(), _infoMgs.getRewardPlayerExp(), _infoMgs.getItemList());
                }, () =>
                {
                    if(_serialId != _m_serialId)
                        return;
                    _m_isInNetRequest = false;

                    //前进失败直接关闭自动
                    _dealQuit();
                });
            }
            
            private void _dealBoss(long _serialId)
            {
                if(_serialId != _m_serialId)
                    return;
                
                ChapterRefObj chapterRefObj = NPPlayer.instance.chapterComp.chapterRefObj;
                if (null == chapterRefObj)
                {
                    _dealQuit();
                    return;
                }
                
                //boss战斗力
                long bossPower = chapterRefObj.boss_power;
                long curPower = NPPlayer.instance.chapterComp.getTotalPower();

                //战力大于boss了直接结算
                if (curPower >= bossPower)
                {
                    if(_serialId != _m_serialId)
                        return;
                    
                    _m_isInNetRequest = true;
                    //直接找服务器结算
                    NPPlayer.instance.chapterComp.reqChapterFightBoss(NPPlayer.instance.chapterComp.curChapterId, (_msg) =>
                    {
                        if (_m_rewardData != null) 
                            _m_rewardData.addRewardItem(_msg.getRewardList());

                        _m_isInNetRequest = false;
                       //播放boss动画
                       _m_forwardLogicMgr.dealAutoBossToChapter(() =>
                       {
                           //播放章节完成表现
                           _m_forwardLogicMgr.playChapterCompleteEffect(NPPlayer.instance.chapterComp.chapterRefObj, 0);
                       });
                    }, (_errCode) =>
                    {
                        _m_isInNetRequest = false;
                        _dealQuit();
                    });
                }
                else
                {
                    //战力小于boss根据鼓舞限制开始自动鼓舞
                    //优先金币鼓舞次数判断
                    if (NPPlayer.instance.chapterComp.getInspireTimes(EChapterInspireType.GOLD) <
                        _m_forwardLogicMgr.getMaxInspireTimes(EChapterInspireType.GOLD))
                    {
                        _m_isInNetRequest = true;
                        NPPlayer.instance.chapterComp.reqChapterFightBossInspire(EChapterInspireType.GOLD, () =>
                        {
                            _m_isInNetRequest = false;
                            _dealBoss(_serialId);
                        },() =>
                        {
                            _m_isInNetRequest = false;
                            _dealQuit();
                        }, false);
                    }
                    //砖石鼓舞次数判断
                    else if (NPPlayer.instance.chapterComp.getInspireTimes(EChapterInspireType.CRYSTAL) < _m_forwardLogicMgr.getMaxInspireTimes(EChapterInspireType.CRYSTAL))
                    {
                        _m_isInNetRequest = true;
                        NPPlayer.instance.chapterComp.reqChapterFightBossInspire(EChapterInspireType.CRYSTAL, () =>
                        {
                            _m_isInNetRequest = false;
                            _dealBoss(_serialId);
                        },() =>
                        {
                            _m_isInNetRequest = false;
                            _dealQuit();
                        }, false);
                    }
                    //砖石鼓舞次数判断
                    else if (NPPlayer.instance.chapterComp.getInspireTimes(EChapterInspireType.ITEM) < _m_forwardLogicMgr.getMaxInspireTimes(EChapterInspireType.ITEM))
                    {
                        _m_isInNetRequest = true;
                        NPPlayer.instance.chapterComp.reqChapterFightBossInspire(EChapterInspireType.ITEM, () =>
                        {
                            _m_isInNetRequest = false;
                            _dealBoss(_serialId);
                        },() =>
                        {
                            _m_isInNetRequest = false;
                            _dealQuit();
                        }, false);
                    }
                    else
                    {
                        _dealQuit();
                    }
                }
            }

            //处理失败逻辑
            private void _dealQuit()
            {
                //失败直接关闭自动
                _m_forwardLogicMgr.stopAutoForwardToChapter();
                //进入idle
                _changeState(EChapterForwardType.IDLE);
            }
        }
    }
}