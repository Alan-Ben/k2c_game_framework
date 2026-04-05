using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 骑士推荐场景上加载GO的Mono
    /// </summary>
    public class GTDHeroRecommendGoMono : MonoBehaviour
    {
        [ALHeader("动画")]
        public Animator showAnimator;
        [ALHeader("进入动画名")]
        public string enterName;
        [ALHeader("退出动画名")]
        public string exitName;

        /// <summary>
        /// 播放进入动画
        /// </summary>
        /// <param name="_onPlayDone"></param>
        public void playEnterAni(Action _onPlayDone = null)
        {
            _playAni(enterName, _onPlayDone);
        }

        /// <summary>
        /// 播放退出动画
        /// </summary>
        /// <param name="_onPlayDone"></param>
        public void playExitAni(Action _onPlayDone = null)
        {
            _playAni(exitName, _onPlayDone);
        }

        //播放动画
        private void _playAni(string _aniName,Action _onPlayDone)
        {
            if (showAnimator == null || showAnimator.runtimeAnimatorController  == null || string.IsNullOrEmpty(_aniName))
            {
                _onPlayDone?.Invoke();
                return;
            }

            AnimationClip[] animationClips = showAnimator.runtimeAnimatorController.animationClips;
            if (animationClips == null)
                return;

            AnimationClip targetClip = null;
            foreach (AnimationClip clip in animationClips)
            {
                if (clip != null && !string.IsNullOrEmpty(clip.name) && clip.name.Equals(_aniName))
                {
                    targetClip = clip;
                    break;
                }
            }

            if (targetClip == null)
            {
                _onPlayDone?.Invoke();
                return;
            }

            showAnimator.Play(_aniName);

            if (_onPlayDone != null)
                ALCommonTaskController.CommonActionAddMonoTask(_onPlayDone, targetClip.length);
        }
    }
}