using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

namespace GOE
{
    /********************
     * 平台GUI中游戏资源更新进度窗口
     **/
    public class NPPGUIWndInputBox : _ANPGGUIBasicWnd<NPPGUIMonoInputBox>
    {
        /** 显示信息 */
        private string _m_sTitle;
        private string _m_sBtnTxt;
        private string _m_sInitTxt;

        /** 点击的操作回调函数 */
        private Action<string> _m_dClickDelegate;

        public NPPGUIWndInputBox()
            : base(EALUIWndLayer.NOTICE)
        {
        }

        /***************
         * 加载并显示本窗口
         **/
        public void loadAndShowWnd(string _title, string _btnTxt, string _initTxt, Action<string> _clickDelegate)
        {
            _m_sTitle = _title;
            _m_sBtnTxt = _btnTxt;
            _m_sInitTxt = _initTxt;

            _m_dClickDelegate = _clickDelegate;

            //开始加载，并注册回调
            load(showWnd);
        }

        /********************
         * 获取资源所在资源加载文件名称
         **/
        protected override string _monoAssetPath { get { return NPPGUIMonoInputBox.assetPath; } }
        protected override string _monoObjName { get { return NPPGUIMonoInputBox.objName; } }
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

            //设置文字
            ALUGUICommon.setLabelTxt(wnd.textLabel, _m_sTitle);
            ALUGUICommon.setLabelTxt(wnd.btnTxt, _m_sBtnTxt);
            ALUGUICommon.setLabelTxt(wnd.inputField, _m_sInitTxt);

            //绑定按键
            ALUGUICommon.combineBtnClick(wnd.btn, _clickBtn);
        }

        /******************
         * 点击注册按钮
         **/
        protected void _clickBtn(GameObject _go)
        {
            if(null != _m_dClickDelegate)
                _m_dClickDelegate(wnd.inputTxt.text);

            //删除本ui
            discard();
        }
    }
}
