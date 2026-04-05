using ALPackage;
using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用动画展示列表，支持单个animation配置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public class CommonAnimationSingleInfo
    {
        [ALHeader("动画")]
        public Animation targetAnimation;
        [ALHeader("动画名称")]
        public string targetAnimationName;
        [ALHeader("重置动画时是否需要重置到最后一帧（不勾选重置到第一帧，勾选重置到最后一帧）")]
        public bool resetEnd;

        //播放的处理序列号，避免不做播放后还有其他处理
        [NonSerialized]
        private long _m_lPlaySerialize;
        //播放完成的回调，统一管理，方便在切换的时候调用
        [NonSerialized]
        private Action _m_dPlayDoneDelegate;


        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="_playDone"></param>
        public void play(Action _playDone = null)
        {
            if (targetAnimation == null || string.IsNullOrEmpty(targetAnimationName))
            {
                _playDone?.Invoke();
                return;
            }

            _m_lPlaySerialize = ALSerializeOpMgr.next();
            long curSerialize = _m_lPlaySerialize;
            //设置回调对象
            _setPlayDoneDelegate(_playDone);

            //播放动画，并在结果中处理
            targetAnimation.Play(targetAnimationName, 
                () => {
                    _onPlayDone(curSerialize);
                });
        }

        /// <summary>
        /// 强制播放动画
        /// </summary>
        public void forcePlay(Action _playDone = null)
        {
            if (targetAnimation == null)
            {
                _playDone?.Invoke();
                return;
            }

            if (targetAnimation == null || string.IsNullOrEmpty(targetAnimationName))
            {
                _playDone?.Invoke();
                return;
            }

            _m_lPlaySerialize = ALSerializeOpMgr.next();
            long curSerialize = _m_lPlaySerialize;
            //设置回调对象
            _setPlayDoneDelegate(_playDone);

            targetAnimation.ForcePlay(targetAnimationName, 0f,
                () => {
                    _onPlayDone(curSerialize);
                });
        }

        /// <summary>
        /// 设置动画
        /// </summary>
        /// <param name="_normalizeTime"></param>
        public void sample(float _normalizeTime)
        {
            if (targetAnimation == null)
                return;

            if (targetAnimation == null || string.IsNullOrEmpty(targetAnimationName))
                return;

            _m_lPlaySerialize = ALSerializeOpMgr.next();
            //设置回调对象
            _setPlayDoneDelegate(null);

            targetAnimation.Sample(targetAnimationName, _normalizeTime);
        }

        /// <summary>
        /// 重置动画
        /// </summary>
        public void resetAni()
        {
            if (targetAnimation == null)
                return;

            if (targetAnimation == null || string.IsNullOrEmpty(targetAnimationName))
                return;

            _m_lPlaySerialize = ALSerializeOpMgr.next();
            //设置回调对象
            _setPlayDoneDelegate(null);

            targetAnimation.Sample(targetAnimationName, resetEnd ? 1 : 0);
        }

        /// <summary>
        /// 在播放完成时调用的处理回调函数
        /// </summary>
        /// <param name="_playSerialize"></param>
        private void _onPlayDone(long _playSerialize)
        {
            if(_playSerialize != _m_lPlaySerialize)
                return;

            Action preDelegate = _m_dPlayDoneDelegate;
            _m_dPlayDoneDelegate = null;
            preDelegate?.Invoke();
        }

        /// <summary>
        /// 设置播放完成的回调对象
        /// </summary>
        /// <param name="_playDone"></param>
        private void _setPlayDoneDelegate(Action _playDone)
        {
            //如原来有对象则直接执行
            Action preDelegate = _m_dPlayDoneDelegate;
            _m_dPlayDoneDelegate = _playDone;
            preDelegate?.Invoke();
        }
    }
}