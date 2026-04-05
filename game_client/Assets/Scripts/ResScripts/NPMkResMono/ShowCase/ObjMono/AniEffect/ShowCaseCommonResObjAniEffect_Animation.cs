using UnityEngine;

namespace GOE
{
    /// <summary>
    /// showcase resObj通用mono的Animation表现接口
    /// </summary>
    public class ShowCaseCommonResObjAniEffect_Animation : _AShowCaseCommonResObjAniEffect
    {
        [ALHeader("动画控制器Animation")]
        public Animation animationMono;
        
        public override void playAni(string _aniName)
        {
            if(null == animationMono)
                return;

            animationMono.ForcePlay(_aniName);
        }

        public override void setAnimTrigger(string _triggerName)
        {
            Debug.LogError_EditorOnly($"animation 不支持Trigger格式");
        }

        public override void setSpeed(float _speed)
        {
            if (animationMono != null && null != animationMono.clip) 
                animationMono.SetAnimSpeed(animationMono.clip.name, _speed);
        }

        public override void forceSetAni(string _aniName)
        {
            if(null == animationMono)
                return;

            animationMono.ForcePlay(_aniName);
        }

        public override bool isPlayingAni(string _aniName)
        {
            if(animationMono == null || !animationMono.isPlaying || string.IsNullOrEmpty(_aniName))
                return false;

            return animationMono != null && animationMono.isPlaying && animationMono.IsPlaying(_aniName);
        }

        public override void enableRender(bool _isEnable)
        {
            if (null != animationMono)
                animationMono.enabled = _isEnable;
        }
    }
}