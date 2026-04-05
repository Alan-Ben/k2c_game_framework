﻿using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /*********************
     * 控制需要加载对象的队列，并对执行顺序进行管理的操作Coroutine对象
     **/
    public class ALMemCollectMgr : _IALBaseMonoTask
    {
        private static ALMemCollectMgr _g_instance = new ALMemCollectMgr();
        public static ALMemCollectMgr instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new ALMemCollectMgr();

                return _g_instance;
            }
        }

        //是否需要收集无效内存
        private bool _m_bNeedCollect;
        //最近一次收集内存的标记
        private int _m_iLastCollectionTag;

        //每次回收时调用的回调，用于一些监控是否需要回调处理的对象
        private Action _m_dCollectDelegate;

        protected ALMemCollectMgr()
        {
            //注册本任务执行操作
            _m_bNeedCollect = false;
            _m_iLastCollectionTag = 0;

            _m_dCollectDelegate = null;
        }

        /************
         * 设定需要收集内存
         **/
        public void collect()
        {
            _m_bNeedCollect = true;

            //调整为设置为需要回收才注册下一帧的处理函数
            ALMonoTaskMgr.instance.addLaterMonoTask(this);
        }

        /******************
         * 注册内存回收的事件回调
         **/
        public void regCollectDelegate(Action _delegate)
        {
            if (null == _m_dCollectDelegate)
                _m_dCollectDelegate = _delegate;
            else
                _m_dCollectDelegate += _delegate;
        }

        /*******************
         * 任务具体的执行函数
         **/
        public void deal()
        {
            if (_m_bNeedCollect)
            {
                int curCollectTag = _getCurCollectTag();
                if (_m_iLastCollectionTag != curCollectTag)
                {
                    _m_bNeedCollect = false;
                    _m_iLastCollectionTag = curCollectTag;
                    //处理资源回收
                    Resources.UnloadUnusedAssets();
                    //调用gc
                    System.GC.Collect();

                    //调用回调
                    if (null != _m_dCollectDelegate)
                        _m_dCollectDelegate();
                }
            }
        }

        /**************
         * 获取当前的标记，避免一秒进行多次回收
         **/
        protected int _getCurCollectTag()
        {
            return ALCommon.getNowTimeSec();
        }
    }
}
