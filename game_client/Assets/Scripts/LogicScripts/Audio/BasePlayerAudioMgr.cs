using ALPackage;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using JetBrains.Annotations;
using UnityEngine;
using Random = System.Random;

namespace GOE
{
    public enum EAudioChgType
    {
        ADD,//音量提升
        REDUCE,//音量衰减
    }

    public abstract partial class BasePlayerAudioMgr
    {
        public const long defaultLastAudioRefId = 1003;
        
        // 资源容器对象，用于从cache中实例化资源并管理的对象
        [NotNull] protected ALResObjListContainer _m_alResObjList = new ALResObjListContainer();
        [NotNull] protected ALResObjListContainer _m_alLanguageResObjList = new ALResObjListContainer();
        // 音效缓存，缓存音效对应的 WCGAudioObject
        [NotNull] protected Dictionary<long, AudioCache> _m_dicAudioCache = new Dictionary<long, AudioCache>();

        // 所有正在播放的音效ID集合，包括背景音效，用于标记还在加载过程中的被stop的音效，如果资源加载完成后id已经不在这个集合了则不再继续播放
        protected HashSet<long> _m_hsPlayingAudioObjectIds;
        // 所有正在播放的音效对象，包括背景音效
        [NotNull]protected Dictionary<long, AudioObject> _m_dPlayingAudioObjectDic = new Dictionary<long, AudioObject>();
        // 所有当前帧播放的音效配置id列表，用于去重同一帧多次调用
        protected HashSet<long> _m_curFramePlayAudioRefIdList;
        // 当前进行音效播放检测的中心位置
        protected Vector3 _m_vAudioPlayCenterPos;
        // 特效总对象的根节点对象
        protected GameObject _m_goRootGo;
        // 音效实例ID种子
        protected long _m_lAudioObjectIDSeed = 1;
        //随机种子
        protected Random rnd;
        //保存上次播放的res id
        protected long _m_tdLastAudioRefId = defaultLastAudioRefId;

        //背景音乐播放进度
        [NotNull]protected Dictionary<long, float> _m_dicBgMusicTimes = new Dictionary<long, float>();
        //背景音乐播放处理队列
        [NotNull]protected List<_ABgAudioDealer> _m_bgAudioDealerList = new List<_ABgAudioDealer>();
        //当前播放的背景音乐
        protected _ABgAudioDealer _m_curBgAudioDealer;
        
        public BasePlayerAudioMgr(string _rootName)
        {
            _m_dicAudioCache = new Dictionary<long, AudioCache>();

            _m_goRootGo = new GameObject();
            _m_goRootGo.name = _rootName;
            _m_goRootGo.transform.localScale = Vector3.one;
            _m_goRootGo.transform.position = Vector3.up * -10000;

            //设置不被删除
            GameObject.DontDestroyOnLoad(_m_goRootGo);

            _m_curBgAudioDealer = null;

            _m_hsPlayingAudioObjectIds = new HashSet<long>();
            _m_curFramePlayAudioRefIdList = new HashSet<long>();

            rnd = new Random();
        }

        // 释放所有资源
        public void discard()
        {
            //重置音效及相关数据
            reset();
            
            GAudioWebResObjCore.instance.discard();

            _discard();
        }

        //重置音效及相关数据
        public void reset()
        {
            //先停止所有音乐播放
            stopAllAudio();
            //清空当前播放集合
            _m_hsPlayingAudioObjectIds.Clear();
            //映射表重置
            _m_dPlayingAudioObjectDic.Clear();
            _m_curFramePlayAudioRefIdList.Clear();

            //清空所有缓存
            foreach(AudioCache audioCache in _m_dicAudioCache.Values)
            {
                audioCache.discard();
            }
            _m_dicAudioCache.Clear();

            if(_m_alResObjList != null)
                _m_alResObjList.discard();
            _m_alLanguageResObjList?.discard();
            

            _reset();
            
            GAudioResCore.instance.tryDiscardAllDisableRes();
        }
        
