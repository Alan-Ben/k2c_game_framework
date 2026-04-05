using System;
using System.Collections.Generic;

/// <summary>
/// 多个步骤需要处理的时候通过本处理对象进行的过程控制处理
/// </summary>
namespace ALPackage
{
    public class ALDelegateProcess : _AALProcess
    {
        /// <summary>
        /// 创建一个多过程过程对象
        /// </summary>
        /// <returns></returns>
        public static ALDelegateProcess CreateDelegateProcess(string _processTag) { return new ALDelegateProcess(_processTag); }

        /// <summary>
        /// 设置处理操作
        /// </summary>
        private Action<Action> _m_lDealAction;

        //失败回调
        private Action _m_dFailAction;
        //是否失败的时候也执行完成函数
        private bool _m_bIsFailDoDone;

        protected ALDelegateProcess(string _processTag)
            : base(_processTag)
        {
            _m_lDealAction = null;

            _m_dFailAction = null;
            _m_bIsFailDoDone = true;
        }

        /// <summary>
        /// 添加一个执行过程
        /// </summary>
        /// <param name="_process"></param>
        /// <returns></returns>
        public ALDelegateProcess setProcess(Action<Action> _action, Action _failDelegate = null, bool _isFailContinue = true)
        {
            if(null == _action)
                return this;

            if(isRunning)
            {
                UnityEngine.Debug.LogError("add process when process started!");
                return this;
            }

            //设置处理
            _m_lDealAction = _action;
            _m_dFailAction = _failDelegate;
            _m_bIsFailDoDone = _isFailContinue;

            return this;
        }

        /// <summary>
        /// 获取出来可以在监控对象外围补充输出的信息
        /// </summary>
        protected override string _processMonitorExInfo
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 进行处理
        /// </summary>
        protected override void _dealProcess(_IALProcessMonitor _monitor)
        {
            //无数据则直接完成
            if(null == _m_lDealAction)
            {
                _onProcessDone();
                return;
            }

            //处理过程
            try
            {
                _m_lDealAction(_onProcessDone);
            }
            catch (Exception _ex)
            {
                //调用回调
                if (null != _monitor)
                {
                    _monitor.onErr(ALCommon.getNowTimeMill() - processStartTimeMS, processTag, _processMonitorExInfo, _ex);
                }
                else
                {
                    ALLog.Sys($"Delegate Process {processTag} Fail! {_processMonitorExInfo}");
                    UnityEngine.Debug.LogError(_ex.ToString());
                }

                //执行失败函数
                if (null != _m_dFailAction)
                    _m_dFailAction();

                //判断失败是否需要执行成功函数
                if (_m_bIsFailDoDone)
                {
                    //执行成功函数
                    _onProcessDone();
                }
                else
                {
                    //调用失败处理函数
                    _onProcessFailStop();
                }
            }
        }

        /// <summary>
        /// 当子处理对象完成时的处理
        /// </summary>
        protected override void _onChildDone()
        {
        }

        /// <summary>
        /// 重置处理
        /// </summary>
        protected override void _onDiscard()
        {
            _m_lDealAction = null;

            _m_dFailAction = null;
            _m_bIsFailDoDone = false;
        }

        /// <summary>
        /// 完成的处理，需要放回缓存池
        /// </summary>
        protected override void _onDone()
        {
        }
    }
}
