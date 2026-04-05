using System;
using System.Collections.Generic;
using System.Diagnostics;
using ALPackage;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Video;

/*********************
 * 实际操作Video播放的对象
 **/
namespace ALPackage
{
    public class ALVideoPlayer
    {
        //默认的Rt颜色
        private Color _m_cDefaultRtColor;

        //播放的视频对象列表，这个列表是跟着播放对象走的，要避免多个对象使用同一个视频对象
        private List<ALVideoPlayerClipObj> _m_clips;
        //当前播放的视频对象
        private ALVideoPlayerClipObj _m_vpCurClip;
        private long _m_lCurClipPlaySerialize;

        //视频的渲染材质对象，所有的Clip会渲染到这个目标对象上
        private Material _m_mCurMaterialTarget;

        /// <summary>
        /// 仅提供底层包内部初始化，不对外开放
        /// </summary>
        protected internal ALVideoPlayer(Material _renderMat)
        {
            _m_clips = new List<ALVideoPlayerClipObj>();
            _m_vpCurClip = null;
            _m_mCurMaterialTarget = _renderMat;
        }

        //Alzq：由于这里会内部不断切换，这里不对外啊开放
        //public Texture texture { get { return null == _m_vpCurClip ? null :_m_vpCurClip.texture; } }

        public bool isPlaying { get { return null == _m_vpCurClip ? false : _m_vpCurClip.isPlaying; } }

        /// <summary>
        /// 设置视频渲染目标
        /// </summary>
        /// <param name="_mat"></param>
        public void setTarget(Material _mat)
        {
            _m_mCurMaterialTarget = _mat;

            //如果已经再播放中，需要设置播放对象
            if (null != _m_vpCurClip)
                _m_vpCurClip.setTarget(_mat);
        }

        /// <summary>
        /// 强制马上开始准备播放一个视频
        /// </summary>
        /// <param name="_resource">视频资源</param>
        /// <param name="_onPrepareDone">准备完成回调</param>
        /// <param name="_audioMode">音频模式</param>
        public void forcePrepare(_IALVideoResource _resource, Action<bool, string> _onPrepareDone, VideoAudioOutputMode _audioMode = VideoAudioOutputMode.None)
        {
            forcePrepare(_resource, _onPrepareDone, _audioMode, null);
        }
        public void forcePrepare(_IALVideoResource _resource, Action<bool, string> _onPrepareDone, _AALVideoPlayerDealerTextureInterface _textureInterface = null)
        {
            forcePrepare(_resource, _onPrepareDone, VideoAudioOutputMode.None, _textureInterface);
        }
        public void forcePrepare(_IALVideoResource _resource, Action<bool, string> _onPrepareDone, VideoAudioOutputMode _audioMode, _AALVideoPlayerDealerTextureInterface _textureInterface)
        {
            //此处直接调用回调
            if (null == _resource)
            {
                //避免死循环，下帧调用
                ALCommonTaskController.CommonActionAddNextFrameTask(()=>_onPrepareDone(false, "Resource is Null!"));
                return;
            }

            ALVideoPlayerClipObj clipObj = _lookupClipObj(_resource);
            if (null == clipObj)
            {
                //创建新对象，并放入队列
                clipObj = new ALVideoPlayerClipObj(_resource, _audioMode, _textureInterface);
                _m_clips.Add(clipObj);
            }

            //用准备函数
            clipObj.forcePrepare(_onPrepareDone);
        }

        /// <summary>
        /// 准备播放一个视频
        /// </summary>
        /// <param name="_resource">视频资源</param>
        /// <param name="_onPrepareDone">准备完成回调</param>
        /// <param name="_audioMode">音频模式</param>
        public void prepare(_IALVideoResource _resource, Action<bool, string> _onPrepareDone, VideoAudioOutputMode _audioMode = VideoAudioOutputMode.None)
        {
            prepare(_resource, _onPrepareDone, _audioMode, null);
        }
        public void prepare(_IALVideoResource _resource, Action<bool, string> _onPrepareDone, _AALVideoPlayerDealerTextureInterface _textureInterface = null)
        {
            prepare(_resource, _onPrepareDone, VideoAudioOutputMode.None, _textureInterface);
        }
        public void prepare(_IALVideoResource _resource, Action<bool, string> _onPrepareDone, VideoAudioOutputMode _audioMode, _AALVideoPlayerDealerTextureInterface _textureInterface)
        {
            //此处直接调用回调
            if (null == _resource)
            {
                //避免死循环，下帧调用
                ALCommonTaskController.CommonActionAddNextFrameTask(()=>_onPrepareDone(false, "Resource is Null!"));
                return;
            }

            ALVideoPlayerClipObj clipObj = _lookupClipObj(_resource);
            if (null == clipObj)
            {
                //创建新对象，并放入队列
                clipObj = new ALVideoPlayerClipObj(_resource, _audioMode, _textureInterface);
                _m_clips.Add(clipObj);
            }

            //用准备函数
            clipObj.prepare(_onPrepareDone);
        }