        /// <summary>
        /// 清除所有语言音效
        /// </summary>
        public long clearAllLanguageAudio()
        {
            // 先停止所有语言音效
            if (_m_dPlayingAudioObjectDic != null)
            {
                List<AudioObject> audioObjectList = _m_dPlayingAudioObjectDic.Values.ToList();
                foreach(AudioObject obj in audioObjectList)
                {
                    if (obj != null && obj.isLanguageAudio)// 若是语言相关音效
                    {
                        //逐个停止播放并放回缓存中
                        _removeClip(obj);

                        _m_dPlayingAudioObjectDic.Remove(obj.id);//从_m_dPlayingAudioObjectDic列表中移除
                        
                        if(obj.refObj != null)
                            _m_curFramePlayAudioRefIdList?.Remove(obj.refObj.id);//从当前帧播放音效中移除
                    }
                }
            }
            
            List<AudioCache> audioCacheList = _m_dicAudioCache.Values.ToList();
            //清空所有语音相关缓存
            foreach(AudioCache audioCache in audioCacheList)
            {
                if (audioCache != null && audioCache.isLangAudio)// 若是语言相关音效缓存
                {
                    releaseAudio(audioCache);
                }
            }
            
            _m_alLanguageResObjList?.discard();
            
            // 这里要注意要清除GAudioResCore中不用的资源, 因为语音相关音效在_ATALObjCore中_m_dLoadedObjInfoDic的索引也还是和普通音效一样,
            // 但是实际上不同语音对应的LoadedObjInfo是不同的, 所以这里要对不用LoadedObjInfo进行清除, 因为前面做了语音资源的清除释放, 所以逻辑正确的话, 到这里语音相关的资源应该都已经被释放了, 对应的LoadedObjInfo可以被销毁
            // 同为音效使用的GAudioWebResObjCore不需要释放, 因为在GAudioWebResObjCore中不存在LoadedObjInfo缓存, 
            GAudioResCore.instance.tryDiscardAllDisableRes();

            long beStoppedBgAudioRefId = NPAudioRefObj.invaildAudioRefId;//被停止的背景音效id
            if (_m_curBgAudioDealer != null)
            {
                beStoppedBgAudioRefId = _m_curBgAudioDealer.refId;
                _m_curBgAudioDealer = null;
            }
            
            foreach (_ABgAudioDealer bgAudioDealer in _m_bgAudioDealerList)
            {
                if (bgAudioDealer != null) 
                    bgAudioDealer.stop();
            }
            _m_bgAudioDealerList.Clear();
            
            return beStoppedBgAudioRefId;
        }

        public void releaseAudio(AudioCache _cache)
        {
            if(_cache == null)
                return;
            
            _cache.discard();
            _m_dicAudioCache.Remove(_cache.mergeIndex);
        }


        //停止所有Audio的播放
        public void stopAllAudio()
        {
            if (_m_dPlayingAudioObjectDic != null)
            {
                foreach(AudioObject obj in _m_dPlayingAudioObjectDic.Values)
                {
                    //逐个停止播放并放回缓存中
                    _removeClip(obj);
                }
            }

            _stopAllAudio();
        }
        
        //停止所有指定AudipID的音效播放
        public void stopAudioWithAudioID(List<long> _audioIdList)
        {
            if(_audioIdList == null)
                return;

            for(int i = 0; i < _audioIdList.Count; i++)
            {
                AudioObject tmpObj = null;
                if(_m_dPlayingAudioObjectDic.TryGetValue(_audioIdList[i], out tmpObj))
                {
                    //逐个停止播放并放回缓存中
                    _removeClip(tmpObj);
                }
            }

            _stopAudioWithAudioID();
        }

        //从音效ID列表中随机取一个播放
        public void playOneShotClip(List<long> _refIdList, Transform _parent, Vector3 _vec)
        {
            if(_refIdList == null || _refIdList.Count == 0)
                return;

            _playOneShotClip(_refIdList, _parent, _vec);
        }

