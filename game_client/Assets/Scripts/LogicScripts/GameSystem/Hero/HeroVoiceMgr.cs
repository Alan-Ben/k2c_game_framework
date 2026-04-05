using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 大臣配音管理器
    /// </summary>
    public class HeroVoiceMgr : _AVoiceMgr<EHeroVoiceType>
    {
        private static HeroVoiceMgr _g_instance;
        public static HeroVoiceMgr instance { get { return _g_instance ??= new HeroVoiceMgr(); } }

        private const int HeroVoiceLayer = 0;//大臣配音音效layer, 先都默认0
        
        [NotNull] private AudioLayerMgr _m_AudioLayerMgr = new AudioLayerMgr();

        protected override _AAudioLayerMgr audioLayerMgr { get { return _m_AudioLayerMgr; } }
        protected override void getCanPlayVoiceIdList(long _voiceOwnerId, EHeroVoiceType _voiceType, List<long> _voiceIdList)
        {
            if(_voiceIdList == null)
                return;
            
            HeroRefObj heroRefObj = GRefdataCoreMgr.instance.heroRefCore.getRef(_voiceOwnerId);
            if (heroRefObj == null)
            {
                Debug.LogError($"[HeroVoiceMgr getCanPlayVoiceIdList]找不到伙伴 : {_voiceOwnerId}的配表数据");
                return;
            }
            
            _voiceIdList.Clear();
            heroRefObj.getCanPlayVoiceIdList(_voiceType, _voiceIdList);
        }
        
        public bool playVoice(long _voiceOwnerId, EHeroVoiceType _voiceType, bool _playRandom = true, bool _playBaseOnPre = true, Action<long, long> _onAudioStartPlay = null, Action<long> _onPlayFail = null, Action _onAudioStop = null)
        {
            return playVoice(_voiceOwnerId, _voiceType, HeroVoiceLayer, _playRandom, _playBaseOnPre, _onAudioStartPlay, _onPlayFail, _onAudioStop);
        }

        public bool playVoice(long _voiceRefId, long _voiceOwnerId = 0, EHeroVoiceType _voiceType = default, Action<long, long> _onAudioStartPlay = null, Action<long> _onPlayFail = null, Action _onAudioStop = null)
        {
            return playVoice(_voiceRefId, HeroVoiceLayer, _voiceOwnerId, _voiceType, _onAudioStartPlay, _onPlayFail, _onAudioStop);
        }
    }
}