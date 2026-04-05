using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace GOE
{
    public class GGUIPlayableAudioPlay : BasicPlayableBehaviour
    {
        [Header("语音id")]
        public long audioId;
        private long _m_audioInstanceId;

        public override void OnBehaviourPlay(Playable _playable, FrameData _info)
        {
#if NP_GAME
            if(!Application.isPlaying)
                return;
        
            _m_audioInstanceId = PlayAudioMgr.instance.playClip(audioId);
            WinMsg.SendMsg(WinMsgType.ON_PLAYABLE_AUDIO_CHANGE, audioId ,_m_audioInstanceId);
#endif
        }

        public override void OnBehaviourPause(Playable _playable, FrameData _info)
        {
#if NP_GAME
            if (!Application.isPlaying)
                return;

            PlayAudioMgr.instance.stopClip(_m_audioInstanceId);
#endif
        }
    }
}