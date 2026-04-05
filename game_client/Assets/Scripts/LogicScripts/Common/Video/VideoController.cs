using ALPackage;
using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Video;
using static RenderHeads.Media.AVProVideo.MediaPlayer.OptionsApple;

namespace GOE
{
    /// <summary>
    /// 视频播放的接口处理对象
    /// </summary>
    public class VideoController
    {
        public static VideoAudioOutputMode audioMode => VideoAudioOutputMode.AudioSource;
        public static _IALVideoResource getVideoResource(string _realAssetPath)
        {
#if UNITY_EDITOR
            // 如果是编辑器下，可以通过这个切换播放方式
            if(MainCameraMono.selfInstance.useVideoPlayerPlay)
                return new ALVideoResourceURL(_realAssetPath); 
#endif
            // AVPro模式：直接使用路径
#if AL_AVPRO_V2
#if UNITY_IOS
                    _IALVideoResource resource = new ALVideoResourceURL(_realAssetPath);
#else
            _IALVideoResource resource = new ALVideoResourceAVPro2(_realAssetPath);
#endif
#else
                    _IALVideoResource resource = null;
#endif
            return resource;
        }
        /// <summary>
        /// 加载视频资源对象
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_loadDelegate"></param>
        public static void loadVideoResource(GVideoClipIndex _index, Action<_IALVideoResource> _loadDelegate)
        {
            //无回调直接不处理
            if (null == _loadDelegate)
                return;

            if(null == _index)
            {
                //直接处理回调
                if(null != _loadDelegate)
                    _loadDelegate(null);
                return;
            }
            VideoResCore.instance.loadObj(_index.assetPath, (_realAssetPath) =>
            {
                if (!string.IsNullOrEmpty(_realAssetPath))
                {
                    _loadDelegate?.Invoke(getVideoResource(_realAssetPath));
                }
                else
                {
                    _loadDelegate?.Invoke(null);
                }
            });
        }
        

        /// <summary>
        /// 使用Vp准备对应视频资源的直接调用接口
        /// </summary>
        /// <param name="_vcIndex"></param>
        /// <param name="_vp"></param>
        /// <param name="_judegePrepareEnable">可以使用本参数增加一些序列号的判断，避免延迟加载的资源已经过了播放的时效</param>
        /// <param name="_onResourcePrepared">在准备视频的时候可以调用的事件函数</param>
        public static void prepareVideoClip(GVideoClipIndex _vcIndex, ALVideoPlayer _vp,  VideoAudioOutputMode _audioMode, Func<bool> _judegePrepareEnable = null, Action<bool, string> _onResourcePrepared = null)
        {
            if (null == _vcIndex || null == _vp)
                return;

            //加载视频资源，放入播放器
            loadVideoResource(_vcIndex
                , (_resource) => {
                    //合法性判断不通过，直接返回
                    if (null != _judegePrepareEnable && !_judegePrepareEnable())
                        return;

                    //准备
                    _vp.prepare(_resource, _onResourcePrepared, _audioMode, new AVProVideoPlayerDealerTextureInterface());
                });
        }

        /// <summary>
        /// 使用Vp播放对应视频资源的直接调用接口
        /// </summary>
        /// <param name="_vcIndex"></param>
        /// <param name="_vp"></param>
        /// <param name="_judegePlayEnable">可以使用本参数增加一些序列号的判断，避免延迟加载的资源已经过了播放的时效</param>
        /// <param name="_onPlayResource">在播放视频的时候可以调用的事件函数</param>
        public static void playVideoClip(GVideoClipIndex _vcIndex, ALVideoPlayer _vp,  VideoAudioOutputMode _audioMode, bool _isBgMusic,  Func<bool> _judegePlayEnable = null, Action<_IALVideoResource> _onPlayResource = null)
        {
            if(null == _vcIndex || null == _vp)
                return;

            //加载视频资源，放入播放器
            loadVideoResource(_vcIndex
                , (_resource) => {
                    //合法性判断不通过，直接返回
                    if (null != _judegePlayEnable && !_judegePlayEnable())
                        return;

                    //播放
                    _vp.playClip(_resource, () => _onPlayResource?.Invoke(_resource), _audioMode, new AVProVideoPlayerDealerTextureInterface()
                        , (_AALVideoPlayerDealer _vp, VideoAudioOutputMode _mode) => { PlayAudioMgr.instance.onVideoClipPlay(_vp, _mode, _isBgMusic, _vcIndex); }
                        , (_AALVideoPlayerDealer _vp, VideoAudioOutputMode _mode) => { PlayAudioMgr.instance.onStopVideoPlaying(_vp, _mode, _isBgMusic); });
                });
        }

        /// <summary>
        /// 使用Vp循环播放对应视频资源的直接调用接口
        /// </summary>
        /// <param name="_vcIndex"></param>
        /// <param name="_vp"></param>
        /// <param name="_judegePlayEnable">可以使用本参数增加一些序列号的判断，避免延迟加载的资源已经过了播放的时效</param>
        /// <param name="_onPlayResource">在播放视频的时候可以调用的事件函数</param>
        public static void loopPlayVideoClip(GVideoClipIndex _vcIndex, ALVideoPlayer _vp, VideoAudioOutputMode _audioMode, bool _isBgMusic, Func<bool> _judegePlayEnable = null, Action<_IALVideoResource> _onPlayResource = null)
        {
            if (null == _vcIndex || null == _vp)
                return;

            //加载视频资源，放入播放器
            loadVideoResource(_vcIndex
                , (_resource) => {
                    //合法性判断不通过，直接返回
                    if (null != _judegePlayEnable && !_judegePlayEnable())
                        return;

                    //播放
                    _vp.loopPlayClip(_resource, () => _onPlayResource?.Invoke(_resource), _audioMode, new AVProVideoPlayerDealerTextureInterface()
                        , (_AALVideoPlayerDealer _vp, VideoAudioOutputMode _mode) => { PlayAudioMgr.instance.onVideoClipPlay(_vp, _mode, _isBgMusic, _vcIndex); }
                        , (_AALVideoPlayerDealer _vp, VideoAudioOutputMode _mode) => { PlayAudioMgr.instance.onStopVideoPlaying(_vp, _mode, _isBgMusic); });
                });
        }
    }
}