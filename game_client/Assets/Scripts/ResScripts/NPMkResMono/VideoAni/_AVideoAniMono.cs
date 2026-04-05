using ALPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using static GOE.VideoAniStateMachine;

////
///使用视频方式进行动画管理的相关脚本
///
namespace GOE
{
    /// <summary>
    /// 使用动画播放的脚本基类对象，子类根据具体RT的显示方式进行实现
    /// </summary>
    public abstract class _AVideoAniMono : MonoBehaviour
    {
        [ALHeader("视频的播放信息的列表，本对象会在本列表中根据实际情况进行切换")]
        public List<VideoAniInfo> aniInfoList;
        [ALHeader("视频起始播放的视频标记")]
        public string startTag;
        [ALHeader("是否跳过准备阶段，针对多个大视频共存的状态机最好勾选避免进入后中低端机直接卡死")]
        public bool noPrepare = false;

        //当前Mono的有效序列号，避免无效加载和处理
        [System.NonSerialized]
        private long _m_lSerialize;
        //当前逻辑上是否显隐，保证显隐函数是相对的
        [System.NonSerialized]
        private bool _m_curActiveInHierarchy;
        //是否需要检测，本标记是为了避免同帧多次出发导致频繁创建释放RT增加的检测行为
        [System.NonSerialized]
        protected bool _m_bNeedCheck = false;

        //播放状态机
        [System.NonSerialized]
        private VideoAniStateMachine _m_sm;

        //实际的视频播放对象，显示时创建，隐藏时销毁
        [System.NonSerialized]
        private ALVideoPlayer _m_vpPlayer;

        //视频前期状态开始准备之前的回调管理器（将状态标记都初始化进入队列）
        private ALCommonStateDelegate _m_prePrepareDoneDelegate = new ALCommonStateDelegate();
        //视频准备完成之后的回调管理器
        private ALCommonStateDelegate _m_prepareDoneDelegate = new ALCommonStateDelegate();

        /// <summary>
        /// 默认在Awake中创建管理对象，确保相关的状态机处理能正常执行
        /// </summary>
        public virtual void Awake()
        {
            _m_sm = new VideoAniStateMachine(this);

            //初始化状态机
            _m_sm.setState<VideoAniNoneState>((_state) => {
                _state._init(_m_sm, null);
            });
        }

        // Update is called once per frame
        void Update()
        {
            //如果播放对象为空则不做tick处理
            if (null == _m_vpPlayer)
                return;

            //调用状态机的tick处理
            if (null != _m_sm)
                _m_sm.tick(Time.deltaTime);
        }

        //有效和无效的时候分别注册和注销显示对象
        protected void OnEnable()
        {
            //判断是否已经准备检测，避免当帧过多检测
            if (_m_bNeedCheck)
                return;

            _m_bNeedCheck = true;

            //到管理对象中进行处理
            ALCommonActionMonoTask.addNextFrameTask(_check);
        }

        protected virtual void OnDisable()
        {
            //判断是否已经准备检测，避免当帧过多检测
            if (_m_bNeedCheck)
                return;

            _m_bNeedCheck = true;

            //到管理对象中进行处理
            ALCommonActionMonoTask.addNextFrameTask(_check);
        }

        protected void OnDestroy()
        {
            _m_bNeedCheck = true;
            //强制释放相关资源和状态机
            _discard();
        }

