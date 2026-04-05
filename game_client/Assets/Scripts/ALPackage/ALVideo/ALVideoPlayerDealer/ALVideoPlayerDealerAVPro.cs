using System;
using System.IO;
using ALPackage;
using UnityEngine;
using UnityEngine.Video;
using System.Threading;
using GOE;



#if AL_AVPRO_V2
using RenderHeads.Media.AVProVideo;
#endif

namespace ALPackage
{
#if AL_AVPRO_V2
    /// <summary>
    /// AVPro Video播放器的封装实现
    /// 实现IALVideoPlayerCore接口
    /// </summary>
    public class ALVideoPlayerDealerAVPro : _AALVideoPlayerDealer
    {
        //构造的原始对象
        private GameObject _m_go;
        private MediaPlayer _m_avproPlayer;
        private ApplyToMaterial _m_applyToMaterial;
        private AudioSource _m_audioSource;
        private AudioOutput _m_audioOutput;

        private long _m_lSeirialize;
        //准备加载时的加载序列号
        private long _m_lPrepareSerialize;
        //记录准备的帧数，同一帧如果进行加载不允许同帧释放
        private int _m_iPreapreFrameCount;
        //用于记录开始的序列号，当序列号与播放一致的时候表明当前正在播放前的准备阶段，isPlaying返回是true
        private long _m_lPlaySerialize;
        //是否已经初始化完成
        private bool _m_bIsInited;
        //是否已经开始准备
        private bool _m_bStartPrepare;
        //准备完成后的回调函数
        private Action<bool, string> _m_dPrepareDoneDelegate;
        //当前使用的视频资源
        private _IALVideoResource _m_currentResource;
        private ALVideoResourceAVPro2 _m_currentPathResource;
        //音频输出模式（AVPro的映射）
        private VideoAudioOutputMode _m_audioOutputMode = VideoAudioOutputMode.None;

        public ALVideoPlayerDealerAVPro(Transform _parentTrans, string _name)
        {
            //创建空go
            _m_go = new GameObject(_name);
            //设置父节点
            _m_go.transform.SetParent(_parentTrans);

            _m_lSeirialize = ALSerializeOpMgr.next();
            _m_lPrepareSerialize = ALSerializeOpMgr.next();
            _m_lPlaySerialize = 0;
            //创建AVPro MediaPlayer组件
            _m_avproPlayer = _m_go.AddComponent<MediaPlayer>();
            _m_avproPlayer.PlatformOptionsAndroid.videoApi = ALVideoPlayerMgr.instance._m_avProAndroidVideoApi;
            _m_applyToMaterial = _m_go.AddComponent<ApplyToMaterial>();
            _m_applyToMaterial.Player = _m_avproPlayer;

            _m_audioSource = _m_go.AddComponent<AudioSource>();
            _m_avproPlayer.AudioSource = _m_audioSource;
            _m_audioOutput = _m_go.AddComponent<AudioOutput>();
            if (_m_audioOutput != null) 
                _m_audioOutput.Player = _m_avproPlayer;

            //初始化相关参数
            _m_avproPlayer.AutoOpen = false;
            _m_avproPlayer.AutoStart = false;
            _m_avproPlayer.Loop = false;
            _m_avproPlayer.PlaybackRate = 1.0f;
            _m_bIsInited = false;
            _m_bStartPrepare = false;
            _m_currentResource = null;
            
            //注册AVPro事件
            _m_avproPlayer.Events.AddListener(OnMediaPlayerEvent);

            _m_dPrepareDoneDelegate = default(Action<bool, string>);
        }

        /// <summary>
        /// 底层内部获取的ApplyToMaterial组件
        /// </summary>
        protected internal ApplyToMaterial _applyToMaterial { get { return _m_applyToMaterial; } }
        public long prepareSerialize { get { return _m_lPrepareSerialize; } }

        public override bool isPlaying
        {
            get
            {
                return null != _m_avproPlayer && (_m_lSeirialize == _m_lPlaySerialize || !_m_avproPlayer.Control.IsFinished());
            }
        }

        public override Action<bool, string> onPrepareDone { get { return _m_dPrepareDoneDelegate; } set { _m_dPrepareDoneDelegate = value; } }

        /// <summary>
        /// 获取当前AVPro的Texture
        /// </summary>
        public override Texture texture { get { return null == _m_avproPlayer ? null : _m_avproPlayer.TextureProducer.GetTexture(); } }