        // 播放整个音效ID列表
        public void playShotClipWithList(List<long> _refIdList, Transform _parent, Vector3 _vec)
        {
            for(int i = 0; i < _refIdList.Count; i++)
            {
                playOneShotClip(_refIdList[i], _parent, _vec);
            }
        }

        // 通过资源ID播放一个短期音效
        public long playOneShotClip(long _refId, Transform _parent, Vector3 _vec)
        {
            NPAudioRefObj audioRef = GRefdataCoreMgr.instance.audioMap.getRef(_refId);
            if(null == audioRef)
            {
                if(_refId != 0)
                {
#if UNITY_EDITOR
                    Debug.LogError("can not find audioRef,resId:" + _refId);
#endif
                }
                return AudioObject.g_iInvalidAudioObjectID;
            }

            //对持续时间进行判断
            if(audioRef.duration < 0)
            {
#if UNITY_EDITOR

                UnityEngine.Debug.LogError("Play a one shot clip with time is unlimit! id: " + _refId);
#endif
            }

            _playOneShotClip(_refId, _parent, _vec);
            return playClip(audioRef, _parent, _vec);
        }

        public long playClip(long _refId, bool _isbgAudio = false, Action<AudioObject> _startPlayAction = null, Action _onPlayFail = null, Action<AudioObject> _stopAction = null, float _audioStartPlayTime = -1)
        {
            NPAudioRefObj audioRef = GRefdataCoreMgr.instance.audioMap.getRef(_refId);

            if(null == audioRef)
                return AudioObject.g_iInvalidAudioObjectID;

            return playClip(audioRef, _m_goRootGo?.transform, Vector3.zero, _isbgAudio, _startPlayAction, _onPlayFail, _stopAction, _audioStartPlayTime);
        }

        // 通过资源ID播放一个短期音效
        public long playClip(long _refId, Transform _parent, Vector3 _vec, bool _isbgAudio = false, Action<AudioObject> _startPlayAction = null, Action _onPlayFail = null, Action<AudioObject> _stopAction = null, float _audioStartPlayTime = -1)
        {
            NPAudioRefObj audioRef = GRefdataCoreMgr.instance.audioMap.getRef(_refId);
            if(null == audioRef)
            {
                return AudioObject.g_iInvalidAudioObjectID;
            }

            return playClip(audioRef, _parent, _vec, _isbgAudio, _startPlayAction, _onPlayFail, _stopAction, _audioStartPlayTime);
        }

        // 通过资源ID播放一个音效
        public long playClip(NPAudioRefObj _audioRef, Transform _parent, Vector3 _vec, bool _isbgAudio = false, Action<AudioObject> _startPlayAction = null, Action _onPlayFail = null, Action<AudioObject> _stopAction = null, float _audioStartPlayTime = -1)
        {
            if(null == _audioRef)
                return AudioObject.g_iInvalidAudioObjectID;
            //同一个音效，同一帧只触发一次
            if (_m_curFramePlayAudioRefIdList.Contains(_audioRef.id))
                return AudioObject.g_iInvalidAudioObjectID;
            _m_curFramePlayAudioRefIdList.Add(_audioRef.id);

            if (_m_curFramePlayAudioRefIdList.Count == 1)
            {
                //同一个音效，同一帧只触发一次
                ALCommonTaskController.CommonActionAddLaterMonoTask(() =>
                {
                    _m_curFramePlayAudioRefIdList.Clear();
                });
            }
            
            if(_parent == null)
                _parent = _m_goRootGo?.transform;

            //获取新的音效Id
            long audioObjectId = _getAudioObjectID();
            //加入到映射表
            _m_hsPlayingAudioObjectIds.Add(audioObjectId);
            //从缓存中取出对象
            _popFromCache(_audioRef, _parent, audioObjectId, _vec, _doPlayAudio, _isbgAudio, _audioStartPlayTime, _startPlayAction, _onPlayFail, _stopAction);
            
            //返回数据
            return audioObjectId;
        }
        
