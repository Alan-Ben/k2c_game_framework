using ALPackage;

namespace GOE
{
    public partial class VideoAniStateMachine
    {
        /// <summary>
        /// 播放单个视频的状态对象
        /// </summary>
        public class VideoAniNoneState : _AVideoAniBaseState
        {
            public override EVideoAniState state => EVideoAniState.NONE;

            protected override void _onEnter()
            {
            }

            /// <summary>
            /// 子类实现的重置数据的处理函数
            /// </summary>
            protected override void _dealOnExit()
            {

            }

            protected override void _onTick(float _deltaTime)
            {
                //此时进行尝试切换处理，如成功则直接返回
                if (_sm._trySwitchAni())
                    return;
            }

            public override bool canEnterState(_ATALStateBase<EVideoAniState> _newState)
            {
                return true;
            }

            protected override void _dealResetData()
            {
            }
        }
    }
}
