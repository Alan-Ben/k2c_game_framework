using System;
using UnityEngine;

namespace GOE
{
    //入口表现类接口
    public interface _IGTDHoneEntryPointView
    {
        /// <summary>
        /// 未解锁时点击回调
        /// </summary>
        public Action<Transform, EntryPointRefObj> onLockClick { get; set; }

        /// <summary>
        /// 入口点表配置
        /// </summary>
        public EntryPointRefObj entryPointRefObj { get;}

        /// <summary>
        /// 入口点位置
        /// </summary>
        public Transform entryPointTransform { get;}

        /// <summary>
        /// 初始化
        /// </summary>
        public void init();

        /// <summary>
        /// 销毁
        /// </summary>
        public void discard();

        /// <summary>
        /// 刷新显示
        /// </summary>
        public void refreshShow();

        /// <summary>
        /// 隐藏
        /// </summary>
        public void hide();

        /// <summary>
        /// 播放解锁动画
        /// </summary>
        /// <param name="_onPlayDone"></param>
        public void playUnlockAni(Action _onPlayDone);

        /// <summary>
        /// 设置解锁动画状态
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_normalizeTime"></param>
        public void setUnlockAniSample(EEntryPointAniType _type, long _normalizeTime);

        /// <summary>
        /// 播放屏幕隐藏显示动画
        /// </summary>
        /// <param name="_onPlayDone"></param>
        public void playScreenShowHideAni(bool _isShow, Action _onPlayDone);
    }
}