        /// <summary>
        /// 准备播放一个视频 (兼容原有VideoClip接口)
        /// </summary>
        /// <param name="_clip">视频片段</param>
        /// <param name="_onPrepareDone">准备完成回调</param>
        /// <param name="_audioMode">音频模式</param>
        public void prepare(VideoClip _clip, Action<bool, string> _onPrepareDone, VideoAudioOutputMode _audioMode = VideoAudioOutputMode.None)
        {
            prepare(_clip, _onPrepareDone, _audioMode, null);
        }
        public void prepare(VideoClip _clip, Action<bool, string> _onPrepareDone, _AALVideoPlayerDealerTextureInterface _textureInterface = null)
        {
            prepare(_clip, _onPrepareDone, VideoAudioOutputMode.None, _textureInterface);
        }
        public void prepare(VideoClip _clip, Action<bool, string> _onPrepareDone, VideoAudioOutputMode _audioMode, _AALVideoPlayerDealerTextureInterface _textureInterface)
        {
            _IALVideoResource resource = _clip != null ? new ALVideoResourceClip(_clip) : null;
            prepare(resource, _onPrepareDone, _audioMode);
        }

        /// <summary>
        /// 直接播放某个视频
        /// </summary>
        /// <param name="_resource">视频资源</param>
        /// <param name="_startAction">开始播放回调</param>
        /// <param name="_audioMode">音频模式</param>
        public void playClip(_IALVideoResource _resource, Action _startAction, VideoAudioOutputMode _audioMode/* = VideoAudioOutputMode.None */, Action<_AALVideoPlayerDealer, VideoAudioOutputMode> _preDealPlay, Action<_AALVideoPlayerDealer, VideoAudioOutputMode> _onStopPlaying)
        {
            playClip(_resource, _startAction, _audioMode, null, _preDealPlay, _onStopPlaying);
        }
        public void playClip(_IALVideoResource _resource, Action _startAction, _AALVideoPlayerDealerTextureInterface _textureInterface/* = null */, Action<_AALVideoPlayerDealer, VideoAudioOutputMode> _preDealPlay, Action<_AALVideoPlayerDealer, VideoAudioOutputMode> _onStopPlaying)
        {
            playClip(_resource, _startAction, VideoAudioOutputMode.None, _textureInterface, _preDealPlay, _onStopPlaying);
        }
        public void playClip(_IALVideoResource _resource, Action _startAction, VideoAudioOutputMode _audioMode, _AALVideoPlayerDealerTextureInterface _textureInterface, Action<_AALVideoPlayerDealer, VideoAudioOutputMode> _preDealPlay, Action<_AALVideoPlayerDealer, VideoAudioOutputMode> _onStopPlaying)
        {
            if (null == _resource)
            {
                //此处直接执行播放，在播放处理里会停止
                _dealPlayClip(_resource, _preDealPlay, _onStopPlaying);
                return;
            }
            //直接调用准备函数，然后在回调中播放
            forcePrepare(_resource, (bool _isSuc, string _err) =>
            {
                if (!_isSuc)
                    ALLog.Sys($"Prepare Clip[{_resource.resourcePath}] fail! Err[{_err}]");

                _dealPlayClip(_resource, _preDealPlay, _onStopPlaying);
                if (_startAction != null)
                    _startAction();
            }, _audioMode, _textureInterface);
        }