        // 开始淡出指定音效实例ID的音效  所有的停止播放都要先开始淡出然后才停止
        public void stopClip(long _id)
        {
            if(_id == AudioObject.g_iInvalidAudioObjectID)
                return;

            AudioObject audioObject = null;
            if (_m_dPlayingAudioObjectDic.TryGetValue(_id, out audioObject))
            {
                //从映射表移除
                _m_dPlayingAudioObjectDic.Remove(_id);
            }

            if (audioObject != null)
            {
                //判断对象是否需要淡入淡出
                if(audioObject.refObj.fade_out_time > 0.001f)
                {
                    //开启淡出音量任务进行处理
                    NPAudioVolueFadeOutTask task = WCGSingleton<AudioFadeOutTaskCache>.instance.popItem();
                    task.setInfo(audioObject, audioObject.refObj.fade_out_time, _removeClip);

                    ALMonoTaskMgr.instance.addNextFrameTask(task);
                }
                else
                {
                    _removeClip(audioObject);
                }
            }
            else
            {
                //从列表删除
                _m_hsPlayingAudioObjectIds.Remove(_id);
            }
        }
        
        /// <summary>
        /// 获取指定音效播放的时间
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public float getAudioPlayedTime(long _id)
        {
            if (_m_dPlayingAudioObjectDic == null)
                return -1f;

            if (_m_dPlayingAudioObjectDic.TryGetValue(_id, out AudioObject audioObject) && audioObject != null && audioObject.audioSource != null)
            {
                return audioObject.audioSource.time;
            }
            else
            {
                return -1f;
            }
        }

        // 通过资源ID播放背景音乐
        public long playBackgroundMusic(long _refId)
        {
            //如果当前没有背景音乐
            if (null == _m_curBgAudioDealer)
            {
                _m_curBgAudioDealer = new BgAudioDealer(_refId);
                _m_bgAudioDealerList.Add(_m_curBgAudioDealer);
                return _m_curBgAudioDealer.play();
            }
            //如果当前播放的背景音乐和要播放的背景音乐是同一个同时返回
            else if (_m_curBgAudioDealer.refId == _refId)
            {
                return _m_curBgAudioDealer.play(); 
            }
            //如果当前播放的背景音乐和要播放的背景音乐不是同一个
            else
            {
                //停止原先的背景音乐
                _m_curBgAudioDealer.stop();

                //添加一个新的背景音乐
                _m_curBgAudioDealer = new BgAudioDealer(_refId);
                _m_bgAudioDealerList.Add(_m_curBgAudioDealer);
                return _m_curBgAudioDealer.play();
            }
        }  
        
        public long playBackgroundMusic()
        {
            if(null != _m_curBgAudioDealer)
            {
                return _m_curBgAudioDealer.play();
            }
            else
            {
                _m_curBgAudioDealer = _m_bgAudioDealerList.GetLast();
                return _m_curBgAudioDealer.play();
            }
        }
        
        // 停止背景音乐
        public void stopBackgroundMusicByRefID(long _refID, bool _needAutoPlayLast = false)
        {
            _ABgAudioDealer bgAudioDealer = null;
            for (int i = _m_bgAudioDealerList.Count - 1; i >= 0; i--)
            {
                bgAudioDealer = _m_bgAudioDealerList[i];
                if(null == bgAudioDealer)
                    continue;

                if (bgAudioDealer.refId == _refID)
                {
                    bgAudioDealer.stop();
                    _m_bgAudioDealerList.RemoveAt(i);
                    break;
                }
            }

            if(null != _m_curBgAudioDealer && _m_curBgAudioDealer.refId == _refID)
            {
                _m_curBgAudioDealer.stop();
                _m_curBgAudioDealer = null;
                
                //如果需要自动播放最后一个
                if (_needAutoPlayLast)
                {
                    _m_curBgAudioDealer = _m_bgAudioDealerList.GetLast();
                    if(null != _m_curBgAudioDealer)
                        _m_curBgAudioDealer.play();
                }
            }
        }
        
