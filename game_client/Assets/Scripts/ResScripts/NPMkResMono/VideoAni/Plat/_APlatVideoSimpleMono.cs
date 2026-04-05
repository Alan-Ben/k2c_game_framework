using ALPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;
using static GOE.VideoAniStateMachine;

////
///使用视频方式进行动画管理的相关脚本
///
namespace GOE
{
    /// <summary>
    /// 简易版的脚本基类对象，子类根据具体RT的显示方式进行实现
    /// </summary>
    public abstract class _APlatVideoSimpleMono : MonoBehaviour
    {
        [ALHeader("StreamingAssets相对的视频路径(例:plat/vc_1_1.mp4)")]
        public string relativeStreamingAssetsVideoPath;
        [ALHeader("是否独立背景音乐")]
        public bool isBgMusic;
        [ALHeader("是否循环播放")]
        public bool isLoop = true;
        
        //当前Mono的有效序列号，避免无效加载和处理
        [System.NonSerialized]
        private long _m_lSerialize;
        //当前逻辑上是否显隐，保证显隐函数是相对的
        [System.NonSerialized]
        private bool _m_curActiveInHierarchy;
        //是否需要检测，本标记是为了避免同帧多次出发导致频繁创建释放RT增加的检测行为
        [System.NonSerialized]
        protected bool _m_bNeedCheck = false;
        
        //实际的视频播放对象，显示时创建，隐藏时销毁
        [System.NonSerialized]
        private ALVideoPlayer _m_vpPlayer;
        
        //视频准备完成之后的回调管理器
        private ALCommonStateDelegate _m_prepareDoneDelegate = new ALCommonStateDelegate();
        
        //有效和无效的时候分别注册和注销显示对象
        protected void OnEnable()
        {
            //判断是否已经准备检测，避免当帧过多检测
            if (_m_bNeedCheck)
                return;

            _m_bNeedCheck = true;

            //到管理对象中进行处理
            ALCommonActionMonoTask.addNextFrameTask(_check);
        }

        protected void OnDisable()
        {
            //判断是否已经准备检测，避免当帧过多检测
            if (_m_bNeedCheck)
                return;

            _m_bNeedCheck = true;

            //到管理对象中进行处理
            ALCommonActionMonoTask.addNextFrameTask(_check);
        }

        protected void OnDestroy()
        {
            _m_bNeedCheck = true;
            //强制释放相关资源和状态机
            _discard();
        }

        protected void _check()
        {
            if (!_m_bNeedCheck || null == this || null == gameObject)
            {
                return;
            }

            _m_bNeedCheck = false;

            //当前需要显示，之前不显示，执行显示函数
            if (gameObject.activeInHierarchy && !_m_curActiveInHierarchy)
            {
                _m_curActiveInHierarchy = true;
                _onVedioEnable();
            }
            //当前不需要显示，之前是显示的，才执行不显示函数
            else if (!gameObject.activeInHierarchy && _m_curActiveInHierarchy)
            {
                _m_curActiveInHierarchy = false;
                _onVedioDisable();
            }
        }

        private _IALVideoResource getVideoResource()
        {
#if NP_GAME
            if (relativeStreamingAssetsVideoPath != null)
            {
                string videoPath = Path.Combine(Application.streamingAssetsPath, relativeStreamingAssetsVideoPath);
                return VideoController.getVideoResource(videoPath);
            }

            return null;
#else
            return null;
#endif
        }

        /// <summary>
        /// 在窗口显示的时候调用的函数
        /// </summary>
        protected void _onVedioEnable()
        {
#if NP_GAME
            //刷新序列号
            _m_lSerialize = ALSerializeOpMgr.next();
            long curSerialize = _m_lSerialize;
            //如果无VP对象则创建VP对象
            if (null == _m_vpPlayer)
            {
                //如果无VP对象则创建VP对象
                _m_vpPlayer = new ALVideoPlayer(_renderMat);
            }
            _m_vpPlayer.prepare(getVideoResource(), (bool _isSuc, string _err) =>
            {
                //播放进入部分动画
                playAniTag();

                //调用事件函数
                if (_m_prepareDoneDelegate != null) 
                    _m_prepareDoneDelegate.setInitDone();
                _onVedioPlayerPrepared();
            }, VideoController.audioMode, new AVProVideoPlayerDealerTextureInterface());
#endif
        }

        /// <summary>
        /// 在窗口无效的时候调用的函数
        /// </summary>
        protected void _onVedioDisable()
        {
            //刷新序列号
            _m_lSerialize = ALSerializeOpMgr.next();

            //释放Player对象
            if (null != _m_vpPlayer)
                _m_vpPlayer.discard();
            _m_vpPlayer = null;
            
            _m_prepareDoneDelegate?.reset();
        }

