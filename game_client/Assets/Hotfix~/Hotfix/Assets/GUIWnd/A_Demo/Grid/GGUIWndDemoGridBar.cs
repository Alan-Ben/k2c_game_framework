using ALPackage;
using GOE;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// grid bar 范例
    /// </summary>
    public class GGUIWndDemoGridBar : _AHotfixBaseSubPrefabWnd<GGUIMonoDemoGridBar>
    {
        public GGUIWndDemoGridBar(Transform _parent) : base(_parent)
        {
        }

        protected override string _monoAssetPath { get { return "gui/game_gui.unity3d"; } }

        protected override string _monoObjName { get { return "prefab_hotfix_demo_Grid_bar"; } }

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

        protected override void _onWndInitDoneHotfix()
        {
        }

        //设置信息
        public void setInfo(string _str)
        {
            if (hotfixWnd == null)
                return;

            ALUGUICommon.setLabelTxt(hotfixWnd.goText, _str);
        }
    }
}