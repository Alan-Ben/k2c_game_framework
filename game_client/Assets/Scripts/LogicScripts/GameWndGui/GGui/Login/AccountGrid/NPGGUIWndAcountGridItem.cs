using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 账号选择下拉窗口的单个选择按钮
    /// </summary>
    public class NPGGUIWndAcountGridItem : _ATALUGUIBasicGridItemWnd<NPGGUIMonoAcountGridItem>
    {

        private string _m_sAccountName;

        public NPGGUIWndAcountGridItem(NPGGUIMonoAcountGridItem _wnd)
            : base(_wnd)
        {
            _m_sAccountName = null;
        }

        //设置文本
        public void setAccount(string _account)
        {
            _m_sAccountName = _account;

            if(null == wnd)
                return;

            wnd.txtAcount.text = _m_sAccountName;
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
            if(null == wnd)
                return;

            _m_sAccountName = null;
            wnd.txtAcount.text = "";
        }

        //重置Grid单个对象
        protected override void _resetGridItem()
        {
            if(null == wnd)
                return;

            _m_sAccountName = null;
            wnd.txtAcount.text = "";
        }

        protected override void _onDiscard()
        {
            _m_sAccountName = null;
        }

        protected override void _onWndInitDone()
        {
            //绑定进入按钮操作
            ALUGUICommon.combineBtnClick(wnd.btnAcount, _onClickAcount);
            ALUGUICommon.combineBtnClick(wnd.btnDel, _onClickDel);
        }

        //账号选择按钮事件
        private void _onClickAcount(GameObject _go)
        {
            //选择对应帐号
            NPGGUIWndAcountGridWnd.instance.triggerSelectAccount(_m_sAccountName);
        }

        //账号删除按钮事件
        private void _onClickDel(GameObject _go)
        {
            ;
            NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.confirm_delete_account, wnd.txtAcount.text)
                , TextTranslate.instance.getLanguage(TransKeyConst.cancel)
                , null
                 , TextTranslate.instance.getLanguage(TransKeyConst.confirm)
                 , (Action)(() =>
                 {
                     InternalAccountMgr.instance.DeleteUser(wnd.txtAcount.text);

                 //触发事件
                 NPGGUIWndAcountGridWnd.instance.removeAccount(wnd.txtAcount.text);
                 }));
        }
    }
}
