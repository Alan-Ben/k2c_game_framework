using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Video;

namespace GOE
{
    public enum EVideoPlayerState
    {
        STOP,//停止状态
        PLAYING,//播放状态
        PAUSE,//暂停状态
    }
    
    /// <summary>
    /// 开篇剧情
    /// </summary>
    public class GGUIWndIntroStoryNew : _ATALBasicUIWnd<GGUIMonoIntroStoryNew>
    {
        protected bool _m_bHasWarmUpWnd;//是否已经预热了窗口
        protected EVideoPlayerState _m_eVideoPlayerState = EVideoPlayerState.STOP;//当前视频播放状态
        
        protected long _m_audioInstanceId;//当前播放的音效实例id 跳过时关闭
        protected long _m_lAudioRefId;//当前播放的音效配表id
        protected float _m_fAudioPausePlayedTime;//音效暂停时已经播放的时间
        
        protected long _m_lVideoPlayTaskSerializeId;
        
        protected long _m_lPreClickDoubleClickSkipBtnTimeMs;//上一次点击双击跳过按钮的时间
        
        [NotNull] private List<onVideoPlayerStartedEventHandle> _m_lOnVideoPlayerStartedDelegateList = new List<onVideoPlayerStartedEventHandle>();//wnd.videoPlayer.started回调
        [NotNull] private List<onVideoPlayerPrepareCompletedEventHandle> _m_lOnPrepareCompletedDelegateList = new List<onVideoPlayerPrepareCompletedEventHandle>();//wnd.videoPlayer.prepareCompleted回调
        [NotNull] private List<onVideoPlayerErrorReceivedEventHandle> _m_lOnErrorReceivedDelegateList = new List<onVideoPlayerErrorReceivedEventHandle>();//wnd.videoPlayer.errorReceived回调
        
        private Action _m_aOnPlayEnd;
        
        public GGUIWndIntroStoryNew(Action _onPlayEnd) : base(EALUIWndLayer.NORMAL)
        {
            _m_aOnPlayEnd = _onPlayEnd;
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(100000); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(100000); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        public override bool needDiscardOnSwitch { get { return true; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            _m_bHasWarmUpWnd = false;
            _m_eVideoPlayerState = EVideoPlayerState.STOP;
            
            ALUGUICommon.combineBtnClick(wnd.btnSkip, _onClickSkipBtn); //跳过按钮

            if (wnd.playableDirector != null)
            {
                wnd.playableDirector.playOnAwake = false;
                wnd.playableDirector.Stop();
            }
        }

        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnSkip, _onClickSkipBtn);
            }

            _m_bHasWarmUpWnd = false;
            _m_eVideoPlayerState = EVideoPlayerState.STOP;
            
            _m_aOnPlayEnd = null;
        }