        // 停止背景音乐,会自动播放队列最后一个
        public void stopBackgroundMusicByInstanceID(long _instanceId, bool _needAutoPlayLast = false)
        {
            _ABgAudioDealer bgAudioDealer = null;
            for (int i = _m_bgAudioDealerList.Count - 1; i >= 0; i--)
            {
                bgAudioDealer = _m_bgAudioDealerList[i];
                if(null == bgAudioDealer)
                    continue;

                if (bgAudioDealer.instanceId == _instanceId)
                {
                    bgAudioDealer.stop();
                    _m_bgAudioDealerList.RemoveAt(i);
                    break;
                }
            }

            if(null != _m_curBgAudioDealer && _m_curBgAudioDealer.instanceId == _instanceId)
            {
                _m_curBgAudioDealer.stop();
                _m_curBgAudioDealer = null;
                
                //如果需要自动播放最后一个
                if (_needAutoPlayLast)
                {
                    _m_curBgAudioDealer = _m_bgAudioDealerList.GetLast();
                    if(null != _m_curBgAudioDealer)
                        _m_curBgAudioDealer.play();
                }
            }
        }
        
        // 停止背景音乐,单纯的停止
        public void stopBackgroundMusic()
        {
            //取出当前播放的背景音乐
            if(null != _m_curBgAudioDealer)
            {
                //停止当前播放的
                _m_curBgAudioDealer.stop();
            }
        }

        //存下当前背景音乐的播放进度
        private void _saveBgMusicTime(long _instanceId)
        {
            AudioObject audioObject;
            if(_m_dPlayingAudioObjectDic.TryGetValue(_instanceId, out audioObject))
            {
                if (null != audioObject && audioObject.refObj != null)
                {
                    if (audioObject.audioSource != null)
                    {
                        _m_dicBgMusicTimes[audioObject.refObj.id] = audioObject.audioSource.time;
                    }
                }
            }
        }

        private void _removeBgAudioDealer(long _refId)
        {
            for (int i = 0; i < _m_bgAudioDealerList.Count; i++)
            {
                _ABgAudioDealer itemDealer = _m_bgAudioDealerList[i];
                if(null == itemDealer)
                    continue;
                
                if(itemDealer.refId == _refId)
                {
                    _m_bgAudioDealerList.RemoveAt(i);
                    break;
                }
            }
        }
        
        //停止播放
        protected void _removeClip(AudioObject _audioObj)
        {
            if(null == _audioObj)
                return;
#if UNITY_EDITOR
            AudioDebugMgr.Instance.StopAudioLog(_audioObj.id);
#endif
            //从列表删除
            _m_hsPlayingAudioObjectIds.Remove(_audioObj.id);
            //停止当前音效的播放
            _audioObj.stop();
            //放回缓存
            _pushbackToCache(_audioObj);
        }

