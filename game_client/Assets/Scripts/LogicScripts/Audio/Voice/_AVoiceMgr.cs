using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Random = UnityEngine.Random;

namespace GOE
{
    public abstract class _AVoiceMgr<T_Enum> where T_Enum : Enum
    {
        // 临时可用配音id列表
        [NotNull] protected List<long> _m_lTmpVoiceTypeCanUseVoiceRefIdList = new List<long>();
        [NotNull] protected Dictionary<long, Dictionary<T_Enum, int>> _m_dPrePlayVoiceDic = new Dictionary<long, Dictionary<T_Enum, int>>();//上一次播放的配音字典

        [NotNull] protected abstract _AAudioLayerMgr audioLayerMgr { get; }

        /// <summary>
        /// 获取可播放配音id列表
        /// </summary>
        /// <param name="_voiceOwnerId">配音所有者id</param>
        /// <param name="_voiceType">配音类型</param>
        /// <param name="_voiceIdList">配音id用这个列表传回来</param>
        protected abstract void getCanPlayVoiceIdList(long _voiceOwnerId, T_Enum _voiceType, List<long> _voiceIdList);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_voiceOwnerId">语音所有者id</param>
        /// <param name="_voiceType">配音类型</param>
        /// <param name="_playRandom">是否随机播放, true随机, false顺序</param>
        /// <param name="_playBaseOnPre">是否在上一次播放的配音基础上播放 (true : 随机的情况会排除掉上一次播放的配音, 顺序的情况会播放上一次配音后一个配音) (false : 随机的情况就全配音随机, 顺序的情况从头开始顺序播放)</param>
        /// <param name="_onAudioStartPlay">配表开始播放的回调，（实例id，voice表id）</param>
        /// <param name="_onPlayFail">播放失败时回调</param>
        /// <param name="_onAudioStop">配音停止时回调(播放失败的时候不会调用)</param>
        public bool playVoice(long _voiceOwnerId, T_Enum _voiceType, int _voiceLayer, bool _playRandom = true, bool _playBaseOnPre = true, Action<long, long> _onAudioStartPlay = null, Action<long> _onPlayFail = null, Action _onAudioStop = null)
        {
            stopAllVoice();
            
            if(_voiceOwnerId <= 0)
                return false;

            _m_lTmpVoiceTypeCanUseVoiceRefIdList.Clear();
            getCanPlayVoiceIdList(_voiceOwnerId, _voiceType, _m_lTmpVoiceTypeCanUseVoiceRefIdList);
            
            return playVoice(_m_lTmpVoiceTypeCanUseVoiceRefIdList, _voiceLayer, _voiceOwnerId, _voiceType, _playRandom, _playBaseOnPre, _onAudioStartPlay, _onPlayFail, _onAudioStop);
        }

