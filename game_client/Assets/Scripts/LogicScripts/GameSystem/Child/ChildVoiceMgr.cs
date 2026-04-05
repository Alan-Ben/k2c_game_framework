using CommonEnum;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 子嗣配音管理器
    /// </summary>
    public class ChildVoiceMgr : _AVoiceMgr<EChildVoiceType>
    {
        private static ChildVoiceMgr _g_instance;
        public static ChildVoiceMgr instance { get { return _g_instance ??= new ChildVoiceMgr(); } }

        private const int HeroVoiceLayer = 0;//大臣配音音效layer, 先都默认0
        
        [NotNull] private AudioLayerMgr _m_AudioLayerMgr = new AudioLayerMgr();

        protected override _AAudioLayerMgr audioLayerMgr { get { return _m_AudioLayerMgr; } }
        protected override void getCanPlayVoiceIdList(long _sex, EChildVoiceType _voiceType, List<long> _voiceIdList)
        {
            if(_voiceIdList == null)
                return;

            GRefdataCoreMgr.instance.getChildVoiceIdList((EChildSexType) _sex, _voiceType, _voiceIdList);
            if (_voiceIdList == null || _voiceIdList.Count ==  0)
            {
                Debug.LogError($"[ChildVoiceMgr getCanPlayVoiceIdList]找不到子嗣 : {_sex}  {_voiceType}的配表数据");
            }
        }
        
        public bool playVoice(EChildSexType _sex, EChildVoiceType _voiceType, bool _playRandom = true, bool _playBaseOnPre = true, Action<long, long> _onAudioStartPlay = null, Action<long> _onPlayFail = null, Action _onAudioStop = null)
        {
            return playVoice((long)_sex, _voiceType, HeroVoiceLayer, _playRandom, _playBaseOnPre, _onAudioStartPlay, _onPlayFail, _onAudioStop);
        }

        public bool playVoice(EChildSexType _sex, long _voiceOwnerId = 0, EChildVoiceType _voiceType = default, Action<long, long> _onAudioStartPlay = null, Action<long> _onPlayFail = null, Action _onAudioStop = null)
        {
            return playVoice((long)_sex, HeroVoiceLayer, _voiceOwnerId, _voiceType, _onAudioStartPlay, _onPlayFail, _onAudioStop);
        }
    }
}