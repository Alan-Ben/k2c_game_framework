using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using LitJson;

using ALPackage;
using NPEnum;


namespace GOE
{
    /// <summary>
    /// 游戏CDN部分初始化过程处理函数
    /// 此过程是无法进行第二次处理的
    /// </summary>
    public abstract class _AGameInitProcess
    {
        //是否开始
        private bool _m_bIsStarted;
        //完成状态回调管理器
        private ALCommonStateDelegate _m_dDelegate;

        //进度管理对象
        private ALProgressNode _m_pnProcess;

        protected _AGameInitProcess()
        {
            _m_bIsStarted = false;
            _m_dDelegate = new ALCommonStateDelegate();

            _m_pnProcess = new ALProgressNode();
        }

        public _IALProgressnterface processNode { get { return _m_pnProcess; } }
        public float curProcess { get { return _m_pnProcess.curProcess; } }
        public bool isDone { get { return _m_dDelegate.isInited; } }

        /// <summary>
        /// 注册完成的回调，如果已经完成会直接调用
        /// </summary>
        /// <param name="_delegate"></param>
        public void regDoneDelegate(Action _delegate)
        {
            _m_dDelegate.regDelegate(_delegate);
        }

        /// <summary>
        /// 调用cdn初始化处理
        /// </summary>
        public void init()
        {
            init(null);
        }
        public void init(Action _doneDelegate)
        {
            //注册回调
            _m_dDelegate.regDelegate(_doneDelegate);

            //已经开始则不处理
            if (_m_bIsStarted)
                return;

            _m_bIsStarted = true;

            //初始化进度管理器
            _IALProgressnterface[] processList = getChildProcess();
            if(null != processList)
            {
                for(int i = 0; i < processList.Length; i++)
                {
                    _m_pnProcess.addChidNode(processList[i], 1f);
                }
            }

            //使用底层Process进行初始化流程
            _dealInit(_finalOp);
        }

        /// <summary>
        /// 重置本进程状态
        /// </summary>
        public void reset()
        {
            if (!canReset)
            {
                return;
            }

            _onReset();
        }

        /// <summary>
        /// 重置时候调用
        /// </summary>
        protected void _onReset()
        {
            //重置子类数据信息
            _resetData();

            _m_bIsStarted = false;
            _m_dDelegate.reset();
        }

        /// <summary>
        /// 根据当前用户情况展示登录界面
        /// 最后登录操作的地方
        /// </summary>
        protected void _finalOp()
        {
            //设置完成
            _m_dDelegate.setInitDone();
        }

        /// <summary>
        /// 是否允许重置，默认允许。
        /// 子类可以重载
        /// </summary>
        protected virtual bool canReset { get { return true; } }

        /// <summary>
        /// 返回子进度队列
        /// </summary>
        /// <returns></returns>
        protected abstract _IALProgressnterface[] getChildProcess();

        /// <summary>
        /// 重置数据部分
        /// </summary>
        protected abstract void _resetData();

        /// <summary>
        /// 加载处理函数
        /// </summary>
        /// <returns></returns>
        protected abstract void _dealInit(Action _doneDelegate);
    }
}
