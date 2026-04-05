using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using System.Collections.Generic;


namespace GOE
{
    public class NPGGUIWndCustom : _ANPGGUIBasicWnd<NPGGUICustomMonoCustomWnd>
    {

        private string _m_sAssetPath;//自定义界面资源路径
        private string _m_sObjName;//自定义界面资源名称

        public NPGGUIWndCustom(string _monoAssetPath, string _monoObjName)
            : base(EALUIWndLayer.NORMAL)
        {
            _m_sAssetPath = _monoAssetPath;
            _m_sObjName = _monoObjName;
        }
        public NPGGUIWndCustom(string _monoAssetPath, string _monoObjName, EALUIWndLayer _layer)
            : base(_layer)
        {
            _m_sAssetPath = _monoAssetPath;
            _m_sObjName = _monoObjName;
        }

        //对外开放索引信息
        public string ObjName { get { return _m_sObjName; } }

        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            //注册
            ALUGUICommon.combineBtnClick(wnd.backBtn, _onClickBack);
        }

        //点击回退按钮的处理
        protected void _onClickBack(GameObject _go)
        {
            //回退处理
            QueueMgr.instance.DoUIRollBackByEsc();
        }
    }
}
