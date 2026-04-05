using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GOE
{
    /// <summary>
    /// 妃子配音气泡附加窗口
    /// </summary>
    public class GGUIWndConsortVoiceBubble : _ATALBasicUISubWnd<GGUIMonoConsortVoiceBubble>
    {
        //妃子id
        private long _m_lConsortId;
        //普通状态下时的配音类型
        private EConsortVoiceType _m_eNormalStateVoiceType;
        [NotNull] private List<long> _m_lTmpVoiceIdList = new List<long>();
        private int _m_iPrePlayVoiceIndex = -1;
        //气泡内容
        private string _m_sBubbleContent;
        //气泡打字机
        private GGUIWndTextTypewriter _m_wBubbleTypewriter;
        // 当前正在展示的配音类型
        private EConsortVoiceType _m_eNowShowVoiceType;
        //气泡是否正在展示中
        private bool _m_bBubbleIsPlaying;
        //配音是否正在播放中
        private bool _m_bVoicePlaying;
        //显示序列
        private long _m_lShowSerialize;

        public GGUIWndConsortVoiceBubble(GGUIMonoConsortVoiceBubble _wnd) : base(_wnd)
        {
            initWnd();
        }
        
        public EConsortVoiceType nowShowVoiceType { get { return _m_eNowShowVoiceType; } }

        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            ConsortVoiceMgr.instance.stopAllVoice();
            _m_wBubbleTypewriter?.hideWnd();
            _m_eNowShowVoiceType = EConsortVoiceType.NONE;
            _m_bBubbleIsPlaying = false;
            _m_bVoicePlaying = false;
            
            _m_lTmpVoiceIdList.Clear();
        }

        protected override void _onReset()
        {
            _m_wBubbleTypewriter?.resetWnd();
            
            _m_lTmpVoiceIdList.Clear();
        }

        protected override void _onDiscard()
        {
            _m_wBubbleTypewriter?.discard();
            _m_wBubbleTypewriter = null;

            _m_lTmpVoiceIdList.Clear();
            
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClick);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoTypewriter != null)
                _m_wBubbleTypewriter = new GGUIWndTextTypewriter(wnd.monoTypewriter);

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClick);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_consortId">妃子id</param>
        /// <param name="_normalStateVoiceType">普通状态下时的配音类型</param>
        public void setInfo(long _consortId, EConsortVoiceType _normalStateVoiceType)
        {
            if (wnd == null)
                return;

            _m_lConsortId = _consortId;
            _m_eNormalStateVoiceType = _normalStateVoiceType;
            _m_lShowSerialize = ALSerializeOpMgr.next();

            //重置状态
            _m_wBubbleTypewriter?.hideWnd();
            ConsortVoiceMgr.instance.stopAllVoice();
            _m_eNowShowVoiceType = EConsortVoiceType.NONE;
            _m_bBubbleIsPlaying = false;
            _m_bVoicePlaying = false;
        }

        public void refreshBubble(EConsortVoiceType _voiceType, Action _showComplete = null)
        {
            long serialize = _m_lShowSerialize = ALSerializeOpMgr.next();
            
            //播放气泡或者配音和气泡
            if (!wnd.isPlayVoiceAndBubble)
                _playBubble(_voiceType, serialize, _showComplete);
            else
                _playVoiceAndBubble(_voiceType, serialize, _showComplete);
        }
        
        public void refreshBubble(Action _showComplete = null)
        {
            long serialize = _m_lShowSerialize = ALSerializeOpMgr.next();

            //播放气泡或者配音和气泡
            if (!wnd.isPlayVoiceAndBubble)
                _playBubble(_m_eNormalStateVoiceType, serialize, _showComplete);
            else
                _playVoiceAndBubble(_m_eNormalStateVoiceType, serialize, _showComplete);
        }
        
        //播放气泡
        private void _playBubble(EConsortVoiceType _voiceType, long _serialize, Action _showComplete)
        {
            if (wnd == null)
            {
                _showComplete?.Invoke();
                return;
            }
            
            GConsortRefObj consortRef = GRefdataCoreMgr.instance.consortRefCore.getRef(_m_lConsortId);
            if (consortRef == null)
            {
                _m_wBubbleTypewriter?.hideWnd();
                _showComplete?.Invoke();
                return;
            }

            consortRef.getCanPlayVoiceIdList(_voiceType, _m_lTmpVoiceIdList);
            if (_m_lTmpVoiceIdList.Count <= 0)
            {
                _m_wBubbleTypewriter?.hideWnd();
                _showComplete?.Invoke();
                return;
            }

            int voiceCount = _m_lTmpVoiceIdList.Count;
            int playVoiceIndex = 0;
            if (voiceCount == 1)
            {
                playVoiceIndex = 0;
            }
            else
            {
                if (wnd.playBaseOnPre)
                {
                    if (wnd.playRandom)
                    {
                        playVoiceIndex = Random.Range(0, voiceCount - 1);//Random.Range是左闭右开的
                        if(playVoiceIndex >= _m_iPrePlayVoiceIndex)
                            playVoiceIndex++;
                    }
                    else
                    {
                        playVoiceIndex = (_m_iPrePlayVoiceIndex + 1) % voiceCount;
                    }

                }
                else
                {
                    if (wnd.playRandom)
                    {
                        playVoiceIndex = Random.Range(0, voiceCount);//Random.Range是左闭右开的
                    }
                    else
                    {
                        playVoiceIndex = 0;
                    }
                }
            }
            
            playVoiceIndex = (playVoiceIndex + voiceCount) % voiceCount;
            _m_iPrePlayVoiceIndex = playVoiceIndex;
            _m_eNowShowVoiceType = _voiceType;
            _playBubble(_m_lTmpVoiceIdList.SafeGet(playVoiceIndex), _serialize, ()=>
            {
                _m_eNowShowVoiceType = EConsortVoiceType.NONE;
                _showComplete?.Invoke();
            });
        }

        //播放气泡
        private void _playBubble(long _voiceId, long _serialize,  Action _showComplete)
        {
            if (wnd == null)
            {
                _showComplete?.Invoke();
                return;
            }

            VoiceKeyRefObj voiceKeyRef = GRefdataCoreMgr.instance.voiceKeyRefCore.getRef(_voiceId);
            if (voiceKeyRef == null || string.IsNullOrEmpty(voiceKeyRef.voice_key))
            {
                _m_wBubbleTypewriter?.hideWnd();
                _showComplete?.Invoke();
                return;
            }

            _m_sBubbleContent = TextTranslate.instance.getLanguage(voiceKeyRef.voice_key);
            _m_bBubbleIsPlaying = true;
            _m_wBubbleTypewriter?.showWnd();
            _m_wBubbleTypewriter?.showTextTypewriter(_m_sBubbleContent, null, () =>
            {
                if(_serialize != _m_lShowSerialize)
                    return;
                
                _m_bBubbleIsPlaying = false;
                _showComplete?.Invoke();

                //延时隐藏气泡
                ALCommonActionMonoTask.addMonoTask(() =>
                {
                    if (_serialize != _m_lShowSerialize)
                        return;

                    _m_wBubbleTypewriter?.hideWnd();
                }, wnd.playDoneDelayHideBubble);
            });
        }
        
        /// <summary>
        /// 播放配音和气泡
        /// </summary>
        /// <param name="_voiceType"></param>
        private void _playVoiceAndBubble(EConsortVoiceType _voiceType, long _serialize, Action _showComplete)
        {
            if (wnd == null)
            {
                _showComplete?.Invoke();
                return;
            }
            
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(() =>
            {
                if(_serialize != _m_lShowSerialize)
                    return;
                
                _m_eNowShowVoiceType = EConsortVoiceType.NONE;
                _showComplete?.Invoke();
            });
            
            ConsortVoiceMgr.instance.stopAllVoice();
            _m_eNowShowVoiceType = _voiceType;
            _m_bVoicePlaying = true;
            ConsortVoiceMgr.instance.playVoice(_m_lConsortId, _voiceType, wnd.playRandom, wnd.playBaseOnPre, (_instanceId, _voiceId) =>
            {
                _m_iPrePlayVoiceIndex = ConsortVoiceMgr.instance.getPrePlayVoiceIndex(_m_lConsortId, _voiceType);
                //显示当前配音对应的气泡
                _playBubble(_voiceId, _serialize, stepCounter.addDoneStepCount);
            }, (_voiceRefId) =>
            {
                _m_iPrePlayVoiceIndex = ConsortVoiceMgr.instance.getPrePlayVoiceIndex(_m_lConsortId, _voiceType);
                //显示当前配音对应的气泡
                _playBubble(_voiceRefId, _serialize, stepCounter.addDoneStepCount);
                
                // 因为播放失败, 这里直接将步骤计数器加1, 让语音播放完成
                _m_bVoicePlaying = false;
                stepCounter.addDoneStepCount();
            }, () =>
            {
                _m_bVoicePlaying = false;
                stepCounter.addDoneStepCount();
            });
        }
        
        //点击按钮
        private void _onClick(GameObject _go)
        {
            if (wnd == null)
                return;

            if (_m_bBubbleIsPlaying)
            {
                //气泡文本加载至一半，语音播放到一半时语音照常播放不干扰,点击瞬间加载所有气泡文本
                _m_wBubbleTypewriter?.showText(_m_sBubbleContent);
            }
            else
            {
                _m_lShowSerialize = ALSerializeOpMgr.next();
                if (_m_bVoicePlaying)
                {
                    //气泡文本加载结束，语音播放到一半时，点击中断语音播放，隐藏气泡
                    ConsortVoiceMgr.instance.stopAllVoice();
                    _m_wBubbleTypewriter?.hideWnd();
                }
                else
                {
                    //气泡文本加载结束，语音播放结束时，点击隐藏气泡
                    //如果已经隐藏，点击播放新的气泡语音
                    if (_m_wBubbleTypewriter != null && _m_wBubbleTypewriter.isShow)
                    {
                        _m_wBubbleTypewriter.hideWnd();
                    }
                    else
                    {
                        refreshBubble(null);
                    }
                }
            }
        }
    }
}