        public override int videoWidth { 
            get {
                if (null == _m_avproPlayer || null == _m_avproPlayer.Info)
                    return 0;

                return _m_avproPlayer.Info.GetVideoWidth();
            } 
        }
        public override int videoHeight
        {
            get
            {
                if (null == _m_avproPlayer || null == _m_avproPlayer.Info)
                    return 0;

                return _m_avproPlayer.Info.GetVideoHeight();
            }
        }

        public override double length
        {
            get { return _m_avproPlayer?.Info?.GetDuration() ?? 60; }
        }

        /// <summary>
        /// 设置音频播放模式
        /// </summary>
        /// <param name="_mode"></param>
        public override void setAudioOutputMode(VideoAudioOutputMode _mode)
        {
            _m_audioOutputMode = _mode;
            
            if (null == _m_avproPlayer)
                return;

            // AVPro的音频控制
            switch (_mode)
            {
                case VideoAudioOutputMode.None:
                    _m_avproPlayer.AudioMuted = true;
                    break;
                case VideoAudioOutputMode.Direct:
                    if (_m_avproPlayer.PlatformOptionsWindows != null)
                        _m_avproPlayer.PlatformOptionsWindows.audioOutput = Windows.AudioOutput.System;
                    if (_m_avproPlayer.PlatformOptionsAndroid != null)
                        _m_avproPlayer.PlatformOptionsAndroid.audioOutput = Android.AudioOutput.System;
                    if (_m_avproPlayer.PlatformOptionsIOS != null)
                        _m_avproPlayer.PlatformOptionsIOS.audioMode = MediaPlayer.OptionsApple.AudioMode.SystemDirect;
                    _m_avproPlayer.AudioMuted = false;
                    break;
                case VideoAudioOutputMode.AudioSource:
                    if (_m_avproPlayer.PlatformOptionsWindows != null)
                        _m_avproPlayer.PlatformOptionsWindows.audioOutput = Windows.AudioOutput.Unity;
                    if (_m_avproPlayer.PlatformOptionsAndroid != null)
                        _m_avproPlayer.PlatformOptionsAndroid.audioOutput = Android.AudioOutput.Unity;
                    if (_m_avproPlayer.PlatformOptionsIOS != null)
                        _m_avproPlayer.PlatformOptionsIOS.audioMode = MediaPlayer.OptionsApple.AudioMode.Unity;
                    _m_avproPlayer.AudioMuted = false;
                    break;
            }
        }

        /// <summary>
        /// 设置视频渲染目标
        /// </summary>
        /// <param name="_mat"></param>
        protected override void _defaultSetTarget(Material _mat)
        {
            if (_m_applyToMaterial != null) 
                _m_applyToMaterial.Material = _mat;
        }

        /// <summary>
        /// 强制开始准备的处理，由于部分视频播放做了队列排序处理，而在部分处理业务上需要强制播放视频
        /// 因此在这里需要提供一个强制开始准备的处理操作
        /// </summary>
        /// <param name="resource">视频资源</param>
        /// <param name="prePrepare">准备前的回调</param>
        /// <param name="onPrepareDone">准备完成的回调</param>
        public override void forcePrepare(_IALVideoResource _resource, Action _prePrepare, Action<bool, string> _onPrepareDone)
        {
            //这里直接开始准备操作，变更准备序列号
            if (_resource is not ALVideoResourceAVPro2 pathResource)
            {
                Debug.LogError("AVPro VideoPlayer unsupported resource type");
                if (null != _onPrepareDone)
                    _onPrepareDone(false, "Resource format err!");
                return;
            }

            //不是同一个播放对象时需要重置本对象准备状态
            if (!_resource.Equals(_m_currentResource))
            {
                //此处需要调用重置
                _resetData();
                _m_currentResource = _resource;
                _m_currentPathResource = pathResource;
            }

            ///如果已经初始化完成，则直接调用回调
            if (_m_bIsInited)
            {
                if (null != _onPrepareDone)
                    _onPrepareDone(true, string.Empty);
                return;
            }

            //注册回调
            _m_dPrepareDoneDelegate += _onPrepareDone;

            //如果已经开始准备则不做处理
            if (_m_bStartPrepare)
                return;

            //还未开始准备的情况下，开始准备
            _m_bStartPrepare = true;

            //调用事件函数
            if (null != _prePrepare)
                _prePrepare();

            //设置序列号
            _m_lPrepareSerialize = ALSerializeOpMgr.next();
            _m_iPreapreFrameCount = Time.frameCount;

            //开始准备 - AVPro使用OpenVideoFromFile
            if (_m_currentPathResource != null)
                _m_avproPlayer?.OpenMedia(MediaPathType.AbsolutePathOrURL, _m_currentPathResource.resourcePath,
                    false);
        }
        /// <summary>
        /// 准备视频，并在准备完成的时候调用回调
        /// </summary>
        /// <param name="resource">视频资源</param>
        /// <param name="_prePrepare">准备前的回调</param>
        /// <param name="__onPrepareDone">准备完成的回调</param>
        public override void prepare(_IALVideoResource _resource, Action _prePrepare, Action<bool, string> _onPrepareDone)
        {
            if (_resource is not ALVideoResourceAVPro2 pathResource)
            {
                Debug.LogError("AVPro VideoPlayer unsupported resource type");
                if (null != _onPrepareDone)
                    _onPrepareDone(false, "Resource format err!");
                return;
            }

            //不是同一个播放对象时需要重置本对象准备状态
            if (!_resource.Equals(_m_currentResource))
            {
                //此处需要调用重置
                _resetData();
                _m_currentResource = _resource;
                _m_currentPathResource = pathResource;
            }

            ///如果已经初始化完成，则直接调用回调
            if (_m_bIsInited)
            {
                if (null != _onPrepareDone)
                    _onPrepareDone(true, string.Empty);
                return;
            }

            //注册回调
            _m_dPrepareDoneDelegate += _onPrepareDone;

            //还未开始准备的情况下，开始准备
            if (!_m_bStartPrepare)
            {
                _m_bStartPrepare = true;

                //调用事件函数
                if (null != _prePrepare)
                    _prePrepare();

                //设置序列号
                _m_lPrepareSerialize = ALSerializeOpMgr.next();

                //放入准备队列中
                ALVideoPlayerMgr.instance._m_avqAVProPrepareQueue.addAVProPlayerPrepare(this);
                ////开始准备 - AVPro使用OpenVideoFromFile
                //if (_m_currentPathResource != null)
                //    _m_avproPlayer?.OpenMedia(MediaPathType.AbsolutePathOrURL, _m_currentPathResource.resourcePath,
                //        false);
            }
        }

