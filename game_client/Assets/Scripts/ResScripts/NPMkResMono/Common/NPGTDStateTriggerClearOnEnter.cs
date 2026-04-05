
using UnityEngine;

public class NPGTDStateTriggerClearOnEnter : StateMachineBehaviour
{
    [ALHeader("想要重置的 trigger 名字")]
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
            animator.ResetTrigger(_m_triggerNameHash);
    }
}
