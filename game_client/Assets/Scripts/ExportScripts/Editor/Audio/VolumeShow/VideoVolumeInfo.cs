using System;
using ALPackage;
using GOE;
#if AL_AVPRO_V2
using RenderHeads.Media.AVProVideo;
#endif
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

namespace MJSoundEditor
{
    public class VideoVolumeInfo : _IAudioVolumeShow
    {
        private GVideoClipIndex _m_videoClipIndex;
        private GSOVideoVolumeRefSet _m_refSet;
        public VideoVolumeInfo(GVideoClipIndex _videoClipIndex, GSOVideoVolumeRefSet _refSet)
        {
            _m_videoClipIndex = _videoClipIndex;
            _m_refSet = _refSet;            
        }
        public string instruction => "";

        public string name => _m_videoClipIndex?.objName;

        public float volume
        {
            get
            {
                if (_m_videoClipIndex != null && _m_refSet != null && _m_refSet.videoVolumeDic != null && _m_refSet.videoVolumeDic.TryGetValue(_m_videoClipIndex, out float _volume))
                    return _volume;
                return 1;
            }
            set
            {
                if (Mathf.Approximately(value, 1))
                {
                    if (_m_videoClipIndex != null && _m_refSet != null && _m_refSet.videoVolumeDic != null &&
                        _m_refSet.videoVolumeDic.ContainsKey(_m_videoClipIndex))
                    {
                        _m_refSet.videoVolumeDic.Remove(_m_videoClipIndex);
                        EditorUtility.SetDirty(_m_refSet);
                    }
                }
                else
                {
                    if (_m_videoClipIndex != null && _m_refSet != null && _m_refSet.videoVolumeDic != null)
                    {
                        float oldVolume = 1;
                        _m_refSet.videoVolumeDic.TryGetValue(_m_videoClipIndex, out oldVolume);

                        if (!Mathf.Approximately(oldVolume, value))
                        {
                            _m_refSet.videoVolumeDic[_m_videoClipIndex] = value;
                            EditorUtility.SetDirty(_m_refSet);
                        }
                    }
                }
           
            }
        }

        public bool isPlaying => _m_instanceGo != null;

        public GameObject _m_instanceGo;
        public void play(bool _isAudoSelectGo)
        {
            if (!Application.isPlaying)
                return;

            _m_instanceGo = new GameObject(name);
#if NP_GAME
            // VideoSimpleRawImageMono videoMono = _m_instanceGo.AddComponent<VideoSimpleRawImageMono>();
            // if (videoMono != null)
            // {
            //     videoMono.isLoop = false;
            //     videoMono.videoIndex = _m_videoClipIndex;
            // }
            var avproPlayer = _m_instanceGo.AddComponent<MediaPlayer>();
            var  applyToMaterial = _m_instanceGo.AddComponent<ApplyToMaterial>();
            if (avproPlayer != null)
            {
                applyToMaterial.Player = avproPlayer;

                var audioSource = _m_instanceGo.AddComponent<AudioSource>();
                avproPlayer.AudioSource = audioSource;
                var matchGroups = AudioMixerMgr.instance.audioMixer.FindMatchingGroups(AudioMixerMgr.BG_MUSIC_VOLUME);
                if (matchGroups != null && matchGroups.Length > 0) audioSource.outputAudioMixerGroup = matchGroups[0];
                var audioOutput = _m_instanceGo.AddComponent<AudioOutput>();
                if (audioOutput != null)
                    audioOutput.Player = avproPlayer;
                if (avproPlayer.PlatformOptionsWindows != null)
                    avproPlayer.PlatformOptionsWindows.audioOutput = Windows.AudioOutput.Unity;
                if (avproPlayer.PlatformOptionsAndroid != null)
                    avproPlayer.PlatformOptionsAndroid.audioOutput = Android.AudioOutput.Unity;
                if (avproPlayer.PlatformOptionsIOS != null)
                    avproPlayer.PlatformOptionsIOS.audioMode = MediaPlayer.OptionsApple.AudioMode.Unity;
                avproPlayer.AudioMuted = false;
                float volume = VideoVolumeMgr.instance.getVolume(_m_videoClipIndex);
                VideoResCore.instance.loadObj(_m_videoClipIndex.assetPath, (_realAssetPath) =>
                {
                    avproPlayer?.OpenMedia(MediaPathType.AbsolutePathOrURL, _realAssetPath,
                        false);
                    avproPlayer.Loop = false;
                    avproPlayer.AudioVolume = volume;
                    avproPlayer.Play();
                });
            }
#endif
            _m_instanceGo.SetActive(false);

            // videoMono.audioMode = VideoAudioOutputMode.Direct;
            _m_instanceGo.SetActive(true);
            if(_isAudoSelectGo)
                Selection.activeObject = _m_instanceGo;
        }

        public void stop()
        {
            if (_m_instanceGo != null)
            {
                GameObject.DestroyImmediate(_m_instanceGo.gameObject);
                _m_instanceGo = null;
            }
        }
    }
}