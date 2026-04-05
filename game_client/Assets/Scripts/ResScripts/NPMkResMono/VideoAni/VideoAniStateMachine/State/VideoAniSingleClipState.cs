using ALPackage;
using static GOE._AVideoAniMono;

namespace GOE
{
    public partial class VideoAniStateMachine
    {
        /// <summary>
        /// 播放单个视频的状态对象
        /// </summary>
        public class VideoAniSingleClipState : _AVideoAniBaseState
        {
            //播放的视频索引
            private GVideoClipIndex _m_clipIndex;
            //循环播放的次数, -1为无限
            private int _m_iLoopCount;
            //已经循环播放的次数
            private int _m_iDealLoopedCount;

            public override EVideoAniState state => EVideoAniState.SINGLE_CLIP;

            protected internal void _initInfo(VideoAniStateMachine _sm, VideoAniInfo _ani)
            {
                //初始化状态机对象
                base._init(_sm, _ani);

                //初始化本状态信息
                _m_clipIndex = _aniInfo.randomClip();
                _m_iLoopCount = _aniInfo.loopCountRng.getRandomValue();
                _m_iDealLoopedCount = 0;
            }

            protected override void _onEnter()
            {
                //播放视频
                _playClip(_m_clipIndex);
            }

            /// <summary>
            /// 子类实现的重置数据的处理函数
            /// </summary>
            protected override void _dealOnExit()
            {

            }

            /// <summary>
            /// 每帧处理判断是否播放完毕没事则播放下一个
            /// </summary>
            /// <param name="_deltaTime"></param>
            protected override void _onTick(float _deltaTime)
            {
                //未开始播放或还在播放的情况下，不需要处理后续逻辑
                if (isPlaying)
                    return;

                //此时进行尝试切换处理，如成功则直接返回
                if (_sm._trySwitchAni())
                    return;

                if (!isStarted)
                    return;

                //此时尝试切换，如切换成功则直接返回
                if (_sm._trySwitchAni())
                    return;

                //不在播放则次数加1
                _m_iDealLoopedCount++;

                //判断是否超出播放次数
                if(_m_iLoopCount > 0 && _m_iDealLoopedCount >= _m_iLoopCount)
                {
                    //此时需要进入下一个状态
                    _sm.setAni(_aniInfo.nextAniTag);
                    return;
                }

                //循环播放视频
                _playClip(_m_clipIndex);
            }

            /// <summary>
            /// 重置数据
            /// </summary>
            protected override void _dealResetData()
            {
                _m_clipIndex = null;
                _m_iLoopCount = 0;
                _m_iDealLoopedCount = 0;
            }
        }
    }
}
