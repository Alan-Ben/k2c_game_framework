using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 头像列表bar
    /// </summary>
    public class GGUIWndPlayerIconListBar : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoPlayerIconListBar>
    {
        public GGUIWndPlayerIconListBar(Transform _parent) : base(_parent)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoPlayerIconListBar.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoPlayerIconListBar.objName; } }
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
        /// <param name="_iconType"></param>
        public void setInfo(EPlayerInfoIconType _iconType)
        {
            if(_iconType == EPlayerInfoIconType.NONE || wnd == null)
                return;

            string desc = string.Empty;
            switch (_iconType)
            {
                case EPlayerInfoIconType.Normal:
                    desc = TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_normalIconTitle_none);
                    break;
                case EPlayerInfoIconType.Consort:
                    desc = TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_consortIconTitle_none);
                    break;
                case EPlayerInfoIconType.Hero:
                    desc = TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_heroIconTitle_none);
                    break;
            }

            ALUGUICommon.setLabelTxt(wnd.txtDesc, desc);
        }
    }
}
