using ALPackage;
using static GOE._AVideoAniMono;

namespace GOE
{
    public partial class VideoAniStateMachine
    {
        /// <summary>
        /// 播放单个视频的状态对象
        /// </summary>
        public class VideoAniExitClipState : _AVideoAniBaseState
        {
            public override EVideoAniState state => EVideoAniState.EXIT_CLIP;

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
            }

            protected override void _onEnter()
            {
                //无数据则直接停止播放
                if (_aniInfo.clipIndexList.Count <= 2)
                {
                    //这里需要停止播放
                    _sm._vp.stop();
                    return;
                }

                //直接播放动画，exit动画不存在循环一说
                _playClip(_aniInfo.clipIndexList[2]);
            }

            /// <summary>
            /// 子类实现的重置数据的处理函数
            /// </summary>
            protected override void _dealOnExit()
            {

            }

            protected override void _onTick(float _deltaTime)
            {
                //未开始播放或还在播放的情况下，不需要处理后续逻辑
                if (!isStarted || isPlaying)
                    return;

                //此时进行尝试切换处理，如成功则直接返回
                if (_sm._trySwitchAni())
                    return;

                //此时需要进入下一个状态
                _sm.setAni(_aniInfo.nextAniTag);
            }

            protected override void _dealResetData()
            {
            }
        }
    }
}
