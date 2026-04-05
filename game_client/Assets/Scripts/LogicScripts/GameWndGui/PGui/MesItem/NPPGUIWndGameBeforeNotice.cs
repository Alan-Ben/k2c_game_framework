using System;
using UnityEngine;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 游戏前公告弹窗
    /// </summary>
    public class NPPGUIWndGameBeforeNotice : _ANPGGUIBasicWnd<NPPGUIMonoGameBeforeNotice>
    {
        private static NPPGUIWndGameBeforeNotice _g_instance = new NPPGUIWndGameBeforeNotice();

        public static NPPGUIWndGameBeforeNotice instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new NPPGUIWndGameBeforeNotice();
                return _g_instance;
            }
        }

        private Action _m_dClickDelegate;
        private Action _m_dClickCloseDelegate;

        public NPPGUIWndGameBeforeNotice() : base(EALUIWndLayer.ADDITION)
        {
        }

        /********************
         * 获取资源所在资源加载文件名称
         **/
        protected override string _monoAssetPath { get { return NPPGUIMonoGameBeforeNotice.assetPath; } }
        protected override string _monoObjName { get { return NPPGUIMonoGameBeforeNotice.objName; } }
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
            _m_dClickCloseDelegate = null;
        }
        /******************
         * 释放资源时触发的事件
         **/
        protected override void _onDiscard()
        {
            _m_dClickDelegate = null;
            _m_dClickCloseDelegate = null;
        }
        /*************
         * 窗口初始化完成调用的函数
         * */
        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;


            //绑定按键
            ALUGUICommon.combineBtnClick(wnd.btn, _clickBtn);

            if (wnd.btnCloseList != null)
            {
                for (int i = 0; i < wnd.btnCloseList.Count; i++)
                {
                    ALUGUICommon.combineBtnClick(wnd.btnCloseList[i], _onClickClose);
                }
            }
        }

        public void setInfo(string _title, string _mes, string _btnTxt, Action _clickDelegate, Action _onClickClose)
        {
            if (wnd == null)
                return;

            if (string.IsNullOrEmpty(_mes))
            {
                ALUGUICommon.setGameObjEnable(wnd.goNoneHideList, false);
                ALUGUICommon.setGameObjEnable(wnd.goNoneShowList, true);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.goNoneShowList, false);
                ALUGUICommon.setGameObjEnable(wnd.goNoneHideList, true);
            }

            //设置文字
            ALUGUICommon.setLabelTxt(wnd.txtTitle, _title);
            ALUGUICommon.setLabelTxt(wnd.txtContent, _mes);
            ALUGUICommon.setLabelTxt(wnd.txtBtn, _btnTxt);
            _m_dClickDelegate = _clickDelegate;
            _m_dClickCloseDelegate = _onClickClose;
        }

        /******************
         * 点击注册按钮
         **/
        protected void _clickBtn(GameObject _go)
        {
            comfirmMes();
        }

        /**************
         * 确认提示消息
         **/
        public void comfirmMes()
        {
            if (null != _m_dClickDelegate)
                _m_dClickDelegate();
            _m_dClickDelegate = null;

            //删除本ui
            discard();
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            _m_dClickCloseDelegate?.Invoke();
            _m_dClickCloseDelegate = null;

            //删除本ui
            discard();
        }
    }
}
