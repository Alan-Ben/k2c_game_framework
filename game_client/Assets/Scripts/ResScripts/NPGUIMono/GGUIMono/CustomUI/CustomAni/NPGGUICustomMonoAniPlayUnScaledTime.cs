using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 忽略时间缩放的自动enable播放动画
    /// </summary>
    public class NPGGUICustomMonoAniPlayUnScaledTime : MonoBehaviour
    {
        [ALHeader("自动播放动画")]
        public Animation animation;
        
        [ALHeader("自动播放动画名字")]
        public string animationName;

        private void OnEnable()
        {
            if(null == animation)
                return;
            
            StartCoroutine( animation.Play(animationName, false, null) );
        }

        private void OnDisable()
        {
            if(null == animation)
                return;
            
            animation.Stop();
        }
    }
}