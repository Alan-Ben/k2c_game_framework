using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 动画状态机播放音效脚本
    /// </summary>
    public class StateMachineBehaviourOnEnterPlayAudio : StateMachineBehaviour
    {
        [ALHeader("播放的audioid")]
        public long audioId;
        [ALHeader("退出状态时是否不销毁音效，选中时不销毁")]
        public bool noDiscardAudioWhenExit;

        private int _m_lastLoopCount;
        private long _m_audioInstanceId;

#if NP_GAME
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _stopAudio();
            _m_audioInstanceId = PlayAudioMgr.instance.playClip(audioId);
            _m_lastLoopCount = 1;
        }
    
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(null == animator)
                return;
            
            AnimatorStateInfo stateInfoCur = animator.GetCurrentAnimatorStateInfo(layerIndex);
            
            if(stateInfoCur.fullPathHash != stateInfo.fullPathHash)
                return;

            if (!stateInfo.loop)
                return;
            
            int currentLoop = Mathf.FloorToInt(stateInfo.normalizedTime);
            if (currentLoop > _m_lastLoopCount)
            {
                _m_lastLoopCount = currentLoop;
                _stopAudio();
                _m_audioInstanceId = PlayAudioMgr.instance.playClip(audioId);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _m_lastLoopCount = 1;

            if(!noDiscardAudioWhenExit)
                _stopAudio();
        }

        private void _stopAudio()
        {
            if (_m_audioInstanceId > 0)
            {
                PlayAudioMgr.instance.stopClip(_m_audioInstanceId);
                _m_audioInstanceId = 0;
            }
        }

#endif
        
    }
}