using System;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.Video;

namespace ALPackage
{
    /// <summary>
    /// 视频播放器中，对单个Clip进行管理的对象
    /// 这里会对对应的实际VideoPlayer对象以及相关的回调及状态进行管理
    /// </summary>
    public class ALVideoPlayerClipObj
    {
        //播放操作序列号，避免多次操作在监听等行为上混淆
        private long _m_lPlaySerialize;

        //播放的视频对象
        private _IALVideoResource _m_videoResource;
        //是否播放音频
        private VideoAudioOutputMode _m_bPlayAudioMode;

        //播放器脚本对象
        private _AALVideoPlayerDealer _m_vpVideoPlayer;
        //播放结束的时间，由于播放帧率不稳定，可能导致无法到达最后一帧而卡主
        private float _m_fEndTimeS;

        //是否处理了播放操作，对应会在reset执行释放操作
        private bool _m_bIsDealPlayed;
        //在已经开始播放之后，如果执行停止会执行的回调处理
        private Action<_AALVideoPlayerDealer, VideoAudioOutputMode> _m_dOnStopPlayAction;

        /// <summary>
        /// 仅提供底层包内部初始化，不对外开放
        /// </summary>
        protected internal ALVideoPlayerClipObj(_IALVideoResource _resource, VideoAudioOutputMode _audioMode, _AALVideoPlayerDealerTextureInterface _textureInterface)
        {
            _m_lPlaySerialize = ALSerializeOpMgr.next();
            _m_videoResource = _resource;
            _m_bPlayAudioMode = _audioMode;

            //取出一个VideoPlayer对象
            _m_vpVideoPlayer = null != _resource ? _resource.vpDealerCacheController.popItem() : null;
            if (null != _m_vpVideoPlayer)
            {
                //调用使用代码
                _m_vpVideoPlayer.onUsed();
                _m_vpVideoPlayer.setAudioOutputMode(_m_bPlayAudioMode);
                _m_vpVideoPlayer.setTetureInterface(_textureInterface);
            }

            _m_fEndTimeS = 0f;

            _m_bIsDealPlayed = false;
            _m_dOnStopPlayAction = null;
        }

        public long playSerialize { get { return _m_lPlaySerialize; } }
        public _IALVideoResource videoResource { get { return _m_videoResource; } }
        /// <summary>
        /// 获取VideoClip对象（如果是VideoClip资源的话）
        /// </summary>
        public VideoClip videoClip { 
            get { 
                if (_m_videoResource is ALVideoResourceClip clipResource)
                    return clipResource.videoClip;
                return null;
            } 
        }
        /// <summary>
        /// 是否正在播放中
        /// </summary>
        /// <returns></returns>
        public bool isPlaying { get { return null == _m_vpVideoPlayer ? false : _m_vpVideoPlayer.isPlaying; } }
        /// <summary>
        /// 是否播放一遍完成
        /// </summary>
        /// <returns></returns>
        public bool isPlayOnceDone { get { return null == _m_vpVideoPlayer ? true : Time.time >= _m_fEndTimeS; } }
        public int videoWidth { get { return null == _m_vpVideoPlayer ? 0 : _m_vpVideoPlayer.videoWidth; } }
        public int videoHeight { get { return null == _m_vpVideoPlayer ? 0 : _m_vpVideoPlayer.videoHeight; } }

        /// <summary>
        /// 获取当前VideoPlayer的Texture，仅限内部访问
        /// </summary>
        protected internal Texture _texture { get { return null == _m_vpVideoPlayer ? null : _m_vpVideoPlayer.texture; } }

        /// <summary>
        /// 设置视频渲染目标
        /// </summary>
        /// <param name="_mat"></param>
        public void setTarget(Material _mat)
        {
            _m_vpVideoPlayer?.setTarget(_mat);
        }