        /// <summary>
        /// 直接播放某个视频
        /// </summary>
        /// <param name="_resource">视频资源</param>
        /// <param name="_startAction">开始播放回调</param>
        /// <param name="_audioMode">音频模式</param>
        public void loopPlayClip(_IALVideoResource _resource, Action _startAction, VideoAudioOutputMode _audioMode/* = VideoAudioOutputMode.None */, Action<_AALVideoPlayerDealer, VideoAudioOutputMode> _preDealPlay, Action<_AALVideoPlayerDealer, VideoAudioOutputMode> _onStopPlaying)
        {
            loopPlayClip(_resource, _startAction, _audioMode, null, _preDealPlay, _onStopPlaying);
        }
        public void loopPlayClip(_IALVideoResource _resource, Action _startAction, _AALVideoPlayerDealerTextureInterface _textureInterface/* = null*/, Action<_AALVideoPlayerDealer, VideoAudioOutputMode> _preDealPlay, Action<_AALVideoPlayerDealer, VideoAudioOutputMode> _onStopPlaying)
        {
            loopPlayClip(_resource, _startAction, VideoAudioOutputMode.None, _textureInterface, _preDealPlay, _onStopPlaying);
        }
        public void loopPlayClip(_IALVideoResource _resource, Action _startAction, VideoAudioOutputMode _audioMode, _AALVideoPlayerDealerTextureInterface _textureInterface, Action<_AALVideoPlayerDealer, VideoAudioOutputMode> _preDealPlay, Action<_AALVideoPlayerDealer, VideoAudioOutputMode> _onStopPlaying)
        {
            if (null == _resource)
            {
                //此处直接执行播放，在播放处理里会停止
                _dealLoopPlayClip(_resource, _preDealPlay, _onStopPlaying);
                return;
            }
            //直接调用准备函数，然后在回调中播放
            forcePrepare(_resource, (bool _isSuc, string _err) =>
            {
                if (!_isSuc)
                    ALLog.Sys($"Prepare Clip[{_resource.resourcePath}] fail! Err[{_err}]");

                _dealLoopPlayClip(_resource, _preDealPlay, _onStopPlaying);
                if (_startAction != null)
                    _startAction();
            }, _audioMode, _textureInterface);
        }

        /// <summary>
        /// 直接播放某个视频 (兼容原有VideoClip接口)
        /// </summary>
        /// <param name="_clip">视频片段</param>
        /// <param name="_startAction">开始播放回调</param>
        /// <param name="_audioMode">音频模式</param>
        public void playClip(VideoClip _clip, Action _startAction, VideoAudioOutputMode _audioMode/* = VideoAudioOutputMode.None */, Action<_AALVideoPlayerDealer, VideoAudioOutputMode> _preDealPlay, Action<_AALVideoPlayerDealer, VideoAudioOutputMode> _onStopPlaying)
        {
            _IALVideoResource resource = _clip != null ? new ALVideoResourceClip(_clip) : null;
            playClip(resource, _startAction, _audioMode, _preDealPlay, _onStopPlaying);
        }

        /// <summary>
        /// 修改播放倍率
        /// </summary>
        /// <param name="_speed"></param>
        public void playbackSpeed(float _speed)
        {
            if (null == _m_vpCurClip)
                return;

            _m_vpCurClip.playbackSpeed(_speed);
        }
        
        /// <summary>
        /// 监控播放状态，本函数只有在播放后调用才有效，否则会直接返回
        /// </summary>
        public void monitorPlay(Action _onPlayEnd)
        {
            if(null == _m_vpCurClip)
            {
                //避免死循环，下帧调用
                ALCommonTaskController.CommonActionAddNextFrameTask(_onPlayEnd);
                return;
            }

            //调用监控
            _m_vpCurClip.monitorPlay(_onPlayEnd, _m_lCurClipPlaySerialize);
        }

        /// <summary>
        /// 停止播放
        /// </summary>
        /// <param name="_clip"></param>
        public void stop()
        {
            //此处直接停止播放
            if (null != _m_vpCurClip)
                _m_vpCurClip.reset();
            _m_vpCurClip = null;

            //单独刷新序列号
            _m_lCurClipPlaySerialize = ALSerializeOpMgr.next();
        }

