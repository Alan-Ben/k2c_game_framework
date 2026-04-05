using System;
using ALPackage;
using UnityEngine;
using UnityEngine.Video;

namespace ALPackage
{
    /// <summary>
    /// Unity VideoPlayer的封装实现,使用路径播放
    /// 继承自原有的_ALVideoPlayerDealer，实现IALVideoPlayerCore接口
    /// </summary>
    public class ALVideoPlayerDealerUnityURL : _AALVideoPlayerDealer
    {
        //构造的原始对象
        private GameObject _m_go;
        private VideoPlayer _m_vpPlayer;

        private long _m_lSeirialize;
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

        public ALVideoPlayerDealerUnityURL(Transform _parentTrans, string _name)
        {
            //创建空go
            _m_go = new GameObject(_name);
            //设置父节点
            _m_go.transform.SetParent(_parentTrans);

            _m_lSeirialize = ALSerializeOpMgr.next();
            _m_lPlaySerialize = 0;
            //创建VideoPlayer脚本
            _m_vpPlayer = _m_go.AddComponent<VideoPlayer>();
            //初始化相关参数
            _m_vpPlayer.source = VideoSource.Url;
            _m_vpPlayer.playOnAwake = false;
            _m_vpPlayer.waitForFirstFrame = true;
            _m_vpPlayer.isLooping = false;
            _m_vpPlayer.skipOnDrop = true;
            _m_vpPlayer.playbackSpeed = 1;
            _m_vpPlayer.renderMode = VideoRenderMode.APIOnly;
            _m_vpPlayer.targetTexture = null;
            //不播放音效
            _m_vpPlayer.audioOutputMode = VideoAudioOutputMode.None;

            _m_bIsInited = false;
            _m_bStartPrepare = false;
            //预注册prepare事件
            _m_vpPlayer.prepareCompleted += _onPrepareDone;
            _m_vpPlayer.started += _onStarted;
            _m_vpPlayer.errorReceived += _onErrorReceived;
            _m_dPrepareDoneDelegate = default(Action<bool, string>);
            _m_currentResource = null;
        }

        /// <summary>
        /// 内部接口获取Vp的属性对象
        /// </summary>
        protected internal VideoPlayer _vp { get { return _m_vpPlayer; } }

        public override bool isPlaying { get { return null == _m_vpPlayer ? false : (_m_lSeirialize == _m_lPlaySerialize || _m_vpPlayer.isPlaying); } }
        public override Action<bool, string> onPrepareDone { get { return _m_dPrepareDoneDelegate; } set { _m_dPrepareDoneDelegate = value; } }
        /// <summary>
        /// 获取当前VideoPlayer的Texture
        /// </summary>
        public override Texture texture { get { return null == _m_vpPlayer ? null : _m_vpPlayer.texture; } }

        public override int videoWidth
        {
            get
            {
                if (null == _m_vpPlayer)
                    return 0;

                return (int)_m_vpPlayer.width;
            }
        }
        public override int videoHeight
        {
            get
            {
                if (null == _m_vpPlayer)
                    return 0;

                return (int)_m_vpPlayer.height;
            }
        }

        public override double length
        {
            get { return _m_vpPlayer?.length ?? 60; }
        }
        /// <summary>
        /// 设置音频播放模式
        /// </summary>
        /// <param name="_mode"></param>
        public override void setAudioOutputMode(VideoAudioOutputMode _mode)
        {
            if (null == _m_vpPlayer)
                return;

            _m_vpPlayer.audioOutputMode = _mode;
        }

        protected override void _defaultSetTarget(Material _mat)
        {
            if (_mat != null)
                _mat.mainTexture = texture;
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
            //Unity未做排序，直接处理
            prepare(_resource, _prePrepare, _onPrepareDone);
        }
        /// <summary>
        /// 准备视频，并在准备完成的时候调用回调
        /// </summary>
        /// <param name="resource">视频资源</param>
        /// <param name="_prePrepare">准备前的回调</param>
        /// <param name="_onPrepareDone">准备完成的回调</param>
        public override void prepare(_IALVideoResource _resource, Action _prePrepare, Action<bool, string> _onPrepareDone)
        {
            // Unity播放器只支持VideoClip资源
            if (!(_resource is ALVideoResourceURL urlResource))
            {
                Debug.LogError("Unity VideoPlayer only supports ALVideoResourceURL");
                if (null != _onPrepareDone)
                    _onPrepareDone(false, "Resource format err!");
                return;
            }
            
            //不是同一个播放对象时需要重置本对象准备状态
            if (!_resource.Equals(_m_currentResource))
            {
                //重置数据，该释放的资源需要释放
                _resetData();
                _m_currentResource = _resource;
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

                //设置clip
                _m_vpPlayer.url = urlResource.resourcePath;

                //调用事件函数
                if (null != _prePrepare)
                    _prePrepare();
                //开始准备
                _m_vpPlayer.Prepare();
            }
        }

        /// <summary>
        /// 设置当前播放的帧数
        /// </summary>
        /// <param name="_frame"></param>
        public override void setFrame(int _frame, float _frameRate = 30f)
        {
            if (null == _m_vpPlayer)
                return;

            _m_vpPlayer.frame = _frame;
        }

        /// <summary>
        /// 播放到Rt上
        /// </summary>
        /// <param name="_isLoop"></param>
        /// <param name="_forceFresh">用于表达是否强制刷新，在视频切换过程如果设置Rt会清空显示，通过这个可以强制刷新一帧，但是可能跳过一帧</param>
        public override void play(bool _isLoop, bool _forceFresh = true)
        {
            _m_lSeirialize = ALSerializeOpMgr.next();
            _m_vpPlayer.isLooping = _isLoop;

            //调用暂停播放，确保onStart会调用
            _m_vpPlayer.Pause();

            //设置播放序列号一致，AV需要在Play之前设置，否则可能无法在播放之后更改
            _m_lPlaySerialize = _m_lSeirialize;

            //alzq:暂时屏蔽，后续如无闪帧问题就删除
            //if (_forceFresh)
            //    _m_vpPlayer.StepForward();
            _m_vpPlayer.Play();
        }