        /// <summary>
        /// 实际处理prepare的函数，此函数需要在统一的管理器中调用
        /// </summary>
        protected internal void _dealPrepare()
        {
            //判断是否允许提前加载，如果不允许则直接调用回调返回
            if (!ALVideoLoadLegalJudger.instance.judgeCanPrepareVideo())
            {
                //重置状态
                _m_bIsInited = false;
                _m_bStartPrepare = false;
                //设置序列号
                _m_lPrepareSerialize = ALSerializeOpMgr.next();

                //直接调用完成回调
                Action<bool, string> prePrepareDoneDelegate = _m_dPrepareDoneDelegate;
                _m_dPrepareDoneDelegate = default(Action<bool, string>);
                if (null != prePrepareDoneDelegate)
                    prePrepareDoneDelegate(true, string.Empty);

                return;
            }

            _m_iPreapreFrameCount = Time.frameCount;
            //开始准备 - AVPro使用OpenVideoFromFile
            if (_m_currentPathResource != null)
            {
                _m_avproPlayer?.OpenMedia(MediaPathType.AbsolutePathOrURL, _m_currentPathResource.resourcePath,
                    false);
            }
        }

        /// <summary>
        /// 设置当前播放的帧数
        /// </summary>
        /// <param name="_frame"></param>
        public override void setFrame(int _frame, float _frameRate = 30f)
        {
            if (null == _m_avproPlayer)
                return;

            if (_m_avproPlayer.Control != null)
                _m_avproPlayer.Control.SeekToFrame(_frame, _frameRate);
        }

        /// <summary>
        /// 播放视频
        /// </summary>
        /// <param name="_isLoop"></param>
        /// <param name="_forceFresh"></param>
        public override void play(bool _isLoop, bool _forceFresh = true)
        {
            if (null == _m_avproPlayer)
                return;

            _m_lSeirialize = ALSerializeOpMgr.next();
            _m_avproPlayer.Loop = _isLoop;

            //设置播放序列号一致，AV需要在Play之前设置，否则可能无法在播放之后更改
            _m_lPlaySerialize = _m_lSeirialize;

            if (null == _m_avproPlayer.Control)
                return;

            //由于如果同一个播放器继续触发播放函数不回执行任何事件，这里为了保证序列号的刷新，需要手动下一帧触发检查
            //下一帧判断是否还在播放，且序列号一致，是则刷新序列号
            long tmpSerialize = _m_lSeirialize;
            ALCommonTaskController.CommonActionAddNextFrameTask(
                () => {
                    if (null == _m_avproPlayer.Control)
                        return;

                    //序列号仍然一致，且还在播放中，补充一个逻辑处理
                    if (tmpSerialize == _m_lSeirialize && _m_avproPlayer.Control.IsPlaying())
                        _onStarted();
                });

            _m_avproPlayer.Control.Play();
        }

