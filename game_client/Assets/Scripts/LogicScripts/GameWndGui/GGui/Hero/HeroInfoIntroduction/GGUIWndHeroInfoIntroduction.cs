using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴信息简介弹窗
    /// </summary>
    public class GGUIWndHeroInfoIntroduction : _ANPGGUIBasicWnd<GGUIMonoHeroInfoIntroduction>
    {
        private static GGUIWndHeroInfoIntroduction _g_instance;
        public static GGUIWndHeroInfoIntroduction instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndHeroInfoIntroduction();
                return _g_instance;
            }
        }

        //伙伴id
        private long _m_lHeroId;

        public GGUIWndHeroInfoIntroduction() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoHeroInfoIntroduction.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoHeroInfoIntroduction.objName; } }
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
            if(wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose,_onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_heroId"></param>
        public void setInfo(long _heroId)
        {
            _m_lHeroId = _heroId;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_m_lHeroId);
            HeroRefObj heroRefObj = GRefdataCoreMgr.instance.heroRefCore.getRef(_m_lHeroId);
            if (heroRefObj == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtName, heroRefObj.transName);
            ALUGUICommon.setLabelTxt(wnd.txtTitle, GCommon.getItemName(ENPItemType.HERO_SKIN, heroInfo != null ? heroInfo.curSkinId : heroRefObj.default_skin_id));
            ALUGUICommon.setLabelTxt(wnd.txtOccupation, TextTranslate.instance.getLanguage(heroRefObj.occupation_name));
            ALUGUICommon.setLabelTxt(wnd.txtIntroduction, TextTranslate.instance.getLanguage(heroRefObj.introduction_desc));
        }

        //点击关闭按钮
        private void _onClickClose(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_HERO_INTRODUCTION);
        }
    }
}