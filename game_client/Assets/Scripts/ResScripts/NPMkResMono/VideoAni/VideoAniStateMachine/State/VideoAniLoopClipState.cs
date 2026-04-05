using ALPackage;
using static GOE._AVideoAniMono;

namespace GOE
{
    public partial class VideoAniStateMachine
    {
        /// <summary>
        /// 播放单个视频的状态对象
        /// </summary>
        public class VideoAniLoopClipState : _AVideoAniBaseState
        {
            //循环播放的次数, -1为无限
            private int _m_iLoopCount;
            //已经循环播放的次数
            private int _m_iDealLoopedCount;

            public override EVideoAniState state => EVideoAniState.LOOP_CLIP;

            /// <summary>
            /// 设置表现信息
            /// </summary>
            /// <param name="_sm"></param>
            /// <param name="_clipIndex"></param>
            /// <param name="_loopCount"></param>
            /// <param name="_nextTag"></param>
            protected internal void _initInfo(VideoAniStateMachine _sm, VideoAniInfo _ani)
            {
                //初始化状态机对象
                base._init(_sm, _ani);

                //初始化本状态信息
                _m_iLoopCount = _aniInfo.loopCountRng.getRandomValue();
                _m_iDealLoopedCount = 0;
            }

            protected override void _onEnter()
            {
                //无数据则直接停止播放
                if (_aniInfo.clipIndexList.Count <= 1)
                {
                    //这里需要停止播放
                    _sm._vp.stop();
                    return;
                }

                //直接播放动画，entry动画不存在循环一说
                _playClip(_aniInfo.clipIndexList[1]);
            }

            /// <summary>
            /// 子类实现的重置数据的处理函数
            /// </summary>
            protected override void _dealOnExit()
            {

            }

            protected override void _onTick(float _deltaTime)
            {
                //未开始播放或还在播放的情况下，不需要处理后续逻辑,Loop由于无法保证entry与exit必然衔接所以至少需要播放一次
                if (!isStarted || isPlaying)
                    return;

                //此时进行尝试切换处理，如成功则直接返回
                if (_sm._trySwitchAni())
                    return;

                //不在播放则次数加1
                _m_iDealLoopedCount++;

                //判断是否超出播放次数
                if (_m_iLoopCount > 0 && _m_iDealLoopedCount >= _m_iLoopCount)
                {
                    //此时需要进入下一个状态
                    _sm.setState<VideoAniExitClipState>(
                        (_stat) =>
                        {
                            //初始化信息
                            _stat._initInfo(_sm, _aniInfo);
                        });
                    return;
                }

                //无数据则直接停止播放
                if (_aniInfo.clipIndexList.Count > 1)
                {
                    //播放视频
                    _playClip(_aniInfo.clipIndexList[1]);
                }
            }

            protected override void _dealResetData()
            {
                _m_iLoopCount = 0;
                _m_iDealLoopedCount = 0;
            }

            /// <summary>
            /// 在部分状态如 entry与loop时处理有所不同
            /// </summary>
            /// <returns></returns>
            protected override bool _dealSwitchAni(VideoAniInfo _aniInfo)
            {
                //此时需要进入exit状态
                _sm.setState<VideoAniExitClipState>(
                    (_stat) =>
                    {
                        //初始化信息
                        _stat._initInfo(_sm, _aniInfo);
                    });

                return true;
            }
        }
    }
}
