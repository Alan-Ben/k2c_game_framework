using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 需要展示的动画类型对应名称
    /// </summary>
    /// <typeparam name="T">自定义动画类型</typeparam>
    [System.Serializable]
    public class AnimationShowTypeNameInfo<T> where T : Enum
    {
        [ALHeader("动画类型")]
        public T type;
        [ALHeader("动画名称")]
        public string name;
        [ALHeader("重置动画时是否需要重置到最后一帧（不勾选重置到第一帧，勾选重置到最后一帧）")]
        public bool resetEnd;
    }

    /// <summary>
    /// 通用动画展示列表，支持单个animation配置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public class CommonAnimationShowTypeInfo<T> where T : Enum
    {
        [ALHeader("需要展示的动画")]
        public Animation targetAnimation;
        [ALHeader("需要展示的动画类型名称列表")]
        public List<AnimationShowTypeNameInfo<T>> aniTypeNameList;



        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_playDone"></param>
        public void play(T _type ,Action _playDone = null)
        {
            if (targetAnimation == null)
            {
                _playDone?.Invoke();
                return;
            }

            AnimationShowTypeNameInfo<T> aniInfo = getAniInfo(_type);
            if (aniInfo == null || string.IsNullOrEmpty(aniInfo.name))
            {
                _playDone?.Invoke();
                return;
            }

            targetAnimation.Play(aniInfo.name, _playDone);
        }

        /// <summary>
        /// 强制播放动画
        /// </summary>
        /// <param name="_type"></param>
        public void forcePlay(T _type, Action _onComplete = null)
        {
            if (targetAnimation == null)
            {
                _onComplete?.Invoke();
                return;
            }

            AnimationShowTypeNameInfo<T> aniInfo = getAniInfo(_type);
            if (aniInfo == null || string.IsNullOrEmpty(aniInfo.name))
            {
                _onComplete?.Invoke();
                return;
            }

            targetAnimation.ForcePlay(aniInfo.name, 0 , _onComplete);
        }

        /// <summary>
        /// 设置动画
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_normalizeTime"></param>
        public void sample(T _type, long _normalizeTime)
        {
            if (targetAnimation == null)
                return;

            AnimationShowTypeNameInfo<T> aniInfo = getAniInfo(_type);
            if (aniInfo == null || string.IsNullOrEmpty(aniInfo.name))
                return;

            targetAnimation.Sample(aniInfo.name, _normalizeTime);
        }

        /// <summary>
        /// 重置动画
        /// </summary>
        /// <param name="_type"></param>
        public void resetAni(T _type)
        {
            if (targetAnimation == null)
                return;

            AnimationShowTypeNameInfo<T> aniInfo = getAniInfo(_type);
            if (aniInfo == null || string.IsNullOrEmpty(aniInfo.name))
                return;

            targetAnimation.Sample(aniInfo.name, aniInfo.resetEnd ? 1 : 0);
        }

        /// <summary>
        /// 重置所有动画
        /// </summary>
        public void resetAllAni()
        {
            if (targetAnimation == null || aniTypeNameList == null)
                return;

            for (int i = 0; i < aniTypeNameList.Count; i++)
            {
                if (aniTypeNameList[i] != null && !string.IsNullOrEmpty(aniTypeNameList[i].name))
                    targetAnimation.Sample(aniTypeNameList[i].name, aniTypeNameList[i].resetEnd ? 1 : 0);
            }
        }

        /// <summary>
        /// 获取动画名
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public AnimationShowTypeNameInfo<T> getAniInfo(T _type)
        {
            if (aniTypeNameList == null)
                return null;

            for (int i = 0; i < aniTypeNameList.Count; i++)
            {
                if (aniTypeNameList[i] != null && aniTypeNameList[i].type != null && aniTypeNameList[i].type.Equals(_type))
                    return aniTypeNameList[i];
            }

            return null;
        }
    }
}