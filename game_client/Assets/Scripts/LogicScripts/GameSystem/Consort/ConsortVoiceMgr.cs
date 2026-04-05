using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 妃子配音管理器
    /// </summary>
    public class ConsortVoiceMgr : _AVoiceMgr<EConsortVoiceType>
    {
        private static ConsortVoiceMgr _g_instance;
        public static ConsortVoiceMgr instance { get { return _g_instance ??= new ConsortVoiceMgr(); } }

        private const int ConsortVoiceLayer = 0;//妃子配音音效layer, 先都默认0
        
        [NotNull] private AudioLayerMgr _m_AudioLayerMgr = new AudioLayerMgr();

        protected override _AAudioLayerMgr audioLayerMgr { get { return _m_AudioLayerMgr; } }
        
        protected override void getCanPlayVoiceIdList(long _voiceOwnerId, EConsortVoiceType _voiceType, List<long> _voiceIdList)
        {
            if(_voiceIdList == null)
                return;
            
            GConsortRefObj consortRefObj = GRefdataCoreMgr.instance.consortRefCore.getRef(_voiceOwnerId);
            if (consortRefObj == null)
            {
                Debug.LogError($"[ConsortVoiceMgr getCanPlayVoiceIdList]找不到妃子 : {_voiceOwnerId}的配表数据");
                return;
            }
            
            _voiceIdList.Clear();
            consortRefObj.getCanPlayVoiceIdList(_voiceType, _voiceIdList);
        }

        public bool playVoice(long _voiceOwnerId, EConsortVoiceType _voiceType, bool _playRandom = true, bool _playBaseOnPre = true, Action<long, long> _onAudioStartPlay = null, Action<long> _onPlayFail = null, Action _onAudioStop = null)
        {
            return playVoice(_voiceOwnerId, _voiceType, ConsortVoiceLayer, _playRandom, _playBaseOnPre, _onAudioStartPlay, _onPlayFail, _onAudioStop);
        }

        public bool playVoice(long _voiceRefId, long _voiceOwnerId = 0, EConsortVoiceType _voiceType = default, Action<long, long> _onAudioStartPlay = null, Action<long> _onPlayFail = null, Action _onAudioStop = null)
        {
            return playVoice(_voiceRefId, ConsortVoiceLayer, _voiceOwnerId, _voiceType, _onAudioStartPlay, _onPlayFail, _onAudioStop);
        }
    }
}