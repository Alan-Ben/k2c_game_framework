using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

namespace GOE
{
    /********************
     * 平台GUI中游戏资源更新进度窗口
     **/
    public class NPPGUIWndMes_OneBtn : _ANPGGUIBasicWnd<NPPGUIMonoMesItem_OneBtn>
    {
        private static NPPGUIWndMes_OneBtn _g_instance = new NPPGUIWndMes_OneBtn();
        public static NPPGUIWndMes_OneBtn instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new NPPGUIWndMes_OneBtn();

                return _g_instance;
            }
        }

        /** 显示信息 */
        private string _m_sTitle;
        private string _m_sMes;
        private string _m_sBtnTxt;

        /** 点击的操作回调函数 */
        private Action _m_dClickDelegate;

        public NPPGUIWndMes_OneBtn()
            : base(EALUIWndLayer.TOP)
        {
        }

        /***************
         * 加载并显示本窗口
         **/
        public void loadAndShowWnd(string _mes, string _btnTxt, Action _clickDelegate, string _title)
        {
            _m_sTitle = _title;
            _m_sMes = _mes;
            _m_sBtnTxt = _btnTxt;

            _m_dClickDelegate = _clickDelegate;

            //开始加载，并注册回调
            load(showWnd);
        }

        public void setInfo(string _mes, string _btnTxt, Action _clickDelegate, string _title)
        {
            _m_sTitle = _title;
            _m_sMes = _mes;
            _m_sBtnTxt = _btnTxt;

            _m_dClickDelegate = _clickDelegate;
            
            _refresh();
        }
        
        /********************
         * 获取资源所在资源加载文件名称
         **/
        protected override string _monoAssetPath { get { return NPPGUIMonoMesItem_OneBtn.assetPath; } }
        protected override string _monoObjName { get { return NPPGUIMonoMesItem_OneBtn.objName; } }
        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return PlatResCore.instance; } }

        /******************
         * 显示窗口的事件函数
         **/
        protected override void _onShowWnd()
        {
            _refresh();
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
            _m_dClickDelegate = null;
        }
        /******************
         * 释放资源时触发的事件
         **/
        protected override void _onDiscard()
        {
            _m_dClickDelegate = null;
        }
        /*************
         * 窗口初始化完成调用的函数
         * */
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            //绑定按键
            ALUGUICommon.combineBtnClick(wnd.btn, _clickBtn);
        }

        /******************
         * 点击注册按钮
         **/
        protected void _clickBtn(GameObject _go)
        {
            comfirmMes();
        }

        protected void _refresh()
        {
            if(null == wnd)
                return;
            
            //设置文字
            ALUGUICommon.setLabelTxt(wnd.textTitle, _m_sTitle);
            ALUGUICommon.setLabelTxt(wnd.textLabel, _m_sMes);
            ALUGUICommon.setLabelTxt(wnd.btnTxt, _m_sBtnTxt);
        }

        /**************
         * 确认提示消息
         **/
        public void comfirmMes()
        {
            if (null != _m_dClickDelegate)
            {
                Action clickAction = _m_dClickDelegate;
                _m_dClickDelegate = null;
                clickAction();
            }

        }
    }
}