        /// <summary>
        /// 释放资源处理
        /// </summary>
        protected void _discard()
        {
            //刷新序列号
            _m_lSerialize = ALSerializeOpMgr.next();

            //释放Player对象
            if (null != _m_vpPlayer)
                _m_vpPlayer.discard();
            _m_vpPlayer = null;
        }

        /// <summary>
        /// 播放对应Tag的动画
        /// </summary>
        /// <param name="_tag"></param>
        public void playAniTag()
        {
#if NP_GAME
            //刷新序列号
            _m_lSerialize = ALSerializeOpMgr.next();
            long curSerialize = _m_lSerialize;

            //以序列号是否匹配作为是否播放的判断条件
            if (isLoop)
            {
                _m_vpPlayer?.loopPlayClip(getVideoResource(), null,  VideoController.audioMode, new AVProVideoPlayerDealerTextureInterface()
                    , (_AALVideoPlayerDealer _vp, VideoAudioOutputMode _mode) => { PlayAudioMgr.instance.onVideoClipPlay(_vp, _mode, isBgMusic); }
                    , (_AALVideoPlayerDealer _vp, VideoAudioOutputMode _mode) => { PlayAudioMgr.instance.onStopVideoPlaying(_vp, _mode, isBgMusic); });
            }
            else
            {
                _m_vpPlayer?.playClip(getVideoResource(), null, VideoController.audioMode, new AVProVideoPlayerDealerTextureInterface()
                    , (_AALVideoPlayerDealer _vp, VideoAudioOutputMode _mode) => { PlayAudioMgr.instance.onVideoClipPlay(_vp, _mode, isBgMusic); }
                    , (_AALVideoPlayerDealer _vp, VideoAudioOutputMode _mode) => { PlayAudioMgr.instance.onStopVideoPlaying(_vp, _mode, isBgMusic); });
            }
#endif
        }
        
        public void setAniTag()
        {
#if NP_GAME
            //刷新序列号
            _m_lSerialize = ALSerializeOpMgr.next();
            long curSerialize = _m_lSerialize;

            //以序列号是否匹配作为是否播放的判断条件
            if (isLoop)
            {
                _m_vpPlayer?.loopPlayClip(getVideoResource(), null,  VideoController.audioMode, new AVProVideoPlayerDealerTextureInterface()
                    , (_AALVideoPlayerDealer _vp, VideoAudioOutputMode _mode) => { PlayAudioMgr.instance.onVideoClipPlay(_vp, _mode, isBgMusic); }
                    , (_AALVideoPlayerDealer _vp, VideoAudioOutputMode _mode) => { PlayAudioMgr.instance.onStopVideoPlaying(_vp, _mode, isBgMusic); });
            }
            else
            {
                _m_vpPlayer?.playClip(getVideoResource(), null, VideoController.audioMode, new AVProVideoPlayerDealerTextureInterface()
                    , (_AALVideoPlayerDealer _vp, VideoAudioOutputMode _mode) => { PlayAudioMgr.instance.onVideoClipPlay(_vp, _mode, isBgMusic); }
                    , (_AALVideoPlayerDealer _vp, VideoAudioOutputMode _mode) => { PlayAudioMgr.instance.onStopVideoPlaying(_vp, _mode, isBgMusic); });
            }
#endif
        }

        /// <summary>
        /// 修改播放倍率
        /// </summary>
        /// <param name="_speed"></param>
        public void playbackSpeed(float _speed)
        {
            if (_m_vpPlayer != null) 
                _m_vpPlayer.playbackSpeed(_speed);
        }

        public bool isPlaying()
        {
            return _m_vpPlayer != null && _m_vpPlayer.isPlaying;
        }
        /// <summary>
        /// 注册视频预加载完成事件
        /// </summary>
        /// <param name="_delegate"></param>
        public void regVideoPreparedDone(Action _delegate)
        {
            if (_m_prepareDoneDelegate != null) 
                _m_prepareDoneDelegate.regDelegate(_delegate);
            else
                _delegate?.Invoke();
        }

        /// <summary>
        /// 获取渲染的材质对象
        /// </summary>
        /// <param name="_rt"></param>
        protected abstract Material _renderMat { get; }

        /// <summary>
        /// 设置渲染的显隐
        /// </summary>
        /// <param name="_isEnable"></param>
        public abstract void setRenderEnable(bool _isEnable);
        
        /// <summary>
        /// 在本视频播放器初始化的时候触发的事件函数
        /// 如果初始化动作有需要，可以在本函数进行切换
        /// </summary>
        protected abstract void _onVedioPlayerPrepared();
    }
}