        /// <summary>
        /// 播放配音, 可以不用传入配音所有者id和配音类型, 若传入的话会记录播放的配音索引, 并且会根据_playBaseOnPre参数判断是否基于上一次播放的记录进行新配音播放
        /// </summary>
        /// <param name="_voiceRefIdList">要播放的配音列表</param>
        /// <param name="_voiceLayer">配音播放层级</param>
        /// <param name="_voiceOwnerId">配音所有者id</param>
        /// <param name="_voiceType">配音类型</param>
        /// <param name="_playRandom">是否随机播放</param>
        /// <param name="_playBaseOnPre">是否在上一次播放的配音基础上播放 (true : 随机的情况会排除掉上一次播放的配音, 顺序的情况会播放上一次配音后一个配音) (false : 随机的情况就全配音随机, 顺序的情况从头开始顺序播放)</param>
        /// <param name="_onAudioStartPlay">配表开始播放的回调，（实例id，voice表id）</param>
        public bool playVoice(List<long> _voiceRefIdList, int _voiceLayer, long _voiceOwnerId = 0, T_Enum _voiceType = default, bool _playRandom = true, bool _playBaseOnPre = true, Action<long, long> _onAudioStartPlay = null, Action<long> _onPlayFail = null, Action _onAudioStop = null)
        {
            stopAllVoice();

            if (_voiceRefIdList == null || _voiceRefIdList.Count <= 0)
            {
                Debug.LogWarning_EditorOnly($"[{GetType()} playVoice(_voiceRefIdList, _voiceLayer:{_voiceLayer}, long _voiceOwnerId:{_voiceOwnerId}, T_Enum:{_voiceType}, _playRandom:{_playRandom}, _playBaseOnPre:{_playBaseOnPre})] 传入的参数_voiceRefIdList数据为空");
                _onPlayFail?.Invoke(0);
                return false;
            }
            
            int canPlayVoiceCount = _voiceRefIdList.Count;
            long voiceRefId = 0;
            
            if (_voiceOwnerId != 0 && !EqualityComparer<T_Enum>.Default.Equals(_voiceType, default)) // 配音所属对象id不为0 且 配音类型不为默认类型
            {
                Dictionary<T_Enum, int> preTypeVoiceIndexDic = tryGetPreTypeVoiceIndexDic(_voiceOwnerId);//上一次播放的配音索引字典
                int playVoiceIndex = -1;//要播放的配音索引
                // 若配音需要基于上一次播放的配音进行播放 且 找到上一次播放的配音索引 且 配音索引有效
                if (_playBaseOnPre && preTypeVoiceIndexDic.TryGetValue(_voiceType, out int _preVoiceIndex) && _preVoiceIndex >= 0 && _preVoiceIndex < canPlayVoiceCount)
                {
                    //若随机播放
                    if (_playRandom)
                    {
                        playVoiceIndex = Random.Range(0, canPlayVoiceCount - 1);//因为要排除上一次播放的配音, 所以这里随机的范围要减1
                        if(playVoiceIndex >= _preVoiceIndex)//若随机到的配音索引大于等于上一次播放的配音索引, 则要加1
                            playVoiceIndex++;
                    }
                    else//若顺序播放
                    {
                        playVoiceIndex = _preVoiceIndex + 1;//直接取下一个配音
                    }
                }

                if (playVoiceIndex < 0)//若上面没有找到可用配音
                {
                    if(_playRandom)//随机的情况就全配音随机
                    {
                        playVoiceIndex = Random.Range(0, canPlayVoiceCount);
                    }
                    else//顺序的情况从头开始顺序播放
                    {
                        playVoiceIndex = 0;
                    }
                }

                playVoiceIndex = (playVoiceIndex + canPlayVoiceCount) % canPlayVoiceCount;// 防止下标越界
                preTypeVoiceIndexDic[_voiceType] = playVoiceIndex;//记录下这次播放的配音索引

                voiceRefId = _voiceRefIdList.SafeGet(playVoiceIndex);//配音配表id
            }
            else
            {
                if (_playRandom)//若随机播放, 从列表中随机一个配音
                {
                    voiceRefId = _voiceRefIdList.GetRandomItem();
                }
                else//若不随机, 直接取第一个配音
                {
                    voiceRefId = _voiceRefIdList.GetFirst();
                }
            }

            return audioLayerMgr.playAudio(_voiceLayer, null, voiceRefId, (_audioObjId)=>
            {
                _onAudioStartPlay?.Invoke(_audioObjId, voiceRefId);//配表开始播放的回调
            }, () =>
            {
                _onPlayFail?.Invoke(voiceRefId);//播放失败的回调
            }, _onAudioStop);
        }

