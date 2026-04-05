using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;

namespace GOE
{
    /// <summary>
    /// 混音器管理器
    /// </summary>
    public class AudioMixerMgr : _IAudioVolumeControl
    {
        private static AudioMixerMgr _g_instance = new AudioMixerMgr();
        [NotNull]public static AudioMixerMgr instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new AudioMixerMgr();
                return _g_instance;
            }
        }

        public const float InvalidVoiceAudioMixerVolume = 100;//无效的配音音量(因为配置音量范围为-80~20)
        
        //资源上的默认大小默认配置
        private const float MinAudioMixerVolume = -50;//这个值基本就听不到了
        private const float MaxAudioMixerVolume = 0;//最大音量
        private const float MuteAudioMixerVolume = -80;//完全静音的值

        //所有组的根节点名字
        public const string AUDIO_MIXER_GROUP_ROOT_NAME = "Master";
        //背景音乐组合集参数
        public const string BG_MUSIC_VOLUME = "music_main_volume";
        //效果音乐组合集参数
        public const string EFFECT_MUSIC_VOLUME = "effect_main_volume";
        //配音音乐组合集参数
        public const string VOICE_MUSIC_VOLUME = "voice_main_volume";

        
        //是否初始化
        private bool _m_bInited;
        //是否加载完成
        private bool _m_isLoadDone;
        //AudioMixer混音器对象
        private AudioMixer _m_audioMixer;
        /** 初始化完成后的回调 */
        private Action _m_doneDelegate;
        
        //音频分组字典
        [NotNull]private Dictionary<long, AudioMixerGroup> _m_audioGroupDict = new Dictionary<long, AudioMixerGroup>();

        //是否初始化完成
        public bool isInitDone
        {
            get { return _m_isLoadDone; }
        }
        
        /// <summary>
        /// 音源本身应该设置的音量的大小
        /// 使用混音器的情况下直接返回1默认值
        /// </summary>
        public float audioSourceVolume
        {
            get { return 1f; }
        }
        
        /// <summary>
        /// 音源本身应该设置的音量的大小
        /// 使用混音器的情况下直接返回1默认值
        /// </summary>
        public float audioSourceBgVolume
        {
            get { return 1f; }
        }
        
        /// <summary>
        /// 音源本身应该设置的音量的大小
        /// 使用混音器的情况下直接返回1默认值
        /// </summary>
        public float voiceSourceVolume
        {
            get { return 1f; }
        }
        
        public AudioMixer audioMixer
        {
            get { return _m_audioMixer; }
        }

        public void init(Action _doneAction)
        {
            //判断是否初始化
            if (_m_bInited)
            {
                if(null != _doneAction)
                    _doneAction();
                return;
            }
            _m_bInited = true;
            _m_isLoadDone = false;
            
            _m_doneDelegate = _doneAction;

            ALAssetLoader<AudioMixer> alAssetLoader = new ALAssetLoader<AudioMixer>(
                GameResCore.instance,
                "audio/audio_mixer.unity3d",
                "audio_mixer"
#if UNITY_EDITOR
                , ".mixer", String.Empty
#endif
                );

            alAssetLoader.loadAsset(_audioMixerLoadDone);
        }
        
        private void _audioMixerLoadDone(AudioMixer _audioMixer)
        {
            if(null == _audioMixer)
            {
                _dealDoneDelegate();
                return;
            }

            _m_audioMixer = _audioMixer;

            _m_audioMixer.GetFloat(BG_MUSIC_VOLUME, out float _bgMusicVolumeDB);
            _m_audioMixer.GetFloat(EFFECT_MUSIC_VOLUME, out float _effectMusicVolumeDB);
            _m_audioMixer.GetFloat(VOICE_MUSIC_VOLUME, out float _voiceMusicVolumeDB);
            // 初始化音量
            if (_initAudioGroupVolume(BG_MUSIC_VOLUME, _bgMusicVolumeDB))
            {
                GameSetting.instance.setBgAudioValue(Mathf.InverseLerp(MinAudioMixerVolume, MaxAudioMixerVolume, _bgMusicVolumeDB));
            }
            if (_initAudioGroupVolume(EFFECT_MUSIC_VOLUME, _effectMusicVolumeDB))
            {
                GameSetting.instance.setAudioValue(Mathf.InverseLerp(MinAudioMixerVolume, MaxAudioMixerVolume, _effectMusicVolumeDB));
            }
            if (_initAudioGroupVolume(VOICE_MUSIC_VOLUME, _voiceMusicVolumeDB))
            {
                GameSetting.instance.setVoiceValue(Mathf.InverseLerp(MinAudioMixerVolume, MaxAudioMixerVolume, _voiceMusicVolumeDB));
            }
            
            //获取资源上配置的所有组
            AudioMixerGroup[] groups = _m_audioMixer.FindMatchingGroups(AUDIO_MIXER_GROUP_ROOT_NAME);
            if (null == groups)
            {
                _dealDoneDelegate();
                return;
            }
            
            //构造组数据
            _m_audioGroupDict.Clear();
            foreach (NPAudioGroupRefObj audioGroupRefObj in GRefdataCoreMgr.instance.audioGroupRefCore.refList)
            {
                if(null == audioGroupRefObj)
                    continue;

                foreach (AudioMixerGroup audioMixerGroup in groups)
                {
                    if (null == audioMixerGroup)
                        continue;
                    
                    if (audioGroupRefObj.audio_group_name == audioMixerGroup.name)
                        _m_audioGroupDict.Add(audioGroupRefObj.id, audioMixerGroup);
                }
            }
            
            //设置初始化完成
            _m_isLoadDone = true;
            _dealDoneDelegate();
            
            //执行完成回调
            void _dealDoneDelegate()
            {
                //判断回调是否需要调用
                if (null != _m_doneDelegate)
                {
                    Action tempSucDelegate = _m_doneDelegate;
                    _m_doneDelegate = null;
                    tempSucDelegate();
                }
            }
        }

        /// <summary>
        /// 初始化音量
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_volumeDB"></param>
        /// <returns></returns>
        private bool _initAudioGroupVolume(string _name, float _volumeDB)
        {
            if (!GameSetting.instance.getInitAudioMixerDB(_name, out float _initVoiceDB) || !Mathf.Approximately(_volumeDB, _initVoiceDB))
            {
                GameSetting.instance.setInitVoiceValueDB(_name, _volumeDB);
                return true;
            }
            
            return false;
        }
        
        //通过组别id获取音效组
        public AudioMixerGroup getAudioMixerGroup(long _groupId)
        {
            if(!_m_bInited || !_m_isLoadDone)
            {
                return null;
            }
            if(_m_audioGroupDict.TryGetValue(_groupId, out AudioMixerGroup audioGroup))
            {
                return audioGroup;
            }
            else
            {
                Debug.LogError($"AudioMixer 未找到指定的AudioMixerGroup_groupId:{_groupId}");
                return null;
            }
        }
        
        /// <summary>
        /// 根据资源参数设置音量
        /// </summary>
        public void setAudioGroupParaVolume(string _name, bool _isOn, float _value)
        {
            if(_m_audioMixer == null)
                return;

            // //https://zhuanlan.zhihu.com/p/591725347 别问，奇怪的知识增加了
            // if (_value <= 0.0f) 
            //     _value = 0.0001f;
            // _value = Mathf.Log10(_value) * 20.0f;

            //如果音量小于0.01则直接静音
            if (_value <= 0.01)
            {
                _m_audioMixer.SetFloat(_name, MuteAudioMixerVolume);
                return;
            }
            
            _value = Mathf.Clamp01(_value);
            _value = Mathf.Lerp(MinAudioMixerVolume, MaxAudioMixerVolume, _value);
            
            if(_isOn)
                _m_audioMixer.SetFloat(_name, _value);
            else
                _m_audioMixer.SetFloat(_name, MuteAudioMixerVolume); 
        }
        
        /// <summary>
        /// 设置背景音量大小
        /// </summary>
        public void setBgAudioVolume(bool _isOn, float _value)
        {
            setAudioGroupParaVolume(BG_MUSIC_VOLUME, _isOn, _value);
        }

        /// <summary>
        /// 设置音效音量大小
        /// </summary>
        public void setAudioVolume(bool _isOn, float _value)
        {
            setAudioGroupParaVolume(EFFECT_MUSIC_VOLUME, _isOn, _value);
        }

        /// <summary>
        /// 设置配音音量大小
        /// </summary>
        public void setVoiceVolume(bool _isOn, float _value)
        {
            setAudioGroupParaVolume(VOICE_MUSIC_VOLUME, _isOn, _value);
        }

        /// <summary>
        /// 获取当前背景音乐音量
        /// </summary>
        /// <returns></returns>
        public static float getCurBgVolume()
        {
            float bgVolume = GameSetting.instance.bgAudioValue;
            //如果音量小于0.01或关闭背景音则直接静音
            if (bgVolume <= 0.01f || !GameSetting.instance.usingBgAudio)
                return 0f;

            bgVolume = Mathf.Clamp01(bgVolume);
            float bgdB = Mathf.Lerp(MinAudioMixerVolume, MaxAudioMixerVolume, bgVolume);

            //https://zhuanlan.zhihu.com/p/591725347
            //根据db = 20*log10(Volume)  得出  Volume = 10^(db/20)
            return Mathf.Pow(10f, (bgdB / 20f));
        }
    }
}