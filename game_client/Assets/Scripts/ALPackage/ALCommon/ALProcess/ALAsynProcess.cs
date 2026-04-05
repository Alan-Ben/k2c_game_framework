using System;
using System.Collections.Generic;

/*********************
 * 异步执行的步骤处理对象，此对象将会开一个线程进行过程处理
 **/
namespace ALPackage
{
    public class ALAsynProcess : _AALProcess
    {
        /// <summary>
        /// 创建一个多过程过程对象
        /// </summary>
        /// <returns></returns>
        public static ALAsynProcess CreateAsynProcess(string _processTag)
        {
            return new ALAsynProcess(_processTag);
        }

        //处理的流程对象
        private _AALProcess _m_apProcess;

        protected ALAsynProcess(string _processTag)
            : base(_processTag)
        {
            _m_apProcess = null;
        }

        /// <summary>
        /// 设置处理流程对象
        /// </summary>
        /// <param name="_process"></param>
        public ALAsynProcess setProcess(_AALProcess _process)
        {
            if (isRunning)
            {
                UnityEngine.Debug.LogError("set Asyn process child process when process started!");
                return this;
            }

            _m_apProcess = _process;

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
            if(null == _m_apProcess)
            {
                //无数据则直接处理完成
                _onProcessDone();
                return;
            }

            //设置父子关系
            _m_apProcess.setParentProcess(this);
            //设置监控对象
            setMonitor(_monitor);

            //开启线程进行处理
            ALThread dealThread = new ALThread(_dealChildProcess);
        }

        /// <summary>
        /// 处理子处理对象
        /// </summary>
        protected void _dealChildProcess()
        {
            //调用处理操作
            _m_apProcess.dealProcess(monitor);
        }

        /// <summary>
        /// 当子处理对象完成时的处理
        /// </summary>
        protected override void _onChildDone()
        {
            //子对象完成则本对象也完成
            _onProcessDone();
        }

        /// <summary>
        /// 重置处理
        /// </summary>
        protected override void _onDiscard()
        {
            _m_apProcess = null;
        }

        /// <summary>
        /// 完成的处理，需要放回缓存池
        /// </summary>
        protected override void _onDone()
        {
        }
    }
}