        // 内部统一的音效播放接口
        private void _doPlayAudio(Transform _parent, long _id, AudioObject _audioObject, Vector3 _vec, bool _isbgAudio, float _audioStartPlayTime, Action<AudioObject> _startPlayAction, Action<AudioObject> _stopAction)
        {
            if(null == _audioObject || _audioObject == default(AudioObject) || null == _audioObject.go || _audioObject.refObj == null)
                return;

            //判断是否在播放集合内，不在则返回缓存中
            if(null == _audioObject.go || !_m_hsPlayingAudioObjectIds.Contains(_id))
            {
                _pushbackToCache(_audioObject);
                return;
            }

            float realVolumeScale = 1f;

            // 设置音效开始播放时间
            if (_audioStartPlayTime < 0)//若设置的开始播放时间小于0，不用进行时间设置
            {
                //如果是背景音乐的话
                if(_isbgAudio)
                {
                    //继续播放之前的背景音乐进度
                    if (_audioObject.refObj != null && _audioObject.audioSource != null && _audioObject.audioSource.clip != null)
                    {
                        if (_m_dicBgMusicTimes.TryGetValue(_audioObject.refObj.id, out float audioTime) && audioTime >= 0)
                        {
                            _audioObject.audioSource.time = audioTime % _audioObject.audioSource.clip.length;
                        };
                    }

                    if (_m_volumeControl != null) 
                        realVolumeScale = _m_volumeControl.audioSourceBgVolume;
                }
                else
                {
                    if (_m_volumeControl != null) 
                        realVolumeScale = _m_volumeControl.audioSourceVolume;
                }
            }
            else//需要设置开始播放时间
            {
                if (_audioObject.audioSource != null && _audioObject.audioSource.clip != null)
                {
                    _audioObject.audioSource.time = _audioStartPlayTime % _audioObject.audioSource.clip.length;
                }
                
                if (_m_volumeControl != null) 
                    realVolumeScale = _m_volumeControl.audioSourceVolume;
            }
            
            //调用播放函数
            _audioObject.play(_id, _parent, _vec, _startPlayAction, _stopAction);

#if UNITY_EDITOR
            // 输出音频log，仅编辑器
            if (null != _audioObject.audioSource && null != _audioObject.audioSource.clip)
            {
                AudioDebugMgr.Instance.PlayAudioLog(_id, _isbgAudio, _audioObject.audioSource.name, _audioObject.audioSource.clip.name,
                    _audioObject.refObj.duration <= 0 ? _audioObject.audioSource.clip.length : _audioObject.refObj.duration / 1000f,
                    _audioObject.refObj._refId.ToString(), _audioObject.refObj.duration == -1, _audioObject.audioSource.time);

                //设置动态的值
                if (_m_audioVolumeDict.ContainsKey(_audioObject.refObj.id))
                {
                    _audioObject.audioSource.volume = _m_audioVolumeDict[_audioObject.refObj.id];
                }   
            }
#endif

            //开启淡入音量任务进行处理
            if(_audioObject.refObj.fade_in_time > 0.001f)
            {
                //获取任务
                NPAudioVolueFadeInTask task = WCGSingleton<AudioFadeInTaskCache>.instance.popItem();
                task.setInfo(_audioObject, _audioObject.refObj.fade_in_time, realVolumeScale, null);

                ALMonoTaskMgr.instance.addMonoTask(task);
            }
            else
            {
                //直接设置比例
                _audioObject.setAudioScale(realVolumeScale);
            }

            //添加到映射表
            _m_dPlayingAudioObjectDic.Add(_id, _audioObject);
    
            //根据是否定时播放进行处理，当配置时间小于0表示时间无限
            if(_audioObject.refObj.duration >= 0)
            {
                float monitorTime = _audioObject.refObj.duration / 1000f;
                if(_audioObject.refObj.duration == 0)
                {
                    //当不配置时间则使用音效时长
                    if(null != _audioObject.audioSource && null != _audioObject.audioSource.clip)
                    {
                        monitorTime = _audioObject.audioSource.clip.length;
                    }
                }

                //在总时长-淡出时长的时间后开始淡出音效，在Duration时刚好结束播放音效
                ALCommonActionMonoTask.addMonoTask(() =>
                {
                    stopClip(_id);
                }, monitorTime);
            }
        }

