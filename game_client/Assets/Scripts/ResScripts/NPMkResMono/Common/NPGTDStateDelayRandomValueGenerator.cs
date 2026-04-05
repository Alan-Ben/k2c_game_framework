
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class NPGTDStateDelayRandomValueGenerator : StateMachineBehaviour
    {
        [ALHeader("对应的随机数 parameter 的名字")]
        [ALInfo("这个类固定会在状态 Enter 时触发随机逻辑，等到 delay 的时间到时产生随机数")]
        public string randomValueParameterName = "random_value";
        [ALHeader("随机数的范围 [min, max]，和没有随机时的值")]
        public WCGIntRange randomRange;
        public int idleValue = -1;
        [ALHeader("延时多久触发随机数")]
        public float delay;

        private int _m_parameterHash;
        private int _m_enterSerialize;
        public void Awake()
        {
            _m_parameterHash = Animator.StringToHash(randomValueParameterName);
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            
            if (animator != null)
                animator.SetInteger(_m_parameterHash, idleValue);
            
            int serialize = _m_enterSerialize;
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                if (serialize != _m_enterSerialize)
                    return;
                
                if (animator != null && randomRange != null)
                    animator.SetInteger(_m_parameterHash, randomRange.getRandomValue());
            }, delay);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            _m_enterSerialize = ALSerializeOpMgr.next();
            if (animator != null)
                animator.SetInteger(_m_parameterHash, idleValue);
        }

        public void OnDestroy()
        {
            _m_enterSerialize = ALSerializeOpMgr.next();
        }
    }
}