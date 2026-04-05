using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;


namespace GOE
{
    public class NPGGUIWndAcountGridWnd : _ANPGGUIBasicWnd<NPGGUIMonoAcountGrid>
    {
        private static NPGGUIWndAcountGridWnd _g_instance = new NPGGUIWndAcountGridWnd();
        public static NPGGUIWndAcountGridWnd instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new NPGGUIWndAcountGridWnd();
                return _g_instance;
            }
        }

        //grid窗口对象
        private NPGGUIWndAcountGrid _m_wgGridWnd;

        /** 选择对应帐号时触发的事件 */
        private Action<string> _m_dOnSelectAccount;

        protected NPGGUIWndAcountGridWnd()
            : base(EALUIWndLayer.ADDITION)
        {
            _m_dOnSelectAccount = default(Action<string>);
        }

        public NPGGUIWndAcountGrid gridWnd { get { return _m_wgGridWnd; } }

        /********************
         * 获取资源所在资源加载文件名称
         **/
        protected override string _monoAssetPath { get { return NPGGUIMonoAcountGrid.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoAcountGrid.objName; } }
        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        /******************
         * 显示窗口的事件函数
         **/
        protected override void _onShowWnd()
        {
        }
        /******************
         * 隐藏窗口的事件函数
         **/
        protected override void _onHideWnd()
        {
        }
        /******************
         * 重置窗口数据的事件函数
         **/
        protected override void _onReset()
        {
        }
        /******************
         * 释放资源时触发的事件
         **/
        protected override void _onDiscard()
        {
            if(null != _m_wgGridWnd)
                _m_wgGridWnd.discard();
            _m_wgGridWnd = null;
        }
        /*************
         * 窗口初始化完成调用的函数
         * */
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            _m_wgGridWnd = new NPGGUIWndAcountGrid((NPGGUIMonoAcountGrid)base.wnd);
            
            ALUGUICommon.combineBtnClick(wnd.btnMask, _onClickMaskBtn);
        }

        //开启下拉窗口
        public void setAccountList(List<InternalAccountInfo> _acountList, Action<string> _onSelectAccount)
        {
            if(null == _m_wgGridWnd)
                return;

            //调用显示函数初始化相关变量
            _m_wgGridWnd.showWnd();
            _m_wgGridWnd.setAccountList(_acountList);
            //设置回调
            _m_dOnSelectAccount = _onSelectAccount;
        }

        /****************
         * 触发选择帐号的操作
         **/
        public void triggerSelectAccount(string _account)
        {
            if(null == _m_dOnSelectAccount)
                return;

            _m_dOnSelectAccount(_account);
        }

        /// <summary>
        /// 删除账号
        /// </summary>
        /// <param name="_account"></param>
        public void removeAccount(string _account)
        {
            if(null == _m_wgGridWnd)
                return;

            _m_wgGridWnd.removeAccount(_account);
        }

        
        /// <summary>
        /// 点击遮罩
        /// </summary>
        /// <param name="_gameObject"></param>
        private void _onClickMaskBtn(GameObject _gameObject)
        {
            QueueMgr.instance.forceCloseNodeByType(typeof(LAddNodeLoginUserNameList));
        }
    }
}
