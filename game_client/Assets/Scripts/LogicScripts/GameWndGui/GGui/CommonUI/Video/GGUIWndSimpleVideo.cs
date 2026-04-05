using System;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;
using UnityEngine.Video;

namespace GOE
{
    /// <summary>
    /// 通用视频播放子窗口
    /// </summary>
    public class GGUIWndSimpleVideo : _ATALBasicUISubWnd<GGUIMonoSimpleVideo>
    {
        private GVideoClipIndex _m_curVideoClipIndex;
        
        //当前Mono的有效序列号，避免无效加载和处理
        private long _m_lSerialize;
        
        private ALVideoPlayerClipObj _m_vpCurClip;

        //视频准备完成之后的回调管理器
        [NotNull]private ALCommonStateDelegate _m_prepareDoneDelegate = new ALCommonStateDelegate();

        public GGUIWndSimpleVideo(GGUIMonoSimpleVideo _wnd) : base(_wnd)
        {
            initWnd();
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_lSerialize = ALSerializeOpMgr.next();
            _m_curVideoClipIndex = null;
            if(_m_vpCurClip != null)
                _m_vpCurClip.discard();
            _m_vpCurClip = null;
            _m_prepareDoneDelegate.reset();
        }

        protected override void _onReset()
        {
            _m_lSerialize = ALSerializeOpMgr.next();
            _m_curVideoClipIndex = null;
            if(_m_vpCurClip != null)
                _m_vpCurClip.discard();
            _m_vpCurClip = null;
            _m_prepareDoneDelegate.reset();
        }
        
        protected override void _onDiscard()
        {
            _m_lSerialize = ALSerializeOpMgr.next();
            _m_curVideoClipIndex = null;
            if(_m_vpCurClip != null)
                _m_vpCurClip.discard();
            _m_vpCurClip = null;
            _m_prepareDoneDelegate.reset();
        }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
#if NP_GAME
            if (wnd.showRawImage != null )
            {
                if( wnd.showRawImage.material == wnd.showRawImage.defaultMaterial)
                    wnd.showRawImage.material = GGameCommonInfo.instance.obj.guiVideoMat;
                wnd.showRawImage.material = GameObject.Instantiate(wnd.showRawImage.material);
            }
#endif
            _m_lSerialize = ALSerializeOpMgr.next();
            _m_prepareDoneDelegate.reset();
            
        }
        
        public void setVideoClip(GVideoClipIndex _videoClipIndex, Action _onClipPrepared = null)
        {
            if (wnd == null || _videoClipIndex == null)
                return;

            //如果当前视频和新视频相同则不处理
            if (_m_curVideoClipIndex != null && _m_curVideoClipIndex.Equals(_videoClipIndex))
                return;

            _m_curVideoClipIndex = _videoClipIndex;
            if(_m_vpCurClip != null)
                _m_vpCurClip.discard();
            _m_prepareDoneDelegate.reset();

            //刷新序列号
            _m_lSerialize = ALSerializeOpMgr.next();
            long curSerialize = _m_lSerialize;

            _m_prepareDoneDelegate.regDelegate(_onClipPrepared);
            //加载视频，放入播放器
            VideoController.loadVideoResource(_m_curVideoClipIndex
                , (_vc) => {
                    //合法性判断不通过，直接返回
                    if (curSerialize != _m_lSerialize)
                        return;
                    //创建新对象，并放入队列
                    _m_vpCurClip = new ALVideoPlayerClipObj(_vc, VideoController.audioMode, new AVProVideoPlayerDealerTextureInterface());
                    _m_vpCurClip.prepare((bool _isSuc, string _err)=>_m_prepareDoneDelegate.setInitDone());
                });
        }
        
        /// <summary>
        /// 播放视频
        /// </summary>
        public void playVideo(Action _onVideoStart = null)
        {
            if(wnd == null)
                return;
            _m_prepareDoneDelegate.regDelegate(() =>
            {
                if (_m_vpCurClip == null)
                {
                    _onVideoStart?.Invoke();
                    return;
                }
                _m_vpCurClip.play(
                    (_AALVideoPlayerDealer _vp, VideoAudioOutputMode _mode) => { PlayAudioMgr.instance.onVideoClipPlay(_vp, _mode, wnd.isBgMusic, _m_curVideoClipIndex);  }
                    , (_AALVideoPlayerDealer _vp, VideoAudioOutputMode _mode) => { PlayAudioMgr.instance.onStopVideoPlaying(_vp, _mode, wnd.isBgMusic); });
                _m_vpCurClip.setTarget(wnd.showRawImage?.material);
                _onVideoStart?.Invoke();
            });
        }
        
        /// <summary>
        /// 循环播放视频
        /// </summary>
        public void playVideoLoop(Action _onVideoStart = null)
        {
            if(wnd == null)
                return;
            _m_prepareDoneDelegate.regDelegate(() =>
            {
                if (_m_vpCurClip == null)
                {
                    _onVideoStart?.Invoke();
                    return;
                }

                _m_vpCurClip.playLoop(
                    (_AALVideoPlayerDealer _vp, VideoAudioOutputMode _mode) => { PlayAudioMgr.instance.onVideoClipPlay(_vp, _mode, wnd.isBgMusic, _m_curVideoClipIndex); }
                    , (_AALVideoPlayerDealer _vp, VideoAudioOutputMode _mode) => { PlayAudioMgr.instance.onStopVideoPlaying(_vp, _mode, wnd.isBgMusic); });
                _m_vpCurClip.setTarget(wnd.showRawImage?.material);
                _onVideoStart?.Invoke();
            });
        }

        /// <summary>
        /// 监控播放状态，本函数只有在播放后调用才有效，否则会直接返回。另外优质playVideo有效， playVideoLoop无效
        /// </summary>
        public void monitorPlay(Action _onVideoEnd = null)
        {
            if(wnd == null)
                return;
            _m_prepareDoneDelegate.regDelegate(() =>
            {
                if (_m_vpCurClip == null)
                {
                    _onVideoEnd?.Invoke();
                    return;
                }
                _m_vpCurClip.monitorPlay(_onVideoEnd);
            });
        }

        public void setFrame(int _frame)
        {
            _m_prepareDoneDelegate.regDelegate(() =>
            {  
                if (_m_vpCurClip == null)
                    return;
                _m_vpCurClip.reset();
                _m_vpCurClip.setFrame(_frame);
            });
        }

        public void reset()
        {
            _m_prepareDoneDelegate.regDelegate(() =>
            {  
                if (_m_vpCurClip == null)
                    return;
                _m_vpCurClip.reset();
            });
        }

        public void setSpeed(float _speed)
        {
            _m_prepareDoneDelegate.regDelegate(() =>
            {  
                if (_m_vpCurClip == null)
                    return;
                
                _m_vpCurClip.playbackSpeed(_speed);
            });
        }
        /// <summary>
        /// 注册视频预加载完成事件
        /// </summary>
        /// <param name="_delegate"></param>
        public void regVideoPreparedDone(Action _delegate)
        {
            _m_prepareDoneDelegate.regDelegate(_delegate);
        }

    }
}