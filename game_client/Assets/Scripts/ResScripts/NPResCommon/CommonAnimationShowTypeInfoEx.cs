using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 通用动画展示列表，支持多个animation配置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public class CommonAnimationShowTypeInfoEx<T> where T : Enum
    {
        [ALHeader("需要展示的动画类型名称列表")]
        public List<CommonAnimationShowTypeInfo<T>> aniTypeNameList;



        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_playDone"></param>
        public void play(T _type, Action _playDone = null)
        {
            if (aniTypeNameList == null || aniTypeNameList.Count == 0)
            {
                _playDone?.Invoke();
                return;
            }

            CommonAnimationShowTypeInfo<T> showInfo = getAniInfo(_type);
            showInfo?.play(_type, _playDone);
        }

        /// <summary>
        /// 强制播放动画
        /// </summary>
        /// <param name="_type"></param>
        public void forcePlay(T _type)
        {
            if (aniTypeNameList == null || aniTypeNameList.Count == 0)
                return;

            CommonAnimationShowTypeInfo<T> showInfo = getAniInfo(_type);
            showInfo?.forcePlay(_type);
        }

        /// <summary>
        /// 设置动画
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_normalizeTime"></param>
        public void sample(T _type, long _normalizeTime)
        {
            if (aniTypeNameList == null || aniTypeNameList.Count == 0)
                return;

            CommonAnimationShowTypeInfo<T> showInfo = getAniInfo(_type);
            showInfo?.sample(_type, _normalizeTime);
        }

        /// <summary>
        /// 重置动画
        /// </summary>
        /// <param name="_type"></param>
        public void resetAni(T _type)
        {
            if (aniTypeNameList == null || aniTypeNameList.Count == 0)
                return;

            CommonAnimationShowTypeInfo<T> showInfo = getAniInfo(_type);
            showInfo?.resetAni(_type);
        }

        /// <summary>
        /// 重置所有动画
        /// </summary>
        public void resetAllAni()
        {
            if (aniTypeNameList == null || aniTypeNameList.Count == 0)
                return;

            for (int i = 0; i < aniTypeNameList.Count; i++)
            {
                aniTypeNameList[i]?.resetAllAni();
            }
        }


        /// <summary>
        /// 获取动画
        /// </summary>
        public CommonAnimationShowTypeInfo<T> getAniInfo(T _type)
        {
            if (aniTypeNameList == null)
                return null;

            for (int i = 0; i < aniTypeNameList.Count; i++)
            {
                if (aniTypeNameList[i] != null)
                {
                    AnimationShowTypeNameInfo<T> target = aniTypeNameList[i].getAniInfo(_type);
                    if (target != null)
                    {
                        return aniTypeNameList[i];
                    }
                }
            }

            return null;
        }
    }
}