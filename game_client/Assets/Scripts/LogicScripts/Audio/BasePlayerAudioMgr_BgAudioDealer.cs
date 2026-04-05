using ALPackage;
using UnityEngine.Video;

namespace GOE
{
    public partial class BasePlayerAudioMgr
    {
        
        protected class BgAudioDealer : _ABgAudioDealer
        {
            public BgAudioDealer(long _refId) : base(_refId)
            {
            }

            protected override void _onAudioStart(AudioObject _audioObject)
            {
                
            }

            protected override void _onAudioEnd(AudioObject _audioObject)
            {
                
            }
        }
        
        protected class VideoAudioDealer : _ABgAudioDealer
        {
            private _AALVideoPlayerDealer _m_videoPlayer;
            private VideoAudioOutputMode _m_mode;
            private float _m_volume;
            
            public VideoAudioDealer(_AALVideoPlayerDealer _videoPlayer, VideoAudioOutputMode _mode, float _volume) : base(GRefdataCoreMgr.instance.npGeneral.video_bg_audio_res_id)
            {
                _m_videoPlayer = _videoPlayer;
                _m_mode = _mode;
                _m_volume = _volume;
            }

            protected override void _onAudioStart(AudioObject _audioObject)
            {
                if(null == _m_videoPlayer)
                    return;
                
                if (_m_mode == VideoAudioOutputMode.AudioSource)
                {
                    _m_videoPlayer.SetTargetAudioSource(0, _audioObject.audioSource);
                }
                _m_videoPlayer.setAudioVolume(0, _m_volume);
                //设置是否静音
                _m_videoPlayer.setAudioMute(0, !GameSetting.instance.usingBgAudio);
            }

            protected override void _onAudioEnd(AudioObject _audioObject)
            {
                if(null == _m_videoPlayer)
                    return;
                if (_m_mode == VideoAudioOutputMode.AudioSource)
                {
                    _m_videoPlayer.SetTargetAudioSource(0, null);
                }
                _m_videoPlayer.setAudioVolume(0, _m_volume);
            }
        }
        
        //背景音乐播放处理器基类
        protected abstract class _ABgAudioDealer
        {
            private bool _m_isPlaying = false;
            private long _m_instanceId;
            private long _m_refId;

            
            //实例id
            public long instanceId{ get { return _m_instanceId; } }
            //配表id
            public long refId { get { return _m_refId; } }
            //是否正在播放
            public bool isPlaying { get { return _m_isPlaying; } }

            protected _ABgAudioDealer(long _refId)
            {
                _m_refId = _refId;
            }
            
            public long play()
            {
                if(_m_isPlaying)
                    return _m_instanceId;
                
                _m_isPlaying = true;
                _m_instanceId = PlayAudioMgr.instance.playClip(_m_refId, true, _onAudioStart, null, _onAudioEnd);
                return _m_instanceId;
            }
            
            public void stop()
            {
                if(!_m_isPlaying)
                    return;
                
                _m_isPlaying = false;
                //记录播放进度
                PlayAudioMgr.instance._saveBgMusicTime(_m_instanceId);
                //停止播放
                PlayAudioMgr.instance.stopClip(_m_instanceId);
            }
            
            protected abstract void _onAudioStart(AudioObject _audioObject);
            
            protected abstract void _onAudioEnd(AudioObject _audioObject);
        }
    }
}