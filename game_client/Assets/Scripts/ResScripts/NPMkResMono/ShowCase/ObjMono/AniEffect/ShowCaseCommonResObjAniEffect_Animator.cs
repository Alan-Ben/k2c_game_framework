using UnityEngine;

namespace GOE
{
    /// <summary>
    /// showcase resObj通用mono的Animator表现接口
    /// </summary>
    public class ShowCaseCommonResObjAniEffect_Animator : _AShowCaseCommonResObjAniEffect
    {
        [ALHeader("动画控制器animator")]
        public Animator animatorMono;
        
        public override void playAni(string _aniName)
        {
            if(null == animatorMono)
                return;

            animatorMono.Play(_aniName);
        }

        public override void setAnimTrigger(string _triggerName)
        {
            if(null == animatorMono)
                return;

            animatorMono.SetTrigger(_triggerName);
        }

        public override void setSpeed(float _speed)
        {
            if (animatorMono != null) 
                animatorMono.speed = _speed;
        }

        public override void forceSetAni(string _aniName)
        {
            if(null == animatorMono)
                return;

            animatorMono.Play(_aniName);
        }

        public override bool isPlayingAni(string _aniName)
        {
            if (animatorMono == null || string.IsNullOrEmpty(_aniName))
                return false;

            int animationHash = Animator.StringToHash(_aniName);
            for (int i = 0; i < animatorMono.layerCount; i++)
            {
                AnimatorStateInfo stateInfo = animatorMono.GetCurrentAnimatorStateInfo(i);
                if (stateInfo.shortNameHash == animationHash)
                    return true;
            }
            
            return false;
        }
        
        public override void enableRender(bool _isEnable)
        {
            if (null != animatorMono)
                animatorMono.enabled = _isEnable;
        }
    }
}