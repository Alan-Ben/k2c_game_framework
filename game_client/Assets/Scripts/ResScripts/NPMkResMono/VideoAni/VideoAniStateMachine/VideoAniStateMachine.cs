using ALPackage;
using System.Collections;
using UnityEngine;
using static GOE._AVideoAniMono;
using static GOE.VideoAniStateMachine;

namespace GOE
{
    /// <summary>
    /// 视频的播放状态
    /// 由于视频的动作衔接有其特殊性
    /// 所以本管理器将视频分为2个类型：
    ///     单个的播放
    ///     有进入和退出过度的中间展示视频
    /// </summary>
    public enum EVideoAniState
    {
        NONE,
        SINGLE_CLIP,        //单个视频
        ENTRY_CLIP,         //某个视频的进入片段
        LOOP_CLIP,          //某个循环的视频片段
        EXIT_CLIP,          //某个循环视频的退出片段
    }

    public partial class VideoAniStateMachine : _TALStateMachine<_AVideoAniBaseState, EVideoAniState>
    {
        //Mono脚本对象
        private _AVideoAniMono _m_mono;
        //实际的视频播放对象
        private ALVideoPlayer _m_vpPlayer;

        //需要切换到的信息对象
        private VideoAniInfo _m_aiSwitchToAniInfo;

        /// <summary>
        /// 构造对象，需要带入使用的Vp
        /// </summary>
        /// <param name="_vpPlayer"></param>
        public VideoAniStateMachine(_AVideoAniMono _mono)
            : base(VideoAniStateFactory.instance)
        {
            _m_mono = _mono;
            _m_vpPlayer = null;
            _m_aiSwitchToAniInfo = null;
        }
        public VideoAniStateMachine(_AVideoAniMono _mono, ALVideoPlayer _vpPlayer)
            : base(VideoAniStateFactory.instance)
        {
            _m_mono = _mono;
            _m_vpPlayer = _vpPlayer;
            _m_aiSwitchToAniInfo = null;
        }

        //返回对象的简化接口
        protected ALVideoPlayer _vp => _m_vpPlayer;
        protected _AVideoAniMono _AVideoAniMono => _m_mono;

        /// <summary>
        /// 设置Vp对象，状态机有可能先初始化，这个设置允许在状态机初始化之后设置播放对象
        /// </summary>
        /// <param name="_vpPlayer"></param>
        protected internal void _setVp(ALVideoPlayer _vpPlayer)
        {
            _m_vpPlayer = _vpPlayer;
        }

        /// <summary>
        /// 释放操作实践
        /// </summary>
        protected override void _onDiscard()
        {
            _m_mono = null;
            _m_vpPlayer = null;
        }

        /// <summary>
        /// 根据动作信息切换对应状态
        /// </summary>
        /// <param name="_aniInfo"></param>
        public void switchAni(string _aniTag)
        {
            //检索数据
            if (null == _m_mono)
                return;

            VideoAniInfo aniInfo = _m_mono.lookupAniInfo(_aniTag);
            if(null == aniInfo)
            {
#if UNITY_EDITOR
                ALLog.Error($"can not find ani tag:{_aniTag} from [{_m_mono.name}]");
#endif
                return;
            }

            //调用播放处理
            switchAni(aniInfo);
        }
        public void switchAni(VideoAniInfo _aniInfo)
        {
            if (null == _aniInfo)
                return;

            //设置需要切换的对象
            _m_aiSwitchToAniInfo = _aniInfo;
        }
        /// <summary>
        /// 重置需要切换的信息，此函数放这里处理是由于不同的状态在切换的时候不一定需要重置信息
        /// 如entry , loop
        /// </summary>
        private void _resetSwitchAni()
        {
            _m_aiSwitchToAniInfo = null;
        }

        /// <summary>
        /// 根据动作信息切换对应状态
        /// </summary>
        /// <param name="_aniInfo"></param>
        public void setAni(string _aniTag)
        {
            //检索数据
            if (null == _m_mono)
                return;
            
            if(string.IsNullOrEmpty(_aniTag))
                return;

            VideoAniInfo aniInfo = _m_mono.lookupAniInfo(_aniTag);
            if (null == aniInfo)
            {
#if UNITY_EDITOR
                ALLog.Error($"can not find ani tag:{_aniTag} from [{_m_mono.name}]");
#endif
                return;
            }

            //调用播放处理
            setAni(aniInfo);
        }
        public void setAni(VideoAniInfo _aniInfo)
        {
            if (null == _aniInfo)
                return;

            //根据动画信息进入不同状态进行播放行为
            switch (_aniInfo.aniState)
            {
                case EVideoAniInfoType.SINGLE_CLIP:
                    //随机播放的对象
                    GVideoClipIndex clipIndex = _aniInfo.randomClip();
                    if (null == clipIndex)
                        return;

                    //进入单状态播放处理
                    setState<VideoAniSingleClipState>(
                        (_stat) =>
                        {
                            //初始化信息
                            _stat._initInfo(this, _aniInfo);
                        });
                    break;
                case EVideoAniInfoType.PROCESS_CLIP:
                    //进入entry状态处理
                    setState<VideoAniEntryClipState>(
                        (_stat) =>
                        {
                            //初始化信息
                            _stat._initInfo(this, _aniInfo);
                        });
                    break;
                default:
                    break;
            }
        }
        
        /// <summary>
        /// 修改播放倍率
        /// </summary>
        /// <param name="_speed"></param>
        public void playbackSpeed(float _speed)
        {
            if (null == _m_vpPlayer)
                return;

            _m_vpPlayer.playbackSpeed(_speed);
        }

        /// <summary>
        /// 尝试切换状态
        /// </summary>
        /// <returns></returns>
        private bool _trySwitchAni()
        {
            //无数据直接返回失败
            if (null == _m_aiSwitchToAniInfo)
                return false;

            //当前状态数据无效则直接切换
            if(null == curState)
            {
                setAni(_m_aiSwitchToAniInfo);
                _m_aiSwitchToAniInfo = null;
                return true;
            }

            //如切换成功则直接返回
            return curState._trySwitchAni(_m_aiSwitchToAniInfo);
        }
        
        public bool isPlayingAni(string _aniName)
        {
            if (_m_vpPlayer == null || !_m_vpPlayer.isPlaying || !curState.isPlaying || curState._aniInfo == null)
                return false;
         
            return curState._aniInfo.aniTag == _aniName;
        }
    }
}