using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using System;
using JetBrains.Annotations;
using UnityEngine.Audio;
using UnityEngine.Video;


namespace GOE
{
    public class CommonClickAudioInfo
    {
        //点击默认音效
        private GameObject _m_aCommonBtnClickAudio;
        //默认音效音源对象
        private AudioSource _m_aCommonBtnClickAs;
        //原始音效大小
        private float _m_fRefAudioVolue;
        public CommonClickAudioInfo(GameObject _go)
        {
            _m_aCommonBtnClickAudio = _go;
            _m_aCommonBtnClickAs = _m_aCommonBtnClickAudio.GetComponent<AudioSource>();

            if(_m_aCommonBtnClickAs != null)
                _m_fRefAudioVolue = _m_aCommonBtnClickAs.volume;
        }

        public void setVolueScale(bool _isPlay, float _volue)
        {
            if(_m_aCommonBtnClickAs == null)
                return;

            if(_isPlay)
            {
                setVolueScale(_volue);
            }
            else
            {
                setVolueScale(0f);
                stop();
            }
        }

        public void setVolueScale(float _volue)
        {
            if(_m_aCommonBtnClickAs == null)
                return;

            _m_aCommonBtnClickAs.volume = _volue * _m_fRefAudioVolue;
        }
        
        //设置默认点击音效的输出混音器组
        public void setAudioMixerGroup(AudioMixerGroup _group)
        {
            if(null == _m_aCommonBtnClickAs)
                return;
            
            _m_aCommonBtnClickAs.outputAudioMixerGroup = _group;
        }


        //停止
        public void stop()
        {
            ALUGUICommon.setGameObjDisable(_m_aCommonBtnClickAudio);

        }

        //播放
        public void play()
        {
            ALUGUICommon.setGameObjEnable(_m_aCommonBtnClickAudio);
#if UNITY_EDITOR
            // 输出音频log，仅编辑器
            AudioDebugMgr.Instance.PlayAudioLog(-1, false, _m_aCommonBtnClickAudio.name,_m_aCommonBtnClickAs.clip.name, 
                _m_aCommonBtnClickAs.clip.length,
                "-1", false );
#endif
        }

        public bool isActive()
        {
            if(_m_aCommonBtnClickAudio == null)
                return false;

            return _m_aCommonBtnClickAudio.activeSelf;
        }

        public void discard()
        {
            ALUnityCommon.releaseGameObj(_m_aCommonBtnClickAudio);
        }
    }



