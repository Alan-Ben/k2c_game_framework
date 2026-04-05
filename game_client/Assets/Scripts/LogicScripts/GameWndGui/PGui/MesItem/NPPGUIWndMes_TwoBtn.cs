using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

namespace GOE
{
    /********************
     * 平台GUI中游戏资源更新进度窗口
     **/
    public class NPPGUIWndMes_TwoBtn : _ANPGGUIBasicWnd<NPPGUIMonoMesItem_TwoBtn>
    {
        private static NPPGUIWndMes_TwoBtn _g_instance = new NPPGUIWndMes_TwoBtn();
        public static NPPGUIWndMes_TwoBtn instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new NPPGUIWndMes_TwoBtn();

                return _g_instance;
            }
        }

        /** 显示信息 */
        private string _m_sTitle;
        private string _m_sMes;

        private string _m_sLeftBtnTxt;
        private Action _m_dLeftBtnDelegate;

        private string _m_sRightBtnTxt;
        private Action _m_dRightBtnDelegate;

        public NPPGUIWndMes_TwoBtn()
            : base(EALUIWndLayer.NOTICE)
        {
        }

        /***************
         * 加载并显示本窗口
         **/
        public void loadAndShowWnd(string _mes, string _leftBtnTxt, Action _clickLeftBtnDelegate, string _rightBtnTxt, Action _clickRightBtnDelegate, string _title)
        {
            _m_sTitle = _title;
            _m_sMes = _mes;
            _m_sLeftBtnTxt = _leftBtnTxt;
            _m_sRightBtnTxt = _rightBtnTxt;

            _m_dLeftBtnDelegate = _clickLeftBtnDelegate;
            _m_dRightBtnDelegate = _clickRightBtnDelegate;

            //开始加载，并注册回调
            load(showWnd);
        }

        /********************
         * 获取资源所在资源加载文件名称
         **/
        protected override string _monoAssetPath { get { return NPPGUIMonoMesItem_TwoBtn.assetPath; } }
        protected override string _monoObjName { get { return NPPGUIMonoMesItem_TwoBtn.objName; } }
        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return PlatResCore.instance; } }

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
            _m_dLeftBtnDelegate = null;
            _m_dRightBtnDelegate = null;
        }
        /******************
         * 释放资源时触发的事件
         **/
        protected override void _onDiscard()
        {
            _m_dLeftBtnDelegate = null;
            _m_dRightBtnDelegate = null;
        }
        /*************
         * 窗口初始化完成调用的函数
         * */
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            //设置文字
            ALUGUICommon.setLabelTxt(wnd.textTitle, _m_sTitle);
            ALUGUICommon.setLabelTxt(wnd.textLabel, _m_sMes);
            ALUGUICommon.setLabelTxt(wnd.leftBtnTxt, _m_sLeftBtnTxt);
            ALUGUICommon.setLabelTxt(wnd.rightBtnTxt, _m_sRightBtnTxt);

            //绑定按键
            ALUGUICommon.combineBtnClick(wnd.leftBtn, _clickLeftBtn);
            ALUGUICommon.combineBtnClick(wnd.rightBtn, _clickRightBtn);
        }

        /******************
         * 点击注册按钮
         **/
        protected void _clickLeftBtn(GameObject _go)
        {
            comfirmMes(_m_dLeftBtnDelegate);
        }
        protected void _clickRightBtn(GameObject _go)
        {
            comfirmMes(_m_dRightBtnDelegate);
        }

        /**************
         * 确认提示消息
         **/
        public void comfirmMes(Action _dealDelegate)
        {
            if(null != _dealDelegate)
                _dealDelegate();
        }
    }
}
