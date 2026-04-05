using System;
using UnityEngine;

namespace GOE
{
    
    /// <summary>
    /// 
    /// </summary>
    public class GTDMonoPlayerCuteActorAnimationEventTrigger : MonoBehaviour
    {
        public event Action OnRunFootsteps;
        public void onRunFootsteps()
        {
            OnRunFootsteps?.Invoke();
        }
    }
}