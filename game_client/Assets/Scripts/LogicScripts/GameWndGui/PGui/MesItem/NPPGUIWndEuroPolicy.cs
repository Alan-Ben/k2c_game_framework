using System;
using UnityEngine;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 欧盟地区隐私政策弹窗
    /// </summary>
    public class NPPGUIWndEuroPolicy : _ANPGGUIBasicWnd<NPPGUIMonoEuroPolicy>
    {
        private static NPPGUIWndEuroPolicy _g_instance = new NPPGUIWndEuroPolicy();

        public static NPPGUIWndEuroPolicy instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new NPPGUIWndEuroPolicy();
                return _g_instance;
            }
        }

        //点击的操作回调函数
        private Action _m_dClickDelegate;

        //LOGO
        private NPPGUIWndLoginLogo _m_wLogo;

        public NPPGUIWndEuroPolicy() : base(EALUIWndLayer.ADDITION)
        {
        }

        /// <summary>
        /// 获取资源所在资源加载文件名称
        /// </summary>
        protected override string _monoAssetPath { get { return NPPGUIMonoEuroPolicy.assetPath; } }
        protected override string _monoObjName { get { return NPPGUIMonoEuroPolicy.objName; } }

        /// <summary>
        /// 获取用于加载资源的管理对象
        /// </summary>
        protected override _AALResourceCore _resourceCore { get { return PlatResCore.instance; } }

        /// <summary>
        /// 显示窗口的事件函数
        /// </summary>
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        /// <summary>
        /// 隐藏窗口的事件函数
        /// </summary>
        protected override void _onHideWnd()
        {
            _m_wLogo?.hideWnd();
        }

        /// <summary>
        /// 重置窗口数据的事件函数
        /// </summary>
        protected override void _onReset()
        {
            _m_dClickDelegate = null;
            _m_wLogo?.resetWnd();
        }

        /// <summary>
        /// 释放资源时触发的事件
        /// </summary>
        protected override void _onDiscard()
        {
            _m_dClickDelegate = null;
            _m_wLogo?.discard();
            _m_wLogo = null;
        }

        /// <summary>
        /// 窗口初始化完成调用的函数
        /// </summary>
        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            //绑定按键
            ALUGUICommon.combineBtnClick(wnd.btn, _clickBtn);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_title"></param>
        /// <param name="_mes"></param>
        /// <param name="_btnTxt"></param>
        /// <param name="_clickDelegate"></param>
        public void setInfo(Action _clickDelegate)
        {
            _m_dClickDelegate = _clickDelegate;
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            //设置logo
            ENPLanguage language = GameSetting.instance.getCurrentLanguage();
            NPCommonAssetPathInfo logoAsset = PLoginCommonInfo.instance.obj.getLanguageLogoAssetPath(language);
            if (_m_wLogo != null)
                _m_wLogo.discard();

            if (wnd.logoParent != null && logoAsset != null)
            {
                _m_wLogo = new NPPGUIWndLoginLogo(logoAsset, wnd.logoParent);
                _m_wLogo.load(_m_wLogo.showWnd);
            }
        }

        /// <summary>
        /// 点击确认按钮
        /// </summary>
        /// <param name="_go"></param>
        protected void _clickBtn(GameObject _go)
        {
            comfirmMes();
        }

        /// <summary>
        /// 确认提示消息
        /// </summary>
        public void comfirmMes()
        {
            if (null != _m_dClickDelegate)
                _m_dClickDelegate();
            _m_dClickDelegate = null;
        }
    }
}
