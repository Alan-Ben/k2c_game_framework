using System;
using System.IO;
using ALPackage;
using UnityEngine;
using UnityEngine.Video;
using System.Collections.Generic;

#if AL_AVPRO_V2
using RenderHeads.Media.AVProVideo;
#endif

namespace ALPackage
{
#if AL_AVPRO_V2
    /// <summary>
    /// AVPro Video播放器对于加载进行队列管理，避免同时加载的处理
    /// 此行为是为了避免多线程引发可能的加载卡顿和问题
    /// </summary>
    public class ALVideoPlayerAVProPrepareQueue : _IALBaseMonoTask
    {
        /// <summary>
        /// 处理队列的接口对象，每个对象实现接口实现在队列中的相关操作
        /// </summary>
        protected abstract class _AAVProPlayerQueueInfo
        {
            //处理对象
            public ALVideoPlayerDealerAVPro playerDealer;
            //在队列中是否已经处理了
            public bool isDealed;
            //加载序列号
            public long prepareSerialize;

            /// <summary>
            /// 当前对应序列号
            /// </summary>
            protected abstract internal long _currentSerialize { get; }
            /// <summary>
            /// 调用具体的处理操作
            /// </summary>
            protected abstract internal void _dealFunc();
        }
        /// <summary>
        /// 存储在队列中用于比对加载必要性的信息
        /// </summary>
        protected class AVProPlayerPrepareInfo : _AAVProPlayerQueueInfo
        {

            /// <summary>
            /// 当前对应序列号，如果已经在处理了，需要返回处理中序列号，处理完成会进行重置
            /// </summary>
            protected override internal long _currentSerialize { get { return playerDealer.prepareSerialize; } }
            /// <summary>
            /// 调用具体的处理操作
            /// </summary>
            protected override internal void _dealFunc()
            {
                playerDealer._dealPrepare();
            }
        }

        //准备进行资源加载的处理对象
        private List<_AAVProPlayerQueueInfo> _m_lAVProPlayerPrepareList;

        protected internal ALVideoPlayerAVProPrepareQueue()
        {
            _m_lAVProPlayerPrepareList = new List<_AAVProPlayerQueueInfo>();
        }

        /// <summary>
        /// 添加一个待加载的处理对象
        /// </summary>
        /// <param name="playerDealer"></param>
        public void addAVProPlayerPrepare(ALVideoPlayerDealerAVPro playerDealer)
        {
            if (playerDealer == null)
                return;

            AVProPlayerPrepareInfo prepareInfo = new AVProPlayerPrepareInfo();
            prepareInfo.playerDealer = playerDealer;
            prepareInfo.isDealed = false;
            prepareInfo.prepareSerialize = playerDealer.prepareSerialize;

            //添加到队列
            _m_lAVProPlayerPrepareList.Add(prepareInfo);
        }

        /// <summary>
        /// 初始化准备，主要是开启tick进行队列处理
        /// </summary>
        public void init()
        {
            //开启任务进行tick
            ALMonoTaskMgr.instance.addNextFrameTask(this);
        }

        /// <summary>
        /// Task执行函数
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void deal()
        {
            //处理tick
            _tick();

            //加入下一帧
            ALMonoTaskMgr.instance.addNextFrameTask(this);
        }


        /// <summary>
        /// 每帧调用进行加载处理
        /// </summary>
        protected void _tick()
        {
            //逐个检查， 序列号不一致则下一个，直到一致进行加载处理
            while(_m_lAVProPlayerPrepareList.Count > 0)
            {
                //取第一个对象
                _AAVProPlayerQueueInfo info = _m_lAVProPlayerPrepareList[0];

                //序列号仍然一致说明还在加载处理
                if (info.prepareSerialize == info._currentSerialize)
                {
                    //判断是否已经处理，未处理则处理
                    if (!info.isDealed)
                    {
                        info.isDealed = true;
                        //调用准备处理
                        info._dealFunc();
                    }

                    //本对象未完成不做下一个处理
                    break;
                }

                //删除第一个数据
                _m_lAVProPlayerPrepareList.RemoveAt(0);
            }
        }
    }
#endif
}
