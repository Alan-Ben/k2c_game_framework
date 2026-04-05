using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 限时称号列表bar
    /// </summary>
    public class GGUIWndPlayerTitleLimitedBar : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoPlayerTitleLimitedBar>
    {
        public GGUIWndPlayerTitleLimitedBar(Transform _parent) : base(_parent)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoPlayerTitleLimitedBar.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoPlayerTitleLimitedBar.objName; } }
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
        /// <param name="_groupId"></param>
        public void setInfo(long _groupId)
        {
            if(_groupId <= 0 || wnd == null)
                return;

            PlayerTitleLimitGroupRefObj refObj = GRefdataCoreMgr.instance.playerTitleLimitGroupRefCore.getRef(_groupId);
            if (refObj == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(refObj.desc, refObj.desc_args));
        }
    }
}