        /// <summary>
        /// 强制调用准备，并在准备完成后调用回调函数
        /// </summary>
        /// <param name="_onPrepareDone"></param>
        public void forcePrepare(Action<bool, string> _onPrepareDone)
        {
            if(null == _m_vpVideoPlayer)
            {
                if (null != _onPrepareDone)
                    _onPrepareDone(false, "Player is Null!");
                return;
            }

            //注册回调
            _m_vpVideoPlayer.forcePrepare(_m_videoResource
                , null
                , (bool _isSuc, string _err) => {
                    if (null != _onPrepareDone)
                        _onPrepareDone(_isSuc, _err);
                });
        }

        /// <summary>
        /// 调用准备，并在准备完成后调用回调函数
        /// </summary>
        /// <param name="_onPrepareDone"></param>
        public void prepare(Action<bool, string> _onPrepareDone)
        {
            if (null == _m_vpVideoPlayer)
            {
                if (null != _onPrepareDone)
                    _onPrepareDone(false, "Player is Null!");
                return;
            }

            //判断是否允许提前加载，如果不允许则直接调用回调返回
            if(!ALVideoLoadLegalJudger.instance.judgeCanPrepareVideo())
            {
                if (null != _onPrepareDone)
                    _onPrepareDone(true, string.Empty);
                return;
            }

            //注册回调
            _m_vpVideoPlayer.prepare(_m_videoResource
                , null
                , (bool _isSuc, string _err) => {
                    if (null != _onPrepareDone)
                        _onPrepareDone(_isSuc, _err);
                });
        }

        /// <summary>
        /// 设置当前帧数
        /// </summary>
        /// <param name="_frame"></param>
        public void setFrame(int _frame, float _frameRate = 30f)
        {
            if(null == _m_vpVideoPlayer)
                return;

            _m_vpVideoPlayer.setFrame(_frame, _frameRate);
        }

        /// <summary>
        /// 向指定RT中播放视频
        /// </summary>
        /// <param name="_rt"></param>
        public long play(Action<_AALVideoPlayerDealer, VideoAudioOutputMode> _preDealPlay, Action<_AALVideoPlayerDealer, VideoAudioOutputMode> _onStopPlaying)
        {
            if (null == _m_vpVideoPlayer)
                return 0;

            //刷新序列号
            _m_lPlaySerialize = ALSerializeOpMgr.next();
            long tmpSerialize = _m_lPlaySerialize;
            forcePrepare((bool _isSuc, string _err) =>
            {
                _dealPlay(tmpSerialize, false, _preDealPlay, _onStopPlaying);
            });

            return _m_lPlaySerialize;
        }
        public long playLoop(Action<_AALVideoPlayerDealer, VideoAudioOutputMode> _preDealPlay, Action<_AALVideoPlayerDealer, VideoAudioOutputMode> _onStopPlaying)
        {
            if (null == _m_vpVideoPlayer)
                return 0;

            //刷新序列号
            _m_lPlaySerialize = ALSerializeOpMgr.next();
            long tmpSerialize = _m_lPlaySerialize;
            forcePrepare((bool _isSuc, string _err) =>
            {
                _dealPlay(tmpSerialize, true, _preDealPlay, _onStopPlaying);
            });

            return _m_lPlaySerialize;
        }

        private void _dealPlay(long _tmpSerialize, bool _isLoop, Action<_AALVideoPlayerDealer, VideoAudioOutputMode> _preDealPlay, Action<_AALVideoPlayerDealer, VideoAudioOutputMode> _onStopPlaying)
        {
            if (null == _m_vpVideoPlayer)
                return ;

            //序列号不一致则不处理
            if (_tmpSerialize != _m_lPlaySerialize)
                return;

            //如果直接调用播放，则调用对应的停止处理
            Action<_AALVideoPlayerDealer, VideoAudioOutputMode> stopPlayAction = _m_dOnStopPlayAction;
            _m_dOnStopPlayAction = null;
            if (_m_bIsDealPlayed)
            {
                if (null != stopPlayAction)
                    stopPlayAction(_m_vpVideoPlayer, _m_bPlayAudioMode);
            }
            _m_bIsDealPlayed = false;

            //调用事件函数
            if (null != _preDealPlay)
                _preDealPlay(_m_vpVideoPlayer, _m_bPlayAudioMode);
            _m_bIsDealPlayed = true;
            _m_dOnStopPlayAction = _onStopPlaying;

            //开始处理播放
            _m_vpVideoPlayer.play(_isLoop);

            // 计算结束时间 - 使用Video长度
            _m_fEndTimeS = Time.time + (float)_m_vpVideoPlayer.length;
        }