        /// <summary>
        /// 暂停到某一帧
        /// </summary>
        /// <param name="_frameCount"></param>
        public override void pauseToFrame(int _frameCount, float _frameRate = 30f)
        {
            _m_lSeirialize = ALSerializeOpMgr.next();
            if (null == _m_vpPlayer)
                return;

            _m_vpPlayer.frame = _frameCount;
            _m_vpPlayer.Pause();
        }

        /// <summary>
        /// 暂停到某一帧
        /// </summary>
        /// <param name="_frameCount"></param>
        public override void stop()
        {
            _m_lSeirialize = ALSerializeOpMgr.next();
            if (null == _m_vpPlayer)
                return;

            _m_vpPlayer.frame = 0;
            _m_vpPlayer.Stop();
        }

        /// <summary>
        /// 修改播放倍率
        /// </summary>
        /// <param name="_speed"></param>
        public override void playbackSpeed(float _speed)
        {
            if (_m_vpPlayer != null) 
                _m_vpPlayer.playbackSpeed = _speed;
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

            if (_m_vpPlayer != null)
            {
                _m_vpPlayer.targetTexture = null;
                _m_vpPlayer.Stop();
                _m_vpPlayer.clip = null;
                _m_vpPlayer.playbackSpeed = 1;
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void discard()
        {
            //重置数据
            reset();

            //直接调用释放函数
            GameObject.Destroy(_m_go);
            _m_go = null;
            _m_vpPlayer = null;
        }

        /// <summary>
        /// 设置音频输出源
        /// </summary>
        /// <param name="_trackIndex"></param>
        /// <param name="_audioSource"></param>
        public override void SetTargetAudioSource(ushort _trackIndex, AudioSource _audioSource)
        {
            _m_vpPlayer?.SetTargetAudioSource(_trackIndex, _audioSource);
        }

        /// <summary>
        /// 设置音频是否静音
        /// </summary>
        /// <param name="_isMute">是否静音</param>
        public override void setAudioMute(ushort _trackIndex, bool _isMute)
        {
            if(null == _m_vpPlayer)
                return;
            
            _m_vpPlayer.SetDirectAudioMute(_trackIndex, _isMute);
            AudioSource audioSource = _m_vpPlayer.GetTargetAudioSource(_trackIndex);
            if(null != audioSource)
                audioSource.mute = _isMute;
        }

        /// <summary>
        /// 设置音频音量
        /// </summary>
        public override void setAudioVolume(ushort _trackIndex, float _volume)
        {
            if(null == _m_vpPlayer)
                return;

            AudioSource audioSource = _m_vpPlayer.GetTargetAudioSource(_trackIndex);
            if(null != audioSource)
                audioSource.volume = _volume;
        }

        /// <summary>
        /// 获取视频实例ID
        /// </summary>
        /// <returns></returns>
        public override long getVideoInstanceId()
        {
            if (_m_vpPlayer != null && _m_vpPlayer.url != null) 
                return _m_vpPlayer.url.GetHashCode();
            return _AALVideoPlayerDealer.g_iInvalidVideoInstanceID;
        }

        /// <summary>
        /// 播放处理完成的回调处理
        /// </summary>
        /// <param name="_vp"></param>
        protected void _onPrepareDone(VideoPlayer _vp)
        {
            //播放器对象不同不进行处理
            if (_vp != _m_vpPlayer)
                return;

            //设置准备完成
            _m_bIsInited = true;

            //停止播放
            _m_vpPlayer.frame = 0;
            //当播放序列号和操作序列号一致时，表明是在play的操作上的prepare，此时需要播放
            //当序列号不一致时此时需要暂停
            if(_m_lPlaySerialize != _m_lSeirialize)
                _m_vpPlayer.Pause();

            //累加控制类的加载统计
            ALVideoLoadLegalJudger.instance._onVideoPrepared(this);

            Action<bool, string> prePrepareDoneDelegate = _m_dPrepareDoneDelegate;
            _m_dPrepareDoneDelegate = default(Action<bool, string>);
            if (null != prePrepareDoneDelegate)
                prePrepareDoneDelegate(true, string.Empty);
        }

        /// <summary>
        /// 开始播放的触发函数，调用start后触发这个才是真的play
        /// </summary>
        /// <param name="_vp"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void _onStarted(VideoPlayer _vp)
        {
            //刷新序列号
            _m_lSeirialize = ALSerializeOpMgr.next();
        }
        
        /// <summary>
        /// 视频播放问题等错误
        /// </summary>
        /// <param name="_vp"></param>
        /// <param name="_msg"></param>
        private void _onErrorReceived(VideoPlayer _vp, string _msg)
        {
            //判断是否在加载中，是需要调用错误回调
            if (_m_bStartPrepare && !_m_bIsInited)
            {
                //设置准备完成
                _m_bIsInited = true;

                Action<bool, string> prePrepareDoneDelegate = _m_dPrepareDoneDelegate;
                _m_dPrepareDoneDelegate = default(Action<bool, string>);
                if (null != prePrepareDoneDelegate)
                    prePrepareDoneDelegate(false, _msg);
            }

            // 调用新的接口，支持IALVideoPlayerCore
            ALGlobalControl.instance._onVideoErrorReceived(_m_currentResource, this, _msg);
        }
        
    }
}
