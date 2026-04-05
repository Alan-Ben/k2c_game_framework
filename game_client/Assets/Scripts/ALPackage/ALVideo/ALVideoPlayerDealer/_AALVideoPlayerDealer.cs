using System;
using UnityEngine;
using UnityEngine.Video;

namespace ALPackage
{
    /// <summary>
    /// 视频播放器核心接口，抽象不同播放器的实现
    /// </summary>
    public abstract class _AALVideoPlayerDealer
    {
        // 无效的视频实例ID
        public const long g_iInvalidVideoInstanceID = -1;

        private _AALVideoPlayerDealerTextureInterface _m_tiTextureInterface = null;

        /// <summary>
        /// 设置纹理设置接口对象
        /// </summary>
        /// <param name="_ti"></param>
        public void setTetureInterface(_AALVideoPlayerDealerTextureInterface _ti)
        {
            _m_tiTextureInterface = _ti;
        }

        /// <summary>
        /// 是否正在播放
        /// </summary>
        public abstract bool isPlaying { get; }

        /// <summary>
        /// 当前播放的纹理
        /// </summary>
        public abstract Texture texture { get; }

        public abstract int videoWidth { get; }
        public abstract int videoHeight { get; }

        /// <summary>
        /// 视频长度
        /// </summary>
        public abstract double length { get; }

        /// <summary>
        /// 准备完成后的回调
        /// </summary>
        public abstract Action<bool, string> onPrepareDone { get; set; }

        /// <summary>
        /// 设置音频输出模式
        /// </summary>
        /// <param name="mode"></param>
        public abstract void setAudioOutputMode(VideoAudioOutputMode _mode);

        /// <summary>
        /// 设置视频渲染目标
        /// </summary>
        /// <param name="_mat"></param>
        public void setTarget(Material _mat)
        {
            //优先使用接口对象
            if (null != _m_tiTextureInterface)
            {
#if AL_AVPRO_V2
                if (this is ALVideoPlayerDealerAVPro avProDealer && _m_tiTextureInterface._setAVProTargetMat(avProDealer._applyToMaterial, _mat))
                    return;
                else
#endif
                if ((this is ALVideoPlayerDealerUnity || this is ALVideoPlayerDealerUnityURL)
                    && _m_tiTextureInterface._setUnityPlayerTargetMat(_mat, this.texture))
                    return;
            }

            //调用默认处理方式
            _defaultSetTarget(_mat);
        }

        /// <summary>
        /// 设置视频渲染目标
        /// </summary>
        /// <param name="_mat"></param>
        protected abstract void _defaultSetTarget(Material _mat);

        /// <summary>
        /// 强制开始准备的处理，由于部分视频播放做了队列排序处理，而在部分处理业务上需要强制播放视频
        /// 因此在这里需要提供一个强制开始准备的处理操作
        /// </summary>
        /// <param name="resource">视频资源</param>
        /// <param name="prePrepare">准备前的回调</param>
        /// <param name="onPrepareDone">准备完成的回调</param>
        public abstract void forcePrepare(_IALVideoResource _resource, Action _prePrepare, Action<bool, string> _onPrepareDone);
        /// <summary>
        /// 准备视频资源
        /// </summary>
        /// <param name="resource">视频资源</param>
        /// <param name="prePrepare">准备前的回调</param>
        /// <param name="onPrepareDone">准备完成的回调</param>
        public abstract void prepare(_IALVideoResource _resource, Action _prePrepare, Action<bool, string> _onPrepareDone);

        /// <summary>
        /// 设置当前播放的帧数
        /// </summary>
        /// <param name="frame"></param>
        public abstract void setFrame(int _frame, float _frameRate = 30f);

        /// <summary>
        /// 播放视频
        /// </summary>
        /// <param name="isLoop">是否循环</param>
        /// <param name="forceFresh">是否强制刷新</param>
        public abstract void play(bool _isLoop, bool _forceFresh = true);

        /// <summary>
        /// 暂停到指定帧
        /// </summary>
        /// <param name="frameCount"></param>
        public abstract void pauseToFrame(int _frameCount, float _frameRate = 30f);

        /// <summary>
        /// 停止播放
        /// </summary>
        public abstract void stop();

        /// <summary>
        /// 修改播放倍率
        /// </summary>
        /// <param name="speed"></param>
        public abstract void playbackSpeed(float _speed);

        /// <summary>
        /// 在被使用时调用的函数，可以把部分需要UnUsed重置的部分开出来
        /// </summary>
        public abstract void onUsed();

        /// <summary>
        /// 在不被使用时调用的函数
        /// </summary>
        public abstract void onUnUsed();

        /// <summary>
        /// 重置播放器状态
        /// </summary>
        public void reset()
        {
            _m_tiTextureInterface = null;

            //调用子类实现的重置函数
            _reset();

            //调用不使用的处理
            onUnUsed();
        }
        /// <summary>
        /// 重置播放器状态
        /// </summary>
        protected abstract internal void _reset();

        /// <summary>
        /// 释放资源
        /// </summary>
        public abstract void discard();

        /// <summary>
        /// 设置音频输出源
        /// </summary>
        /// <param name="_trackIndex"></param>
        /// <param name="_audioSource"></param>
        public abstract void SetTargetAudioSource(ushort _trackIndex, AudioSource _audioSource);

        /// <summary>
        /// 设置音频是否静音
        /// </summary>
        /// <param name="_isMute">是否静音</param>
        public abstract void setAudioMute(ushort _trackIndex, bool _isMute);
        
        /// <summary>
        /// 设置音频音量
        /// </summary>
        public abstract void setAudioVolume(ushort _trackIndex, float _volume = 1f);
        
        /// <summary>
        /// 获取视频实例ID
        /// </summary>
        /// <returns></returns>
        public abstract long getVideoInstanceId();
    }
}