        /// <summary>
        /// 监控播放状态，本函数只有在播放后调用才有效，否则会直接返回
        /// </summary>
        public void monitorPlay(Action _onPlayEnd)
        {
            //直接使用当前序列号监控
            monitorPlay(_onPlayEnd, _m_lPlaySerialize);
        }
        public void monitorPlay(Action _onPlayEnd, long _playSerialize)
        {
            if (null == _m_vpVideoPlayer)
            {
                //避免死循环，下帧调用
                ALCommonTaskController.CommonActionAddNextFrameTask(_onPlayEnd);
                return;
            }

            long curPlaySerialize = _playSerialize;
            //开启任务进行监控，当监控完成后不再循环执行
            ALCommonTaskController.CommonTickActionAddMonoTask(() =>
            {
                //如果不在播放或者序列号不一致直接调用后返回
                if (!isPlaying || curPlaySerialize != _m_lPlaySerialize)
                {
                    if (null != _onPlayEnd)
                        _onPlayEnd();
                    return false;
                }

                //继续监控
                return true;
            });
        }
        
        /// <summary>
        /// 修改播放倍率
        /// </summary>
        /// <param name="_speed"></param>
        public void playbackSpeed(float _speed)
        {
            if (null == _m_vpVideoPlayer)
                return;

            _m_vpVideoPlayer.playbackSpeed(_speed);
        }

        /// <summary>
        /// 重置到开始播放的位置
        /// </summary>
        public void reset()
        {
            //如果调用了播放则调用对应的停止处理
            Action<_AALVideoPlayerDealer, VideoAudioOutputMode> stopPlayAction = _m_dOnStopPlayAction;
            _m_dOnStopPlayAction = null;
            if (_m_bIsDealPlayed)
            {
                if (null != stopPlayAction)
                    stopPlayAction(_m_vpVideoPlayer, _m_bPlayAudioMode);
            }
            _m_bIsDealPlayed = false;

            if (null == _m_vpVideoPlayer)
                return;

            //刷新序列号
            _m_lPlaySerialize = ALSerializeOpMgr.next();
            //这里使用pauseToFrame，如果使用play会在下次播放的时候重新调用prepare函数，所以这里不做stop处理
            _m_vpVideoPlayer.pauseToFrame(0, 30f);
            _m_vpVideoPlayer.setTarget(null);

            //判断是否超出了安全区，是则需要释放
            if(ALVideoLoadLegalJudger.instance.judgeNeedDisposeVideo())
            {
                _m_vpVideoPlayer._reset();
            }
        }

        /// <summary>
        /// 释放相关资源
        /// </summary>
        public void discard()
        {
            //如果调用了播放则调用对应的停止处理
            Action<_AALVideoPlayerDealer, VideoAudioOutputMode> stopPlayAction = _m_dOnStopPlayAction;
            _m_dOnStopPlayAction = null;
            if (_m_bIsDealPlayed)
            {
                if (null != stopPlayAction)
                    stopPlayAction(_m_vpVideoPlayer, _m_bPlayAudioMode);
            }
            _m_bIsDealPlayed = false;

            //刷新序列号
            _m_lPlaySerialize = ALSerializeOpMgr.next();
            if (null != _m_vpVideoPlayer)
            {
                //强制重置帧
                _m_vpVideoPlayer.pauseToFrame(0, 30f);
                _m_vpVideoPlayer.setTarget(null);
            }

            //取出一个VideoPlayer对象
            if (null != _m_videoResource)
                _m_videoResource.vpDealerCacheController.pushBackCacheItem(_m_vpVideoPlayer);
            _m_vpVideoPlayer = null;
            
            _m_videoResource = null;
        }
    }
}
