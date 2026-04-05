using System;
using System.Collections.Generic;

/*********************
 * 单个流程的控制对象，在本流程处理的时候会创建一个新的流程控制对象
 * 并将创建的新对象带入到设置的处理过程中，由处理过程进行过程控制
 * 当创建的新对象处理完成之后将调用child done，并在本流程进行结束处理
 **/
namespace ALPackage
{
    public class ALActionProcess : _AALProcess
    {
        /// <summary>
        /// 创建一个多过程过程对象
        /// </summary>
        /// <returns></returns>
        public static ALActionProcess CreateFuncProcess(Action<ALProcess> _process, string _processTag)
        {
            ALActionProcess process = new ALActionProcess(_processTag);
            process._setProcess(_process);

            return process;
        }

        /// <summary>
        /// 执行的函数
        /// </summary>
        private Action<ALProcess> _m_fFunc;

        protected ALActionProcess(string _processTag)
            : base(_processTag)
        {
            _m_fFunc = null;
        }

        /// <summary>
        /// 设置执行过程
        /// </summary>
        /// <param name="_process"></param>
        /// <returns></returns>
        public void _setProcess(Action<ALProcess> _process)
        {
            _m_fFunc = _process;
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
            if(null != _m_fFunc)
            {
                ALProcess process = ALProcess.CreateProcess(processTag);
                //设置本对象为此父节点
                process.setParentProcess(this);
                //设置步骤的监控对象
                process.setMonitor(_monitor);

                //开始执行
                _m_fFunc(process);
            }
            else
            {
                _onProcessDone();
            }
        }

        /// <summary>
        /// 当子处理对象完成时的处理
        /// </summary>
        protected override void _onChildDone()
        {
            _onProcessDone();
        }

        /// <summary>
        /// 重置处理
        /// </summary>
        protected override void _onDiscard()
        {
            _m_fFunc = null;
        }

        /// <summary>
        /// 完成的处理，需要放回缓存池
        /// </summary>
        protected override void _onDone()
        {
        }
    }
}