        /// <summary>
        /// 直接播放配音, 可以不用传入配音所有者id和配音类型, 若传入的话会记录播放的配音索引
        /// </summary>
        public bool playVoice(long _voiceRefId, int _voiceLayer, long _voiceOwnerId = 0, T_Enum _voiceType = default, Action<long, long> _onAudioStartPlay = null, Action<long> _onPlayFail = null, Action _onAudioStop = null)
        {
            stopAllVoice();//停止所有其他配音

            if (_voiceRefId <= 0)
            {
                return false;
            }
            
            if (_voiceOwnerId != 0 && !EqualityComparer<T_Enum>.Default.Equals(_voiceType, default))// 配音所属对象id不为0 且 配音类型不为默认类型
            {
                _m_lTmpVoiceTypeCanUseVoiceRefIdList.Clear();
                getCanPlayVoiceIdList(_voiceOwnerId, _voiceType, _m_lTmpVoiceTypeCanUseVoiceRefIdList);//获取可播放配音id列表

                int voiceIndex = _m_lTmpVoiceTypeCanUseVoiceRefIdList.FindIndex((_x) => _x == _voiceRefId);//查找_voiceId在可播放配音id列表_m_lTmpVoiceTypeCanUseVoiceRefIdList中的索引
                
                Dictionary<T_Enum, int> preTypeVoiceIndexDic = tryGetPreTypeVoiceIndexDic(_voiceOwnerId);//上一次播放的配音索引字典
                preTypeVoiceIndexDic[_voiceType] = voiceIndex;
            }
            
            // 播放音效
            return audioLayerMgr.playAudio(_voiceLayer, null, _voiceRefId, (_audioObjId)=>
            {
                _onAudioStartPlay?.Invoke(_audioObjId, _voiceRefId);
            }, () =>
            {
                _onPlayFail?.Invoke(_voiceRefId);
            }, _onAudioStop);
        }
        
        [NotNull] protected Dictionary<T_Enum, int>  tryGetPreTypeVoiceIndexDic(long _voiceOwnerId)
        {
            Dictionary<T_Enum, int> preTypeVoiceIndexDic = null;//上一次播放的配音索引字典
            if (!_m_dPrePlayVoiceDic.TryGetValue(_voiceOwnerId, out preTypeVoiceIndexDic) || preTypeVoiceIndexDic == null)
            {
                preTypeVoiceIndexDic = new Dictionary<T_Enum, int>();
                _m_dPrePlayVoiceDic[_voiceOwnerId] = preTypeVoiceIndexDic;
            }

            return preTypeVoiceIndexDic;
        }
        
        /// <summary>
        /// 获取上一次播放的配音索引
        /// </summary>
        /// <param name="_voiceOwnerId"></param>
        /// <param name="_voiceType"></param>
        /// <returns></returns>
        public int getPrePlayVoiceIndex(long _voiceOwnerId, T_Enum _voiceType)
        {
            Dictionary<T_Enum, int> preTypeVoiceIndexDic = tryGetPreTypeVoiceIndexDic(_voiceOwnerId);
            if (preTypeVoiceIndexDic.TryGetValue(_voiceType, out int _preVoiceIndex))
            {
                return _preVoiceIndex;
            }

            return -1;
        }

        /// <summary>
        /// 停止音效
        /// </summary>
        public void stopAllVoice()
        {
            audioLayerMgr.stopAllAudio();
        }

        /// <summary>
        /// 注册所有配音播放完成回调
        /// </summary>
        /// <param name="???"></param>
        public void regAllVoicePlayDone(Action _voicePlayDoneAction)
        {
            audioLayerMgr.regAllAudioStopCallback(_voicePlayDoneAction);   
        }

        public void unRegAllVoicePlayDone(Action _voicePlayDoneAction)
        {
            audioLayerMgr.unRegAllAudioStopCallback(_voicePlayDoneAction);   
        }

        /// <summary>
        /// 注册配音开始播放回调
        /// </summary>
        /// <param name="_voicePlay"></param>
        public void regVoicePlayAction(Action<int> _voicePlay)
        {
            audioLayerMgr.regAudioPlayCallback(_voicePlay);
        }
        
        public void unRegVoicePlayAction(Action<int> _voicePlay)
        {
            audioLayerMgr.unRegAudioPlayCallback(_voicePlay);
        }
    }
}