        // 取出对应索引的资源缓存对象
        delegate void DEAL_PLAY_AUDIO(Transform _parent, long _id, AudioObject _audioObject, Vector3 _vec, bool _isbgAudio, float _audioStartPlayTime, Action<AudioObject> _startPlayAction, Action<AudioObject> _stopAction);
        private void _popFromCache(NPAudioRefObj _refObj, Transform _parent, long _id, Vector3 _vec, DEAL_PLAY_AUDIO _call, bool _isbgAudio = false, float _audioStartPlayTime = -1, Action<AudioObject> _startPlayAction = null, Action _onPlayFail = null, Action<AudioObject> _stopAction = null)
        {
            if (null == _refObj)
            {
                _onPlayFail?.Invoke();
                return;
            }

            //拼凑Id
            long mergeIndex = ALCommon.mergeInt(_refObj.audio_index.mainId, _refObj.audio_index.subId);

            AudioCache cacheObj = null;
            if(!_m_dicAudioCache.TryGetValue(mergeIndex, out cacheObj))
            {
                //构建对象
                cacheObj = new AudioCache(_refObj, _m_goRootGo?.transform);
                //加入映射表
                _m_dicAudioCache.Add(mergeIndex, cacheObj);
            }

            if(null == cacheObj)
            {
                _onPlayFail?.Invoke();
                return;
            }

            // 判断缓存是否初始化完成
            if(cacheObj.isInitialized())
            {
                _call?.Invoke(_parent, _id, cacheObj.popItem(), _vec, _isbgAudio, _audioStartPlayTime, _startPlayAction, _stopAction);
            }
            else
            {
                // 未初始化的缓存需要加载模板对象初始化缓存
                _loadAudioResObj(_refObj, (resObj) =>
                {
                    if (resObj == null)
                    {
                        _onPlayFail?.Invoke();
                        return;
                    }
                    
                    // 再判断一次缓存是否初始化完成，防止加载资源过程的重复请求产生对缓存的重复初始化
                    if(cacheObj.isInitialized())
                    {
                        //释放资源
                        resObj.discard();
                        //调用回调
                        _call?.Invoke(_parent, _id, cacheObj.popItem(), _vec, _isbgAudio, _audioStartPlayTime, _startPlayAction, _stopAction);
                    }
                    else
                    {
                        AudioObject audioObject = new AudioObject(resObj, _refObj);
                        //初始化cache
                        cacheObj.init(audioObject);
                        //调用回调
                        _call?.Invoke(_parent, _id, cacheObj.popItem(), _vec, _isbgAudio, _audioStartPlayTime, _startPlayAction, _stopAction);
                    }
                });
            }
        }

        /// <summary>
        /// 加载音效resObj对象
        /// </summary>
        /// <param name="_refObj"></param>
        /// <param name="_loadDone"></param>
        protected void _loadAudioResObj(NPAudioRefObj _refObj, Action<_IAudioResObj> _loadDone)
        {
            if (_refObj == null)
            {
                Debug.LogError("[_loadAudioResObj] _refObj == null");
                _loadDone?.Invoke(null);
                return;
            }

            _IAudioResObjCore resObjCore = null;
            if (!_refObj.is_async_audio)
            {
                resObjCore = GAudioResCore.instance;
            }
            else
            {
                resObjCore = GAudioWebResObjCore.instance;
            }

            if (resObjCore != null)
            {
                resObjCore.loadObj(_refObj, _loadDone);    
            }
            else
            {
                _loadDone?.Invoke(null);
            }
        }

        // 添加进各自的缓存
        private void _pushbackToCache(AudioObject _audioObject)
        {
            if(_audioObject == null || _audioObject.refObj == null)
                return;

            // 拼凑Id
            long mergeIndex = ALCommon.mergeInt(_audioObject.refObj.audio_index.mainId, _audioObject.refObj.audio_index.subId);
            
            // 无法缓存可放入的时候销毁(例如：WCGPlayAudioMgr.discard在加载过程已经被调用)
            if(!_m_dicAudioCache.ContainsKey(mergeIndex))
            {
                _audioObject.discard();
                return;
            }
            
            _m_dicAudioCache[mergeIndex].pushBackCacheItem(_audioObject);
        }

        // 简单的音效实例ID生成器
        private long _getAudioObjectID()
        {
            return ++_m_lAudioObjectIDSeed;
        }

        public void chgBgAudioValue(bool _isUsingBgAudio, float _value)
        {
            if (_m_volumeControl != null) 
                _m_volumeControl.setBgAudioVolume(_isUsingBgAudio, _value);

            _chgBgAudioValue(_isUsingBgAudio, _value);
        }

