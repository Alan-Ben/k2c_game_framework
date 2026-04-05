using UnityEngine;

namespace GOE
{
    public class NPGTDStateRandomValueGenerator : StateMachineBehaviour
    {
        [ALHeader("对应的随机数 parameter 的名字")]
        public string randomValueParameterName = "random_value";
        [ALHeader("随机数的范围 [min, max]")]
        public WCGIntRange randomRange;
        [ALHeader("随机时机")]
        public RandomType randomType;

        private int _m_parameterHash; 
        public void Awake()
        {
            _m_parameterHash = Animator.StringToHash(randomValueParameterName);
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            if (((int)randomType & 1) == 0)
                return;
            
            if (animator != null && randomRange != null)
                animator.SetInteger(_m_parameterHash, randomRange.getRandomValue());
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            
            if (((int)randomType & 2) == 0)
                return;
            
            if (animator != null && randomRange != null)
                animator.SetInteger(_m_parameterHash, randomRange.getRandomValue());
        }

        public enum RandomType
        {
            RandomOnEnter = 1,
            RandomOnExit = 2,
            RandomOnEnterAndExit = 3
        }
    }
}