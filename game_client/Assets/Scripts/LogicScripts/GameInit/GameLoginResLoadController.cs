using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOE
{
    /// <summary>
    /// 游戏登录过程的资源加载管理对象
    /// 玩家组件需要等待此对象加载完成才会开始初始化处理
    /// </summary>
    public class GameLoginResLoadController
    {
        private static GameLoginResLoadController _g_instance;
        public static GameLoginResLoadController instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GameLoginResLoadController();

                return _g_instance;
            }
        }

        //是否资源加载完成
        private bool _m_bIsResLoadDone;
        //资源加载完成之后的处理函数
        private Action _m_dLoadDoneDelegate;

        protected GameLoginResLoadController()
        {
            _m_bIsResLoadDone = false;
            _m_dLoadDoneDelegate = default(Action);
        }

        public bool isResLoadDone { get { return _m_bIsResLoadDone; } }

        /// <summary>
        /// 设置资源加载完成
        /// </summary>
        public void setResLoadDone()
        {
            _m_bIsResLoadDone = true;
            //尝试调用回调
            Action tmpAction = _m_dLoadDoneDelegate;
            _m_dLoadDoneDelegate = default(Action);

            //调用
            if (null != tmpAction)
                tmpAction();
            tmpAction = null;
        }

        /// <summary>
        /// 尝试处理加载完成之后的操作
        /// </summary>
        /// <param name="_doneDelegate"></param>
        public void tryDealDoneDelegate(Action _doneDelegate)
        {
            //已经加载完成则直接处理
            if (_m_bIsResLoadDone)
            {
                if (null != _doneDelegate)
                    _doneDelegate();

                return;
            }

            //注册回调
            _m_dLoadDoneDelegate += _doneDelegate;
        }
    }
}