        public void chgAudioValue(bool _isUsingAudio, float _value)
        {
            if (_m_volumeControl != null) 
                _m_volumeControl.setAudioVolume(_isUsingAudio, _value);
            
            _chgAudioValue(_isUsingAudio, _value);
        }

        public void chgVoiceValue(bool _isUsingVoice, float _value)
        {
            if (_m_volumeControl != null) 
                _m_volumeControl.setVoiceVolume(_isUsingVoice, _value);

            _chgVoiceValue(_isUsingVoice, _value);
        }
        
#if UNITY_EDITOR
        //运行时候动态工具改的值
        [NotNull]private Dictionary<long, float> _m_audioVolumeDict = new Dictionary<long, float>();
        
        /// <summary>
        /// 改变某一个音频的最大音量，编辑器下音效调整使用
        /// </summary>
        /// <param name="_audioName"></param>
        /// <param name="_fVolume"></param>
        public void chgAudioMaxVolume_Editor(string _audioName ,float _fVolume)
        {
            if(!Application.isPlaying)
                return;
            GRefdataCoreMgr.instance.audioMap.dealAllRef(_ref =>
            {
                if (_ref.audio_index.objName.Equals(_audioName))
                {
                    //拼凑Id
                    long mergeIndex = ALCommon.mergeInt(_ref.audio_index.mainId, _ref.audio_index.subId);

                    AudioCache cacheObj = null;
                    if (_m_dicAudioCache.TryGetValue(mergeIndex, out cacheObj))
                    {
                        //保存设置动态的值
                        if (_m_audioVolumeDict.ContainsKey(_ref.id))
                        {
                            _m_audioVolumeDict[_ref.id] = _fVolume;
                        }
                        else
                        {
                            _m_audioVolumeDict.Add(_ref.id, _fVolume);
                        }
                        
                        foreach (AudioObject audioObject in cacheObj.usedItemList)
                        {
                            audioObject.setAudioVolue(_fVolume);
                        }
                    }
                }
            });
        }
#endif
        
        //减低游戏整体音效音量
        /*
        public void ReduceAudioVolume(float reduceScale) {

            if (_m_bReduceAudioVolume || reduceScale>=1)
                return;
            _m_bReduceAudioVolume = true;

            _m_fReduceScale = reduceScale;

            foreach (WCGAudioObject obj in _m_dPlayingAudioObjectList.Values)
            {
                AudioSource audioSouce = obj.go.GetComponent<AudioSource>();
                if(null != audioSouce)
                {
                    audioSouce.volume = audioSouce.volume* _m_fReduceScale;
                }
            }
        }
        */

        //重置游戏整体音效音量
        /*
        public void resetAudioVolume()
        {
            if (!_m_bReduceAudioVolume || _m_fReduceScale>=1)
                return;

            _m_bReduceAudioVolume = false;

            foreach (WCGAudioObject obj in _m_dPlayingAudioObjectList.Values)
            {
                AudioSource audioSouce = obj.go.GetComponent<AudioSource>();
                if (null != audioSouce)
                {
                    audioSouce.volume = audioSouce.volume / _m_fReduceScale;
                }
            }
            _m_fReduceScale = 1f;
        }
        */
        
        //音量控制接口，子类实现
        protected abstract _IAudioVolumeControl _m_volumeControl { get; }

        public abstract void _stopAudioWithAudioID();

        public abstract void _reset();

        public abstract void _discard();

        public abstract void _stopAllAudio();

        public abstract void _chgBgAudioValue(bool _isUsingAudio, float _value);

        public abstract void _chgAudioValue(bool _isUsingAudio, float _value);
        public abstract void _chgVoiceValue(bool _isUsingVoice, float _value);

        public abstract void _playOneShotClip(List<long> _refIdList, Transform _parent, Vector3 _vec);
        public abstract void _playOneShotClip(long _refId, Transform _parent, Vector3 _vec);
    }
}
