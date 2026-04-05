using UnityEditor;
using UnityEngine;

namespace MJSoundEditor
{
    internal class AudioSourceVolumeInfo : _IAudioVolumeShow
    {
        private AudioSource _m_audioSource;
        private string _m_audioMixerGroup;
        private string _m_instruction;

        private AudioSource _m_instanceAudio;
        public string instruction => _m_instruction;

        public string name => _m_audioSource.name;

        public float volume
        {
            get => _m_audioSource.volume;
            set
            {
                _m_audioSource.volume = value;
                if(_m_instanceAudio != null)
                    _m_instanceAudio.volume = value;
            }
        }

        public bool isPlaying => _m_instanceAudio != null && _m_instanceAudio.isPlaying;

        public AudioSourceVolumeInfo(AudioSource _audioSource, string _audioMixerGroup, string _instruction)
        {
            _m_audioSource = _audioSource;
            _m_audioMixerGroup = _audioMixerGroup;
            _m_instruction = _instruction;
        }

        public void play(bool _isAudoSelectGo)
        {
            if (_m_instanceAudio == null)
            {
                _m_instanceAudio = Object.Instantiate(_m_audioSource);
                if (_m_instanceAudio != null)
                {
                    _m_instanceAudio.loop = false;
                    _m_instanceAudio.Play();
                }
            }
            else
            {
                _m_instanceAudio.volume = volume;
                _m_instanceAudio.Play();
            }
            if(_isAudoSelectGo)
                Selection.activeObject = _m_instanceAudio;
        }

        public void stop()
        {
            if (_m_instanceAudio != null)
            {
                Object.DestroyImmediate(_m_instanceAudio.gameObject);
                _m_instanceAudio = null;
            }
        }
    }
}