using UnityEngine;
using UnityEngine.UI;
using ALPackage;
using System.Text.RegularExpressions;

namespace GOE
{
    /*******************
     *账号密码登录窗口对象
     **/
    public class NPPGUIWndActivationCodeVerification : _ANPGGUIBasicWnd<NPPGUIMonoActivationCodeVerification>
    {
        private static NPPGUIWndActivationCodeVerification _g_instance = new NPPGUIWndActivationCodeVerification();
        public static NPPGUIWndActivationCodeVerification instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new NPPGUIWndActivationCodeVerification();
                return _g_instance;
            }
        }

        protected NPPGUIWndActivationCodeVerification()
            : base(EALUIWndLayer.NOTICE)
        {
        }

        /********************
         * 获取资源所在资源加载文件名称
         **/
        protected override string _monoAssetPath { get { return NPPGUIMonoActivationCodeVerification.assetPath; } }
        protected override string _monoObjName { get { return NPPGUIMonoActivationCodeVerification.objName; } }
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
        }
        /******************
         * 释放资源时触发的事件
         **/
        protected override void _onDiscard()
        {
        }
        /*************
         * 窗口初始化完成调用的函数
         * */
        protected override void _onWndInitDone()
        {
            //绑定进入按钮操作
            ALUGUICommon.combineBtnClick(wnd.checkBtn, _onCheckEnterBtn);
            ALUGUICommon.combineBtnClick(wnd.backBtn, _onClickBackBtn);
        }

        /**********************
         * 当点击进入按钮时的处理
         **/
        protected void _onCheckEnterBtn(GameObject _go)
        {
            if(wnd.checkBtn == null || wnd.activeCode == null)
            {
                Debug.LogError("Control has problems");
                return;
            }
            if(!_checkActiveCode(wnd.activeCode.text))
            {
                NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.enter_active_code), TextTranslate.instance.getLanguage(TransKeyConst.confirm), null);
                return;
            }

            //发送激活码验证协议
            LSMgr.instance.sendFinalLSMsgByLog(NPLSWriter_001_LoginOp.make_001_006_ReqEnterSNCode(wnd.activeCode.text));
        }

        //激活码简单校验
        private bool _checkActiveCode(string _code)
        {
            //空判断
            if(string.IsNullOrEmpty(_code))
                return false;

            //长度校验
            if(_code.Length > wnd.codeLenght)
                return false;

            //格式校验:只允许字母和数字的组合
            Regex r = new Regex(@"^[A-Za-z0-9]+$");

            return r.IsMatch(_code);
        }

        /**********************
        * 当点击进入按钮时的处理
        **/
        protected void _onClickBackBtn(GameObject _go)
        {
            //退出验证码节点
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_Login_SN);
        }

        //验证码错误表现
        public void showCodeError()
        {
            if(wnd == null || wnd.codeErrorAnim == null)
                return;

            wnd.codeErrorAnim.Play("error");
        }

    }
}
