using ALPackage;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using static GOE._AVideoAniMono;

namespace GOE
{
    public partial class VideoAniStateMachine
    {
        /// <summary>
        /// 视频的播放的基本状态对象
        /// </summary>
        public abstract class _AVideoAniBaseState : _ATALStateBase<EVideoAniState>
        {
            private VideoAniStateMachine _m_smStateMachine;
            //播放的动作信息
            private VideoAniInfo _m_aiAniInfo;

            //播放的操作序列号，进出状态都会重置，避免不该播放视频的时候播放了
            private long _m_lSerialize;
            //是否开始播放，用于记录是否要在tick判断正在播放
            private bool _m_bIsStarted;

            //是否需要退出本状态，有些状态要求设置本状态才允许退出
            private bool _m_bCanExit;


            protected VideoAniStateMachine _sm => _m_smStateMachine;
            public VideoAniInfo _aniInfo => _m_aiAniInfo;
            public bool isStarted => _m_bIsStarted;
            public bool isPlaying => _sm._vp.isPlaying;
            public bool canExit => _m_bCanExit;

            /// <summary>
            /// 初始化数据
            /// </summary>
            /// <param name="_smStateMachine"></param>
            protected internal void _init(VideoAniStateMachine _smStateMachine, VideoAniInfo _aniInfo)
            {
                _m_smStateMachine = _smStateMachine;
                //初始化本状态信息
                _m_aiAniInfo = _aniInfo;
            }

            /// <summary>
            /// 重置数据的处理函数
            /// </summary>
            public override void resetData()
            {
                _m_smStateMachine = null;
                _m_aiAniInfo = null;
                _m_lSerialize = ALSerializeOpMgr.next();

                _m_bIsStarted = false;
                _m_bCanExit = false;

                //调用子类处理函数
                _dealResetData();
            }

            /// <summary>
            /// 播放当前视频对象
            /// </summary>
            protected void _playClip(GVideoClipIndex _vcIndex)
            {
                // 播放视频
                if (null == _sm._vp)
                    return;

                if (null == _vcIndex)
                    _sm._vp.stop();

                //设置状态
                _m_bIsStarted = false;
                _m_lSerialize = ALSerializeOpMgr.next();
                //存储临时序列号
                long curSerialize = _m_lSerialize;
#if NP_GAME
                bool isBgMusic = false;
                if(null != _m_aiAniInfo)
                {
                    isBgMusic = _m_aiAniInfo.isBgMusic;
                }
                
                //以序列号是否匹配作为是否播放的判断条件
                VideoController.playVideoClip(_vcIndex, _sm._vp, VideoController.audioMode, isBgMusic
                    , () => curSerialize == _m_lSerialize
                    , (_vc) => { _m_bIsStarted = true; });
#endif
            }

            /// <summary>
            /// 退出状态必然改变播放序列号
            /// </summary>
            protected override void _onExit()
            {
                // 退出单个视频状态时的处理
                _m_lSerialize = ALSerializeOpMgr.next();

                //调用子类处理函数
                _dealResetData();
            }

            /// <summary>
            /// 视频状态机比较特别，在播放过程的情况下都不允许切换
            /// </summary>
            /// <param name="_newState"></param>
            /// <returns></returns>
            public override bool canEnterState(_ATALStateBase<EVideoAniState> _newState)
            {
                //全部不允许手动切换
                return false;
            }

            /// <summary>
            /// 设置本状态需要退出
            /// </summary>
            public void setNeedExit()
            {
                _m_bCanExit = true;
            }

            /// <summary>
            /// 尝试切换状态
            /// 在部分状态如 entry与loop时处理有所不同
            /// </summary>
            /// <returns></returns>
            protected internal virtual bool _trySwitchAni(VideoAniInfo _aniInfo)
            {
                //未开始的情况下可以直接做判断，已经在播放的情况下不允许判断
                if(isStarted && isPlaying)
                    return false;

                //如果需要退出才能切换，且目前不可退出时不做切换
                if (_aniInfo.needExit && !canExit)
                    return false;

                //此时进行切换
                return _dealSwitchAni(_aniInfo);
            }
            /// <summary>
            /// 在部分状态如 entry与loop时处理有所不同
            /// </summary>
            /// <returns></returns>
            protected virtual bool _dealSwitchAni(VideoAniInfo _aniInfo)
            {
                //重置状态机切换目标
                _sm._resetSwitchAni();
                //此时进行切换
                _sm.setAni(_aniInfo);

                return true;
            }

            /// <summary>
            /// 子类实现的重置数据的处理函数
            /// </summary>
            protected abstract void _dealOnExit();
            /// <summary>
            /// 子类实现的重置数据的处理函数
            /// </summary>
            protected abstract void _dealResetData();
        }
    }
}
