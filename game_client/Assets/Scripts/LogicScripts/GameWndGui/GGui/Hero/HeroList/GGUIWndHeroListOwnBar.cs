using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴列表已拥有bar
    /// </summary>
    public class GGUIWndHeroListOwnBar : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoHeroListOwnBar>
    {
        public GGUIWndHeroListOwnBar(Transform _parent) : base(_parent)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoHeroListOwnBar.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoHeroListOwnBar.objName; } }
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
        /// 设置伙伴拥有数量
        /// </summary>
        /// <param name="_heroOwnCount"></param>
        public void setInfo(long _heroOwnCount)
        {
            if(wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtNum, _heroOwnCount);
        }
    }
}
