
using UnityEngine;

public class NPGTDStateTriggerSetterOnEnter : StateMachineBehaviour
{
    [ALHeader("想要设置的 trigger 名字")]
    public string triggerName;

    private int _m_triggerNameHash;
    
    public void Awake()
    {
        _m_triggerNameHash = Animator.StringToHash(triggerName);
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        
        if (animator != null)
            animator.SetTrigger(_m_triggerNameHash);
    }
}
