using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// showcase resObj通用mono的动画表现接口
    /// </summary>
    public abstract class _AShowCaseCommonResObjAniEffect : MonoBehaviour
    {
        //播放指定动画
        public abstract void playAni(string _aniName);

        //设置动画触发器
        public abstract void setAnimTrigger(string _triggerName);
        
        //设置动画速度
        public abstract void setSpeed(float _speed);

        //强制切换动画
        public abstract void forceSetAni(string _aniName);
        
        /// <summary>
        /// 是否正在播放动画
        /// </summary>
        /// <returns></returns>
        public abstract bool isPlayingAni(string _aniName);
        
        /// <summary>
        /// 显隐对象，只要用于视频
        /// </summary>
        /// <param name="_isEnable"></param>
        public abstract void enableRender(bool _isEnable);
        
        /// <summary>
        /// 注册初始化完成后额外要处理的操作
        /// </summary>
        /// <param name="_loadDone"></param>
        public virtual void doPreDoneAction(Action _loadDone)
        {
            _loadDone?.Invoke();
        }

        
    }
}