        /// <summary>
        /// 暂停到某一帧
        /// </summary>
        /// <param name="_frameCount"></param>
        public override void pauseToFrame(int _frameCount, float _frameRate = 30f)
        {
            _m_lSeirialize = ALSerializeOpMgr.next();
            if (null == _m_avproPlayer)
                return;

            if (_m_avproPlayer.Control != null)
                _m_avproPlayer.Control.Pause();
            setFrame(_frameCount, _frameRate);
        }

        /// <summary>
        /// 停止播放
        /// </summary>
        public override void stop()
        {
            _m_lSeirialize = ALSerializeOpMgr.next();
            if (_m_avproPlayer != null && _m_avproPlayer.Control != null)
            {
                _m_avproPlayer.Control.Stop();
                //重置到0帧
                _m_avproPlayer.Control.SeekToFrame(0);
            }
            
            if (_m_applyToMaterial != null) 
                _m_applyToMaterial.Material = null;
        }

        /// <summary>
        /// 修改播放倍率
        /// </summary>
        /// <param name="_speed"></param>
        public override void playbackSpeed(float _speed)
        {
            if (_m_avproPlayer != null) 
                _m_avproPlayer.PlaybackRate = _speed;
        }

        /// <summary>
        /// 在被使用时调用的函数，可以把部分需要在reset重置的部分开出来
        /// </summary>
        public override void onUsed()
        {
            ALUGUICommon.setGameObjEnable(_m_go);
        }

        /// <summary>
        /// 在不被使用时调用的函数
        /// </summary>
        public override void onUnUsed()
        {
            ALUGUICommon.setGameObjDisable(_m_go);
        }

        /// <summary>
        /// 重置数据
        /// </summary>
        protected override internal void _reset()
        {
            //重置相关数据
            _m_lPrepareSerialize = ALSerializeOpMgr.next();
            _m_lSeirialize = ALSerializeOpMgr.next();

            //重置数据
            _resetData();
        }

