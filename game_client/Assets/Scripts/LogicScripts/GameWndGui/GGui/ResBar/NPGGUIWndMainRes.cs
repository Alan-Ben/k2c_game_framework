using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using System.Collections.Generic;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 通用资源bar
    /// </summary>
    public class NPGGUIWndMainRes : _ANPGGUIBasicWnd<NPGGUIMonoMainRes>
    {
        //路径
        private string _m_assetPath;
        //名字
        private string _m_objName;
        
        public NPGGUIWndMainRes(string _assetPath, string _objName) : base(EALUIWndLayer.NORMAL)
        {
            _m_assetPath = _assetPath;
            _m_objName = _objName;
        }

        protected override string _monoAssetPath { get { return _m_assetPath; } }
        protected override string _monoObjName { get { return _m_objName; } }
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
        }

        private void _refreshWindow()
        {
        }
    }
}