    /****************
     * 玩家音效管理对象
     **/
    public class PlayAudioMgr : BasePlayerAudioMgr
    {
        private static PlayAudioMgr _g_instance;
        public static PlayAudioMgr instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new PlayAudioMgr("audio_root");
                return _g_instance;
            }
        }

        //点击默认音效信息
        private CommonClickAudioInfo _m_aCommonBtnClickAudio;

        //音量控制接口
        protected override _IAudioVolumeControl _m_volumeControl { get { return AudioMixerMgr.instance; } }

        public PlayAudioMgr(string _rootName) : base(_rootName)
        {
            //当切换音频输出设备回调
            AudioSettings.OnAudioConfigurationChanged += _onAudioConfigurationChanged;
        }

        //初始化点击音效，注册点击事件
        public void init()
        {
            ALMsgSys.RegisterMsg((int)ALMsgType.UI_CLICK, _onUiClick);
            
            //初始化通用点击音效
            if (null == _m_aCommonBtnClickAudio)
            {
                if(PLoginCommonInfo.instance.obj != null && null != PLoginCommonInfo.instance.obj.commonClickSoundPrefab)
                {
                    GameObject commonClickgo = UnityEngine.Object.Instantiate(PLoginCommonInfo.instance.obj.commonClickSoundPrefab);
                    commonClickgo.SetActive(false);
                    commonClickgo.transform.SetParent(_m_goRootGo?.transform);
                    commonClickgo.transform.position = Vector3.zero;

                    _m_aCommonBtnClickAudio = new CommonClickAudioInfo(commonClickgo);
                }   
            }
        }

        /// <summary>
        /// 设置默认点击音效的输出混音器组
        /// </summary>
        /// <param name="_group"></param>
        public void setClickAudioMixerGroup(AudioMixerGroup _group)
        {
            if(null == _m_aCommonBtnClickAudio)
                return;
            _m_aCommonBtnClickAudio.setAudioMixerGroup(_group);
        }

        //UI按钮点击事件的音源响应
        private void _onUiClick(params object[] _objs)
        {
            GameObject go = (GameObject)_objs[0];

            if(go == null)
                return;
            
            NPBaseBtnClickSound baseBtnClickSound = go.GetComponent<NPBaseBtnClickSound>();

            if(baseBtnClickSound == null)
            {
                //未找到，播放PlatConmonInfo上配置的音效
                playCommonBtnClickSound();
            }else
            {
                //如果配置不播放，跳过处理
                if (!baseBtnClickSound.IsPlaySound)
                    return;

                //播放UI点击音效
                playClip(baseBtnClickSound.GetSoundResID);
            }
        }

        //播放默认点击音效
        private void playCommonBtnClickSound()
        {
            if(_m_aCommonBtnClickAudio == null)
                return;

            if(_m_aCommonBtnClickAudio.isActive())
                _m_aCommonBtnClickAudio.stop();

            _m_aCommonBtnClickAudio.play();
        }
        
        public override void _stopAudioWithAudioID()
        {
        }

        public override void _reset()
        {
        }

        public override void _discard()
        {
            ALMsgSys.UnregisterMsg((int)ALMsgType.UI_CLICK, _onUiClick);

            if(_m_aCommonBtnClickAudio != null)
                _m_aCommonBtnClickAudio.discard();
        }

        public override void _stopAllAudio()
        {
        }

        public override void _chgBgAudioValue(bool _isUsingAudio, float _value)
        {
        }

        public override void _chgAudioValue(bool _isUsingAudio, float _value)
        {
            //没有混音器的情况下才需要单独设置默认点击音量
            if(null != _m_volumeControl && _m_volumeControl.isInitDone)
                return;
            
            if(_m_aCommonBtnClickAudio != null)
                _m_aCommonBtnClickAudio.setVolueScale(_isUsingAudio, _value);
        }

        public override void _chgVoiceValue(bool _isUsingVoice, float _value)
        {
        }

        public override void _playOneShotClip(List<long> __refIdList, Transform _parent, Vector3 _vec)
        {
            playOneShotClip(__refIdList[rnd.Next(0, __refIdList.Count)], _parent, _vec);
        }

        public override void _playOneShotClip(long _refId, Transform _parent, Vector3 _vec)
        {
        }
        
        // 当切换音频输出设备的时候重新设置一下音量，解决 蓝牙耳机连接或者断开，导致音效开启的bug
        private void _onAudioConfigurationChanged(bool deviceWasChanged)
        {
            //根据当前设置重新设置一次音量
            chgBgAudioValue(GameSetting.instance.usingBgAudio, GameSetting.instance.bgAudioValue);
            chgAudioValue(GameSetting.instance.usingAudio, GameSetting.instance.audioValue);
            chgVoiceValue(GameSetting.instance.usingVoice, GameSetting.instance.voiceValue);

            long bgInstanceId = _m_curBgAudioDealer == null ? -1 : _m_curBgAudioDealer.instanceId;
            // 再播放一次背景音乐，防止切换音频输出设备，导致背景音乐无法直接通过开关开启的bug。
            if (_m_dPlayingAudioObjectDic.TryGetValue(bgInstanceId, out AudioObject audioObject))
            {
                if (null != audioObject && audioObject.audioSource != null) 
                    audioObject.audioSource.Play();
            }
        }

        /// <summary>
        /// 预加载资源(仿照BasePlayerAudioMgr._popFromCache)
        /// </summary>
        public void preload(long _audioRefId, Action _complete)
        {
            if (_audioRefId <= 0)
            {
                _complete?.Invoke();
                return;
            }

            NPAudioRefObj audioRefObj = GRefdataCoreMgr.instance.audioMap.getRef(_audioRefId);
            if(audioRefObj == null || audioRefObj.audio_index == null || !audioRefObj.audio_index.isValid())
            {
                _complete?.Invoke();
                return;
            }
            
            //拼凑Id
            long mergeIndex = ALCommon.mergeInt(audioRefObj.audio_index.mainId, audioRefObj.audio_index.subId);

            AudioCache cacheObj = null;
            if(!_m_dicAudioCache.TryGetValue(mergeIndex, out cacheObj))
            {
                //构建对象
                cacheObj = new AudioCache(audioRefObj, _m_goRootGo?.transform);
                //加入映射表
                _m_dicAudioCache.Add(mergeIndex, cacheObj);
            }

            if(null == cacheObj)
            {
                _complete?.Invoke();
                return;
            }

            // 判断缓存是否初始化完成
            if(cacheObj.isInitialized())
            {
                AudioObject audioObject = cacheObj.popItem();
                cacheObj.pushBackCacheItem(audioObject);
                
                _complete?.Invoke();
            }
            else
            {
                // 未初始化的缓存需要加载模板对象初始化缓存
                _loadAudioResObj(audioRefObj, (resObj) =>
                {
                    if (resObj == null)
                    {
                        _complete?.Invoke();
                        return;
                    }
                    
                    // 再判断一次缓存是否初始化完成，防止加载资源过程的重复请求产生对缓存的重复初始化
                    if(cacheObj.isInitialized())
                    {
                        //释放资源
                        resObj.discard();
                        
                        AudioObject audioObject = cacheObj.popItem();
                        cacheObj.pushBackCacheItem(audioObject);
                        
                        _complete?.Invoke();
                    }
                    else
                    {
                        AudioObject audioObject = new AudioObject(resObj, audioRefObj);
                        //初始化cache
                        cacheObj.init(audioObject);
                        
                        audioObject = cacheObj.popItem();
                        cacheObj.pushBackCacheItem(audioObject);
                        
                        _complete?.Invoke();
                    }
                });
            }
        }
        
        
        [NotNull]private Dictionary<long, long> _m_dicAudioCacheInstanceId = new Dictionary<long, long>();
        /// <summary>
        /// 当开始播放一段视频时，在此函数中处理音频相关操作
        /// </summary>
        /// <param name="_vp"></param>
        /// <param name="_mode"></param>
        /// <param name="_isBgMusic"></param>
        public void onVideoClipPlay(_AALVideoPlayerDealer _vp, VideoAudioOutputMode _mode, bool _isBgMusic, GVideoClipIndex _vcIndex = null)
        {
            if(null == _vp)
                return;
            
            float volume = VideoVolumeMgr.instance.getVolume(_vcIndex);
            
            //无效配置不处理
            if(_mode == VideoAudioOutputMode.None || _mode == VideoAudioOutputMode.APIOnly)
                return;

            long audioInstanceId = 0;
            if (_isBgMusic)
            {
                //设置背景音乐
                audioInstanceId = _playBackgroundVideoMusic(_vp, _mode, volume);   
            }
            else
            {
                //播放视频音效
                audioInstanceId = playClip(GRefdataCoreMgr.instance.npGeneral.video_effect_audio_res_id, false, (audioObject) =>
                {
                    if (audioObject != null)
                        _vp.SetTargetAudioSource(0, audioObject.audioSource);
                }, null, null);

                _vp.setAudioVolume(0, volume);
                //设置是否静音
                _vp.setAudioMute(0, !GameSetting.instance.usingAudio);
            }
            
#if UNITY_EDITOR
            // 输出音频log，仅编辑器
            if (_vcIndex != null)
                AudioDebugMgr.Instance.PlayVideoLog(_isBgMusic, _vcIndex.objName, _vcIndex.objName,
                    (float)_vp.length);
#endif
            
            long videoInstanceId = _vp.getVideoInstanceId();
            if(videoInstanceId == _AALVideoPlayerDealer.g_iInvalidVideoInstanceID)
                return;
            _m_dicAudioCacheInstanceId.TryAdd(videoInstanceId, audioInstanceId);
        }
        
        //当结束播放一段视频
        public void onStopVideoPlaying(_AALVideoPlayerDealer _vp, VideoAudioOutputMode _mode, bool _isBgMusic)
        {
            if(null == _vp)
                return;
            
            //无效配置不处理
            if(_mode == VideoAudioOutputMode.None || _mode == VideoAudioOutputMode.APIOnly)
                return;

            if (_m_dicAudioCacheInstanceId.TryGetValue(_vp.getVideoInstanceId(), out long audioInstanceId))
            {
                if (_isBgMusic)
                {
                    //关闭背景音乐
                    stopBackgroundMusicByInstanceID(audioInstanceId, true);
                }
                else
                {
                    //关闭音效
                    stopClip(audioInstanceId);
                    _vp.SetTargetAudioSource(0, null);
                }
                
                _m_dicAudioCacheInstanceId.Remove(_vp.getVideoInstanceId());
            }

        }
        
        // 通过资源ID播放背景音乐
        private long _playBackgroundVideoMusic(_AALVideoPlayerDealer _videoPlayer, VideoAudioOutputMode _mode,  float _volume)
        {
            //如果当前没有背景音乐
            if (null == _m_curBgAudioDealer)
            {
                _m_curBgAudioDealer = new VideoAudioDealer(_videoPlayer, _mode, _volume);
                _m_bgAudioDealerList.Add(_m_curBgAudioDealer);
                return _m_curBgAudioDealer.play();
            }
            //如果当前播放的背景音乐和要播放的背景音乐不是同一个
            else
            {
                //停止原先的背景音乐
                _m_curBgAudioDealer.stop();

                //添加一个新的背景音乐
                _m_curBgAudioDealer = new VideoAudioDealer(_videoPlayer, _mode, _volume);
                _m_bgAudioDealerList.Add(_m_curBgAudioDealer);
                return _m_curBgAudioDealer.play();
            }
        }  
    }
}
