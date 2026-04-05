using Spine.Unity;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// showcase resObj通用mono的Spine表现接口
    /// </summary>
    public class ShowCaseCommonResObjAniEffect_Spine : _AShowCaseCommonResObjAniEffect
    {
        [ALHeader("动画控制器Spine")]
        public SkeletonAnimation spineAnimation;
        
        [ALHeader("spine的默认通道,一般是0不需要改")]
        public int trackIndex = 0;
        
        public override void playAni(string _aniName)
        {
            if(null == spineAnimation || null == spineAnimation.state)
                return;

            spineAnimation.state.SetAnimation(trackIndex, _aniName, false);
        }

        public override void setAnimTrigger(string _triggerName)
        {
            Debug.LogError_EditorOnly($"Spine 不支持Trigger格式");
        }

        public override void setSpeed(float _speed)
        {
            if (spineAnimation != null) 
                spineAnimation.timeScale = _speed;
        }

        public override void forceSetAni(string _aniName)
        {
            if(null == spineAnimation || null == spineAnimation.state)
                return;

            spineAnimation.state.SetAnimation(trackIndex, _aniName, false);
        }

        public override bool isPlayingAni(string _aniName)
        {
            if (spineAnimation == null || spineAnimation.state == null || string.IsNullOrEmpty(_aniName))
                return false;

            var currentTrackEntry = spineAnimation.state.GetCurrent(trackIndex);
            return currentTrackEntry != null && currentTrackEntry.Animation != null && currentTrackEntry.Animation.Name == _aniName;
        }
        
        public override void enableRender(bool _isEnable)
        {
            if (null != spineAnimation)
                spineAnimation.enabled = _isEnable;
        }
    }
}