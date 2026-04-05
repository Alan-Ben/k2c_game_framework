using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using LitJson;
using ALBasicProtocolPack;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 客户端向服务器进行回调式请求时，回调的注册管理对象
    /// </summary>
    public class NPRequestCallbackMonitorMgr
    {
        public const long C_MonitorTimeMS = 30000;

        private static NPRequestCallbackMonitorMgr _g_instance = new NPRequestCallbackMonitorMgr();
        public static NPRequestCallbackMonitorMgr instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new NPRequestCallbackMonitorMgr();

                return _g_instance;
            }
        }

        //是否初始化
        private bool _m_bIsInited;

        //序列号监控对象列表
        private List<NPRequestCallbackTimerInfo> _m_lMonitorList;
        //临时存储的超时队列
        private List<NPRequestCallbackTimerInfo> _m_lTmpTimeoutList;

        //当前操作的序列号
        private long _m_lCallbackOpSerialize;

        protected NPRequestCallbackMonitorMgr()
        {
            _m_bIsInited = false;

            _m_lMonitorList = new List<NPRequestCallbackTimerInfo>();
            _m_lTmpTimeoutList = new List<NPRequestCallbackTimerInfo>();

            _m_lCallbackOpSerialize = 1;
        }

        //获取操作序列号
        public long getCallbackOpSerialize()
        {
            return _m_lCallbackOpSerialize;
        }

        //初始化处理
        public void init()
        {
            if (_m_bIsInited)
                return;

            _m_bIsInited = true;

            //开启任务每秒清理对应的任务
            _startMonitorTask();
        }

        /*********************
         * 开启监控任务
         */
        protected void _startMonitorTask()
        {
            _m_lCallbackOpSerialize++;

            //开启任务每秒清理对应的任务
            ALMonoTaskMgr.instance.addMonoTask(new NPRequestCallbackMonitorTask(_m_lCallbackOpSerialize));
        }

        /******************
         * 增加一个回调监听对象
         * @param _dealer
         * @return
         */
        public void addCallback(long _callbackSerialize)
        {
            //判断数据合法性
            if (0 == _callbackSerialize)
                return;

            //判断是否初始化，未初始化不添加
            if (!_m_bIsInited)
                return;

            long nowTimeMS = ALCommon.getNowTimeMill();
            //全部按照30秒计算，减少其他的排序消耗
            _m_lMonitorList.Add(new NPRequestCallbackTimerInfo(_callbackSerialize, nowTimeMS + C_MonitorTimeMS));

            //在数据队列长度刚好是1000的倍数的时候，做额外判断处理
            if (_m_lMonitorList.Count % 1000 == 0)
            {
                //判断第一个数据如果超过当前时间10秒，则需要另外开启监控任务，避免监控任务断层
                NPRequestCallbackTimerInfo tmpInfo = _m_lMonitorList[0];
                //超时时间增加10秒是当前时间表示已经超过10秒了
                if (tmpInfo.getTimeoutTimeMS() + 10000 < nowTimeMS)
                {
                    //此时大概率出现了任务断层，额外开启一个
                    _startMonitorTask();
                }
            }
        }

        /*************
         * 根据带入的时间标记，清理需要处理的超时任务
         * @param _nowMS
         */
        public void _dealTimeoutTask(long _opSerialize, long _nowMS)
        {
            //提前判断序列号，如不一致则直接不处理
            if (_opSerialize != _m_lCallbackOpSerialize)
                return;

            _m_lTmpTimeoutList.Clear();

            //取出需要处理的回调队列
            do
            {
                NPRequestCallbackTimerInfo tmpInfo = _m_lMonitorList[0];

                //队列为空或者时间还未超时，此时跳出循环
                if (tmpInfo.getTimeoutTimeMS() >= _nowMS)
                {
                    //时间未超过，则跳出循环
                    break;
                }

                //删除第一个
                _m_lMonitorList.RemoveAt(0);
                //超时时间在当前时间之前的任务数据则加入队列
                _m_lTmpTimeoutList.Add(tmpInfo);
            }
            while (_m_lMonitorList.Count > 0 && _m_lTmpTimeoutList.Count < 1024);

            //逐个回调进行失败处理
            for (int i = 0; i < _m_lTmpTimeoutList.Count; i++)
            {
                NPRequestCallbackTimerInfo tmpInfo = _m_lTmpTimeoutList[i];

                //处理失败
                NPRequestCallbackMgr.instance.dealCallbackTimeoutFailDealer(tmpInfo);
            }
            //清空临时队列
            _m_lTmpTimeoutList.Clear();
        }
    }
}
