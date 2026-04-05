using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// SkeletonGraphic的动画配置
    /// </summary>
    [Serializable]
    public class CommonSkeletonGraphicAnimationConfig
    {
        [ALHeader("spine动画名")]
        public string spineAnimationName;

        [ALHeader("spine的默认通道, 一般是0不用修改")]
        public int trackIndex = 0;
        
        [ALHeader("动画时长(不一定要配置的和动画实际时长一致，主要是为了在动画播放完成后回调以继续进行后续操作, 若希望在动画播放完成前就进行后续表现, 可缩短动画时长配置)")]
        public float animationTime;

        [ALHeader("是否循环播放")]
        public bool isLoop;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="skeletonGraphic"></param>
        /// <param name="_onOncePlayDone">单次动画播放完成回调</param>
        public void playAnimation(Spine.Unity.SkeletonGraphic skeletonGraphic, Action _onOncePlayDone = null)
        {
            if (skeletonGraphic == null || skeletonGraphic.AnimationState == null)
            {
                _onOncePlayDone?.Invoke();
                return;
            }

            if (string.IsNullOrEmpty(spineAnimationName))
            {
                _onOncePlayDone?.Invoke();
                return;
            }
            
            var track = skeletonGraphic.AnimationState.Data.SkeletonData.FindAnimation(spineAnimationName);
            if (track == null)
            {
                UnityEngine.Debug.LogWarning($"[CommonSkeletonGraphicAnimationConfig] 动画名不存在: '{spineAnimationName}'");
                _onOncePlayDone?.Invoke();
                return;
            }

            skeletonGraphic.AnimationState.SetAnimation(trackIndex, spineAnimationName, isLoop);
            if (animationTime <= 0)
            {
                _onOncePlayDone?.Invoke();
            }
            else
            {
                _createOncePlayDoneTask(_onOncePlayDone, isLoop);
            }
        }

        private void _createOncePlayDoneTask(Action _done, bool _isLoop)
        {
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                _done?.Invoke();
                
                // 判断若是循环播放，则继续创建任务
                if(_isLoop)
                    _createOncePlayDoneTask(_done, _isLoop);
            }, animationTime);
        }
    }
}