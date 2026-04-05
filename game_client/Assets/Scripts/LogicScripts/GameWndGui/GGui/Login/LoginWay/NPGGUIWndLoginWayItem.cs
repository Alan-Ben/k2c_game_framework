using ALPackage;
using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 登录方式选择窗口对象
    /// </summary>
    public class NPGGUIWndLoginWayItem : _ANPGGUIBasicSubWnd<NPGGUIMonoLoginWayItem>
    {
        private NPGGuiWndTexture _m_icon;//图标
        private NPLoginWayRefObj _m_wrWayRef;//登录方式配置数据
        private bool _m_bNeedCheckLoginType;//是否需要检查当前登录方式（游戏初始化时登录失败不需要检查，登录成功后切换账号需要检查）

        //点击处理函数
        private Action<NPLoginWayRefObj> _m_dOnClick;

        public NPGGUIWndLoginWayItem(NPGGUIMonoLoginWayItem _wnd)
            : base(_wnd)
        {
            _m_wrWayRef = null;
            initWnd();
        }

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
            if (null != _m_icon)
                _m_icon.discardTexture();
        }
        /******************
         * 释放资源时触发的事件
         **/
        protected override void _onDiscard()
        {
            if (null != _m_icon)
                _m_icon.discard();
            _m_icon = null;
        }

        /*************
        * 窗口初始化完成调用的函数
        * */
        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            ALUGUICommon.combineBtnClick(wnd.clickGo, __onClick);
            if (null != wnd.icon)
                _m_icon = new NPGGuiWndTexture(wnd.icon);
        }

        /// <summary>
        /// 设置登录方式信息
        /// </summary>
        /// <param name="_refObj"></param>
        public void setLoginWay(NPLoginWayRefObj _refObj, bool _needCheckLoginType, Action<NPLoginWayRefObj> _onClick = null)
        {
            if (null == wnd)
                return;
                
            _m_wrWayRef = _refObj;
            _m_dOnClick = _onClick;
            _m_bNeedCheckLoginType = _needCheckLoginType;

            if (null == _m_wrWayRef)
                return;

            //设置信息
            if (null != _m_icon)
                _m_icon.setTexture(_m_wrWayRef.icon_index);

            //设置名称
            ALUGUICommon.setLabelTxt(wnd.wayName, TextTranslate.instance.getLanguage(_m_wrWayRef.name));
            ALUGUICommon.setLabelTxt(wnd.curWayName, TextTranslate.instance.getLanguage(TransKeyConst.account_typeCurLogin_str, _m_wrWayRef.name));

            //检查是否是当前登录方式
            if (_m_bNeedCheckLoginType)
            {
                ALUGUICommon.setGameObjEnable(wnd.goCurLoginShowList, _m_wrWayRef.login_type == SDKLoginSetting.instance.getSDKLoginType());
                ALUGUICommon.setGameObjEnable(wnd.goCurLoginHideList, _m_wrWayRef.login_type != SDKLoginSetting.instance.getSDKLoginType());
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.goCurLoginShowList, false);
                ALUGUICommon.setGameObjEnable(wnd.goCurLoginHideList, true);
            }
        }


        //点击 游客登录
        private void __onClick(GameObject _go)
        {
            if (_AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log("点击登录....... " + (null == _m_wrWayRef ? "none" : _m_wrWayRef.login_type.ToString()));
            }

            if (null != _m_dOnClick)
                _m_dOnClick(_m_wrWayRef);
        }
    }
}
