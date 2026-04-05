using ALPackage;
using UnityEngine;
using System;

namespace GOE
{
    /// <summary>
    /// LOGO资源窗口
    /// </summary>
    public class NPPGUIWndLoginLogo : _ATALBasicLoadPrefabSubUIWnd<NPGGUIMonoLoginLogo>
    {
        private string _m_sAssetPath;//自定义界面资源路径
        private string _m_sObjName;//自定义界面资源名称

        public NPPGUIWndLoginLogo(NPCommonAssetPathInfo _info, Transform _parent) : base(_parent)
        {
            if (_info == null)
                return;

            _m_sAssetPath = _info.asset_path;
            _m_sObjName = _info.obj_name;
        }

        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }
        protected override _AALResourceCore _resourceCore { get { return PlatResCore.instance; } }
        


        protected override void _onDiscard()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }


        protected override void _onShowWnd()
        {
        }

        protected override void _onWndInitDone()
        {
        }

    }
}