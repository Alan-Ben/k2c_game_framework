using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟日志列表bar
    /// </summary>
    public class GGUIWndGuildLogListBar : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoGuildLogListBar>
    {
        public GGUIWndGuildLogListBar(Transform _parent) : base(_parent)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildLogListBar.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoGuildLogListBar.objName; } }
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
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_timeMs"></param>
        public void setInfo(long _timeMs)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtDate, TimeUtil.DateTime2StringMD(TimeUtil.FromUTCByTimeZone(_timeMs)));
        }
    }
}