        /// <summary>
        /// 单纯重置数据，不更改序列号
        /// 避免在数据设置的时候序列号更改导致回调问题
        /// </summary>
        protected internal void _resetData()
        {
            Action<bool, string> prePrepareDoneDelegate = _m_dPrepareDoneDelegate;
            _m_dPrepareDoneDelegate = default(Action<bool, string>);
            if (null != prePrepareDoneDelegate)
                prePrepareDoneDelegate(false, "Be Reset!");

            //如果已经初始化，则需要从统计中删除，因为要删除所以要先调用
            if (_m_bIsInited)
            {
                ALVideoLoadLegalJudger.instance._onVideoReset(this);
            }

            //重置相关数据
            _m_bIsInited = false;
            _m_bStartPrepare = false;
            _m_currentResource = null;
            _m_currentPathResource = null;

            if (_m_avproPlayer != null)
            {
                _m_avproPlayer.CloseMedia();
                _m_avproPlayer.PlaybackRate = 1.0f;
                _m_avproPlayer.AudioMuted = true;

                if (Time.frameCount == _m_iPreapreFrameCount)
                {
                    //记录当前准备操作序列号
                    long tmpSerialize = _m_lPrepareSerialize;

                    ALCommonTaskController.CommonActionAddNextFrameTask(() =>
                    {
                        //如果做了其他的加载行为，则此处不允许释放
                        if (tmpSerialize != _m_lPrepareSerialize)
                            return;

                        _m_avproPlayer.ForceDispose();
                    });
                }
                else
                {
                    _m_avproPlayer.ForceDispose();
                }
            }

            if (_m_applyToMaterial != null)
                _m_applyToMaterial.Material = null;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void discard()
        {
            //重置数据
            reset();

            if (_m_avproPlayer != null)
            {
                _m_avproPlayer.Events?.RemoveListener(OnMediaPlayerEvent);
            }

            //直接调用释放函数
            GameObject.Destroy(_m_go);
            _m_go = null;
            _m_avproPlayer = null;
        }

        /// <summary>
        /// 设置音频输出源
        /// </summary>
        /// <param name="_trackIndex"></param>
        /// <param name="_audioSource"></param>
        public override void SetTargetAudioSource(ushort _trackIndex, AudioSource _audioSource)
        {
            if(_audioSource == null)
                return;
            if (_m_audioSource != null)
                _m_audioSource.outputAudioMixerGroup = _audioSource.outputAudioMixerGroup;
               
            // if (_m_avproPlayer != null)
            // {
            //     _m_avproPlayer.AudioSource = _audioSource;   // Apply volume and mute from the MediaPlayer to the AudioSource
            //     if (_audioSource != null && _m_avproPlayer != null && _m_avproPlayer.Control != null)
            //     {
            //         float volume = _m_avproPlayer.Control.GetVolume();
            //         bool isMuted = _m_avproPlayer.Control.IsMuted();
            //         float rate = _m_avproPlayer.Control.GetPlaybackRate();
            //         _audioSource.volume = volume;
            //         _audioSource.mute = isMuted;
            //         _audioSource.pitch = rate;
            //     }
            // }
        }

        /// <summary>
        /// 设置音频是否静音
        /// </summary>
        /// <param name="_isMute">是否静音</param>
        public override void setAudioMute(ushort _trackIndex, bool _isMute)
        {
            if(null == _m_avproPlayer)
                return;

            _m_avproPlayer.AudioMuted = _isMute;
        }

        /// <summary>
        /// 设置音频音量
        /// </summary>
        public override void setAudioVolume(ushort _trackIndex, float _volume)
        {
            if(null == _m_avproPlayer)
                return;

            _m_avproPlayer.AudioVolume = _volume;
        }


        /// <summary>
        /// 获取视频实例ID
        /// </summary>
        /// <returns></returns>
        public override long getVideoInstanceId()
        {
            if (_m_currentResource != null && _m_currentResource.resourcePath != null) 
                return _m_currentResource.resourcePath.GetHashCode();
            return _AALVideoPlayerDealer.g_iInvalidVideoInstanceID;
        }

        /// <summary>
        /// AVPro事件处理
        /// </summary>
        /// <param name="player"></param>
        /// <param name="eventType"></param>
        /// <param name="errorCode"></param>
        private void OnMediaPlayerEvent(MediaPlayer player, MediaPlayerEvent.EventType eventType, ErrorCode errorCode)
        {
            switch (eventType)
            {
                case MediaPlayerEvent.EventType.FirstFrameReady:
                    _onPrepareDone();
                    break;
                    
                case MediaPlayerEvent.EventType.Started:
                case MediaPlayerEvent.EventType.StartedSeeking:
                    _onStarted();
                    break;
                    
                case MediaPlayerEvent.EventType.Error:
                    //判断是否在加载中，是需要调用错误回调
                    if(_m_bStartPrepare && !_m_bIsInited)
                    {
                        //设置准备完成
                        _m_bIsInited = true;
                        //设置序列号
                        _m_lPrepareSerialize = ALSerializeOpMgr.next();

                        //累加控制类的加载统计
                        ALVideoLoadLegalJudger.instance._onVideoPrepared(this);

                        Action<bool, string> prePrepareDoneDelegate = _m_dPrepareDoneDelegate;
                        _m_dPrepareDoneDelegate = default(Action<bool, string>);
                        if (null != prePrepareDoneDelegate)
                            prePrepareDoneDelegate(false, errorCode.ToString());
                    }

                    _onErrorReceived(errorCode.ToString());
                    break;
            }
        }

        /// <summary>
        /// 准备完成的回调处理
        /// </summary>
        private void _onPrepareDone()
        {
            //设置准备完成
            _m_bIsInited = true;
            //设置序列号
            _m_lPrepareSerialize = ALSerializeOpMgr.next();
            //停止播放
            _m_avproPlayer.Control.SeekToFrame(0);
            //当播放序列号和操作序列号一致时，表明是在play的操作上的prepare，此时需要播放
            //当序列号不一致时此时需要暂停
            if (_m_lPlaySerialize != _m_lSeirialize)
                _m_avproPlayer.Control.Pause();

            //累加控制类的加载统计
            ALVideoLoadLegalJudger.instance._onVideoPrepared(this);

            Action<bool, string> prePrepareDoneDelegate = _m_dPrepareDoneDelegate;
            _m_dPrepareDoneDelegate = default(Action<bool, string>);
            if (null != prePrepareDoneDelegate)
                prePrepareDoneDelegate(true, string.Empty);
        }

        /// <summary>
        /// 开始播放的触发函数
        /// </summary>
        private void _onStarted()
        {
            //刷新序列号
            _m_lSeirialize = ALSerializeOpMgr.next();
        }
        
        /// <summary>
        /// 视频播放错误
        /// </summary>
        /// <param name="_msg"></param>
        private void _onErrorReceived(string _msg)
        {
            // 调用新的接口，支持IALVideoPlayerCore
            ALGlobalControl.instance._onVideoErrorReceived(_m_currentResource, this, _msg);
        }
    }
#endif
}