        protected override void _onReset()
        {
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYABLE_SUBTITLE_CHANGE, _onSubtitleRefresh);
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYABLE_AUDIO_CHANGE, _onAudioRefresh);
        }

        protected override void _onHideWnd()
        {
            _stop();
            
            _m_lOnVideoPlayerStartedDelegateList.Clear();
            _m_lOnPrepareCompletedDelegateList.Clear();
            _m_lOnErrorReceivedDelegateList.Clear();
            
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYABLE_SUBTITLE_CHANGE, _onSubtitleRefresh);
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYABLE_AUDIO_CHANGE, _onAudioRefresh);
            PlayAudioMgr.instance.playBackgroundMusic();
        }

        #region 预热窗口

        /// <summary>
        /// 预热窗口
        /// </summary>
        public virtual void warmUpWnd(Action _complete)
        {
            if (wnd == null || !isLoaded)
            {
                Debug.LogError("[_AGGUIPrefabSubWndVideoPlayer<T> warmUpWnd] 窗口还未加载完成或已经销毁了但是却请求预热窗口, 请检查");
                _complete?.Invoke();
                return;
            }
            
            if (_m_bHasWarmUpWnd)
            {
                _complete?.Invoke();
                return;
            }
            
            showWnd();//预热前应该先将窗口显示出来
            ALUGUICommon.setUIObjScale(wnd, 0);//为了防止窗口提前显示, 先将Scale设为0
            
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(()=>
            {
                _m_bHasWarmUpWnd = true;
                
                _complete?.Invoke();
            });

            _warmUpVideoPlayer(stepCounter.addDoneStepCount);
            _warmUpAudio(stepCounter.addDoneStepCount);
        }

        /// <summary>
        /// 预热VideoPlayer
        /// </summary>
        /// <param name="_complete"></param>
        protected void _warmUpVideoPlayer(Action _complete)
        {
            if (wnd == null || !isLoaded)
            {
                Debug.LogError($"[_AGGUIWndVideoPlayer _warmUpVideoPlayer] 窗口还未加载完成或已经销毁了但是却请求预热窗口, 请检查");
                _complete?.Invoke();
                return;
            }
            
            _complete?.Invoke();
        }
        
        /// <summary>
        /// 预热音效
        /// </summary>
        protected void _warmUpAudio(Action _complete)
        {
            if (wnd == null || !isLoaded)
            {
                Debug.LogError($"[_AGGUIWndVideoPlayer _warmUpVideoPlayer] 窗口还未加载完成或已经销毁了但是却请求预热窗口, 请检查");
                _complete?.Invoke();
                return;
            }
            
            if(_m_bHasWarmUpWnd || wnd.warmUpAudioRefId == null || wnd.warmUpAudioRefId.Count <= 0)
            {
                _complete?.Invoke();
                return;
            }

            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(wnd.warmUpAudioRefId.Count);
            stepCounter.regAllDoneDelegate(()=>
            {
                // _complete?.Invoke();
            });

            foreach (long audioRefId in wnd.warmUpAudioRefId)
            {
                PlayAudioMgr.instance.preload(audioRefId, stepCounter.addDoneStepCount);
            }
            
            _complete?.Invoke();//不需要等音效预热完成就继续, 防止音效预热失败时卡住视频播放
        }

        #endregion
        
        /// <summary>
        /// 开始播放
        /// </summary>
        public virtual void play()
        {
            if (wnd == null)
            {
                ALUGUICommon.setUIObjScale(wnd, 1);//窗口缩放恢复
                _m_eVideoPlayerState = EVideoPlayerState.PLAYING;
                GCommon.sendStepReport(TraceConst.VIDEO_ERROR_SKIP);//跳过视频播放埋点
                _stop();
                return;
            }
            
            if(!_m_bHasWarmUpWnd)
                Debug.LogWarning("[_AGGUIPrefabSubWndVideoPlayer<T> _play] 窗口未进行预热, 可能会导致开始播放前卡顿或音效播放延迟", wnd);

            if (_m_eVideoPlayerState != EVideoPlayerState.STOP)
            {
                Debug.LogError($"[_AGGUIPrefabSubWndVideoPlayer<T> _play] 不处于EVideoPlayerState.STOP状态, 但是再次调用了play, 当前状态:{_m_eVideoPlayerState}", wnd);
            }
            
            _m_eVideoPlayerState = EVideoPlayerState.PLAYING;
                
            ALUGUICommon.setUIObjScale(wnd, 1);//开始播放时完成窗口缩放恢复
                
            //根据不同语言使用不同playableAsset
            if (wnd.playableDirector != null)
            {
                PlayableAsset playableAsset = wnd.getAssetByLanguage(GameSetting.instance.getCurrentLanguage());
                if(playableAsset != null)
                {
                    wnd.playableDirector.playableAsset = playableAsset;
                }
                
                wnd.playableDirector.Play();
            }
            //停止播放背景音乐
            PlayAudioMgr.instance.stopBackgroundMusic();
                
            _initVideoAutoStopTask();
        }
        
        /// <summary>
        /// 停止播放
        /// </summary>
        protected virtual void _stop()
        {
            if (wnd == null || _m_eVideoPlayerState == EVideoPlayerState.STOP)//当窗口被销毁 或 当前已经处于停止状态时, 不做任何处理
                return;

            _m_eVideoPlayerState = EVideoPlayerState.STOP;
            
            GCommon.sendStepReport(TraceConst.VIDEO_PLAY_END);//发送埋点-视频播放结束
            
            PlayAudioMgr.instance.stopClip(_m_audioInstanceId);

            if(wnd.playableDirector != null)
                wnd.playableDirector.Stop();

            _m_lVideoPlayTaskSerializeId = ALSerializeOpMgr.next();
            
            _onVideoPlayEnd();
            //发送视频结束的引导消息
            WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.VIDEO_END);
        }

        /// <summary>
        /// 暂停播放
        /// </summary>
        private void _pause()
        {
            if(wnd == null || _m_eVideoPlayerState != EVideoPlayerState.PLAYING)//当窗口被销毁 或 当前不处于正在播放状态 时, 不做任何处理
                return;

            _m_eVideoPlayerState = EVideoPlayerState.PAUSE;
            
            _m_lVideoPlayTaskSerializeId = ALSerializeOpMgr.next();//取消视频自动停止任务

            // 暂停timeline播放
            if (wnd.playableDirector != null)
            {
                wnd.playableDirector.Pause();
            }

            // 暂停音效播放
            _m_fAudioPausePlayedTime = PlayAudioMgr.instance.getAudioPlayedTime(_m_audioInstanceId);//记录音效已经播放的时长
            PlayAudioMgr.instance.stopClip(_m_audioInstanceId);//停止播放音效
        }
        
        /// <summary>
        /// 恢复播放
        /// </summary>
        private void _resume()
        {
            if(wnd == null || _m_eVideoPlayerState != EVideoPlayerState.PAUSE)//当窗口被销毁 或 当前不处于暂停状态 时, 不做任何处理
                return;

            _m_eVideoPlayerState = EVideoPlayerState.PLAYING;//恢复播放状态
            
            _initVideoAutoStopTask();//重新初始化视频自动停止任务

            // 恢复timeline播放
            if (wnd.playableDirector != null)
            {
                wnd.playableDirector.Resume();
            }
            
            // 恢复音效播放
            _m_audioInstanceId = PlayAudioMgr.instance.playClip(_m_lAudioRefId, false, null, null, null, _m_fAudioPausePlayedTime);
        }
        
        /// <summary>
        /// 显示跳过视频提示
        /// </summary>
        protected virtual void _showSkipVideoTip()
        {
            // _pause();
            //
            // // 显示弹窗提示
            // NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(wnd.doubleClickSkipTip), 
            //     TextTranslate.instance.getLanguage(TransKeyConst.cancel), () =>
            //     {
            //         _resume();
            //     }, 
            //     TextTranslate.instance.getLanguage(TransKeyConst.confirm), () =>
            //     {
            //         GCommon.sendStepReport(TraceConst.VIDEO_SKIP);//发送埋点-跳过视频播放
            //
            //         _stop();
            //     });
            
            GCommon.sendStepReport(TraceConst.VIDEO_SKIP);//发送埋点-跳过视频播放

            _stop();
        }

        /// <summary>
        /// 初始化视频自动停止任务
        /// </summary>
        protected void _initVideoAutoStopTask()
        {
            if (wnd == null)
            {
                Debug.LogError("[_AGGUIPrefabSubWndVideoPlayer<T> _initVideoAutoStopTask] 窗口未加载完成或者视频播放器未初始化完成, 无法初始化视频自动停止任务");
                return;
            }
            
            long serializeId = _m_lVideoPlayTaskSerializeId = ALSerializeOpMgr.next();

            ALCommonTaskController.CommonActionAddMonoTask(()=>
            {
                if(_m_lVideoPlayTaskSerializeId != serializeId)
                    return;
                
                _stop();
            }, wnd.videoPlayTimeS);
        }
        
        #region 监听消息 事件

        /// <summary>
        /// 点击跳过按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickSkipBtn(GameObject _go)
        {
            _showSkipVideoTip();
        }
        
        /// <summary>
        /// 当监听到字幕变化时
        /// </summary>
        /// <param name="_objs"></param>
        private void _onSubtitleRefresh(params object[] _objs)
        {
            if (wnd == null)
                return;
            
        }
        
        /// <summary>
        /// 当监听到音频变化时
        /// </summary>
        /// <param name="_objs"></param>
        private void _onAudioRefresh(params object[] _objs)
        {
            if (wnd == null || _objs == null)
                return;

            int objLen = _objs.Length;

            if(objLen >= 1 && _objs[0] != null && _objs[0] is long)
                _m_lAudioRefId = (long) _objs[0];
            
            if (objLen >= 2 && _objs[1] != null && _objs[1] is long)
                _m_audioInstanceId = (long) _objs[1];
        }
        
        #endregion
        
        #region VideoPlayer事件监听

        /// <summary>
        /// VideoPlayer.started事件处理
        /// </summary>
        #region VideoPlayer.started
        
        private class onVideoPlayerStartedEventHandle
        {
            public Action<VideoPlayer> action;
            public bool dealOnce;//是否只处理一次, 若只处理一次, 则处理后会被移除
            
            public onVideoPlayerStartedEventHandle(Action<VideoPlayer> _action, bool _dealOnce)
            {
                action = _action;
                dealOnce = _dealOnce;
            }
        }
        
        private void _onVideoPlayerStarted(VideoPlayer _videoPlayer)
        {
            for (int i = _m_lOnVideoPlayerStartedDelegateList.Count - 1; i >= 0; i--)
            {
                onVideoPlayerStartedEventHandle eventHandle = _m_lOnVideoPlayerStartedDelegateList[i];
                if (eventHandle == null || eventHandle.action == null)
                {
                    _m_lOnVideoPlayerStartedDelegateList.RemoveAt(i);
                    break;
                }
                
                Action<VideoPlayer> action = eventHandle.action;
                if (eventHandle.dealOnce)
                    _m_lOnVideoPlayerStartedDelegateList.RemoveAt(i);
                action?.Invoke(_videoPlayer);
            }
        }
        
        /// <summary>
        /// 注册VideoPlayer.started事件回调
        /// </summary>
        /// <param name="_action">回调方法</param>
        /// <param name="_dealOnce">是否只执行一次</param>
        public void regOnVideoPlayerStarted(Action<VideoPlayer> _action, bool _dealOnce)
        {
            if (_action == null)
                return;

            _m_lOnVideoPlayerStartedDelegateList.Add(new onVideoPlayerStartedEventHandle(_action, _dealOnce));
        }

        /// <summary>
        /// 注销VideoPlayer.started事件回调
        /// </summary>
        /// <param name="_action"></param>
        public void unRegOnVideoPlayerStarted(Action<VideoPlayer> _action)
        {
            if (_action == null)
                return;

            for (int i = _m_lOnVideoPlayerStartedDelegateList.Count - 1; i >= 0; i--)
            {
                onVideoPlayerStartedEventHandle eventHandle = _m_lOnVideoPlayerStartedDelegateList[i];
                if (eventHandle != null && eventHandle.action == _action)
                {
                    _m_lOnVideoPlayerStartedDelegateList.RemoveAt(i);
                }
            }
        }
        
        #endregion

        #region VideoPlayer.prepareCompleted事件

        private class onVideoPlayerPrepareCompletedEventHandle
        {
            public Action<VideoPlayer> action;
            public bool dealOnce;//是否只处理一次, 若只处理一次, 则处理后会被移除
            
            public onVideoPlayerPrepareCompletedEventHandle(Action<VideoPlayer> _action, bool _dealOnce)
            {
                action = _action;
                dealOnce = _dealOnce;
            }
        }
        
        private void _onVideoPlayerPrepareCompleted(VideoPlayer _videoPlayer)
        {
            for (int i = _m_lOnPrepareCompletedDelegateList.Count - 1; i >= 0; i--)
            {
                onVideoPlayerPrepareCompletedEventHandle eventHandle = _m_lOnPrepareCompletedDelegateList[i];
                if (eventHandle == null || eventHandle.action == null)
                {
                    _m_lOnPrepareCompletedDelegateList.RemoveAt(i);
                    break;
                }
                
                Action<VideoPlayer> action = eventHandle.action;
                if (eventHandle.dealOnce)
                    _m_lOnPrepareCompletedDelegateList.RemoveAt(i);
                action?.Invoke(_videoPlayer);
            }
        }
        
        /// <summary>
        /// 注册VideoPlayer.prepareCompleted事件回调
        /// </summary>
        /// <param name="_action">回调方法</param>
        /// <param name="_dealOnce">是否只执行一次</param>
        public void regOnVideoPlayerPlayerPrepareCompleted(Action<VideoPlayer> _action, bool _dealOnce)
        {
            if (_action == null)
                return;

            _m_lOnPrepareCompletedDelegateList.Add(new onVideoPlayerPrepareCompletedEventHandle(_action, _dealOnce));
        }

        /// <summary>
        /// 注销VideoPlayer.prepareCompleted事件回调
        /// </summary>
        /// <param name="_action"></param>
        public void unRegOnVideoPlayerPrepareCompleted(Action<VideoPlayer> _action)
        {
            if (_action == null)
                return;

            for (int i = _m_lOnPrepareCompletedDelegateList.Count - 1; i >= 0; i--)
            {
                onVideoPlayerPrepareCompletedEventHandle eventHandle = _m_lOnPrepareCompletedDelegateList[i];
                if (eventHandle != null && eventHandle.action == _action)
                {
                    _m_lOnPrepareCompletedDelegateList.RemoveAt(i);
                }
            }
        }
        
        #endregion

        #region VideoPlayer.errorReceived事件

        private class onVideoPlayerErrorReceivedEventHandle
        {
            public Action<VideoPlayer, string> action;
            public bool dealOnce;//是否只处理一次, 若只处理一次, 则处理后会被移除
            
            public onVideoPlayerErrorReceivedEventHandle(Action<VideoPlayer, string> _action, bool _dealOnce)
            {
                action = _action;
                dealOnce = _dealOnce;
            }
        }
        
        private void _onVideoPlayerErrorReceived(VideoPlayer _videoPlayer, string _message)
        {
            for (int i = _m_lOnErrorReceivedDelegateList.Count - 1; i >= 0; i--)
            {
                onVideoPlayerErrorReceivedEventHandle eventHandle = _m_lOnErrorReceivedDelegateList[i];
                if (eventHandle == null || eventHandle.action == null)
                {
                    _m_lOnErrorReceivedDelegateList.RemoveAt(i);
                    break;
                }

                Action<VideoPlayer, string> action = eventHandle.action;
                if (eventHandle.dealOnce)
                    _m_lOnErrorReceivedDelegateList.RemoveAt(i);
                action?.Invoke(_videoPlayer, _message);
            }
        }
        
        /// <summary>
        /// 注册VideoPlayer.errorReceived事件回调
        /// </summary>
        /// <param name="_action">回调方法</param>
        /// <param name="_dealOnce">是否只执行一次</param>
        public void regOnVideoPlayerPlayerErrorReceived(Action<VideoPlayer, string> _action, bool _dealOnce)
        {
            if (_action == null)
                return;

            _m_lOnErrorReceivedDelegateList.Add(new onVideoPlayerErrorReceivedEventHandle(_action, _dealOnce));
        }

        /// <summary>
        /// 注销VideoPlayer.errorReceived事件回调
        /// </summary>
        /// <param name="_action"></param>
        public void unRegOnVideoPlayerPrepareCompleted(Action<VideoPlayer, string> _action)
        {
            if (_action == null)
                return;

            for (int i = _m_lOnErrorReceivedDelegateList.Count - 1; i >= 0; i--)
            {
                onVideoPlayerErrorReceivedEventHandle eventHandle = _m_lOnErrorReceivedDelegateList[i];
                if (eventHandle != null && eventHandle.action == _action)
                {
                    _m_lOnErrorReceivedDelegateList.RemoveAt(i);
                }
            }
        }
        
        #endregion
        
        #endregion
        
        // protected override void _setVideoPlayerRenderMode()
        // {
        //     if(wnd == null || wnd.videoPlayer == null)
        //         return;
        //
        //     if (!(wnd.videoPlayer.renderMode is VideoRenderMode.CameraFarPlane or VideoRenderMode.CameraNearPlane))
        //     {
        //         Debug.LogError($"GGUIWndIntroStoryNew::_setVideoPlayerRenderMode() - videoPlayer.renderMode is not CameraFarPlane or CameraNearPlane", wnd);
        //         return;
        //     }
        //
        //     wnd.videoPlayer.targetCamera = Game.instance.mainCamera.fullCanvas.worldCamera;
        //     wnd.videoPlayer.targetCameraAlpha = 1;
        // }

        private void _onVideoPlayEnd()
        {
            _m_aOnPlayEnd?.Invoke();
            
            QueueMgr.instance.forceCloseNodeByType(typeof(GNodeIntroStoryNew));//播放完成后退出开篇剧情节点
        }
    }
}