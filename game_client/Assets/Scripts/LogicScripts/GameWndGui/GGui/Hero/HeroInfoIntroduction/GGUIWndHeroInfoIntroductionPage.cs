using System;
using UnityEngine;
using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 伙伴信息简介页签
    /// </summary>
    public class GGUIWndHeroInfoIntroductionPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoHeroInfoIntroductionPage>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字

        //伙伴id
        private long _m_lHeroId;
        //点击关闭按钮
        private Action _m_aOnClickClose;

        public GGUIWndHeroInfoIntroductionPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent)
            : base(_parent)
        {
            _m_sAssetPath = _assetPathInfo.asset_path;
            _m_sObjName = _assetPathInfo.obj_name;
        }

        /**************
         * 窗口相关加载配置
         **/
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
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
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
        public void setInfo(long _heroId, Action _onClickClose)
        {
            _m_lHeroId = _heroId;
            _m_aOnClickClose = _onClickClose;

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
            ALUGUICommon.setLabelTxt(wnd.txtTitle, GCommon.getItemName(ENPItemType.HERO_SKIN, heroInfo != null ? heroInfo.curSkinId: heroRefObj.default_skin_id));
            ALUGUICommon.setLabelTxt(wnd.txtBirthplace, TextTranslate.instance.getLanguage(heroRefObj.birthplace));
            ALUGUICommon.setLabelTxt(wnd.txtOccupation, TextTranslate.instance.getLanguage(heroRefObj.occupation_name));
            ALUGUICommon.setLabelTxt(wnd.txtIntroduction, TextTranslate.instance.getLanguage(heroRefObj.introduction_desc));
        }

        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            _m_aOnClickClose?.Invoke();
        }
    }
}