        protected void _check()
        {
            if (!_m_bNeedCheck || null == this || null == gameObject)
            {
                return;
            }

            _m_bNeedCheck = false;

            //当前需要显示，之前不显示，执行显示函数
            if (gameObject.activeInHierarchy && !_m_curActiveInHierarchy)
            {
                _m_curActiveInHierarchy = true;
                _onVedioEnable();
            }
            //当前不需要显示，之前是显示的，才执行不显示函数
            else if (!gameObject.activeInHierarchy && _m_curActiveInHierarchy)
            {
                _m_curActiveInHierarchy = false;
                _onVedioDisable();
            }
        }

/// <summary>
/// 在窗口显示的时候调用的函数
/// </summary>
protected void _onVedioEnable()
        {
            //刷新序列号
            _m_lSerialize = ALSerializeOpMgr.next();
            long curSerialize = _m_lSerialize;
            //如果无VP对象则创建VP对象
            if (null == _m_vpPlayer)
            {
                //如果无VP对象则创建VP对象
                _m_vpPlayer = new ALVideoPlayer(_renderMat);
                //设置状态机播放对象
                _m_sm._setVp(_m_vpPlayer);
            }

            //调用事件函数
            if (_m_prePrepareDoneDelegate != null)
                _m_prePrepareDoneDelegate.setInitDone();

            //构造阶段数据，并根据阶段数据进行加载处理
            ALStepCounter prepareStepCounter = new ALStepCounter();
            //初始增加一个步骤，避免提前调用了加载完成
            prepareStepCounter.chgTotalStepCount(1);
            prepareStepCounter.regAllDoneDelegate(() =>
            {
                //判断序列号
                if (curSerialize != _m_lSerialize)
                    return;
                
                //调用事件函数
                if (_m_prepareDoneDelegate != null) 
                    _m_prepareDoneDelegate.setInitDone();
                _onVedioPlayerPrepared();
            });

            //此时将队列中的动画数据都进行加载
            for (int i = 0; i < aniInfoList.Count; i++)
            {
                VideoAniInfo aniInfo = aniInfoList[i];
                if (null == aniInfo)
                    continue;

                //增加步骤总数
                prepareStepCounter.chgTotalStepCount(aniInfo.clipIndexList.Count);
                //遍历clip准备
                foreach (GVideoClipIndex clipIndex in aniInfo.clipIndexList)
                {
                    if (clipIndex.mainId == 0 && clipIndex.subId == 0)
                    {
#if UNITY_EDITOR
                        ALLog.Error($"{this.name} - has clip index 0-0");
#endif
                        prepareStepCounter.addDoneStepCount();
                        continue;
                    }

#if NP_GAME
                    // 提前准备
                    if(noPrepare)
                    {
                        //不准备则直接完成，不调用prepare处理
                        prepareStepCounter.addDoneStepCount();
                    }
                    else
                    {
                        VideoController.prepareVideoClip(clipIndex, _m_vpPlayer, VideoController.audioMode
                            , () => curSerialize == _m_lSerialize
                            , (bool _isSuc, string _err) => prepareStepCounter.addDoneStepCount());
                    }
#endif
                }
            }

            //添加一个固定步骤，确保执行完成
            prepareStepCounter.addDoneStepCount();

            //在所有准备前先播放一次
            if (!string.IsNullOrEmpty(startTag))
            {
                //因为这边晚了一针，如果外部调用了播放，这边就不需要播放开场动画了，不然就不会按状态机逻辑走
                //为了避免外部设置被覆盖，这里需要先做一次检查
                if (_m_sm?.curState.state == EVideoAniState.NONE)
                {
                    //做一次tick检查一次状态，如果仍然是NONE状态则播放起始动作
                    _m_sm?.curState.tick(0);

                    if (_m_sm?.curState.state == EVideoAniState.NONE)
                        playAniTag(startTag);
                }
            }
        }

        /// <summary>
        /// 在窗口无效的时候调用的函数
        /// </summary>
        protected void _onVedioDisable()
        {
            //刷新序列号
            _m_lSerialize = ALSerializeOpMgr.next();
            
            //释放Player对象
            if (null != _m_vpPlayer)
            {
                _m_vpPlayer.discard();
            }
            _m_vpPlayer = null;

            //设置状态机播放对象
            _m_sm._setVp(null);
            //重置状态机
            _m_sm.setState<VideoAniNoneState>((_state) => {
                _state._init(_m_sm, null);
            });
            //释放回调
            _m_prePrepareDoneDelegate?.reset();
            _m_prepareDoneDelegate?.reset();
            _onVideoRealDisable();
        }

        /// <summary>
        /// 释放资源处理
        /// </summary>
        protected void _discard()
        {
            //刷新序列号
            _m_lSerialize = ALSerializeOpMgr.next();

            //释放Player对象
            if (null != _m_vpPlayer)
                _m_vpPlayer.discard();
            if (null != _m_sm)
                _m_sm.discard();
            _m_vpPlayer = null;
            _m_sm = null;
        }

        /// <summary>
        /// 根据Tag检索对应动画的信息
        /// </summary>
        /// <param name="_tag"></param>
        /// <returns></returns>
        public VideoAniInfo lookupAniInfo(string _tag)
        {
            for (int i = 0; i < aniInfoList.Count; i++)
            {
                if (aniInfoList[i].aniTag == _tag)
                    return aniInfoList[i];
            }
            return null;
        }