        /// <summary>
        /// 清空所有播放的数据，不清理RT
        /// </summary>
        public void resetClip()
        {
            //先停止当前播放的视频
            if (null != _m_vpCurClip)
                _m_vpCurClip.reset();
            _m_vpCurClip = null;

            //将队列中所有播放数据清空
            for (int i = 0; i < _m_clips.Count; ++i)
            {
                _m_clips[i].discard();
            }
            _m_clips.Clear();
            //设置目标为空
            _m_mCurMaterialTarget = null;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void discard()
        {
            //先停止当前播放的视频
            if (null != _m_vpCurClip)
                _m_vpCurClip.reset();
            _m_vpCurClip = null;

            //将队列中所有播放数据清空
            List<ALVideoPlayerClipObj> tmpList = _m_clips;
            _m_clips = new List<ALVideoPlayerClipObj>();
            for (int i = 0; i < tmpList.Count; ++i)
            {
                tmpList[i].discard();
            }

            
            //设置目标为空
            _m_mCurMaterialTarget = null;
        }

        /// <summary>
        /// 查询播放视频数据的对象
        /// </summary>
        /// <param name="_resource"></param>
        /// <returns></returns>
        private ALVideoPlayerClipObj _lookupClipObj(_IALVideoResource _resource)
        {
            for(int i = 0; i < _m_clips.Count; ++i)
            {
                if (_m_clips[i].videoResource != null && _m_clips[i].videoResource.Equals(_resource))
                    return _m_clips[i];
            }

            return null;
        }

        /// <summary>
        /// 直接处理播放的操作
        /// </summary>
        /// <param name="_resource"></param>
        private void _dealPlayClip(_IALVideoResource _resource, Action<_AALVideoPlayerDealer, VideoAudioOutputMode> _preDealPlay, Action<_AALVideoPlayerDealer, VideoAudioOutputMode> _onStopPlaying)
        {
            if (null == _resource)
            {
                //此处直接停止播放
                if (null != _m_vpCurClip)
                    _m_vpCurClip.reset();
                _m_vpCurClip = null;

                return;
            }

            ALVideoPlayerClipObj clipObj = _lookupClipObj(_resource);
            if (null == clipObj)
            {
#if UNITY_EDITOR
                Debug.LogError("ALVideoPlayer _dealPlayClip Error, clipObj is null");
#endif
                return;
            }

            //同一个Clip对象在目前reset后马上播放的话是有问题的，经过测试应该是底层Player同帧处理会出问题，因此这里针对同一个对象使用不同的处理方式
            if (clipObj == _m_vpCurClip)
            {
                _m_vpCurClip.setFrame(0);
                //开始播放
                _m_lCurClipPlaySerialize = _m_vpCurClip.play(_preDealPlay, _onStopPlaying);
                //调用贴图变更
                _m_vpCurClip.setTarget(_m_mCurMaterialTarget);

            }
            else
            {
                //重置当前播放的对象
                if (null != _m_vpCurClip)
                    _m_vpCurClip.reset();

                //设置当前播放对象
                _m_vpCurClip = clipObj;
                //开始播放
                _m_vpCurClip.setFrame(0);
                _m_lCurClipPlaySerialize = _m_vpCurClip.play(_preDealPlay, _onStopPlaying);
                //调用贴图变更
                _m_vpCurClip.setTarget(_m_mCurMaterialTarget);
            }
        }

        /// <summary>
        /// 直接处理播放的操作
        /// </summary>
        /// <param name="_resource"></param>
        private void _dealLoopPlayClip(_IALVideoResource _resource, Action<_AALVideoPlayerDealer, VideoAudioOutputMode> _preDealPlay, Action<_AALVideoPlayerDealer, VideoAudioOutputMode> _onStopPlaying)
        {
            if (null == _resource)
            {
                //此处直接停止播放
                if (null != _m_vpCurClip)
                    _m_vpCurClip.reset();
                _m_vpCurClip = null;

                return;
            }

            ALVideoPlayerClipObj clipObj = _lookupClipObj(_resource);
            if (null == clipObj)
            {
                Debug.LogError("ALVideoPlayer _dealPlayClip Error, clipObj is null");
                return;
            }

            //同一个Clip对象在目前reset后马上播放的话是有问题的，经过测试应该是底层Player同帧处理会出问题，因此这里针对同一个对象使用不同的处理方式
            if (clipObj == _m_vpCurClip)
            {
                _m_vpCurClip.setFrame(0);
                //开始播放
                _m_lCurClipPlaySerialize = _m_vpCurClip.playLoop(_preDealPlay, _onStopPlaying);
                //调用贴图变更
                _m_vpCurClip.setTarget(_m_mCurMaterialTarget);
            }
            else
            {
                //重置当前播放的对象
                if (null != _m_vpCurClip)
                    _m_vpCurClip.reset();

                //设置当前播放对象
                _m_vpCurClip = clipObj;
                //开始播放
                _m_vpCurClip.setFrame(0);
                _m_lCurClipPlaySerialize = _m_vpCurClip.playLoop(_preDealPlay, _onStopPlaying);
                //调用贴图变更
                _m_vpCurClip.setTarget(_m_mCurMaterialTarget);
            }
        }
    }
}