        /// <summary>
        /// 播放对应Tag的动画
        /// </summary>
        /// <param name="_tag"></param>
        public void playAniTag(string _tag)
        {
            //获取标记信息
            VideoAniInfo aniInfo = lookupAniInfo(_tag);
            if(null == aniInfo)
            {
                ALLog.Error($"can not find ani tag:{_tag} from [{this.name}]");
                return;
            }

            //根据动画信息进入不同状态进行播放行为
            if (_m_sm != null) 
                _m_sm.switchAni(aniInfo);
        }
        
        public void setAniTag(string _tag)
        {
            //获取标记信息
            VideoAniInfo aniInfo = lookupAniInfo(_tag);
            if(null == aniInfo)
            {
                ALLog.Error($"can not find ani tag:{_tag} from [{this.name}]");
                return;
            }

            //根据动画信息进入不同状态进行播放行为
            if (_m_sm != null) 
                _m_sm.setAni(aniInfo);
        }

        /// <summary>
        /// 修改播放倍率
        /// </summary>
        /// <param name="_speed"></param>
        public void playbackSpeed(float _speed)
        {
            if (null == _m_sm)
                return;

            _m_sm.playbackSpeed(_speed);
        }
        
        public bool isPlayingAni(string _aniName)
        {
            if (_m_sm == null)
                return false;

            return _m_sm.isPlayingAni(_aniName);
        }

        /// <summary>
        /// 注册视频预加载完成事件
        /// </summary>
        /// <param name="_delegate"></param>
        public void regVideoPrePreparedDone(Action _delegate)
        {
            if (_m_prePrepareDoneDelegate != null)
                _m_prePrepareDoneDelegate.regDelegate(_delegate);
            else
                _delegate?.Invoke();
        }
        public void regVideoPreparedDone(Action _delegate)
        {
            if (_m_prepareDoneDelegate != null)
                _m_prepareDoneDelegate.regDelegate(_delegate);
            else
                _delegate?.Invoke();
        }

        /// <summary>
        /// 获取渲染的材质对象
        /// </summary>
        /// <param name="_rt"></param>
        protected abstract Material _renderMat { get; }
        
        /// <summary>
        /// 在本视频播放器初始化的时候触发的事件函数
        /// 如果初始化动作有需要，可以在本函数进行切换
        /// </summary>
        protected abstract void _onVedioPlayerPrepared();

        /// <summary>
        /// 设置渲染的显隐
        /// </summary>
        /// <param name="_isEnable"></param>
        public abstract void setRenderEnable(bool _isEnable);
        
        protected virtual void _onVideoRealDisable()
        {
            
        }

        public enum EVideoAniInfoType
        {
            NONE,
            SINGLE_CLIP,    //单个播放的视频
            PROCESS_CLIP,   //持续过程视频，本类型会附带一个进入和退出的视频
        }
        /// <summary>
        /// 单个视频的播放信息
        /// </summary>
        [System.Serializable]
        public class VideoAniInfo
        {
            [ALHeader("视频的Tag，用于在本视频中播放及切换的标记")]
            public string aniTag;
            [ALHeader("视频的播放状态，根据不同状态会进入不同的视频播放处理")]
            public EVideoAniInfoType aniState;
            [ALHeader("视频的资源信息，如果是Single_clip模式会从队列中随机一个进行播放，如果是Process_clip模式，队列的0，1，2索引表示进入动画，循环动画，退出动画")]
            public List<GVideoClipIndex> clipIndexList;
            [ALHeader("循环播放的次数范围, -1为无限，如非-1会在播放时随机一个数字，最小为1")]
            public WCGIntRange loopCountRng;

            [ALHeader("本视频播放完毕后的下一个视频tag")]
            public string nextAniTag;
            [ALHeader("是否需要指定退出时才可以再播放完成前退出，默认false")]
            public bool needExit = false;
            [ALHeader("是否独立背景音乐")]
            public bool isBgMusic;
            
            /// <summary>
            /// 随机选择一个Clip进行播放
            /// </summary>
            public GVideoClipIndex randomClip()
            {
                if (clipIndexList.Count <= 0)
                    return null;

                return clipIndexList[UnityEngine.Random.Range(0, clipIndexList.Count)];
            }
        }
    }
}